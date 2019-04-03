using System;
using System.Collections.Generic;
using System.Text;

namespace PasPasPas.Infrastructure.Utils {

    /// <summary>
    ///     helper class for a string builder
    /// </summary>
    public static class StringBuilderHelper {

        /// <summary>
        ///     get the last char or <c>0</c> from the string
        /// </summary>
        /// <param name="data">string</param>
        /// <returns>last char</returns>
        public static char LastCharOrDefault(this string data) {
            var length = data.Length;
            if (length < 1)
                return default;
            else
                return data[length - 1];
        }

        /// <summary>
        ///     test if a string builder ends with a given string
        /// </summary>
        /// <param name="stringBuilder">string builder to look at</param>
        /// <param name="test">search string</param>
        /// <returns><c>true</c> if the string builder ends with that string</returns>
        public static bool EndsWith(this StringBuilder stringBuilder, char test) {
            if (stringBuilder.Length < 1)
                return false;

            return test == stringBuilder[stringBuilder.Length - 1];
        }

        /// <summary>
        ///     test if a string builder ends with a given string
        /// </summary>
        /// <param name="stringBuilder">string builder to look at</param>
        /// <param name="test">search string</param>
        /// <returns><c>true</c> if the string builder ends with that string</returns>
        public static bool EndsWith(this StringBuilder stringBuilder, string test) {
            if (stringBuilder.Length < test.Length)
                return false;

            var offset = stringBuilder.Length - test.Length;
            for (var i = 0; i < test.Length; i++) {
                if (test[i] != stringBuilder[offset + i])
                    return false;
            }

            return true;
        }
    }

    /// <summary>
    ///     integer helpers
    /// </summary>
    public static class RangeHelpers {

        /// <summary>
        ///     test if one integer equals another value
        /// </summary>
        /// <param name="value">value to compare</param>
        /// <param name="t1">first item</param>
        /// <param name="t2">second item</param>
        /// <returns></returns>
        public static bool In(this int value, int t1, int t2)
            => value == t1 || value == t2;


        /// <summary>
        ///     test if one integer equals another value
        /// </summary>
        /// <param name="value">value to compare</param>
        /// <param name="t1">first item</param>
        /// <param name="t2">second item</param>
        /// <param name="t3">third item</param>
        /// <returns></returns>
        public static bool In(this int value, int t1, int t2, int t3)
            => value == t1 || value == t2 || value == t3;


    }

    /// <summary>
    ///     some linq extensions
    /// </summary>
    public static class VariousExtensions {

        /// <summary>
        ///     drop last elementsof an list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<T> DropLast<T>(this IEnumerable<T> source) {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return InternalDropLast(source, 1);
        }


        /// <summary>
        ///     drop last elements of an list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="numberOfElements"></param>
        /// <returns></returns>
        public static IEnumerable<T> DropLast<T>(this IEnumerable<T> source, int numberOfElements) {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (numberOfElements < 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfElements), "Argument should be non-negative.");

            return InternalDropLast(source, numberOfElements);
        }

        private static IEnumerable<T> InternalDropLast<T>(IEnumerable<T> source, int n) {
            var buffer = new Queue<T>(n + 1);

            foreach (var x in source) {
                buffer.Enqueue(x);

                if (buffer.Count == n + 1)
                    yield return buffer.Dequeue();
            }
        }

        /// <summary>
        ///     test if this object is a number
        /// </summary>
        /// <param name="value">object to test</param>
        /// <returns></returns>
        public static bool IsNumber(this object value) =>
            value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;

    }
}
