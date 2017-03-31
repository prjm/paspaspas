using System;
using System.Collections.Generic;

namespace PasPasPas.Infrastructure.Utils {

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

            foreach (T x in source) {
                buffer.Enqueue(x);

                if (buffer.Count == n + 1)
                    yield return buffer.Dequeue();
            }
        }

    }
}
