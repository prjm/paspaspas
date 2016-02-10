using System;
using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     try statement
    /// </summary>
    public class TryStatement : SyntaxPartBase {

        /// <summary>
        ///     create a new try statement
        /// </summary>
        /// <param name="parser"></param>
        public TryStatement(IParserInformationProvider parser) : base(parser) { }

        /// <summary>
        ///     format try statement
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            throw new NotImplementedException();
        }
    }
}