using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     record helper items
    /// </summary>
    public class RecordHelperItems : ComposedPart<RecordHelperItem> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public RecordHelperItems(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     format record helper items
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            FlattenToPascal(result, x => x.NewLine());
        }
    }
}