using System;
using System.Collections.Generic;

namespace PasPasPas.Infrastructure.Environment {

    /// <summary>
    ///     simple string pool for chars
    /// </summary>
    public class CharsAsString : IManualStaticCache {

        private Dictionary<char, string> pool
            = new Dictionary<char, string>();

        private static Lazy<CharsAsString> instance
            = new Lazy<CharsAsString>(() => new CharsAsString(), true);

        /// <summary>
        ///     get the number of items in the pool
        /// </summary>
        public int Count
            => pool.Count;

        private CharsAsString()
            => StaticEnvironment.Provide(StaticDependency.CharStringPool, this);

        /// <summary>
        ///     add this string to the string pool or get a reference to this item
        ///     out of the string pool
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string PoolString(char value) {
            string result;
            var pool = instance.Value.pool;

            if (pool.TryGetValue(value, out var poolRef))
                result = poolRef;
            else {
                result = value.ToString();
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

    /// <summary>
    ///     basic string pool
    /// </summary>
    public class StringPool : IManualStaticCache {

        private Dictionary<string, string> pool
            = new Dictionary<string, string>();

        private static Lazy<StringPool> instance
            = new Lazy<StringPool>(() => new StringPool(), true);

        private const int MaxStringLength = 300;

        /// <summary>
        ///     count strings
        /// </summary>
        public int Count =>
            pool.Count;

        /// <summary>
        ///     clear the buffer pool
        /// </summary>
        public void Clear() =>
            pool.Clear();

        private StringPool()
            => StaticEnvironment.Provide(StaticDependency.StringPool, this);

        /// <summary>
        ///     add this string to the string pool or get a reference to this item
        ///     out of the string pool
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string PoolString(string value) {
            if (!string.IsNullOrEmpty(value) && value.Length <= MaxStringLength) {
                var pool = instance.Value.pool;

                if (pool.TryGetValue(value, out var poolRef))
                    value = poolRef;
                else
                    pool.Add(value, value);
            }

            return value;
        }


    }
}
