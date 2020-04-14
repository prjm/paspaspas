using System.Collections;
using System.Collections.Generic;

namespace PasPasPas.Infrastructure.src.Utils {

    /// <summary>
    ///     enumerate two objects
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct PairEnumerator<T> : IEnumerator<T> {

        private byte position;
        private readonly T value1;
        private readonly T value2;

        /// <summary>
        ///     create a new enumerator
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public PairEnumerator(T arg1, T arg2) {
            position = 0;
            value1 = arg1;
            value2 = arg2;
        }

        /// <summary>
        ///     current element
        /// </summary>
        public T Current {
            get {
                switch (position) {
                    case 1:
                        return value1;
                    case 2:
                        return value2;
                    default:
                        return default;
                }
            }
        }

        object IEnumerator.Current {
            get {
                switch (position) {
                    case 1:
                        return value1;
                    case 2:
                        return value2;
                    default:
                        return default;
                }
            }
        }

        /// <summary>
        ///     nothing to do
        /// </summary>
        public void Dispose() { }

        /// <summary>
        ///     move to the next element
        /// </summary>
        /// <returns></returns>
        public bool MoveNext() {
            position++;
            return 0 < position && position < 3;
        }

        /// <summary>
        ///     reset
        /// </summary>
        public void Reset()
            => position = 0;
    }
}
