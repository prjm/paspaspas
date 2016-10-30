namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     constant expression
    /// </summary>
    public class ConstantExpression : SyntaxPartBase {

        /// <summary>
        ///     <c>true</c> if this is an array constant
        /// </summary>
        public bool IsArrayConstant { get; set; }

        /// <summary>
        ///     <c>true</c> if this in an record constant
        /// </summary>
        public bool IsRecordConstant { get; set; }

        /// <summary>
        ///     value of the expression
        /// </summary>
        public Expression Value { get; set; }

    }
}