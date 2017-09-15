using System.Collections;
using System.Collections.Generic;

namespace PasPasPas.Infrastructure.Utils {

    /// <summary>
    /// 
    /// </summary>
    public interface IOrderedDictionary : ICollection, IDictionary {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        new IDictionaryEnumerator GetEnumerator();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Insert(int index, object key, object value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        void RemoveAt(int index);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        object this[int index] { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface IOrderedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IOrderedDictionary {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        new TValue this[int index] { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        new TValue this[TKey key] { get; set; }

        /// <summary>
        /// 
        /// </summary>
        new int Count { get; }

        /// <summary>
        /// 
        /// </summary>
        new ICollection<TKey> Keys { get; }

        /// <summary>
        /// 
        /// </summary>
        new ICollection<TValue> Values { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        new void Add(TKey key, TValue value);

        /// <summary>
        /// 
        /// </summary>
        new void Clear();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Insert(int index, TKey key, TValue value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        int IndexOf(TKey key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool ContainsValue(TValue value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        bool ContainsValue(TValue value, IEqualityComparer<TValue> comparer);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        new bool ContainsKey(TKey key);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        new IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        new bool Remove(TKey key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        new void RemoveAt(int index);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        new bool TryGetValue(TKey key, out TValue value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TValue GetValue(TKey key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetValue(TKey key, TValue value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        KeyValuePair<TKey, TValue> GetItem(int index);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        void SetItem(int index, TValue value);
    }

}


