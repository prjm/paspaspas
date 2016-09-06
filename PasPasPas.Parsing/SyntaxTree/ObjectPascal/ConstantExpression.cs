namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     constant expression
    /// </summary>
    public class ConstantExpression : SyntaxPartBase {

        /// <summary>
        ///     record constant
        /// </summary>
        public RecordConstantExpression RecordConstant { get; set; }

        /// <summary>
        ///     value of the expression
        /// </summary>
        public Expression Value { get; set; }

    }
}