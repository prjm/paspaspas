using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     real literal
    /// </summary>
    public class PascalRealNumber : SyntaxPartBase {

        /// <summary>
        ///     real literal
        /// </summary>
        /// <param name="pascalToken"></param>
        /// <param name="informationProvider"></param>
        public PascalRealNumber(PascalToken pascalToken, IParserInformationProvider informationProvider) : base(informationProvider) {
            Value = pascalToken.Value;
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
