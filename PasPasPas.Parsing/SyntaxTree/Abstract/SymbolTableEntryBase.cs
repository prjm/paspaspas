using PasPasPas.Parsing.Parser;
using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     base class for symbol table entries
    /// </summary>
    public abstract class SymbolTableEntryBase : ISymbolTableEntry, ISyntaxPart {

        private string symbolName;

        /// <summary>
        ///     parent table
        /// </summary>
        public ISyntaxPart Parent { get; set; }

        /// <summary>
        ///     parts
        /// </summary>
        public virtual IReadOnlyList<ISyntaxPart> Parts
            => EmptyCollection<ISyntaxPart>.ReadOnlyInstance;

        /// <summary>
        ///     symbol name
        /// </summary>
        public string SymbolName
            => symbolName;

        public void DefineSymbolName(string newName) {
            symbolName = newName;
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
    }
}