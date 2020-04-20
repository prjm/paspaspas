using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PasPasPas.Infrastructure.ObjectPooling;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.Tokenizer;

namespace PasPasPas.Parsing.SyntaxTree.Utils {

    /// <summary>
    ///     pooled terminal symbol
    /// </summary>
    public class PooledTerminal {

        /// <summary>
        ///     create an empty pooled terminal
        /// </summary>
        public PooledTerminal() { }

        /// <summary>
        ///     terminal symbol
        /// </summary>
        public Terminal Terminal { get; private set; }

        /// <summary>
        ///     tokens
        /// </summary>
        public TokenSequence TokenSequence { get; private set; }

        /// <summary>
        ///     hash value
        /// </summary>
        public int ComputedHashValue { get; private set; }

        /// <summary>
        ///     create a new terminal pool entry from a search entry
        /// </summary>
        /// <param name="oldEntry">existing entry</param>
        public PooledTerminal(PooledTerminal oldEntry) {
            Terminal = new Terminal(oldEntry.TokenSequence);
            TokenSequence = null;
            ComputedHashValue = oldEntry.ComputedHashValue;
        }

        /// <summary>
        ///     initialize this terminal
        /// </summary>
        /// <param name="tokens"></param>
        public void Initialize(TokenSequence tokens) {
            TokenSequence = tokens;
            ComputedHashValue = ComputeHashValue(tokens);
        }

        /// <summary>
        ///     compute a hash value
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        private static int ComputeHashValue(TokenSequence tokens) {
            var result = 17;

            unchecked {
                result = result * 31 + tokens.Value.GetHashCode();

                if (tokens.Prefix != default)
                    foreach (var prefix in tokens.Prefix)
                        result = result * 31 + prefix.GetHashCode();

                if (tokens.Suffix != default)
                    foreach (var suffix in tokens.Suffix)
                        result = result * 31 + suffix.GetHashCode();
            }

            return result;
        }

        /// <summary>
        ///     clear this entry
        /// </summary>
        internal void Clear() {
            ComputedHashValue = 0;
            TokenSequence = default;
            Terminal = default;
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
            if (!(obj is PooledTerminal entry))
                return false;

            if (Terminal != null && entry.Terminal != null)
                return Terminal.Equals(entry.Terminal);
            else if (entry.TokenSequence != null && Terminal != null)
                return EqualsTokens(entry.TokenSequence, Terminal);
            else if (entry.Terminal != null && TokenSequence != null)
                return EqualsTokens(TokenSequence, entry.Terminal);
            else if (entry.TokenSequence != null && TokenSequence != null)
                return EqualsTokens(entry.TokenSequence, TokenSequence);

            return false;
        }

        private static bool EqualsTokens(TokenSequence other, Terminal terminal) {
            if (!terminal.Token.Equals(other.Value))
                return false;

            if (other.Prefix == null && terminal.Prefix != null || other.Prefix != null && terminal.Prefix == null)
                return false;

            if (other.Suffix == null && terminal.Suffix != null || other.Suffix != null && terminal.Suffix == null)
                return false;

            if (other.Prefix != null) {
                if (other.Prefix.Length != terminal.Prefix.Length)
                    return false;
                for (var i = 0; i < terminal.Prefix.Length; i++)
                    if (!terminal.Prefix[i].Equals(other.Prefix[i]))
                        return false;
            }

            if (other.Suffix != null) {
                if (other.Suffix.Length != terminal.Suffix.Length)
                    return false;
                for (var i = 0; i < terminal.Suffix.Length; i++)
                    if (!terminal.Suffix[i].Equals(other.Suffix[i]))
                        return false;

            }

            return true;
        }

        private static bool EqualsTokens(TokenSequence other, TokenSequence terminal) {
            if (!terminal.Value.Equals(other.Value))
                return false;

            if (other.Prefix == null && terminal.Prefix != null || other.Prefix != null && terminal.Prefix == null)
                return false;

            if (other.Suffix == null && terminal.Suffix != null || other.Suffix != null && terminal.Suffix == null)
                return false;

            if (other.Prefix != null) {
                if (other.Prefix.Length != terminal.Prefix.Length)
                    return false;
                for (var i = 0; i < terminal.Prefix.Length; i++)
                    if (!terminal.Prefix[i].Equals(other.Prefix[i]))
                        return false;
            }

            if (other.Suffix != null) {
                if (other.Suffix.Length != terminal.Suffix.Length)
                    return false;
                for (var i = 0; i < terminal.Suffix.Length; i++)
                    if (!terminal.Suffix[i].Equals(other.Suffix[i]))
                        return false;

            }

            return true;
        }
    }

    /// <summary>
    ///     pooled terminal entries
    /// </summary>
    public class PooledTerminalEntries : ObjectPool<PooledTerminal> {

        /// <summary>
        ///     prepare a string pool item
        /// </summary>
        /// <param name="result"></param>
        protected override void Prepare(PooledTerminal result)
            => result.Clear();

    }

    /// <summary>
    ///     object pooling for terminal values
    /// </summary>
    public class Terminals {

        private readonly HashSet<PooledTerminal> pool
            = new HashSet<PooledTerminal>();

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
        ///     string pool entries
        /// </summary>
        public PooledTerminalEntries Entries { get; }
            = new PooledTerminalEntries();

        /// <summary>
        ///     get a terminal token
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public Terminal GetTerminal(TokenSequence tokens) {
            using (var poolItem = Entries.Borrow(out var searchEntry)) {
                searchEntry.Initialize(tokens);

                lock (lockObject) {
                    if (pool.TryGetValue(searchEntry, out var data)) {
                        LogHistogram(data);
                        return data.Terminal;
                    }
                }

                var newEntry = new PooledTerminal(searchEntry);

                lock (lockObject)
                    pool.Add(newEntry);

                return newEntry.Terminal;
            }
        }

        [Conditional("DEBUG")]
        private static void LogHistogram(PooledTerminal data) {
            if (Histograms.Enable) {
                var value = string.Empty;
                if (data.Terminal.Prefix != null)
                    value += string.Join(string.Empty, data.Terminal.Prefix.Select(t => t.Value));
                value += data.Terminal.Value;
                if (data.Terminal.Suffix != null)
                    value += string.Join(string.Empty, data.Terminal.Suffix.Select(t => t.Value));
                Histograms.Value(HistogramKeys.TerminalPoolValues, value);
            }
        }
    }
}
