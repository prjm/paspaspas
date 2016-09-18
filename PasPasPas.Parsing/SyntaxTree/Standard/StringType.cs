namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     string type
    /// </summary>
    public class StringType : SyntaxPartBase {

        /// <summary>
        ///     code page
        /// </summary>
        public ConstantExpression CodePage { get; set; }

        /// <summary>
        ///     kind of the string
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     string length
        /// </summary>
        public Expression StringLength { get; set; }

    }
}