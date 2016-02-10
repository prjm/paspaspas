using System;
using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     raise statemenmt
    /// </summary>
    public class RaiseStatement : SyntaxPartBase {

        /// <summary>
        ///     create a new raise statement
        /// </summary>
        /// <param name="parser"></param>
        public RaiseStatement(IParserInformationProvider parser) : base(parser) { }

        /// <summary>
        ///     format raise statement
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            throw new NotImplementedException();
        }
    }
}