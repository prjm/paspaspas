using System.Collections.Generic;
using System.Text;
using PasPasPas.Infrastructure.ObjectPooling;

namespace PasPasPas.Infrastructure.Environment {

    /// <summary>
    ///     basic string pool / lookup table
    /// </summary>
    public class StringPool : IEnvironmentItem {

        private IDictionary<StringPoolEntry, StringPoolEntry> pool
            = new Dictionary<StringPoolEntry, StringPoolEntry>();

        private readonly object lockObject = new object();

        private const int MaxStringLength
            = 300;

        /// <summary>
        ///     number of pooled strings
        /// </summary>
        public int Count
            => pool.Count;

        /// <summary>
        ///     clear the buffer pool
        /// </summary>
        public void Clear()
            => pool.Clear();

        /// <summary>
        ///     add a string value manually to the pool
        /// </summary>
        /// <param name="value">string to add</param>
        public void AddString(string value) {
            var newEntry = new StringPoolEntry();
            newEntry.Initialize(value);

            lock (lockObject)
                pool[newEntry] = newEntry;
        }

        /// <summary>
        ///     string pool entries
        /// </summary>
        public StringPoolEntries Entries { get; }
            = new StringPoolEntries();

        /// <summary>
        ///     pool a string by a char array
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string PoolString(StringBuilder value) {

            if (value.Length > MaxStringLength)
                return value.ToString();

            using (var poolItem = Entries.Borrow(out var searchEntry)) {
                searchEntry.Initialize(value);

                lock (lockObject) {
                    if (pool.TryGetValue(searchEntry, out var data))
                        return data.PoolItem;
                }

                var newEntry = new StringPoolEntry(searchEntry);

                lock (lockObject)
                    pool[newEntry] = newEntry;

                return newEntry.PoolItem;
            }
        }


    }
}
