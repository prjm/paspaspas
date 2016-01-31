using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {


    /// <summary>
    ///     a list of expressions
    /// </summary>
    public class ExpressionList : ComposedPart<ExpressionBase> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ExpressionList(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     format expression list
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            FlattenToPascal(result, x => x.Punct(", "));
        }
    }
}