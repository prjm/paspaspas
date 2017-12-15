using PasPasPas.Typings.Common;
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
            ParseString("program test; const x = a.b.c^.d[q].d(asd:d, asd:d:d, ad:d); .");
            ParseString("program test; const x : array[0..1+1] of x = (); .");
            ParseString("program test; const x = TDemo.GetInstance().Func<T>(); .");
            ParseString("program test; const x = a(((x[0]).q)^); .");
            ParseString("program test; const x = TDemo<string>.Create(); .");
            ParseString("program test; const x = TDemo<A,B>.Create(); .");
            ParseString("program test; const x = TDemo<A,B, C<D>>.Create(); .");
            ParseString("program test; const x = 5 as 5; .");
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
            ParseString("program test; const x = 5 < 5; .");
            ParseString("program test; const x = 5 > 5; .");
            ParseString("program test; const x = 5 <= 5; .");
            ParseString("program test; const x = 5 >= 5; .");
            ParseString("program test; const x = 5 <> 5; .");
            ParseString("program test; const x = 5 = 5; .");
            ParseString("program test; const x = 5 in 5; .");
            ParseString("program test; const x = x is TObject; .");
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

        [Fact]
        public void TestConstValues() {
            TestConstant("10");
            TestConstant("10.1");
            TestConstant("True");
            TestConstant("False");
            TestConstant("Nil");
            TestConstant("'a'");
            TestConstant("'aa'");
            TestConstant("5; y = x", "y");
            TestConstant("-3");
            TestConstant("+3");
            TestConstant("not 3");
            TestConstant("3 or 4");
            TestConstant("4 xor 3");
            TestConstant("4 and 13");
            TestConstant("4 - 34");
            TestConstant("4 * 4");
            TestConstant("12 div 4");
            TestConstant("11 mod 4");
            TestConstant("12 /  4");
            TestConstant("12 shl  4");
            TestConstant("12 shr  4");
            TestConstant("12 =  4");
            TestConstant("12 <>  4");
            TestConstant("12 < 4");
            TestConstant("12 > 4");
            TestConstant("12 <= 4");
            TestConstant("12 >= 4");
            TestConstant("3 + 4");
            TestConstant("3.0 + 4");
            TestConstant("'a' + 'b'");
            TestConstant("'as' + 'b'");
            TestConstant("'a' + 'db'");
            TestConstant("'ad' + 'db'");
            TestConstant("[1,2,3,4]");
            TestConstant("['a'..'z']");
            TestConstant("(1,2,3)");
            TestConstant("Abs(5)", typeId: TypeIds.ShortInt);
            TestConstant("Abs(-501)", typeId: TypeIds.SmallInt);
        }

    }
}
