using System.Collections.Concurrent;
using System.Text;
using PasPasPas.Infrastructure.ObjectPooling;

namespace PasPasPas.Infrastructure.Environment {

    /// <summary>
    ///     basic string pool / lookup table
    /// </summary>
    public class StringPool : IEnvironmentItem {

        private ConcurrentDictionary<StringPoolEntry, StringPoolEntry> pool
            = new ConcurrentDictionary<StringPoolEntry, StringPoolEntry>();

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

                if (pool.TryGetValue(searchEntry, out var data))
                    return data.PoolItem;

                var newEntry = new StringPoolEntry(searchEntry);
                pool[newEntry] = newEntry;
                return newEntry.PoolItem;
            }
        }


    }
}
