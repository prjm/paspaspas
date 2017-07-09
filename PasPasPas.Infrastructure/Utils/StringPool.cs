using System.Collections.Generic;

namespace PasPasPas.Infrastructure.Utils {

    /// <summary>
    ///     simple string pool for chars
    /// </summary>
    public static class CharsAsString {

        private static Dictionary<char, string> pool
            = new Dictionary<char, string>();

        /// <summary>
        ///     add this string to the string pool or get a reference to this item
        ///     out of the string pool
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Pool(this char value) {
            string result;

            if (pool.TryGetValue(value, out var poolRef))
                result = poolRef;
            else {
                result = value.ToString();
                pool.Add(value, result);
            }

            return result;
        }

    }

    /// <summary>
    ///     basic string pool
    /// </summary>
    public static class StringPool {

        private static Dictionary<string, string> pool
            = new Dictionary<string, string>();

        private const int MaxStringLength = 300;

        /// <summary>
        ///     add this string to the string pool or get a reference to this item
        ///     out of the string pool
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Pool(this string value) {
            if (!string.IsNullOrEmpty(value) && value.Length <= MaxStringLength) {

                if (pool.TryGetValue(value, out var poolRef))
                    value = poolRef;
                else
                    pool.Add(value, value);
            }

            return value;
        }


    }
}
