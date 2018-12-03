using System;
using System.Text;

namespace PasPasPas.Infrastructure.ObjectPooling {

    /// <summary>
    ///     string pool entry
    /// </summary>
    public class StringPoolEntry {

        private const int FnvOffsetBias = unchecked((int)2166136261);
        private const int FnvPrime = 16777619;

        private static int ComputeFNVHashCode(StringBuilder text) {
            var hashCode = FnvOffsetBias;
            var start = 0;
            var end = text.Length;

            for (var i = start; i < end; i++) {
                hashCode = unchecked((hashCode ^ text[i]) * FnvPrime);
            }

            return hashCode;
        }

        private static int ComputeFNVHashCode(string text) {
            var hashCode = FnvOffsetBias;
            var start = 0;
            var end = text.Length;

            for (var i = start; i < end; i++) {
                hashCode = unchecked((hashCode ^ text[i]) * FnvPrime);
            }

            return hashCode;
        }

        /// <summary>
        ///     create a new string pool entry
        /// </summary>
        public StringPoolEntry() { }

        /// <summary>
        ///     create a new string pool entry from a search entry
        /// </summary>
        /// <param name="oldEntry">existing entry</param>
        public StringPoolEntry(StringPoolEntry oldEntry) {
            PoolItem = oldEntry.StringBuffer.ToString();
            StringBuffer = null;
            ComputedHashCode = oldEntry.ComputedHashCode;
        }

        /// <summary>
        ///     generated hash code
        /// </summary>
        public int ComputedHashCode { get; private set; }

        /// <summary>
        ///     string buffer
        /// </summary>
        public StringBuilder StringBuffer { get; private set; }

        /// <summary>
        ///     cached, immutable pool item
        /// </summary>
        public string PoolItem { get; private set; }

        private static bool EqualChars(StringBuilder l, StringBuilder r) {
            if (l.Length != r.Length)
                return false;

            for (var i = 0; i < l.Length; i++)
                if (l[i] != r[i])
                    return false;

            return true;
        }

        private static bool EqualChars(string l, StringBuilder r) {
            if (l.Length != r.Length)
                return false;

            for (var i = 0; i < l.Length; i++)
                if (l[i] != r[i])
                    return false;

            return true;
        }

        /// <summary>
        ///     initialize the pool entry
        /// </summary>
        /// <param name="value">string buffer</param>
        public void Initialize(StringBuilder value) {
            StringBuffer = value;
            ComputedHashCode = ComputeFNVHashCode(value);
        }

        /// <summary>
        ///     initialize the pool entry
        /// </summary>
        /// <param name="value">string buffer</param>
        public void Initialize(string value) {
            PoolItem = value;
            ComputedHashCode = ComputeFNVHashCode(value);
        }

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj">object to compare</param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (!(obj is StringPoolEntry entry))
                return false;

            if (entry.PoolItem != null && PoolItem != null)
                return string.Equals(entry.PoolItem, PoolItem, StringComparison.Ordinal);
            else if (entry.PoolItem != null && StringBuffer != null)
                return EqualChars(entry.PoolItem, StringBuffer);
            else if (entry.StringBuffer != null && PoolItem != null)
                return EqualChars(PoolItem, entry.StringBuffer);
            else if (entry.StringBuffer != null && StringBuffer != null)
                return EqualChars(entry.StringBuffer, StringBuffer);

            return false;
        }

        /// <summary>
        ///     get the compute hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => ComputedHashCode;

        /// <summary>
        ///     clear this entry
        /// </summary>
        public void Clear() {
            ComputedHashCode = 0;
            PoolItem = null;
            StringBuffer = null;
        }
    }

    /// <summary>
    ///     string pool entries
    /// </summary>
    public class StringPoolEntries : ObjectPool<StringPoolEntry> {

        /// <summary>
        ///     prepare a string pool item
        /// </summary>
        /// <param name="result"></param>
        protected override void Prepare(StringPoolEntry result)
            => result.Clear();

    }

}
