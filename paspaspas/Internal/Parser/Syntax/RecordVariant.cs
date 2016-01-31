using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     one part of a variant part of a record
    /// </summary>
    public class RecordVariant : ComposedPart<ConstantExpression> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public RecordVariant(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     field list
        /// </summary>
        public RecordFieldList FieldList { get; internal set; }

        /// <summary>
        ///     format record variant
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            FlattenToPascal(result, x => x.Punct(",").Space());
            result.Space().Punct(":").Space().Punct("(").Space().Part(FieldList).Space().Punct(")").NewLine();
        }
    }
}