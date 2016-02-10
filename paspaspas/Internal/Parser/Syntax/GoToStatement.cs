using System;
using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     goto statement
    /// </summary>
    public class GoToStatement : SyntaxPartBase {

        /// <summary>
        ///     create a new goto statement
        /// </summary>
        /// <param name="parser"></param>
        public GoToStatement(IParserInformationProvider parser) : base(parser) { }

        /// <summary>
        ///     format goto statement
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            throw new NotImplementedException();
        }
    }
}