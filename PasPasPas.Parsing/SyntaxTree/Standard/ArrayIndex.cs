namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     array index definition
    /// </summary>
    public class ArrayIndex : SyntaxPartBase {

        /// <summary>
        ///     start index
        /// </summary>
        public ConstantExpression StartIndex { get; set; }

        /// <summary>
        ///     end index
        /// </summary>
        public ConstantExpression EndIndex { get; set; }

    }
}