namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     attribute
    /// </summary>
    public class SymbolAttributeItem {

        /// <summary>
        ///     attribute name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     symbol name
        /// </summary>
        public string SymbolName
            => Name?.CompleteName;
    }
}