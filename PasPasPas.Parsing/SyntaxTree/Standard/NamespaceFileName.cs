namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     namespace identifiert with file name
    /// </summary>
    public class NamespaceFileName : SyntaxPartBase {

        /// <summary>
        ///     Namespace name
        /// </summary>
        public NamespaceName NamespaceName { get; set; }

        /// <summary>
        ///     filename
        /// </summary>
        public QuotedString QuotedFileName { get; set; }

    }
}