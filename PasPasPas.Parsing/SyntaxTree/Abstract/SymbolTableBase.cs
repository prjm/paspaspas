using System;
using System.Collections;
using System.Collections.Generic;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     base class for symbol tables
    /// </summary>
    /// <typeparam name="T">symbol type</typeparam>
    public abstract class SymbolTableBase<T> :
        AbstractSyntaxPartBase,
        ISymbolTable<T>,
        IReadOnlyList<T>
        where T : class, ISymbolTableEntry, ISyntaxPart {

        /// <summary>
        ///     symbols
        /// </summary>
        private OrderedDictionary<string, T> symbols = null;

        /// <summary>
        ///     create symbol table
        /// </summary>
        /// <returns>symbol table</returns>
        protected virtual OrderedDictionary<string, T> CreateSymbols()
            => new OrderedDictionary<string, T>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        ///     add a symbol table entry
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="logSource"></param>
        /// <returns></returns>
        public bool Add(T entry, LogSource logSource) {
            var name = entry.SymbolName;

            if (symbols == null)
                symbols = CreateSymbols();

            if (!Contains(name)) {
                symbols.Add(name, entry);
                return true;
            }

            if (!HasDuplicateReplacement) {
                LogDuplicateSymbolError(entry, logSource);
                return false;
            }

            var replacement = MergeDuplicates(this[name], entry);

            if (replacement == null)
                return false;

            symbols[name] = replacement;
            return true;
        }

        /// <summary>
        ///     merge duplicates
        /// </summary>
        /// <param name="existingEntry">existing entry</param>
        /// <param name="entry">new entry</param>
        /// <returns></returns>
        protected virtual T MergeDuplicates(T existingEntry, T entry)
            => default;

        /// <summary>
        ///     check if duplicates are allowd
        /// </summary>
        protected virtual bool HasDuplicateReplacement
            => false;

        /// <summary>
        ///     test if a given key exits
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key)
            => symbols == null ? false : symbols.ContainsKey(key);

        /// <summary>
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                if (symbols != null)
                    foreach (var value in symbols.Values)
                        yield return value;
            }
        }

        /// <summary>
        ///     number of symbols
        /// </summary>
        public int Count
            => symbols == null ? 0 : symbols.Count;

        /// <summary>
        ///     acces item by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
            => symbols[index];

        /// <summary>
        ///     log duplicate symbol error
        /// </summary>
        /// <param name="newDuplicate"></param>
        /// <param name="logSource"></param>
        protected virtual void LogDuplicateSymbolError(T newDuplicate, LogSource logSource) {
            //...
        }

        /// <summary>
        ///     get an enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
            => symbols != null ?
                symbols.Values.GetEnumerator() :
                new EmptyEnumerable<T>().GetEnumerator();

        /// <summary>
        ///     simple wrapper
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
            => symbols != null ?
                symbols.Values.GetEnumerator() :
                new EmptyEnumerable<T>().GetEnumerator();

        /// <summary>
        ///     remove an entry
        /// </summary>
        /// <param name="result"></param>
        public void Remove(ISyntaxPart result) {
            if (result is T entry)
                Remove(entry);
        }

        /// <summary>
        ///     remove an entry
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public bool Remove(T entry)
            => symbols.Remove(entry.SymbolName);

        /// <summary>
        ///     get a symbol by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T this[string key]
            => symbols[key];

    }
}
