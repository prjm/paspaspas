using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     requires clause
    /// </summary>
    public class PackageRequires : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public PackageRequires(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     required packages
        /// </summary>
        public NamespaceNameList RequiresList { get; internal set; }

        /// <summary>
        ///     format requires list
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("requires");
            result.StartIndent();
            result.NewLine();
            result.Part(RequiresList);
            result.EndIndent();
            result.NewLine();
        }
    }
}