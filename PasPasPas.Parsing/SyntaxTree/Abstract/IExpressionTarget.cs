namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     target for an expression
    /// </summary>
    public interface IExpressionTarget {

        /// <summary>
        ///     value of this expression target
        /// </summary>
        IExpression Value { get; set; }

    }
}