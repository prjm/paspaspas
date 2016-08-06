using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     record declaration
    /// </summary>
    public class RecordDeclaration : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public RecordDeclaration(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     field list
        /// </summary>
        public RecordFieldList FieldList { get; internal set; }

        /// <summary>
        ///     record items
        /// </summary>
        public RecordItems Items { get; internal set; }

        /// <summary>
        ///     variant section
        /// </summary>
        public RecordVariantSection VariantSection { get; internal set; }

        /// <summary>
        ///     format record declaration
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("record"); ;
            result.StartIndent();
            result.NewLine();
            result.Part(FieldList);
            result.Part(VariantSection);
            result.Part(Items);
            result.Keyword("end");
            result.EndIndent();
        }
    }
}