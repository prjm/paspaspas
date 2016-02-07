using System;
using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     set expression
    /// </summary>
    public class SetSectn : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax element
        /// </summary>
        /// <param name="informationProvider"></param>
        public SetSectn(IParserInformationProvider informationProvider) : base(informationProvider) {
        }

        /// <summary>
        ///     format set section
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            throw new NotImplementedException();
        }
    }
}