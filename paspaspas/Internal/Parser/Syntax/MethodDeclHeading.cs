using System;
using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     method declaration heading
    /// </summary>
    public class MethodDeclHeading : SyntaxPartBase {


        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public MethodDeclHeading(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     format headinger
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            throw new NotImplementedException();
        }
    }
}