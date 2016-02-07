using System;
using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     designator
    /// </summary>
    public class DesignatorStatement : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax element
        /// </summary>
        /// <param name="informationProvider"></param>
        public DesignatorStatement(IParserInformationProvider informationProvider) : base(informationProvider) {
        }

        /// <summary>
        ///     format designator statemet
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            throw new NotImplementedException();
        }
    }
}