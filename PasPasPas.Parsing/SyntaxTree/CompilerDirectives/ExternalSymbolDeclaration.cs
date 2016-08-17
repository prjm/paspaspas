namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     external symbol declaration
    /// </summary>
    public class ExternalSymbolDeclaration : SyntaxPartBase {

        /// <summary>
        ///     identifier
        /// </summary>
        public string IdentifierName { get; internal set; }

        /// <summary>
        ///     symbol
        /// </summary>
        public string SymbolName { get; internal set; }

        /// <summary>
        ///     union
        /// </summary>
        public string UnionName { get; internal set; }
    }
}
