namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     a declared symbol
    /// </summary>
    public class DeclaredSymbol : SymbolTableEntryBase {

        /// <summary>
        ///     constant symbol name
        /// </summary>
        protected override string InternalSymbolName
            => Name?.CompleteName;

        /// <summary>
        ///     name of the constant
        /// </summary>
        public SymbolName Name { get; set; }

    }
}