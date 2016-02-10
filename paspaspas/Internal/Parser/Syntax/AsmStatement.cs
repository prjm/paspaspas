using System;
using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     asm statement block
    /// </summary>
    public class AsmStatement : SyntaxPartBase {

        /// <summary>
        ///     create a new asm statement
        /// </summary>
        /// <param name="parser"></param>
        public AsmStatement(IParserInformationProvider parser) : base(parser) { }

        /// <summary>
        ///     format statement
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            throw new NotImplementedException();
        }
    }
}