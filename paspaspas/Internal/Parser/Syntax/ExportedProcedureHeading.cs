using System;
using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     exported procedure heading for an interace section
    /// </summary>
    public class ExportedProcedureHeading : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ExportedProcedureHeading(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     format exported procedure headiing
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            throw new NotImplementedException();
        }
    }
}
