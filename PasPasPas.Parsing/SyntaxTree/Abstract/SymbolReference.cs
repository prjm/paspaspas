namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     reference to a symbol
    /// </summary>
    public class SymbolReference : AbstractSyntaxPart, IExpression {

        /// <summary>
        ///     identifier name
        /// </summary>
        public SymbolName Name { get; internal set; }
    }
}
