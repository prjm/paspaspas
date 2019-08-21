using PasPasPas.Globals.Files;
using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.SyntaxTree.Standard;

namespace PasPasPas.Parsing.Parser.Standard {
    public partial class StandardParser {

        #region ParseUnit

        /// <summary>
        ///     parse a unit declaration
        /// </summary>
        /// <param name="path">unit path</param>
        /// <returns>parsed unit</returns>

        [Rule("Unit", "UnitHead UnitInterface UnitImplementation UnitBlock '.' ")]
        public UnitSymbol ParseUnit(IFileReference path) => new UnitSymbol() {
            UnitHead = ParseUnitHead(),
            UnitInterface = ParseUnitInterface(),
            UnitImplementation = ParseUnitImplementation(),
            UnitBlock = ParseUnitBlock(),
            DotSymbol = ContinueWithOrMissing(TokenKind.Dot),
            FilePath = path,
        };

        #endregion

    }
}
