using PasPasPas.Infrastructure.Files;
using PasPasPas.Parsing.Parser;
using PasPasPasTests.Common;

namespace PasPasPasTests.Parser {

    /// <summary>
    ///     special test cases
    /// </summary>
    public class SpecialCaseTests : ParserTestBase {

        [TestMethod]
        public void TestInvalidAttribute()
            => RunCstTest(p => p.ParseUnit(new FileReference(CstPath)), "unit z.x; interface [a] implementation end.", ParserBase.MissingToken);

    }
}
