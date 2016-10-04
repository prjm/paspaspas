using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PasPasPasTests.Parser {

    public class StatementTest : ParserTestBase {

        [Fact]
        public void TestMiscStatements() {
            ParseString("program test; procedure x; external x name x + 'x' dependency 'x', 'x', 'x' + x; .");
            ParseString("program test; procedure x; external x name x + 'x'; .");
            ParseString("program test; begin if a(a) then a(a, a.a < a.a, a.a > 0); end .");
            ParseString("program test; begin a(a < x, b > (c)); end .");
            ParseString("program test; begin a(a < x, b > 0); end .");
            ParseString("program test; begin a(a < x, b > x); end .");
            ParseString("program test; begin a.goto(5); end .");
            ParseString("program test; begin a(x:=z,,y:=z); end .");
            ParseString("program test; begin a(z,,z); end .");
            ParseString("unit test; interface implementation procedure x; type x = record case 0: (v: w); 1: (z: T); end ; begin end .");
            ParseString("program test; begin x := string(z); end .");
            ParseString("program test; begin @x := z; end .");
            ParseString("program test; label x; begin x: ; end .");
            ParseString("program test; begin (x as TB).z(); end .");
        }

        [Fact]
        public void TestIfStatement() {
            ParseString("program test; begin if a <= b then begin a() end else b() end .");
            ParseString("program test; begin if a <> b then a() ; end .");
            ParseString("program test; begin if a <> b then a() else b() ; end .");
            ParseString("program test; begin if a <> b then begin a(); end end .");
            ParseString("program test; begin if a <> b then begin a(); end else begin b(); end end .");
        }

        [Fact]
        public void TestCaseStatement() {
            ParseString("program test; begin case a() of 1: a() end end .");
            ParseString("program test; begin case a() of 1: a() 2: b() else c(); d(); end end .");
            ParseString("program test; begin case a() of 1..5: a() end end .");
        }

        [Fact]
        public void TestRepeatStatement() {
            ParseString("program test; begin repeat until true end .");
            ParseString("program test; begin repeat a() until true end .");
            ParseString("program test; begin repeat a(); until true end .");
        }

        [Fact]
        public void TestWhileStatement() {
            //ParseString("program test; begin while a <= a do begin if a = b then (if not x(a, false) then break) else (if not t(a, true) then break); a :=  a + 1; end; ; end .");
            ParseString("program test; begin while a() do b() ; end .");
            ParseString("program test; begin while a() do begin b(); end end .");
        }

        [Fact]
        public void TestForStatement() {
            ParseString("program test; begin for a := 0 to 10 do a() ; end .");
            ParseString("program test; begin for a := 10 downto 0 do a() ; end .");
            ParseString("program test; begin for a in b() do a() ; end .");
        }

        [Fact]
        public void TestWithStatement() {
            ParseString("program test; begin with a do a() ; end .");
            ParseString("program test; begin with a do begin a(); end end .");
        }

        [Fact]
        public void TestRaiseStatement() {
            ParseString("program test; begin raise; end .");
            ParseString("program test; begin raise a(); end .");
            ParseString("program test; begin raise a() at b(); end .");
            ParseString("program test; begin raise at b(); end .");
        }

        [Fact]
        public void TestTryStatement() {
            ParseString("program test; begin try a(); finally b(); end end .");
            ParseString("program test; begin try a(); finally b(); b(); end end .");
            ParseString("program test; begin try a(); except b(); end end .");
            ParseString("program test; begin try a(); except on x : x.x do b(); end end .");
            ParseString("program test; begin try a(); except on x : x.x do b(); else b(); end end .");
        }

        [Fact]
        public void TestGoToStatements() {
            ParseString("program test; begin goto x; end .");
            ParseString("program test; begin exit; end .");
            ParseString("program test; begin exit(5); end .");
            ParseString("program test; begin break; end .");
            ParseString("program test; begin continue; end .");
        }

    }
}
