using System;
using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     factor
    /// </summary>
    public class Factor : SyntaxPartBase {

        /// <summary>
        ///     create new factor
        /// </summary>
        /// <param name="informationProvider"></param>
        public Factor(IParserInformationProvider informationProvider) : base(informationProvider) {
        }

        /// <summary>
        ///     address of operator
        /// </summary>
        public Factor AddressOf { get; internal set; }

        /// <summary>
        ///     int value
        /// </summary>
        public PascalInteger IntValue { get; internal set; }

        /// <summary>
        ///     minus
        /// </summary>
        public Factor Minus { get; internal set; }

        /// <summary>
        ///     nor
        /// </summary>
        public Factor Not { get; internal set; }

        /// <summary>
        ///     plus
        /// </summary>
        public Factor Plus { get; internal set; }

        /// <summary>
        ///     pointer to
        /// </summary>
        public PascalIdentifier PointerTo { get; internal set; }

        /// <summary>
        ///     format factor
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            if (AddressOf != null) {
                result.Punct("@");
                result.Part(AddressOf);
                return;
            }

            if (Not != null) {
                result.Keyword("not");
                result.Part(AddressOf);
                return;
            }
        }
    }
}