using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     label section
    /// </summary>
    public class LabelDeclarationSection : ComposedPart<Label> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public LabelDeclarationSection(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     format label section
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("label");
            result.StartIndent();
            result.NewLine();
            FlattenToPascal(result, x => x.Punct(",").NewLine());
            result.Punct(";").NewLine();
            result.EndIndent();
        }
    }
}
