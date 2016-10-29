namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     attribute
    /// </summary>
    public class SymbolAttribute : SymbolTableEntryBase {

        /// <summary>
        ///     constant symbol name
        /// </summary>
        public override string SymbolName
            => Name.Name;

        /// <summary>
        ///     attribute name
        /// </summary>
        public SymbolName Name { get; set; }
    }
}