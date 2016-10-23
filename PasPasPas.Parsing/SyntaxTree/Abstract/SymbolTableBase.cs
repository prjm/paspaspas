using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using PasPasPas.Parsing.Parser;
using PasPasPas.Infrastructure.Log;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     base class for symbolic tables
    /// </summary>
    /// <typeparam name="T">symbol type</typeparam>
    public abstract class SymbolTableBase<T> : ISymbolTable<T>, IReadOnlyList<ISyntaxPart>
        where T : class, ISymbolTableEntry {

        /// <summary>
        ///     symbols
        /// </summary>
        private Lazy<OrderedDictionary> symbols;

        /// <summary>
        ///     create a new symbol table
        /// </summary>
        protected SymbolTableBase() {
            symbols = new Lazy<OrderedDictionary>(() => CreateSymbols());
        }

        /// <summary>
        ///     parent symbol table
        /// </summary>
        public ISyntaxPart Parent { get; set; }

        /// <summary>
        ///     create symbols
        /// </summary>
        /// <returns></returns>
        protected virtual OrderedDictionary CreateSymbols()
            => new OrderedDictionary(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        ///     add a symbol table entry
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="logSource"></param>
        /// <returns></returns>
        public bool Add(T entry, LogSource logSource) {
            var name = entry.SymbolName;
            var symbolTable = symbols.Value;
            if (!symbolTable.Contains(name)) {
                symbolTable.Add(name, entry);
                return true;
            }

            if (!HasDuplicateReplacement) {
                LogDuplicateSymbolError(entry, logSource);
                return false;
            }

            T replacement = MergeDuplicates(this[name], entry);

            if (replacement == null)
                return false;

            symbolTable[name] = replacement;
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
        ///     parts
        /// </summary>
        public IReadOnlyList<ISyntaxPart> Parts
            => this;

        /// <summary>
        ///     number of symbols
        /// </summary>
        public int Count
            => symbols.IsValueCreated ? symbols.Value.Count : 0;

        /// <summary>
        ///     acces item by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ISyntaxPart this[int index]
            => ((ISyntaxPart)symbols.Value[index]);

        /// <summary>
        ///     log duplicate symbol error
        /// </summary>
        /// <param name="newDuplicate"></param>
        /// <param name="logSource"></param>
        protected virtual void LogDuplicateSymbolError(T newDuplicate, LogSource logSource) {
            //...
        }

        /// <summary>
        ///     accept a visitors
        /// </summary>
        /// <typeparam name="TVisitorType"></typeparam>
        /// <param name="visitor"></param>
        /// <param name="visitorParameter"></param>
        /// <returns></returns>
        public bool Accept<TVisitorType>(ISyntaxPartVisitor<TVisitorType> visitor, TVisitorType visitorParameter) {
            if (!visitor.BeginVisit(this, visitorParameter))
                return false;

            var result = true;

            foreach (var part in Parts)
                result = result && part.Accept(visitor, visitorParameter);

            if (!visitor.EndVisit(this, visitorParameter))
                return false;

            return result;
        }

        /// <summary>
        ///     get an enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<ISyntaxPart> GetEnumerator() {
            if (symbols.IsValueCreated) {
                return symbols.Value.Values.Cast<ISyntaxPart>().GetEnumerator();
            }
            else {
                return EmptyCollection<ISyntaxPart>.Instance.GetEnumerator();
            }
        }

        /// <summary>
        ///     simple wrapper
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() {
            if (symbols.IsValueCreated) {
                return symbols.Value.Values.GetEnumerator();
            }
            else {
                return EmptyCollection<object>.Instance.GetEnumerator();
            }
        }

        /// <summary>
        ///     remove an entry
        /// </summary>
        /// <param name="result"></param>
        public void Remove(ISyntaxPart result) {
            var entry = result as T;
            if (entry != null)
                Remove(entry);
        }

        /// <summary>
        ///     remove an entry
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public bool Remove(T entry) {
            var name = entry.SymbolName;

            if (!symbols.Value.Contains(name))
                return false;
            else {
                symbols.Value.Remove(name);
                return true;
            }
        }

        /// <summary>
        ///     get a symbol by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T this[string key]
            => (T)symbols.Value[key];

    }
}
