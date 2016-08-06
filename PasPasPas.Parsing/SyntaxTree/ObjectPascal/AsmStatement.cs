using System;
using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

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