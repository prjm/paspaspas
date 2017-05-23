using System;
using System.Collections;
using System.Collections.Generic;

namespace PasPasPas.Infrastructure.Utils {

    /// <summary>
    ///     enumerator which returns no elements
    /// </summary>                             
    /// <typeparam name="T">enumerator type</typeparam>
    public class EmptyEnumerator<T> : IEnumerator<T> {

        /// <summary>
        ///     current element
        /// </summary>
        public T Current
            => default(T);

        /// <summary>
        ///     current element
        /// </summary>
        object IEnumerator.Current
            => default(T);

        /// <summary>
        ///     dispose the enumerator
        /// </summary>
        public void Dispose() { }

        /// <summary>
        ///     reset the enumerator
        /// </summary>
        public void Reset() { }

        /// <summary>
        ///     is there a next element?
        /// </summary>
        /// <returns>returns always <c>false</c></returns>
        public bool MoveNext()
            => false;

    }

    /// <summary>
    ///     enumerable without items items
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EmptyEnumerable<T> : IEnumerable<T> {

        /// <summary>
        ///     get the enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
            => new EmptyEnumerator<T>();

        /// <summary>
        ///     get the enumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
            => new EmptyEnumerator<T>();

    }

    /// <summary>
    ///     empty list
    /// </summary>
    /// <typeparam name="T">list type</typeparam>
    public class EmptyList<T> : EmptyEnumerable<T>, IList<T> {

        /// <summary>
        ///     access an element by index
        /// </summary>
        /// <param name="index">elemet index</param>
        /// <returns>element at index position</returns>
        /// <exception cref="System.IndexOutOfRangeException">always</exception>
        public T this[int index] {
            get {
                ExceptionHelper.IndexOutOfRange(index);
                return default(T);
            }
            set => ExceptionHelper.IndexOutOfRange(index);
        }

        /// <summary>
        ///     gets the numer of elements
        /// </summary>
        public int Count =>
            0;

        /// <summary>
        ///     empty lists are read-only
        /// </summary>
        public bool IsReadOnly
            => true;

        /// <summary>
        ///     adds an item to the list
        /// </summary>
        /// <param name="item">item to add</param>
        /// <exception cref="System.InvalidOperationException">always</exception>
        public void Add(T item)
            => ExceptionHelper.InvalidOperation();

        /// <summary>
        ///     clear the list
        /// </summary>
        /// <exception cref="System.InvalidOperationException">always</exception>
        public void Clear()
            => ExceptionHelper.InvalidOperation();

        /// <summary>
        ///     test if an element is contained in this list
        /// </summary>
        /// <param name="item">item to search</param>
        /// <returns><c>false</c> because the list is empty</returns>
        public bool Contains(T item)
            => false;

        /// <summary>
        ///     copy the list to an array
        /// </summary>
        /// <param name="array">target array</param>
        /// <param name="arrayIndex">target array index</param>
        public void CopyTo(T[] array, int arrayIndex) { }

        /// <summary>
        ///     index of an item
        /// </summary>
        /// <param name="item">item to search</param>
        /// <returns><c>-1</c> every time</returns>
        public int IndexOf(T item)
            => -1;


        /// <summary>
        ///     insert an itrem
        /// </summary>
        /// <param name="index">item index</param>
        /// <param name="item">item to insert</param>
        /// <exception cref="System.InvalidOperationException">always</exception>
        public void Insert(int index, T item)
            => ExceptionHelper.InvalidOperation();

        /// <summary>
        ///     remove an item
        /// </summary>
        /// <param name="item">item to remove</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">always</exception>
        public bool Remove(T item) {
            ExceptionHelper.InvalidOperation();
            return false;
        }

        /// <summary>
        ///     remove element at an index
        /// </summary>
        /// <param name="index">element index</param>
        /// <exception cref="System.InvalidOperationException">always</exception>
        public void RemoveAt(int index)
            => ExceptionHelper.InvalidOperation();
    }

}
