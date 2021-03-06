﻿#nullable disable
using PasPasPasTests.Common;

namespace PasPasPasTests.Parser {

    /// <summary>
    ///     test statements
    /// </summary>
    public class StatementTest : ParserTestBase {

        /// <summary>
        ///     test misc statements
        /// </summary>
        [TestMethod]
        public void TestMiscStatements() {
            ParseString("program test; const x: array[x] of Integer = (a, {$IFNDEF X} b {$ELSE} 0 {$ENDIF}, c); begin end.");
            ParseString("program test; var x: x.x<x,x,x,x>.x.x; begin end.");
            ParseString("program test; procedure x([ref] a, [ref] b: string); .");
            ParseString("program test; begin a := 5.ToString(); end .");
            ParseString("program test; begin a := 5.0.ToString(); end .");
            ParseString("program test; begin a := 'a'.ToString(); end .");
            ParseString("program test; begin a := $5.ToString(); end .");
            ParseString("program test; begin a := #5.ToString(); end .");
            ParseString("program test; begin a := false.ToString(); end .");
            ParseString("program test; begin a := true.ToString(); end .");
            ParseString("program test; begin if a > b then a.a<x> else a.a<x>; end .");
            ParseString("program test; begin if a<b> then asm 5 end; end .");
            ParseString("program test; begin if (a < b) then asm 5 end; end .");
            ParseString("program test; procedure x; external x name x + 'x' dependency 'x', 'x', 'x' + x; .");
            ParseString("program test; procedure x; external x name x + 'x'; .");
            ParseString("program test; begin if a(a) then a(a, a.a < a.a, a.a > 0); end .");
            ParseString("program test; begin a(a < x, b > (c)); end .");
            ParseString("program test; begin a(a < x, b > 0); end .");
            ParseString("program test; begin a(a < x, b > x); end .");
            ParseString("program test; begin a.goto(5); end .");
            ParseString("program test; begin a(x:=z,,y:=z); end .");
            ParseString("program test; begin a(z,,z); end .");
            ParseString("unit test; interface implementation procedure x; type x = record case integer of 0: (v: w); 1: (z: T); end ; end .");
            ParseString("program test; begin x := string(z); end .");
            ParseString("program test; begin x := @@@@z; end .");
            ParseString("program test; label x; begin x: z := 0; end .");
            ParseString("program test; begin (x as TB).z(); end .");
        }

        /// <summary>
        ///     test if statements
        /// </summary>
        [TestMethod]
        public void TestIfStatement() {
            ParseString("program test; begin if a <= b then begin a() end else b() end .");
            ParseString("program test; begin if a <> b then a() ; end .");
            ParseString("program test; begin if a <> b then a() else b() ; end .");
            ParseString("program test; begin if a <> b then begin a(); end end .");
            ParseString("program test; begin if a <> b then begin a(); end else begin b(); end end .");
        }

        /// <summary>
        ///     test case statements
        /// </summary>
        [TestMethod]
        public void TestCaseStatement() {
            ParseString("program test; begin case a() of 1: a() end end .");
            ParseString("program test; begin case a() of 1: a() 2: b() else c(); d(); end end .");
            ParseString("program test; begin case a() of 1..5: a() end end .");
        }

        /// <summary>
        ///     test repeat statements
        /// </summary>
        [TestMethod]
        public void TestRepeatStatement() {
            ParseString("program test; begin repeat until true end .");
            ParseString("program test; begin repeat a() until true end .");
            ParseString("program test; begin repeat a(); until true end .");
        }

        /// <summary>
        ///     test a while statement
        /// </summary>
        [TestMethod]
        public void TestWhileStatement() {
            //ParseString("program test; begin while a <= a do begin if a = b then (if not x(a, false) then break) else (if not t(a, true) then break); a :=  a + 1; end; ; end .");
            ParseString("program test; begin while a() do b() ; end .");
            ParseString("program test; begin while a() do begin b(); end end .");
        }

        /// <summary>
        ///     test a form statement
        /// </summary>
        [TestMethod]
        public void TestForStatement() {
            ParseString("program test; begin for a := 0 to 10 do a() ; end .");
            ParseString("program test; begin for a := 10 downto 0 do a() ; end .");
            ParseString("program test; begin for a in b() do a() ; end .");
        }

        /// <summary>
        ///     test a with statement
        /// </summary>
        [TestMethod]
        public void TestWithStatement() {
            ParseString("program test; begin with a do a() ; end .");
            ParseString("program test; begin with a do begin a(); end end .");
        }

        /// <summary>
        ///     test a raise statement
        /// </summary>
        [TestMethod]
        public void TestRaiseStatement() {
            ParseString("program test; begin raise; end .");
            ParseString("program test; begin raise a(); end .");
            ParseString("program test; begin raise a() at b(); end .");
            ParseString("program test; begin raise at b(); end .");
        }

        /// <summary>
        ///     test a try statement block
        /// </summary>
        [TestMethod]
        public void TestTryStatement() {
            ParseString("program test; begin try a(); finally b(); end end .");
            ParseString("program test; begin try a(); finally b(); b(); end end .");
            ParseString("program test; begin try a(); except b(); end end .");
            ParseString("program test; begin try a(); except on x : x.x do b(); end end .");
            ParseString("program test; begin try a(); except on x : x.x do b(); else b(); end end .");
        }

        /// <summary>
        ///     test goto statement
        /// </summary>
        [TestMethod]
        public void TestGoToStatements() {
            ParseString("program test; begin goto x; end .");
            ParseString("program test; begin exit; end .");
            ParseString("program test; begin exit(5); end .");
            ParseString("program test; begin break; end .");
            ParseString("program test; begin continue; end .");
        }

        /// <summary>
        ///     test assembler statements
        /// </summary>
        [TestMethod]
        public void TestAsmStatement() {
            ParseString("program test; asm end .");
            ParseString("program test; asm mov 1, not 1 end .");
            ParseString("program test; asm mov 1, not 1 xor 1 end .");
            ParseString("program test; asm mov 1, not 1 and 1 end .");
            ParseString("program test; asm mov 1, not 1 or 1 end .");
            ParseString("program test; asm mov 1, type (1) end .");
            ParseString("program test; asm mov 1, byte ptr (1) end .");
            ParseString("program test; asm mov 1, word ptr (1) end .");
            ParseString("program test; asm mov 1, dword ptr (1) end .");
            ParseString("program test; asm mov 1, qword ptr (1) end .");
            ParseString("program test; asm mov 1, tbyte ptr (1) end .");
        }
    }
}
