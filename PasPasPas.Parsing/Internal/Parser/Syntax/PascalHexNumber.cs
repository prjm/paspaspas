using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     hex numer literal
    /// </summary>
    public class PascalHexNumber : SyntaxPartBase {

        /// <summary>
        ///     create a new integer hex number literal
        /// </summary>
        /// <param name="token"></param>
        /// <param name="informationProvider"></param>
        public PascalHexNumber(PascalToken token, IParserInformationProvider informationProvider) : base(informationProvider) {
            Value = token.Value;
        }

        /// <summary>
        ///     number value
        /// </summary>
        public string Value { get; }

        /// <summary>
        ///     format value
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Number(Value);
        }
    }
}
