using PasPasPasTests.Common;
using PasPasPasTests.Parser;
using PasPasPasTests.src.Common;

namespace PasPasPasTests.src.Parser {
    public class FileReferenceTest : ParserTestBase {

        [TestMethod]
        public void TestSimpleReferencedConstant() {
            var a = "unit a; interface const x = 5; implementation end.";
            var b = "unit b; interface uses a; const b = a.x; implementation end.";
            var t = new FilesAndPaths(("a.pas", a), ("b.pas", b));

        }

    }
}
