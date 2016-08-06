using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     record field list
    /// </summary>
    public class RecordFieldList : ComposedPart<RecordField> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public RecordFieldList(IParserInformationProvider informationProvider) : base(informationProvider) { }



        /// <summary>
        ///     format field list
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            FlattenToPascal(result, x => x.NewLine());
            result.NewLine();
        }
    }
}