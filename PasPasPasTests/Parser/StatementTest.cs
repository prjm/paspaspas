using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasPasPasTests.Parser {

    [TestClass]
    public class StatementTest : ParserTestBase {

        [TestMethod]
        public void TestIfStatement() {
            ParseString("program test; begin if a <> b then a() end .");
        }

    }
}
