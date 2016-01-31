using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     record items
    /// </summary>
    public class RecordItems : ComposedPart<RecordItem> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public RecordItems(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     format record items
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            FlattenToPascal(result, x => x.NewLine());
        }
    }
}