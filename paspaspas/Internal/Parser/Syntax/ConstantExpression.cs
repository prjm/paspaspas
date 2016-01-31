using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     constant expression
    /// </summary>
    public class ConstantExpression : ComposedPart<ConstantExpression> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ConstantExpression(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     record constant
        /// </summary>
        public RecordConstantExpression RecordConstant { get; internal set; }

        /// <summary>
        ///     value of the expression
        /// </summary>
        public ExpressionBase Value { get; internal set; }

        /// <summary>
        ///     format expression
        /// </summary>
        /// <param name="result">result</param>
        public override void ToFormatter(PascalFormatter result) {
            if (RecordConstant != null) {
                RecordConstant.ToFormatter(result);
                return;
            }

            if (Count > 0) {
                result.Punct("(");
                FlattenToPascal(result, x => x.Punct(", "));
                result.Punct(")");
                return;
            }

            Value.ToFormatter(result);
        }
    }
}