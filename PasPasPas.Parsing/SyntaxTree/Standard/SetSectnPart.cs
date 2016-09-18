namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     set section part
    /// </summary>
    public class SetSectnPart : SyntaxPartBase {

        /// <summary>
        ///     continuation
        /// </summary>
        public int Continuation { get; set; }

        /// <summary>
        ///     set expression
        /// </summary>
        public Expression SetExpression { get; set; }

    }
}