namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     attribute
    /// </summary>
    public class SymbolAttribute : SymbolTableEntryBase {

        /// <summary>
        ///     constant symbol name
        /// </summary>
        protected override string InternalSymbolName
            => Name?.CompleteName;

        /// <summary>
        ///     attribute name
        /// </summary>
        public SymbolName Name { get; set; }
    }
}