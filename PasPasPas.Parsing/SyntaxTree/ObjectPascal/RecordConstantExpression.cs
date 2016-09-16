namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     class for a record constant expressopn
    /// </summary>
    public class RecordConstantExpression : SyntaxPartBase {

        /// <summary>
        ///     field name
        /// </summary>
        public PascalIdentifier Name { get; set; }

        /// <summary>
        ///     field value
        /// </summary>
        public ConstantExpression Value { get; set; }

    }
}