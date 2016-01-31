using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     class for a record constant expressopn
    /// </summary>
    public class RecordConstantExpression : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public RecordConstantExpression(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     field name
        /// </summary>
        public PascalIdentifier Name { get; internal set; }

        /// <summary>
        ///     field value
        /// </summary>
        public ConstantExpression Value { get; internal set; }

        /// <summary>
        ///     format record constant
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Punct("(");
            result.Identifier(Name.Value);
            result.Punct(":");
            result.Space();
            Value.ToFormatter(result);
            result.Punct(")");
        }
    }
}