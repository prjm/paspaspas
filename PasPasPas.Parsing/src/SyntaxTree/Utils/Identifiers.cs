using System.Collections.Generic;
using System.Diagnostics;
using PasPasPas.Infrastructure.ObjectPooling;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Standard;

namespace PasPasPas.Parsing.SyntaxTree.Utils {

    /// <summary>
    ///     pooled identifier
    /// </summary>
    public class PooledIdentifier {

        /// <summary>
        ///     create a new pooled identifier
        /// </summary>
        public PooledIdentifier() { }

        /// <summary>
        ///     create a new pooled identifier
        /// </summary>
        /// <param name="oldEntry"></param>
        public PooledIdentifier(PooledIdentifier oldEntry) {
            Ident = new IdentifierSymbol(oldEntry.Terminal);
            Terminal = default;
            ComputedHashValue = oldEntry.ComputedHashValue;
        }

        /// <summary>
        ///     terminal symbol
        /// </summary>
        public Terminal Terminal { get; private set; }

        /// <summary>
        ///     identifier
        /// </summary>
        public IdentifierSymbol Ident { get; private set; }

        /// <summary>
        ///     hash value
        /// </summary>
        public int ComputedHashValue { get; private set; }

        /// <summary>
        ///     clear this entry
        /// </summary>
        public void Clear() {
            ComputedHashValue = 0;
            Ident = default;
            Terminal = default;
        }

        /// <summary>
        ///     compute a hash value
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        private static int ComputeHashValue(Terminal tokens)
            => tokens.GetHashCode();

        /// <summary>
        ///     initialize this pool entry
        /// </summary>
        /// <param name="terminal"></param>
        public void Initialize(Terminal terminal) {
            Terminal = terminal;
            ComputedHashValue = ComputeHashValue(terminal);
        }

        /// <summary>
        ///     get a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => ComputedHashValue;

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj">object to compare</param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (!(obj is PooledIdentifier entry))
                return false;

            if (Terminal != null && entry.Terminal != null)
                return Terminal.Equals(entry.Terminal);
            else if (entry.Ident != null && Terminal != null)
                return entry.Ident.Symbol.Equals(Terminal);
            else if (entry.Terminal != null && Ident != null)
                return entry.Terminal.Equals(Ident.Symbol);
            else if (entry.Ident != null && Ident != null)
                return entry.Ident.Symbol.Equals(Ident.Symbol);

            return false;
        }


    }


    /// <summary>
    ///     pooled identifier entries
    /// </summary>
    public class PooledIdentifierEntries : ObjectPool<PooledIdentifier> {

        /// <summary>
        ///     create a new identifier pool
        /// </summary>
        public PooledIdentifierEntries() { }

        /// <summary>
        ///     prepare a string pool item
        /// </summary>
        /// <param name="result"></param>
        protected override void Prepare(PooledIdentifier result)
            => result.Clear();

    }

    /// <summary>
    ///     object pooling for terminal values
    /// </summary>
    public class Identifiers {

        private readonly HashSet<PooledIdentifier> pool
            = new HashSet<PooledIdentifier>();

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
        ///     identifier pool entries
        /// </summary>
        public PooledIdentifierEntries Entries { get; }
            = new PooledIdentifierEntries();

        /// <summary>
        ///     get an identifier symbol
        /// </summary>
        /// <param name="terminal"></param>
        /// <returns></returns>
        public IdentifierSymbol GetIdentifier(Terminal terminal) {
            using (var poolItem = Entries.Borrow(out var searchEntry)) {
                searchEntry.Initialize(terminal);

                lock (lockObject) {
                    if (pool.TryGetValue(searchEntry, out var data)) {
                        LogHistogram(data);
                        return data.Ident;
                    }
                }

                var newEntry = new PooledIdentifier(searchEntry);

                lock (lockObject)
                    pool.Add(newEntry);

                return newEntry.Ident;
            }

        }

        [Conditional("DEBUG")]
        private static void LogHistogram(PooledIdentifier data) {
            if (Histograms.Enable)
                Histograms.Value(HistogramKeys.IdentifierPoolValues, data.Ident.Value);
        }

    }

}
