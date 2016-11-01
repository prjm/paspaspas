namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     part of a constant array
    /// </summary>
    public class ArrayConstantItem : AbstractSyntaxPart, IExpressionTarget {

        /// <summary>
        ///     item value
        /// </summary>
        public ExpressionBase Value { get; set; }

    }
}