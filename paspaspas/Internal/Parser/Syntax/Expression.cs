using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     expression
    /// </summary>
    public class Expression : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider"></param>
        public Expression(IParserInformationProvider informationProvider) : base(informationProvider) {
        }

        /// <summary>
        ///     closue expression
        /// </summary>
        public ClosureExpr ClosureExpression { get; internal set; }

        /// <summary>
        ///     simple expression
        /// </summary>
        public SimplExpr SimpleExpression { get; internal set; }

        /// <summary>
        ///     format expression
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Part(ClosureExpression);
            result.Part(SimpleExpression);
        }
    }
}
