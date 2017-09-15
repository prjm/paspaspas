using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PasPasPas.Infrastructure.Utils {

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class DictionaryEnumerator<TKey, TValue> : IDictionaryEnumerator, IDisposable {
        readonly IEnumerator<KeyValuePair<TKey, TValue>> _impl;
        /// <summary>
        /// 
        /// </summary>
        public void Dispose() => _impl.Dispose();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public DictionaryEnumerator(IDictionary<TKey, TValue> value) => _impl = value.GetEnumerator();

        /// <summary>
        /// 
        /// </summary>
        public void Reset() => _impl.Reset();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool MoveNext() => _impl.MoveNext();

        /// <summary>
        /// 
        /// </summary>
        public DictionaryEntry Entry {
            get {
                var pair = _impl.Current;
                return new DictionaryEntry(pair.Key, pair.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public object Key
            => _impl.Current.Key;

        /// <summary>
        /// 
        /// </summary>
        public object Value
            => _impl.Current.Value;

        /// <summary>
        /// 
        /// </summary>
        public object Current
            => Entry;
    }
}