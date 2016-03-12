using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     finalization part
    /// </summary>
    public class UnitFinalization : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public UnitFinalization(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     format finalization part
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("finalization").NewLine();
        }
    }
}