namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     binding directive
    /// </summary>
    public class BindingDirective : SyntaxPartBase {

        /// <summary>
        ///     directive kind
        /// </summary>
        public int Kind { get; internal set; }

        /// <summary>
        ///     message expression
        /// </summary>
        public Expression MessageExpression { get; internal set; }

    }
}
