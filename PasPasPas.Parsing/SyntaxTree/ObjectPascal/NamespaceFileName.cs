using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     namespace identifiert with file name
    /// </summary>
    public class NamespaceFileName : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public NamespaceFileName(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     Namespace name
        /// </summary>
        public NamespaceName NamespaceName { get; internal set; }

        /// <summary>
        ///     filename
        /// </summary>
        public QuotedString QuotedFileName { get; internal set; }

        /// <summary>
        ///     format namespace with file name
        /// </summary>
        /// <param name="result">formatter</param>
        public override void ToFormatter(PascalFormatter result) {
            NamespaceName.ToFormatter(result);
            if (QuotedFileName != null) {
                result.Space();
                result.Keyword("in");
                result.Space();
                QuotedFileName.ToFormatter(result);
            }
        }
    }
}