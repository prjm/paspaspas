using System;

namespace PasPasPas.Infrastructure.Utils {

    /// <summary>
    ///     indexed queue
    /// </summary>
    /// <typeparam name="T">queue type</typeparam>
    public class IndexedQueue<T> {

        private T[] array;
        private int start;
        private int len;

        /// <summary>
        ///     create a new queue
        /// </summary>
        public IndexedQueue() : this(4) { }

        /// <summary>
        ///     create a new queue
        /// </summary>
        /// <param name="initialBufferSize"></param>
        public IndexedQueue(int initialBufferSize) {

            if (initialBufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(initialBufferSize));

            array = new T[initialBufferSize];
            start = 0;
            len = 0;
        }

        /// <summary>
        ///     add an element to the queue
        /// </summary>
        /// <param name="item">element to add</param>
        public void Enqueue(T item) {
            if (len == array.Length) {
                T[] bigger = new T[array.Length * 2];
                for (var i = 0; i < len; i++) {
                    bigger[i] = array[(start + i) % len];
                }
                start = 0;
                array = bigger;
            }
            array[(start + len) % array.Length] = item;
            ++len;
        }

        /// <summary>
        ///     remove an item of the queueu
        /// </summary>
        /// <returns>element to remove</returns>
        public T Dequeue() {

            if (len < 1)
                throw new InvalidOperationException();

            T result = array[start];
            ++start;
            --len;
            return result;
        }

        /// <summary>
        ///     get item count
        /// </summary>
        public int Count
            => len;

        /// <summary>
        ///     look at the first element
        /// </summary>
        public T First {
            get {
                if (len < 1)
                    throw new InvalidOperationException();
                return this[0];
            }
        }

        /// <summary>
        ///     look at the first element
        /// </summary>
        public T Last {
            get {
                if (len < 1)
                    throw new InvalidOperationException();
                return this[Count - 1];
            }
        }

        /// <summary>
        ///     get element at position
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index] {
            get {
                return array[(start + index) % array.Length];
            }
        }
    }
}
