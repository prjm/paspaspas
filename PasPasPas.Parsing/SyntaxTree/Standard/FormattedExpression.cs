namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     formatted expression
    /// </summary>
    public class FormattedExpression : SyntaxPartBase {

        /// <summary>
        ///     decimals subexpression
        /// </summary>
        public Expression Decimals { get; set; }

        /// <summary>
        ///     width subexpression
        /// </summary>
        public Expression Width { get; set; }

        /// <summary>
        ///     base expression
        /// </summary>
        public Expression Expression { get; set; }


    }
}