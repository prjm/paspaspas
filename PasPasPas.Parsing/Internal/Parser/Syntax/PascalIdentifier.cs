using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     simple pascal identifier
    /// </summary>
    public class PascalIdentifier : SyntaxPartBase {

        /// <summary>
        ///     creates a new identifer
        /// </summary>
        /// <param name="token"></param>
        /// <param name="informationProvider"></param>
        public PascalIdentifier(PascalToken token, IParserInformationProvider informationProvider) : base(informationProvider) {
            Value = token.Value;
        }

        /// <summary>
        ///     identifier value
        /// </summary>
        public string Value { get; }

        /// <summary>
        ///     format the value
        /// </summary>        
        /// <param name="result">formatter</param>
        public override void ToFormatter(PascalFormatter result)
            => result.Identifier(Value);
    }
}
