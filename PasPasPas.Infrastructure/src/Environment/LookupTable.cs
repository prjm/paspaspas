using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace PasPasPas.Infrastructure.Environment {

    /// <summary>
    ///     non-generic base class for the lookup table
    /// </summary>
    public class LookupTable {

        /// <summary>
        ///     table data
        /// </summary>
        private IDictionary data;

        /// <summary>
        ///     set the data value
        /// </summary>
        /// <param name="dict"></param>
        protected void SetData(IDictionary dict)
            => data = dict;

        /// <summary>
        ///     count number of objects
        /// </summary>
        public int Count
            => data.Count;

    }


    /// <summary>
    ///     simplified way to add a lookup table to a function
    /// </summary>
    /// <typeparam name="TKey">key type</typeparam>
    /// <typeparam name="TValue">value type</typeparam>
    public class LookupTable<TKey, TValue> : LookupTable {

        private readonly Func<TKey, TValue> lookupFunction;
        private readonly ConcurrentDictionary<TKey, TValue> lockedTable;
        private readonly Dictionary<TKey, TValue> standardTable;
        private readonly bool withLocks;

        /// <summary>
        ///     create a new lookup table
        /// </summary>
        /// <param name="function">lookup function</param>
        /// <param name="withLocking">if <c>true</c> </param>
        /// <param name="keyComparer"></param>
        /// <param name="initialCapacity"></param>
        public LookupTable(Func<TKey, TValue> function, bool withLocking = false, IEqualityComparer<TKey> keyComparer = null, int initialCapacity = 8) {
            lookupFunction = function;

            if (withLocking) {
                withLocks = true;
                lockedTable = new ConcurrentDictionary<TKey, TValue>(1, initialCapacity, keyComparer ?? EqualityComparer<TKey>.Default);
                SetData(lockedTable);
            }
            else {
                withLocks = false;
                standardTable = new Dictionary<TKey, TValue>(initialCapacity, keyComparer ?? EqualityComparer<TKey>.Default);
                SetData(standardTable);
            }
        }

        /// <summary>
        ///     clear the lookup table
        /// </summary>
        public void Clear() {
            if (withLocks)
                lockedTable.Clear();
            else
                standardTable.Clear();
        }

        /// <summary>
        ///     get a value by table key
        /// </summary>
        /// <param name="key">key value</param>
        /// <returns>retrieved value</returns>
        public TValue GetValue(TKey key) {
            if (withLocks) {
                return lockedTable.GetOrAdd(key, lookupFunction);
            }
            else {
                if (!standardTable.TryGetValue(key, out var result)) {
                    result = lookupFunction(key);
                    standardTable.Add(key, result);
                }
                return result;
            }

        }

    }
}
