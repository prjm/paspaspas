using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     package head
    /// </summary>
    public class PackageHead : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public PackageHead(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     package name
        /// </summary>
        public NamespaceName PackageName { get; internal set; }

        /// <summary>
        ///     format package head
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("package").Space();
            result.Part(PackageName);
            result.Punct(";");
            result.NewLine();
        }
    }
}