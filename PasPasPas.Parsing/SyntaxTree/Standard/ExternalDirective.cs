namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     external directive
    /// </summary>
    public class ExternalDirective : SyntaxPartBase {

        /// <summary>
        ///     external expression
        /// </summary>
        public ConstantExpression ExternalExpression { get; set; }

        /// <summary>
        ///     kind
        /// </summary>
        public int Kind { get; set; }
    }
}
