using System;
using System.Diagnostics;
using System.Text;
using PasPasPas.Globals.Environment;
using PasPasPas.Infrastructure.ObjectPooling;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Infrastructure.Environment {

    /// <summary>
    ///     basic string pool / lookup table
    /// </summary>
    public class StringPool : IStringPool {

        private readonly HashedStrings pool
            = new HashedStrings();

        private readonly object lockObject = new object();

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
            lock (lockObject)
                pool.Add(value);
        }

        /// <summary>
        ///     pool a string by a char array
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string PoolString(StringBuilder item) {
            lock (lockObject) {
                if (pool.TryGetValue(item, out var data)) {
                    LogHistogram(data);
                    return data;
                }

                var newEntry = pool.Add(item);
                return newEntry;
            }
        }

        [Conditional("DEBUG")]
        private static void LogHistogram(string data) {
            if (Histograms.Enable)
                Histograms.Value(HistogramKeys.StringPoolValues, data);
        }

        /// <summary>
        ///     test if a string is already included
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ContainsString(string value) {
            lock (lockObject)
                return pool.Contains(value);
        }

        /// <summary>
        ///     pool an utf16 encoded string
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string PoolString(Span<byte> item) {
            lock (lockObject) {
                if (pool.TryGetValue(item, out var data)) {
                    LogHistogram(data);
                    return data;
                }

                var newEntry = pool.Add(item);
                return newEntry;
            }
        }

    }
}
