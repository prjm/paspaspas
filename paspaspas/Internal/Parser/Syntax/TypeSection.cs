using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     type section
    /// </summary>
    public class TypeSection : ComposedPart<TypeDeclaration> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public TypeSection(IParserInformationProvider informationProvider) : base(informationProvider) { }


        /// <summary>
        ///     format type section
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            if (Count < 1)
                return;

            result.Keyword("type");
            result.StartIndent();
            result.NewLine();

            FlattenToPascal(result, x => { });

            result.EndIndent();
        }
    }
}