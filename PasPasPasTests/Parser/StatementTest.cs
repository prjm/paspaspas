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
            ParseString("program test; begin if a <> b then a() ; end .");
            ParseString("program test; begin if a <> b then a() else b() ; end .");
            ParseString("program test; begin if a <> b then begin a(); end end .");
            ParseString("program test; begin if a <> b then begin a(); end else begin b(); end end .");
        }

        [TestMethod]
        public void TestCaseStatement() {
            ParseString("program test; begin case a() of 1: a() end end .");
            ParseString("program test; begin case a() of 1: a() 2: b() else c(); d(); end end .");
            ParseString("program test; begin case a() of 1..5: a() end end .");
        }

        [TestMethod]
        public void TestRepeatStatement() {
            ParseString("program test; begin repeat until true end .");
            ParseString("program test; begin repeat a() until true end .");
            ParseString("program test; begin repeat a(); until true end .");
        }

        [TestMethod]
        public void TestWhileStatement() {
            ParseString("program test; begin while a() do b() ; end .");
            ParseString("program test; begin while a() do begin b(); end end .");
        }

        [TestMethod]
        public void TestForStatement() {
            ParseString("program test; begin for a := 0 to 10 do a() ; end .");
            ParseString("program test; begin for a := 10 downto 0 do a() ; end .");
            ParseString("program test; begin for a in b() do a() ; end .");
        }

        [TestMethod]
        public void TestWithStatement() {
            ParseString("program test; begin with a do a() ; end .");
            ParseString("program test; begin with a do begin a(); end end .");
        }

    }
}
