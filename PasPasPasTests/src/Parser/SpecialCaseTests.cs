#nullable disable
using PasPasPas.Globals.Log;
using PasPasPas.Parsing.Parser.Standard;
using PasPasPas.Parsing.SyntaxTree.Standard;
using PasPasPasTests.Common;

namespace PasPasPasTests.Parser {

    /// <summary>
    ///     special test cases
    /// </summary>
    public class SpecialCaseTests : ParserTestBase {

        /// <summary>
        ///     test an invalid attribute
        /// </summary>
        [TestMethod]
        public void TestInvalidAttribute()
            => RunCstTest(p => p.ParseUnit(p.Environment.CreateFileReference(CstPath)), "unit z.x; interface [a] implementation end.", MessageNumbers.MissingToken);

        /// <summary>
        ///     test a forward declaration
        /// </summary>
        [TestMethod]
        public void TestForwardDeclaration() {
            var p = "program z.x; procedure x; forward; procedure x; begin end; begin end.";
            ProgramSymbol t(StandardParser s) => s.ParseProgram(s.Environment.CreateFileReference(CstPath));
            RunCstTest(t, p);
        }

        /// <summary>
        ///     test a export declaration
        /// </summary>
        [TestMethod]
        public void TestExportDeclaration() {
            var p = "program z.x; procedure x; begin end; exports x; begin end.";
            ProgramSymbol t(StandardParser s) => s.ParseProgram(s.Environment.CreateFileReference(CstPath));
            RunCstTest(t, p);
        }

    }
}
