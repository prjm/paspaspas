namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     label declaration
    /// </summary>
    public class Label : SyntaxPartBase {

        /// <summary>
        ///     label name
        /// </summary>
        public SyntaxPartBase LabelName { get; set; }
    }
}