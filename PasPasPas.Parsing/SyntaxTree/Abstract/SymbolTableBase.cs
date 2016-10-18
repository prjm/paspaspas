using System;
using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     base class for symbolic tables
    /// </summary>
    /// <typeparam name="T">symbol type</typeparam>
    public abstract class SymbolTableBase<T> : ISymbolTable<T> where T : ISymbolTableEntry {

        /// <summary>
        ///     symbols
        /// </summary>
        private Lazy<IDictionary<string, T>> symbols;
        private IDictionary<string, T> Symbols
            => symbols.Value;

        /// <summary>
        ///     create a new symbol table
        /// </summary>
        protected SymbolTableBase() {
            symbols = new Lazy<IDictionary<string, T>>(() => CreateSymbols());
        }

        /// <summary>
        ///     create symbols
        /// </summary>
        /// <returns></returns>
        protected virtual IDictionary<string, T> CreateSymbols()
            => new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        ///     add a symbol table entry
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public bool Add(T entry) {
            if (!Symbols.ContainsKey(entry.SymbolName)) {
                Symbols.Add(entry.SymbolName, entry);
                return true;
            }

            if (!HasDuplicateReplacement) {
                LogDuplicateSymbolError(entry);
                return false;
            }

            T replacement = MergeDuplicates(Symbols[entry.SymbolName], entry);

            if (replacement == null)
                return false;

            Symbols[entry.SymbolName] = replacement;
            return true;
        }

        /// <summary>
        ///     merge duplicates
        /// </summary>
        /// <param name="t">existing entry</param>
        /// <param name="entry">new entry</param>
        /// <returns></returns>
        protected virtual T MergeDuplicates(T t, T entry) => default(T);

        /// <summary>
        ///     check if duplicates are allowd
        /// </summary>
        protected virtual bool HasDuplicateReplacement
            => false;

        /// <summary>
        ///     log duplicate symbol error
        /// </summary>
        /// <param name="newDuplicate"></param>
        protected virtual void LogDuplicateSymbolError(T newDuplicate) {
            //...
        }
    }
}
