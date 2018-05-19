﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Utils {

    /// <summary>
    ///     collection of syntax part elemens
    /// </summary>
    /// <typeparam name="T">elemet type</typeparam>
    public class SyntaxPartCollection<T> : ISyntaxPartList<T> where T : class, ISyntaxPart {

        private readonly ISyntaxPart parent;

        /// <summary>
        ///     create a new syntax part collection
        /// </summary>
        /// <param name="parentItem">parent item</param>
        public SyntaxPartCollection(ISyntaxPart parentItem)
            => parent = parentItem;

        private IList<T> internalList
            = null;

        /// <summary>
        ///     get the number of items in this collection
        /// </summary>
        public int Count =>
            internalList == null ? 0 : internalList.Count;

        /// <summary>
        ///     check if the collection is readonly
        /// </summary>
        /// <remarks>returns aleways <c>false</c></remarks>
        public bool IsReadOnly =>
            false;

        /// <summary>
        ///     acces an item of this collection
        /// </summary>
        /// <param name="index">item index</param>
        /// <returns></returns>
        public T this[int index] {
            get {
                if ((index >= 0) && (index < Count))
                    return internalList[index];
                else {
                    throw new IndexOutOfRangeException();
                };
            }

            set {
                if ((index >= 0) && (index < Count))
                    internalList[index] = value;
                else {
                    throw new IndexOutOfRangeException($"Invald index {index}");
                };

            }
        }

        /// <summary>
        ///     add an item to this collection
        /// </summary>
        /// <param name="item">item to add</param>
        public void Add(T item) {

            if (item == default(T))
                throw new ArgumentNullException(nameof(item));

            item.ParentItem = parent;

            if (internalList == null)
                internalList = new List<T>();

            internalList.Add(item);
        }

        /// <summary>
        ///     removes all items from this collection
        /// </summary>
        public void Clear() {
            if (internalList == null)
                return;

            for (var index = 0; index < internalList.Count; index++) {
                internalList[index].ParentItem = null;
            }

            internalList.Clear();
        }

        /// <summary>
        ///     test if this collection contains a specific item
        /// </summary>
        /// <param name="item">item to find</param>
        /// <returns><c>true</c> if the item is contained in this collection</returns>
        public bool Contains(T item) {

            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (internalList == null)
                return false;

            return internalList.Contains(item);
        }

        /// <summary>
        ///     removes an item from the collection
        /// </summary>
        /// <param name="item">item to remove</param>
        /// <returns><c>true</c> if the item could be removed</returns>
        public bool Remove(T item) {

            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (internalList == null)
                return false;

            var index = internalList.IndexOf(item);

            if (index >= 0) {
                var itemToRemove = internalList[index];
                itemToRemove.ParentItem = null;
                internalList.RemoveAt(index);
                return true;
            }

            return false;
        }

        /// <summary>
        ///     copy contents to an array
        /// </summary>
        /// <param name="array">array</param>
        /// <param name="arrayIndex">start index</param>
        public void CopyTo(T[] array, int arrayIndex) {

            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (internalList == null)
                return;

            internalList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        ///     get the enumerator for all syntax parts
        /// </summary>
        /// <returns>enumerator</returns>
        public IEnumerator<T> GetEnumerator()
            => internalList == null ? Enumerable.Empty<T>().GetEnumerator() : internalList.GetEnumerator();

        /// <summary>
        ///     get the enumerator for all syntax parts
        /// </summary>
        /// <returns>enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
            => internalList == null ? Enumerable.Empty<T>().GetEnumerator() : internalList.GetEnumerator();

        /// <summary>
        ///     get the index of an item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(T item) {

            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (internalList == null)
                return -1;

            return internalList.IndexOf(item);


        }

        /// <summary>
        ///     add a node
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, T item)
            => throw new NotImplementedException();

        /// <summary>
        ///     remove a node
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
            => throw new NotImplementedException();

        /// <summary>
        ///     last item or <c>null</c>
        /// </summary>
        /// <returns></returns>
        public T LastOrDefault() {
            var count = Count;
            return count > 0 ? this[count - 1] : default;
        }
    }
}