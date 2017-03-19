namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     reference to a symbol
    /// </summary>
    public class SymbolReference : AbstractSyntaxPart, IExpression, ILabelTarget {

        /// <summary>
        ///     identifier name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     referencing label
        /// </summary>
        public SymbolName LabelName { get; set; }
    }
}
