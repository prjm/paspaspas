namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     variable value
    /// </summary>
    public class VarValueSpecification : SyntaxPartBase {

        /// <summary>
        ///     absolute index
        /// </summary>
        public ConstantExpression Absolute { get; set; }

        /// <summary>
        ///     initial value
        /// </summary>
        public ConstantExpression InitialValue { get; set; }


    }
}