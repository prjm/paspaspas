using System;
using System.Collections.Generic;
using System.Globalization;

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

    /// <summary>
    ///     basic string pool
    /// </summary>
    public class StringPool : IEnvironmentItem {

        private Dictionary<string, string> pool
            = new Dictionary<string, string>();

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
            if (!string.IsNullOrEmpty(value) && value.Length <= MaxStringLength) {
                if (pool.TryGetValue(value, out var poolRef))
                    value = poolRef;
                else
                    pool[value] = value;
            }

            return value;
        }


    }
}
