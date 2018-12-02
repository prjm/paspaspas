using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.ObjectPooling;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Utils {

    /// <summary>
    ///     pooled token array
    /// </summary>
    public class PooledTokenArray {

        /// <summary>
        ///     empty entry
        /// </summary>
        public PooledTokenArray() { }

        private static ImmutableArray<Token> BuildArray(Queue<Token> tokens) => ListPools.GetFixedArray<Token>(tokens);

        /// <summary>
        ///     create a new pooled token array
        /// </summary>
        /// <param name="searchEntry"></param>
        public PooledTokenArray(PooledTokenArray searchEntry) {
            TokenArray = BuildArray(searchEntry.TokenQueue);
            TokenQueue = default;
            ComputedHashCode = searchEntry.ComputedHashCode;
        }

        /// <summary>
        ///     tokens
        /// </summary>
        public ImmutableArray<Token> TokenArray { get; private set; }

        /// <summary>
        ///     tokens
        /// </summary>
        public Queue<Token> TokenQueue { get; private set; }

        /// <summary>
        ///     compute a hash code
        /// </summary>
        public int ComputedHashCode { get; private set; }

        /// <summary>
        ///     initialize this pool entry
        /// </summary>
        /// <param name="tokens"></param>
        public void Initialize(Queue<Token> tokens) {
            TokenQueue = tokens;
            TokenArray = default;
            ComputedHashCode = ComputeHashCode(tokens);
        }

        private static int ComputeHashCode(Queue<Token> tokens) {
            var result = 17;

            unchecked {
                foreach (var token in tokens)
                    result = result * 31 + token.GetHashCode();
            }

            return result;
        }

        /// <summary>
        ///     get a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => ComputedHashCode;


        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj">object to compare</param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (!(obj is PooledTokenArray entry))
                return false;

            if (TokenArray != null && entry.TokenArray != null)
                return EqualsTokens(TokenArray, entry.TokenArray);
            else if (entry.TokenQueue != null && TokenArray != null)
                return EqualsTokens(entry.TokenQueue, TokenArray);
            else if (entry.TokenArray != null && TokenQueue != null)
                return EqualsTokens(TokenQueue, entry.TokenArray);
            else if (entry.TokenQueue != null && entry.TokenQueue != null)
                return EqualsTokens(entry.TokenQueue, TokenQueue);

            return false;
        }

        private static bool EqualsTokens(Queue<Token> tokenQueue1, Queue<Token> tokenQueue2) {
            if (tokenQueue1.Count != tokenQueue2.Count)
                return false;

            if (tokenQueue1.Count == 0)
                return true;

            var enum1 = tokenQueue1.GetEnumerator();
            var enum2 = tokenQueue1.GetEnumerator();

            while (enum1.MoveNext() && enum2.MoveNext())
                if (!enum1.Current.Equals(enum2.Current))
                    return false;

            return true;
        }

        private static bool EqualsTokens(Queue<Token> tokenQueue, ImmutableArray<Token> tokenArray) {
            if (tokenQueue.Count != tokenArray.Length)
                return false;

            if (tokenQueue.Count == 0)
                return true;

            var i = 0;
            foreach (var token in tokenQueue) {
                if (!token.Equals(tokenArray[i]))
                    return false;
                i++;
            }

            return true;
        }

        private static bool EqualsTokens(ImmutableArray<Token> tokenArray1, ImmutableArray<Token> tokenArray2) {
            if (tokenArray1.Length != tokenArray2.Length)
                return false;

            if (tokenArray1.Length == 0)
                return true;

            for (var i = 0; i < tokenArray1.Length; i++)
                if (!tokenArray1[i].Equals(tokenArray2[i]))
                    return false;

            return true;
        }

        /// <summary>
        ///     clear this entry
        /// </summary>
        internal void Clear() {
            ComputedHashCode = default;
            TokenArray = default;
            TokenQueue = default;
        }
    }

    /// <summary>
    ///     pooled terminal entries
    /// </summary>
    public class PooledTokenArrays : ObjectPool<PooledTokenArray> {

        /// <summary>
        ///     create a new token array pool
        /// </summary>
        public PooledTokenArrays() {
        }

        /// <summary>
        ///     prepare a string pool item
        /// </summary>
        /// <param name="entry"></param>
        protected override void Prepare(PooledTokenArray entry)
            => entry.Clear();

    }



    /// <summary>
    ///     object pooling for token arrays
    /// </summary>
    public class TokenArrays : IEnvironmentItem {

        private readonly HashSet<PooledTokenArray> pool
            = new HashSet<PooledTokenArray>();

        private readonly object lockObject = new object();

        /// <summary>
        ///     number of pooled token arrays
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
        public PooledTokenArrays Entries { get; }
            = new PooledTokenArrays();

        /// <summary>
        ///     get a terminal token
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public ImmutableArray<Token> GetTokenArray(Queue<Token> tokens) {
            using (var poolItem = Entries.Borrow(out var searchEntry)) {
                searchEntry.Initialize(tokens);

                lock (lockObject) {
                    if (pool.TryGetValue(searchEntry, out var data)) {
                        LogHistogram(data);
                        tokens.Clear();
                        return data.TokenArray;
                    }
                }

                var newEntry = new PooledTokenArray(searchEntry);

                lock (lockObject)
                    pool.Add(newEntry);

                return newEntry.TokenArray;
            }
        }

        [Conditional("DEBUG")]
        private static void LogHistogram(PooledTokenArray data) {
            if (Histograms.Enable) {
                var value = string.Join(string.Empty, data.TokenArray.Select(t => t.Value));
                Histograms.Value(HistogramKeys.TokenArrayValues, value);
            }
        }
    }
}
