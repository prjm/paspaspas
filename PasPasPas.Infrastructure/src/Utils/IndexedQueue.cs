using System;
using System.Collections;
using System.Collections.Generic;

namespace PasPasPas.Infrastructure.Utils {

    /// <summary>
    ///     enumerator for queues
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class IndexedQueueEnumerator<T> : IEnumerator<T> {

        private readonly IndexedQueue<T> refQueue;
        private int index = 0;

        /// <summary>
        ///     create a new enumerator
        /// </summary>
        /// <param name="queue"></param>
        public IndexedQueueEnumerator(IndexedQueue<T> queue)
            => refQueue = queue;

        /// <summary>
        ///     get current item
        /// </summary>
        public T Current =>
            refQueue[index];

        /// <summary>
        ///     current item
        /// </summary>
        object IEnumerator.Current
            => Current;

        /// <summary>
        ///     dispose enumerator
        /// </summary>
        public void Dispose() { }

        /// <summary>
        ///     move to the next item
        /// </summary>
        /// <returns></returns>
        public bool MoveNext() {
            if (index >= refQueue.Count)
                return false;
            index++;
            return true;
        }

        /// <summary>
        ///     reset enumerator
        /// </summary>
        public void Reset()
            => index = 0;
    }

    /// <summary>
    ///     indexed queue
    /// </summary>
    /// <typeparam name="T">queue type</typeparam>
    public class IndexedQueue<T> : IEnumerable<T> {

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
                var bigger = new T[array.Length * 2];
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
        ///     remove an item of the queue
        /// </summary>
        /// <returns>element to remove</returns>
        public T Dequeue() {

            if (len < 1)
                throw new InvalidOperationException();

            var result = First;
            ++start;
            --len;
            return result;
        }

        /// <summary>
        ///     clear the queue
        /// </summary>
        public void Clear() {
            start = 0;
            len = 0;
        }

        /// <summary>
        ///     get an enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
            => new IndexedQueueEnumerator<T>(this);

        /// <summary>
        ///     enumerate all queue items
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

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
        public ref T this[int index]
            => ref array[(start + index) % array.Length];
    }
}
