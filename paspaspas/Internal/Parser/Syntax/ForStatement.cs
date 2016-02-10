using System;
using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     create a new for statement
    /// </summary>
    public class ForStatement : SyntaxPartBase {

        /// <summary>
        ///     create a new for statement
        /// </summary>
        /// <param name="paser"></param>
        public ForStatement(IParserInformationProvider paser) : base(paser) { }

        /// <summary>
        ///     format statement
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            throw new NotImplementedException();
        }
    }
}