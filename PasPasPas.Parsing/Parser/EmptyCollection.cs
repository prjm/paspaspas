using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree;
using System;
using System.Collections;
using System.Linq;

namespace PasPasPas.Parsing.Parser {

    /// <summary>
    ///     stub for an empty collection
    /// </summary>
    /// <typeparam name="T">collection type</typeparam>
    public class EmptyCollection<T> : ICollection<T> {

        /// <summary>
        ///     field for lazy single instance
        /// </summary>
        private static Lazy<ICollection<T>> instance
            = new Lazy<ICollection<T>>(() => new EmptyCollection<T>());

        /// <summary>
        ///     single instance
        /// </summary>
        public static ICollection<T> Instance
            => instance.Value;

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
        ///     removes an item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item) => false;

        /// <summary>
        ///     gets an empty enumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
            => Enumerable.Empty<T>().GetEnumerator();
    }
}