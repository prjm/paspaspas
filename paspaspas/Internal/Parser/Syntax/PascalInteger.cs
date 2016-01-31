using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     constant integer
    /// </summary>
    public class PascalInteger : ExpressionBase {

        /// <summary>
        ///     create a new integer constant
        /// </summary>
        /// <param name="token"></param>
        /// <param name="informationProvider"></param>
        public PascalInteger(PascalToken token, IParserInformationProvider informationProvider) : base(informationProvider) {
            Value = token.Value;
        }

        /// <summary>
        ///     integer value
        /// </summary>
        public string Value { get; }

        /// <summary>
        ///     format integer
        /// </summary>
        /// <param name="result">formatter</param>
        public override void ToFormatter(PascalFormatter result) {
            result.Number(Value);
        }
    }
}
