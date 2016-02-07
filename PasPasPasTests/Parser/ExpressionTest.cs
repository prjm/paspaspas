﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasPasPasTests.Parser {


    [TestClass]
    public class ExpressionTest : ParserTestBase {

        [TestMethod]
        public void TestClosureExpressions() {
            ParseString("program test; const x = procedure begin end ; .");
            ParseString("program test; const x = function : integer begin end ; .");
            ParseString("program test; const x = function (const x : string ): integer begin end ; .");
        }

        [TestMethod]
        public void TestSimpleExpressions() {
            ParseString("program test; const x = 5 + 5; .");
            ParseString("program test; const x = 5 - 5; .");
            ParseString("program test; const x = 5 xor 5; .");
            ParseString("program test; const x = 5 or 5; .");
        }

        [TestMethod]
        public void TestSimpleTerms() {
            ParseString("program test; const x = 5 + 3 * 4; .");
            ParseString("program test; const x = 5 + 3 / 4; .");
            ParseString("program test; const x = 5 + 3 div 4; .");
            ParseString("program test; const x = 5 + 3 mod 4; .");
            ParseString("program test; const x = 5 + 3 and 4; .");
            ParseString("program test; const x = 5 + 3 shl 4; .");
            ParseString("program test; const x = 5 + 3 shr 4; .");
            ParseString("program test; const x = 5 + 3 as 4; .");
        }

        [TestMethod]
        public void TestSimpleFactors() {
            ParseString("program test; const x = 5 + 3 * @4; .");
            ParseString("program test; const x = 5 + 3 * not 4; .");
            ParseString("program test; const x = 5 + 3 * +4; .");
            ParseString("program test; const x = 5 + 3 * -4; .");
            ParseString("program test; const x = 5 + 3 * ^x; .");
            ParseString("program test; const x = 5 + 3 * $4; .");
            ParseString("program test; const x = 5 + 3 * 4.44; .");
            ParseString("program test; const x = 5 + 3 * true; .");
            ParseString("program test; const x = 5 + 3 * false; .");
            ParseString("program test; const x = 5 + 3 * nil; .");
            ParseString("program test; const x = 5 + 3 * (4); .");
            ParseString("program test; const x = 5 + 3 * 'x'; .");
        }

    }
}
