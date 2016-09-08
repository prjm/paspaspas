namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     disp id directive
    /// </summary>
    public class DispIdDirective : SyntaxPartBase {

        /// <summary>
        ///     disp id expression
        /// </summary>
        public Expression DispExpression { get; set; }

    }
}