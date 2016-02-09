using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     formatted expression
    /// </summary>
    public class FormattedExpression : SyntaxPartBase {

        /// <summary>
        ///     create a new formatted expression
        /// </summary>
        /// <param name="informationProvider"></param>
        public FormattedExpression(IParserInformationProvider informationProvider) : base(informationProvider) {
        }

        /// <summary>
        ///     decimals subexpression
        /// </summary>
        public Expression Decimals { get; internal set; }

        /// <summary>
        ///     width subexpression
        /// </summary>
        public Expression Width { get; internal set; }

        /// <summary>
        ///     base expression
        /// </summary>
        public Expression Expression { get; internal set; }

        /// <summary>
        ///     format expression
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Part(Expression);
            if (Width != null) {
                result.Punct(":");
                result.Part(Width);
                if (Decimals != null) {
                    result.Punct(":");
                    result.Part(Decimals);
                }
            }
        }
    }
}