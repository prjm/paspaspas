using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;

namespace PasPasPas.Parsing.Parser {

    /// <summary>
    ///     stub for an empty collection
    /// </summary>
    /// <typeparam name="T">collection type</typeparam>
    public class EmptyCollection<T> : ICollection<T>, IList<T>, IReadOnlyList<T> {

        /// <summary>
        ///     field for lazy single instance
        /// </summary>
        private static Lazy<IList<T>> instance
            = new Lazy<IList<T>>(() => new EmptyCollection<T>());

        /// <summary>
        ///     access item
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                throw new InvalidOperationException();
            }

            set
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     single instance
        /// </summary>
        public static IList<T> Instance
            => instance.Value;

        /// <summary>
        ///     single instance
        /// </summary>
        public static IReadOnlyList<T> ReadOnlyInstance
            => (IReadOnlyList<T>)instance.Value;


        /// <summary>
        ///     get item count (is always <code>0</code>)
        /// </summary>
        public int Count
            => 0;

        /// <summary>
        ///     check if readonly (always <code>true</code>)
        /// </summary>
        public bool IsReadOnly
            => true;

        /// <summary>
        ///     add an item
        /// </summary>
        /// <param name="item">item to add</param>
        /// <remarks>operation is not supported</remarks>
        public void Add(T item) {
            throw new InvalidOperationException();
        }

        /// <summary>
        ///     clear the list
        /// </summary>
        public void Clear() { }


        /// <summary>
        ///     check if an item is included
        /// </summary>
        /// <param name="item">item to check</param>
        /// <returns>always <c>false</c></returns>
        public bool Contains(T item)
            => false;

        /// <summary>
        ///     copies nothing
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex) { }

        /// <summary>
        ///     get an empty enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
            => Enumerable.Empty<T>().GetEnumerator();

        /// <summary>
        ///     index of an item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(T item)
            => -1;

        /// <summary>
        ///     insert ist not supported
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, T item) {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     removes an item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item) => false;

        /// <summary>
        ///     remove is not supported
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index) {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     gets an empty enumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
            => Enumerable.Empty<T>().GetEnumerator();
    }
}