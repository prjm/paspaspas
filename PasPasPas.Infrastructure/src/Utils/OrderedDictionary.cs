using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PasPasPas.Infrastructure.Utils {

    /// <summary>
    ///     A dictionary object that allows rapid hash lookups using keys, but also
    ///     maintains the key insertion order so that values can be retrieved by
    ///     key index.
    /// </summary>
    /// <remarks>
    ///     Similar to the way a DataColumn is indexed by column position and by column name, this
    ///     advanced dictionary construct allows for a very natural and robust handling of indexed
    ///     structured data.
    /// </remarks>
    [DebuggerDisplay("Count = {Count}")]
    public sealed class OrderedDictionary<TKey, TValue> : IOrderedDictionary<TKey, TValue> {

        private DelegateKeyedCollection<TKey, KeyValuePair<TKey, TValue>> _keyedCollection;

        /// <summary>
        ///     Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key associated with the value to get or set.</param>
        public TValue this[TKey key] {
            get => GetValue(key);
            set => SetValue(key, value);
        }

        /// <summary>
        ///     Gets or sets the value at the specified index.
        /// </summary>
        /// <param name="index">The index of the value to get or set.</param>
        public TValue this[int index] {
            get => GetItem(index).Value;
            set => SetItem(index, value);
        }

        /// <summary>
        ///     Gets the number of items in the dictionary
        /// </summary>
        public int Count
            => _keyedCollection.Count;

        /// <summary>
        ///     Gets all the keys in the ordered dictionary in their proper order.
        /// </summary>
        public ICollection<TKey> Keys
            => _keyedCollection.Select(x => x.Key).ToList();

        /// <summary>
        ///     Gets all the values in the ordered dictionary in their proper order.
        /// </summary>
        public ICollection<TValue> Values
            => _keyedCollection.Select(x => x.Value).ToList();

        /// <summary>
        /// Gets the key comparer for this dictionary
        /// </summary>
        public IEqualityComparer<TKey> Comparer {
            get;
            private set;
        }


        /// <summary>
        ///     create a new ordered dictionary
        /// </summary>
        public OrderedDictionary()
            => Initialize();

        /// <summary>
        ///
        /// </summary>
        /// <param name="comparer"></param>
        public OrderedDictionary(IEqualityComparer<TKey> comparer)
            => Initialize(comparer);

        /// <summary>
        ///
        /// </summary>
        /// <param name="dictionary"></param>
        public OrderedDictionary(IOrderedDictionary<TKey, TValue> dictionary) {
            Initialize();
            foreach (var pair in dictionary) {
                _keyedCollection.Add(pair);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="comparer"></param>
        public OrderedDictionary(IOrderedDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) {
            Initialize(comparer);
            foreach (var pair in dictionary) {
                _keyedCollection.Add(pair);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="items"></param>
        public OrderedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> items) {
            Initialize();
            foreach (var pair in items) {
                _keyedCollection.Add(pair);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="items"></param>
        /// <param name="comparer"></param>
        public OrderedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> items, IEqualityComparer<TKey> comparer) {
            Initialize(comparer);
            foreach (var pair in items) {
                _keyedCollection.Add(pair);
            }
        }

        private void Initialize(IEqualityComparer<TKey> comparer = null) {
            Comparer = comparer;
            if (comparer != null) {
                _keyedCollection = new DelegateKeyedCollection<TKey, KeyValuePair<TKey, TValue>>(x => x.Key, comparer);
            }
            else {
                _keyedCollection = new DelegateKeyedCollection<TKey, KeyValuePair<TKey, TValue>>(x => x.Key);
            }
        }

        /// <summary>
        ///     Adds the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add.  The value can be null for reference types.</param>
        public void Add(TKey key, TValue value)
            => _keyedCollection.Add(new KeyValuePair<TKey, TValue>(key, value));

        /// <summary>
        /// Removes all keys and values from this object.
        /// </summary>
        public void Clear()
            => _keyedCollection.Clear();

        /// <summary>
        /// Inserts a new key-value pair at the index specified.
        /// </summary>
        /// <param name="index">The insertion index.  This value must be between 0 and the count of items in this object.</param>
        /// <param name="key">A unique key for the element to add</param>
        /// <param name="value">The value of the element to add.  Can be null for reference types.</param>
        public void Insert(int index, TKey key, TValue value) => _keyedCollection.Insert(index, new KeyValuePair<TKey, TValue>(key, value));

        /// <summary>
        /// Gets the index of the key specified.
        /// </summary>
        /// <param name="key">The key whose index will be located</param>
        /// <returns>Returns the index of the key specified if found.  Returns -1 if the key could not be located.</returns>
        public int IndexOf(TKey key) {
            if (_keyedCollection.Contains(key)) {
                return _keyedCollection.IndexOf(_keyedCollection[key]);
            }
            else {
                return -1;
            }
        }

        /// <summary>
        /// Determines whether this object contains the specified value.
        /// </summary>
        /// <param name="value">The value to locate in this object.</param>
        /// <returns>True if the value is found.  False otherwise.</returns>
        public bool ContainsValue(TValue value)
            => Values.Contains(value);

        /// <summary>
        /// Determines whether this object contains the specified value.
        /// </summary>
        /// <param name="value">The value to locate in this object.</param>
        /// <param name="comparer">The equality comparer used to locate the specified value in this object.</param>
        /// <returns>True if the value is found.  False otherwise.</returns>
        public bool ContainsValue(TValue value, IEqualityComparer<TValue> comparer)
            => Values.Contains(value, comparer);

        /// <summary>
        /// Determines whether this object contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in this object.</param>
        /// <returns>True if the key is found.  False otherwise.</returns>
        public bool ContainsKey(TKey key) => _keyedCollection.Contains(key);

        /// <summary>
        /// Returns the KeyValuePair at the index specified.
        /// </summary>
        /// <param name="index">The index of the KeyValuePair desired</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the index specified does not refer to a KeyValuePair in this object
        /// </exception>
        public KeyValuePair<TKey, TValue> GetItem(int index) {
            if (index < 0 || index >= _keyedCollection.Count) {
                throw new ArgumentException(StringUtils.Invariant($"The index was outside the bounds of the dictionary: {index}"));
            }
            return _keyedCollection[index];
        }

        /// <summary>
        /// Sets the value at the index specified.
        /// </summary>
        /// <param name="index">The index of the value desired</param>
        /// <param name="value">The value to set</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the index specified does not refer to a KeyValuePair in this object
        /// </exception>
        public void SetItem(int index, TValue value) {
            if (index < 0 || index >= _keyedCollection.Count) {
                throw new ArgumentException(StringUtils.Invariant($"The index is outside the bounds of the dictionary: {index}"));
            }
            var kvp = new KeyValuePair<TKey, TValue>(_keyedCollection[index].Key, value);
            _keyedCollection[index] = kvp;
        }

        /// <summary>
        /// Returns an enumerator that iterates through all the KeyValuePairs in this object.
        /// </summary>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _keyedCollection.GetEnumerator();

        /// <summary>
        /// Removes the key-value pair for the specified key.
        /// </summary>
        /// <param name="key">The key to remove from the dictionary.</param>
        /// <returns>True if the item specified existed and the removal was successful.  False otherwise.</returns>
        public bool Remove(TKey key) => _keyedCollection.Remove(key);

        /// <summary>
        /// Removes the key-value pair at the specified index.
        /// </summary>
        /// <param name="index">The index of the key-value pair to remove from the dictionary.</param>
        public void RemoveAt(int index) {
            if (index < 0 || index >= _keyedCollection.Count) {
                throw new ArgumentException(StringUtils.Invariant($"The index was outside the bounds of the dictionary: {index}"));
            }
            _keyedCollection.RemoveAt(index);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key associated with the value to get.</param>
        public TValue GetValue(TKey key) {
            if (_keyedCollection.Contains(key) == false) {
                throw new ArgumentException(StringUtils.Invariant($"The given key is not present in the dictionary: {key}"));
            }
            var kvp = _keyedCollection[key];
            return kvp.Value;
        }

        /// <summary>
        /// Sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key associated with the value to set.</param>
        /// <param name="value">The value to set.</param>
        public void SetValue(TKey key, TValue value) {
            var kvp = new KeyValuePair<TKey, TValue>(key, value);
            var idx = IndexOf(key);
            if (idx > -1) {
                _keyedCollection[idx] = kvp;
            }
            else {
                _keyedCollection.Add(kvp);
            }
        }

        /// <summary>
        /// Tries to get the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the desired element.</param>
        /// <param name="value">
        /// When this method returns, contains the value associated with the specified key if
        /// that key was found.  Otherwise it will contain the default value for parameter's type.
        /// This parameter should be provided uninitialized.
        /// </param>
        /// <returns>True if the value was found.  False otherwise.</returns>
        /// <remarks></remarks>
        public bool TryGetValue(TKey key, out TValue value) {

            if (key == null) {
                value = default;
                return false;
            }

            if (_keyedCollection.Contains(key)) {
                value = _keyedCollection[key].Value;
                return true;
            }
            else {
                value = default;
                return false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void SortKeys() => _keyedCollection.SortByKeys();

        /// <summary>
        ///
        /// </summary>
        /// <param name="comparer"></param>
        public void SortKeys(IComparer<TKey> comparer) => _keyedCollection.SortByKeys(comparer);

        /// <summary>
        ///
        /// </summary>
        /// <param name="comparison"></param>
        public void SortKeys(Comparison<TKey> comparison) => _keyedCollection.SortByKeys(comparison);

        /// <summary>
        ///
        /// </summary>
        public void SortValues() {
            var comparer = Comparer<TValue>.Default;
            SortValues(comparer);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="comparer"></param>
        public void SortValues(IComparer<TValue> comparer) => _keyedCollection.Sort((x, y) => comparer.Compare(x.Value, y.Value));

        /// <summary>
        ///
        /// </summary>
        /// <param name="comparison"></param>
        public void SortValues(Comparison<TValue> comparison) => _keyedCollection.Sort((x, y) => comparison(x.Value, y.Value));

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value) => Add(key, value);

        bool IDictionary<TKey, TValue>.ContainsKey(TKey key) => ContainsKey(key);

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => Keys;

        bool IDictionary<TKey, TValue>.Remove(TKey key) => Remove(key);

        bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value) => TryGetValue(key, out value);

        ICollection<TValue> IDictionary<TKey, TValue>.Values => Values;

        TValue IDictionary<TKey, TValue>.this[TKey key] {
            get => this[key];
            set => this[key] = value;
        }


        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => _keyedCollection.Add(item);

        void ICollection<KeyValuePair<TKey, TValue>>.Clear() => _keyedCollection.Clear();

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) => _keyedCollection.Contains(item);

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => _keyedCollection.CopyTo(array, arrayIndex);

        int ICollection<KeyValuePair<TKey, TValue>>.Count => _keyedCollection.Count;

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) => _keyedCollection.Remove(item);


        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => GetEnumerator();



        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IDictionaryEnumerator IOrderedDictionary.GetEnumerator() => new DictionaryEnumerator<TKey, TValue>(this);

        void IOrderedDictionary.Insert(int index, object key, object value) => Insert(index, (TKey)key, (TValue)value);

        void IOrderedDictionary.RemoveAt(int index) => RemoveAt(index);

        object IOrderedDictionary.this[int index] {
            get => this[index];
            set => this[index] = (TValue)value;
        }

        void IDictionary.Add(object key, object value) => Add((TKey)key, (TValue)value);

        void IDictionary.Clear() => Clear();

        bool IDictionary.Contains(object key) => _keyedCollection.Contains((TKey)key);

        IDictionaryEnumerator IDictionary.GetEnumerator() => new DictionaryEnumerator<TKey, TValue>(this);

        bool IDictionary.IsFixedSize => false;

        bool IDictionary.IsReadOnly => false;

        ICollection IDictionary.Keys
            => (ICollection)Keys;

        void IDictionary.Remove(object key) => Remove((TKey)key);

        ICollection IDictionary.Values
            => (ICollection)Values;

        object IDictionary.this[object key] {
            get => this[(TKey)key];
            set => this[(TKey)key] = (TValue)value;
        }


        void ICollection.CopyTo(Array array, int index) => ((ICollection)_keyedCollection).CopyTo(array, index);

        int ICollection.Count => ((ICollection)_keyedCollection).Count;

        bool ICollection.IsSynchronized => ((ICollection)_keyedCollection).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)_keyedCollection).SyncRoot;

    }
}