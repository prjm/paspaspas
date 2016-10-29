﻿namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     a declared symbol
    /// </summary>
    public class DeclaredSymbol : SymbolTableEntryBase {

        /// <summary>
        ///     constant symbol name
        /// </summary>
        public override string SymbolName
            => Name.Name;

        /// <summary>
        ///     name of the constant
        /// </summary>
        public SymbolName Name { get; set; }

    }
}