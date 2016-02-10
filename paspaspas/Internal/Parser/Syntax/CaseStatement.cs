using System;
using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     case statement
    /// </summary>
    public class CaseStatement : SyntaxPartBase {


        /// <summary>
        ///     create a new case statement
        /// </summary>
        /// <param name="parser"></param>
        public CaseStatement(IParserInformationProvider parser) : base(parser) { }

        /// <summary>
        ///     format case statement
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            throw new NotImplementedException();
        }
    }
}