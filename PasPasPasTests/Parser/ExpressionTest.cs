using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PasPasPasTests.Parser {


    public class ExpressionTest : ParserTestBase {

        [Fact]
        public void TestClosureExpressions() {
            ParseString("program test; const x = procedure begin end ; .");
            ParseString("program test; const x = function : integer begin end ; .");
            ParseString("program test; const x = function (const x : string ): integer begin end ; .");
        }

        [Fact]
        public void TestSimpleExpressions() {
            ParseString("program test; const x = 5 + 5; .");
            ParseString("program test; const x = 5 - 5; .");
            ParseString("program test; const x = 5 xor 5; .");
            ParseString("program test; const x = 5 or 5; .");
            ParseString("program test; const x = []; .");
            ParseString("program test; const x = [5]; .");
            ParseString("program test; const x = [5, 6]; .");
            ParseString("program test; const x = [5, 6..8, 9..224]; .");
            ParseString("program test; const x = a.b.c; .");
            ParseString("program test; const x = a.b.c^; .");
            ParseString("program test; const x = a.b.c^.d[q]; .");
            ParseString("program test; const x = a.b.c^.d[q].d(asd, asd, ad); .");
            ParseString("program test; const x = a.b.c^.d[q].d(asd:d, asd:d:d, ad:d); .");
            ParseString("program test; const x = 5 < 5; .");
            ParseString("program test; const x = 5 > 5; .");
            ParseString("program test; const x = 5 <= 5; .");
            ParseString("program test; const x = 5 >= 5; .");
            ParseString("program test; const x = 5 <> 5; .");
            ParseString("program test; const x = 5 = 5; .");
            ParseString("program test; const x = 5 in 5; .");
            ParseString("program test; const x = 5 as 5; .");
        }

        [Fact]
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

        [Fact]
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
