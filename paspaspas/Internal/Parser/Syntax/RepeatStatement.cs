using System;
using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     repeat statement
    /// </summary>
    public class RepeatStatement : SyntaxPartBase {

        /// <summary>
        ///     create a new repeat statement
        /// </summary>
        /// <param name="parser"></param>
        public RepeatStatement(IParserInformationProvider parser) : base(parser) { }

        /// <summary>
        ///     format repeat statement
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            throw new NotImplementedException();
        }
    }
}