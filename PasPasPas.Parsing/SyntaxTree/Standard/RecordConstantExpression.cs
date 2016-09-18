namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     class for a record constant expressopn
    /// </summary>
    public class RecordConstantExpression : SyntaxPartBase {

        /// <summary>
        ///     field name
        /// </summary>
        public Identifier Name { get; set; }

        /// <summary>
        ///     field value
        /// </summary>
        public ConstantExpression Value { get; set; }

    }
}