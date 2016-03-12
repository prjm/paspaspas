using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     contains clause
    /// </summary>
    public class PackageContains : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public PackageContains(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     contained units
        /// </summary>
        public NamespaceFileNameList ContainsList { get; internal set; }

        /// <summary>
        ///     format clause
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("contains");
            result.StartIndent();
            result.NewLine();
            result.Part(ContainsList);
            result.EndIndent();
            result.NewLine();
        }
    }
}