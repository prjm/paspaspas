using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasPasPasTests.Parser {

    [TestClass]
    public class ProgramTest : ParserTestBase {

        [TestMethod]
        public void TestPrograms() {
            ParseString("program test; .");
            ParseString("program test(a, c); .");
            ParseString("program test.x(a, c); .");
            ParseString("library test.x; .");
            ParseString("library test.x deprecated; .");
            ParseString("library test.x deprecated 'x'; .");
            ParseString("library test.x deprecated 'x' experimental platform library; .");
            ParseString("package test.x; requires x; end .");
            ParseString("package test.x; requires x; contains x; end .");
            ParseString("unit test.x; interface implementation end .");
            ParseString("unit test.x; interface uses x, x, x; implementation end .");
            ParseString("unit test.x; interface uses x, x, x; implementation uses x, x, x; end .");
            ParseString("unit test.x deprecated experimental library; interface uses x, x, x; implementation uses x, x, x; end .");
        }

    }
}
