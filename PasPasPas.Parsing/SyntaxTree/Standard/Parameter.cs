namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     parameter for a method call
    /// </summary>
    public class Parameter : SyntaxPartBase {

        /// <summary>
        ///     parameter expression
        /// </summary>
        public FormattedExpression Expression { get; internal set; }

        /// <summary>
        ///     parameter name
        /// </summary>
        public Identifier ParameterName { get; internal set; }
    }
}
