using System;
using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///  while statement
    /// </summary>
    public class WhileStatement : SyntaxPartBase {

        /// <summary>
        ///     create a new while statement
        /// </summary>
        /// <param name="parser"></param>
        public WhileStatement(IParserInformationProvider parser) : base(parser) { }

        /// <summary>
        ///     format while statement
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            throw new NotImplementedException();
        }
    }
}