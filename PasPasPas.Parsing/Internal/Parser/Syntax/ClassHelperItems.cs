using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     format class helper items
    /// </summary>
    public class ClassHelperItems : ComposedPart<ClassHelperItem> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ClassHelperItems(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     format class helper items
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            FlattenToPascal(result, x => x.NewLine());
            result.NewLine();
        }
    }
}