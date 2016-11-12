using System;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     generic type
    /// </summary>
    public class GenericType : SymbolTableBase<GenericConstraint>, ISymbolTableEntry {

        /// <summary>
        ///     type name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     symbol name
        /// </summary>
        public string SymbolName
            => Name?.CompleteName;

    }
}