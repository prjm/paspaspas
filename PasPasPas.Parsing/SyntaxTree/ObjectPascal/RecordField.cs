using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     record field
    /// </summary>
    public class RecordField : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public RecordField(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     fiel type
        /// </summary>
        public TypeSpecification FieldType { get; internal set; }

        /// <summary>
        ///     hinting directive
        /// </summary>
        public HintingInformationList Hint { get; internal set; }


        /// <summary>
        ///     field names
        /// </summary>
        public IdentList Names { get; internal set; }

        /// <summary>
        ///     format record field
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Part(Names);
            result.Punct(":").Space();
            result.Part(FieldType).Space();
            result.Part(Hint);
            result.Punct(";");
        }
    }
}