using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     exports section
    /// </summary>
    public class ExportsSection : ComposedPart<ExportItem> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ExportsSection(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     export name
        /// </summary>
        public PascalIdentifier ExportName { get; internal set; }

        /// <summary>
        ///     format exports section
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("exports");
            result.StartIndent();
            result.NewLine();
            result.Part(ExportName);
            FlattenToPascal(result, x => x.Punct(",").NewLine());
            result.Punct(";");
            result.EndIndent();
        }
    }
}
