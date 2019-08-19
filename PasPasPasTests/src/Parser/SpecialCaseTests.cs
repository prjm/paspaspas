using PasPasPas.Globals.Files;
using PasPasPas.Globals.Log;
using PasPasPas.Parsing.Parser;
using PasPasPas.Parsing.Parser.Standard;
using PasPasPas.Parsing.SyntaxTree.Standard;
using PasPasPasTests.Common;

namespace PasPasPasTests.Parser {

    /// <summary>
    ///     special test cases
    /// </summary>
    public class SpecialCaseTests : ParserTestBase {

        [TestMethod]
        public void TestInvalidAttribute()
            => RunCstTest(p => p.ParseUnit(new FileReference(CstPath)), "unit z.x; interface [a] implementation end.", MessageNumbers.MissingToken);

        [TestMethod]
        public void TestForwardDeclaration() {
            var p = "program z.x; procedure x; forward; procedure x; begin end; begin end.";
            ProgramSymbol t(StandardParser s) => s.ParseProgram(new FileReference(CstPath));
            RunCstTest(t, p);
        }

        [TestMethod]
        public void TestExportDeclaration() {
            var p = "program z.x; procedure x; begin end; exports x; begin end.";
            ProgramSymbol t(StandardParser s) => s.ParseProgram(new FileReference(CstPath));
            RunCstTest(t, p);
        }

    }
}
