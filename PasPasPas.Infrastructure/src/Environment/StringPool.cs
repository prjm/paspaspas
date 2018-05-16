using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PasPasPas.Infrastructure.Environment {

    /// <summary>
    ///     simple string pool for chars
    /// </summary>
    public class CharsAsString : IEnvironmentItem {

        private Dictionary<char, string> pool
            = new Dictionary<char, string>();

        /// <summary>
        ///     get the number of items in the pool
        /// </summary>
        public int Count
            => pool.Count;

        /// <summary>
        ///     caption
        /// </summary>
        public string Caption
            => "CharPool";

        /// <summary>
        ///     add this string to the string pool or get a reference to this item
        ///     out of the string pool
        /// </summary>
        /// <param name="environment">environment</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string PoolString(IBasicEnvironment environment, char value) {
            string result;
            var instance = environment.CharStringPool;
            var pool = instance.pool;

            if (pool.TryGetValue(value, out var poolRef))
                result = poolRef;
            else {
                result = value.ToString(CultureInfo.InvariantCulture);
                pool.Add(value, result);
            }

            return result;
        }

        /// <summary>
        ///     clear the pool
        /// </summary>
        public void Clear()
            => pool.Clear();
    }

    class StringPoolEntry {
        public int HashCode;
        public IList<char> Data;
        public string PoolItem;

        private static bool EqualChars(IList<char> l, IList<char> r) {
            if (l.Count != r.Count)
                return false;

            for (int i = 0; i < l.Count; i++)
                if (l[i] != r[i])
                    return false;

            return true;
        }

        private static bool EqualChars(string l, IList<char> r) {
            if (l.Length != r.Count)
                return false;

            for (int i = 0; i < l.Length; i++)
                if (l[i] != r[i])
                    return false;

            return true;
        }

        private static bool EqualChars(IList<char> l, string r) {
            if (l.Count != r.Length)
                return false;

            for (int i = 0; i < l.Count; i++)
                if (l[i] != r[i])
                    return false;

            return true;
        }



        public override bool Equals(object obj) {
            if (!(obj is StringPoolEntry entry))
                return false;

            if (entry.PoolItem != null && PoolItem != null)
                return string.Equals(entry.PoolItem, PoolItem, StringComparison.Ordinal);
            else if (entry.PoolItem != null && Data != null)
                return EqualChars(entry.PoolItem, Data);
            else if (entry.Data != null && PoolItem != null)
                return EqualChars(entry.Data, PoolItem);
            else if (entry.Data != null && Data != null)
                return EqualChars(entry.Data, Data);

            return false;
        }

        public override int GetHashCode()
            => HashCode;
    }

    /// <summary>
    ///     basic string pool
    /// </summary>
    public class StringPool : IEnvironmentItem {

        private Dictionary<StringPoolEntry, StringPoolEntry> pool
            = new Dictionary<StringPoolEntry, StringPoolEntry>();

        private const int MaxStringLength
            = 300;

        /// <summary>
        ///     number of pooled strings
        /// </summary>
        public int Count
            => pool.Count;

        /// <summary>
        ///     caption
        /// </summary>
        public string Caption
            => "StringPool";

        /// <summary>
        ///     clear the buffer pool
        /// </summary>
        public void Clear() =>
            pool.Clear();

        /// <summary>
        ///     add this string to the string pool or get a reference to this item
        ///     out of the string pool
        /// </summary>
        /// <param name="value">string to pool</param>
        /// <returns></returns>
        public string PoolString(string value) {
            return value;
            /*
            if (!string.IsNullOrEmpty(value) && value.Length <= MaxStringLength) {
                if (pool.TryGetValue(value, out var poolRef))
                    value = poolRef;
                else
                    pool[value] = value;
            }

            return value;
            */
        }

        internal const int FnvOffsetBias = unchecked((int)2166136261);
        internal const int FnvPrime = 16777619;

        internal static int GetFNVHashCode(IList<char> text, int start, int length) {
            int hashCode = FnvOffsetBias;
            int end = start + length;

            for (int i = start; i < end; i++) {
                hashCode = unchecked((hashCode ^ text[i]) * FnvPrime);
            }

            return hashCode;
        }

        private ObjectPool<StringPoolEntry> entryPool
            = new ObjectPool<StringPoolEntry>();


        public string PoolString(IList<char> value) {

            using (var searchEntry = entryPool.Borrow()) {
                searchEntry.Data.PoolItem = null;
                searchEntry.Data.Data = value;
                searchEntry.Data.HashCode = GetFNVHashCode(value, 0, value.Count);

                if (pool.TryGetValue(searchEntry.Data, out var data))
                    return data.PoolItem;

                var entry = new StringPoolEntry() {
                    Data = null,
                    HashCode = searchEntry.Data.HashCode,
                    PoolItem = new string(value.ToArray())
                };

                pool[entry] = entry;
                return entry.PoolItem;
            }
        }


    }
}
