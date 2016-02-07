using System;
using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     type case expression
    /// </summary>
    public class Cast : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax element
        /// </summary>
        /// <param name="informationProvider"></param>
        public Cast(IParserInformationProvider informationProvider) : base(informationProvider) {
        }

        /// <summary>
        ///     format type cast
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            throw new NotImplementedException();
        }
    }
}