#nullable disable
using System.Collections;
using System.Collections.Generic;

namespace PasPasPas.Infrastructure.Utils {

    /// <summary>
    ///     single element enumerator
    /// </summary>
    public struct SingleEnumerator<T> : IEnumerator<T> {

        private readonly T value;
        private bool atElement;

        /// <summary>
        ///     create a new single element enumerator
        /// </summary>
        /// <param name="aValue"></param>
        public SingleEnumerator(T aValue) {
            value = aValue;
            atElement = false;
        }

        /// <summary>
        ///     single element
        /// </summary>
        public T Current
            => atElement ? value : default;

        object IEnumerator.Current
            => atElement ? value : default;

        /// <summary>
        ///     move to the next element
        /// </summary>
        /// <returns></returns>
        public bool MoveNext() {
            if (atElement)
                return false;

            atElement = true;
            return true;
        }

        /// <summary>
        ///     reset the enumerator
        /// </summary>
        public void Reset()
            => atElement = false;

        /// <summary>
        ///     nothing to do
        /// </summary>
        public void Dispose() { }

    }
}
