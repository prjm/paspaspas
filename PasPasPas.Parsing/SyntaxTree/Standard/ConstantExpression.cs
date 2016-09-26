namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     constant expression
    /// </summary>
    public class ConstantExpression : SyntaxPartBase {

        /// <summary>
        ///     value of the expression
        /// </summary>
        public Expression Value { get; set; }

    }
}