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
        protected IDictionary data;

        /// <summary>
        ///     count number of objects
        /// </summary>
        public int Count
            => data.Count;

    }


    /// <summary>
    ///     simplified way to add a lookup table to a function
    /// </summary>
    /// <typeparam name="Key">key type</typeparam>
    /// <typeparam name="Value">value type</typeparam>
    public class LookupTable<Key, Value> : LookupTable {

        private readonly Func<Key, Value> lookupFunction;
        private readonly ConcurrentDictionary<Key, Value> lockedTable;
        private readonly Dictionary<Key, Value> standardTable;
        private readonly bool withLocks;

        /// <summary>
        ///     create a new lookup table
        /// </summary>
        /// <param name="function">lookup function</param>
        /// <param name="withLocking">if <c>true</c> </param>
        /// <param name="keyComparer"></param>
        /// <param name="initialCapacity"></param>
        public LookupTable(Func<Key, Value> function, bool withLocking = false, IEqualityComparer<Key> keyComparer = null, int initialCapacity = 8) {
            lookupFunction = function;

            if (withLocking) {
                withLocks = true;
                lockedTable = new ConcurrentDictionary<Key, Value>(1, initialCapacity, keyComparer ?? EqualityComparer<Key>.Default);
                data = lockedTable;
            }
            else {
                withLocks = false;
                standardTable = new Dictionary<Key, Value>(initialCapacity, keyComparer ?? EqualityComparer<Key>.Default);
                data = standardTable;
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
        public Value GetValue(Key key) {
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
