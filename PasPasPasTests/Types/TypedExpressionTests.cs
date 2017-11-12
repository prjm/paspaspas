using PasPasPas.Typings.Common;
using PasPasPasTests.Common;
using Xunit;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test cases for typed expressions
    /// </summary>
    public class TypedExpressionTest : TypeTest {

        [Fact]
        public void TestIntegerLiteralTypes() {
            AssertExprType("1", TypeIds.ShortInt);
            AssertExprType("255", TypeIds.ShortInt);
            AssertExprType("256", TypeIds.SmallInt);
            AssertExprType("65535", TypeIds.SmallInt);
            AssertExprType("65536", TypeIds.IntegerType);
            AssertExprType("4294967295", TypeIds.IntegerType);
            AssertExprType("4294967296", TypeIds.Int64Type);
            AssertExprType("18446744073709551615", TypeIds.Int64Type);
        }

        [Fact]
        public void TestBooleanLiteralTypes() {
            AssertExprType("true", TypeIds.BooleanType);
            AssertExprType("false", TypeIds.BooleanType);
        }

        [Fact]
        public void TestCharLiteralTypes() {
            AssertExprType("'C'", TypeIds.WideCharType);
            AssertExprType("#9", TypeIds.WideCharType);
        }

        [Fact]
        public void TestStringLiteralTypes() {
            AssertExprType("'CD'", TypeIds.StringType);
            AssertExprType("#9#9", TypeIds.StringType);
        }

        [Fact]
        public void TestExtendedLiteralTypes() {
            AssertExprType("3.5", TypeIds.Extended);
            AssertExprType("2.33434343", TypeIds.Extended);
        }

        [Fact]
        public void TestBooleanOperators() {
            AssertExprType("not true", TypeIds.BooleanType);
            AssertExprType("not false", TypeIds.BooleanType);
            AssertExprType("true and false", TypeIds.BooleanType);
            AssertExprType("true or false", TypeIds.BooleanType);
            AssertExprType("true xor false", TypeIds.BooleanType);
        }

        [Fact]
        public void TestArithmetikOperatorsInteger() {
            AssertExprType("+ 1", TypeIds.IntegerType);
            AssertExprType("- 1", TypeIds.IntegerType);
            AssertExprType("1 + 1", TypeIds.IntegerType);
            AssertExprType("1 - 1", TypeIds.IntegerType);
            AssertExprType("1 * 1", TypeIds.IntegerType);
            AssertExprType("1 div 1", TypeIds.IntegerType);
            AssertExprType("1 mod 1", TypeIds.IntegerType);
            AssertExprType("1 / 1", TypeIds.Extended);
        }

        [Fact]
        public void TestArithmetikOperatorsInt64() {
            AssertExprType("+ 4294967296", TypeIds.Int64Type);
            AssertExprType("- 4294967296", TypeIds.Int64Type);
            AssertExprType("4294967296 + 1", TypeIds.Int64Type);
            AssertExprType("4294967296 - 1", TypeIds.Int64Type);
            AssertExprType("4294967296 * 1", TypeIds.Int64Type);
            AssertExprType("4294967296 div 1", TypeIds.Int64Type);
            AssertExprType("4294967296 mod 1", TypeIds.Int64Type);
            AssertExprType("4294967296 / 1", TypeIds.Extended);
            AssertExprType("1 + 4294967296", TypeIds.Int64Type);
            AssertExprType("1 - 4294967296", TypeIds.Int64Type);
            AssertExprType("1 * 4294967296", TypeIds.Int64Type);
            AssertExprType("1 div 4294967296", TypeIds.Int64Type);
            AssertExprType("1 mod 4294967296", TypeIds.Int64Type);
            AssertExprType("1 / 4294967296", TypeIds.Extended);
            AssertExprType("4294967296 + 4294967296", TypeIds.Int64Type);
            AssertExprType("4294967296 - 4294967296", TypeIds.Int64Type);
            AssertExprType("4294967296 * 4294967296", TypeIds.Int64Type);
            AssertExprType("4294967296 div 4294967296", TypeIds.Int64Type);
            AssertExprType("4294967296 mod 4294967296", TypeIds.Int64Type);
            AssertExprType("4294967296 / 4294967296", TypeIds.Extended);
        }

        [Fact]
        public void TestArithmetikOperatorsReal() {
            AssertExprType("+ 1.0", TypeIds.Extended);
            AssertExprType("- 1.0", TypeIds.Extended);
            AssertExprType("1.0 + 1", TypeIds.Extended);
            AssertExprType("1.0 - 1", TypeIds.Extended);
            AssertExprType("1.0 * 1", TypeIds.Extended);
            AssertExprType("1.0 / 1", TypeIds.Extended);
            AssertExprType("1 + 1.0", TypeIds.Extended);
            AssertExprType("1 - 1.0", TypeIds.Extended);
            AssertExprType("1 * 1.0", TypeIds.Extended);
            AssertExprType("1 / 1.0", TypeIds.Extended);
            AssertExprType("1.0 + 1.0", TypeIds.Extended);
            AssertExprType("1.0 - 1.0", TypeIds.Extended);
            AssertExprType("1.0 * 1.0", TypeIds.Extended);
            AssertExprType("1.0 / 1.0", TypeIds.Extended);
            AssertExprType("1.0 + 4294967296", TypeIds.Extended);
            AssertExprType("1.0 - 4294967296", TypeIds.Extended);
            AssertExprType("1.0 * 4294967296", TypeIds.Extended);
            AssertExprType("1.0 / 4294967296", TypeIds.Extended);
            AssertExprType("4294967296 + 1.0", TypeIds.Extended);
            AssertExprType("4294967296 - 1.0", TypeIds.Extended);
            AssertExprType("4294967296 * 1.0", TypeIds.Extended);
            AssertExprType("4294967296 / 1.0", TypeIds.Extended);
        }

        [Fact]
        public void TestConcatOperator() {
            AssertExprType("'a' + 'b'", TypeIds.UnicodeStringType);
            AssertExprType("'ac' + 'b'", TypeIds.UnicodeStringType);
            AssertExprType("'a' + 'bc'", TypeIds.UnicodeStringType);
            AssertExprType("'ac' + 'bc'", TypeIds.UnicodeStringType);
        }

        [Fact]
        public void TestBitwiseOperators() {
            AssertExprType("not 1", TypeIds.ShortInt);
            AssertExprType("not 256", TypeIds.SmallInt);
            AssertExprType("not 4294967295", TypeIds.IntegerType);
            AssertExprType("not 4294967296", TypeIds.Int64Type);

            AssertExprType("1 and 1", TypeIds.ShortInt);
            AssertExprType("1 and 256", TypeIds.SmallInt);
            AssertExprType("1 and 65536", TypeIds.IntegerType);
            AssertExprType("1 and 4294967296", TypeIds.Int64Type);
            AssertExprType("1 and 1", TypeIds.ShortInt);
            AssertExprType("256 and 1", TypeIds.SmallInt);
            AssertExprType("65536 and 1", TypeIds.IntegerType);
            AssertExprType("4294967296 and 1", TypeIds.Int64Type);

            AssertExprType("1 or 1", TypeIds.ShortInt);
            AssertExprType("1 or 256", TypeIds.SmallInt);
            AssertExprType("1 or 65536", TypeIds.IntegerType);
            AssertExprType("1 or 4294967296", TypeIds.Int64Type);
            AssertExprType("1 or 1", TypeIds.ShortInt);
            AssertExprType("256 or 1", TypeIds.SmallInt);
            AssertExprType("65536 or 1", TypeIds.IntegerType);
            AssertExprType("4294967296 or 1", TypeIds.Int64Type);

            AssertExprType("1 xor 1", TypeIds.ShortInt);
            AssertExprType("1 xor 256", TypeIds.SmallInt);
            AssertExprType("1 xor 65536", TypeIds.IntegerType);
            AssertExprType("1 xor 4294967296", TypeIds.Int64Type);
            AssertExprType("1 xor 1", TypeIds.ShortInt);
            AssertExprType("256 xor 1", TypeIds.SmallInt);
            AssertExprType("65536 xor 1", TypeIds.IntegerType);
            AssertExprType("4294967296 xor 1", TypeIds.Int64Type);
        }

        [Fact]
        public void TestShiftingOperators() {
            AssertExprType("1 shr 1", TypeIds.ShortInt);
            AssertExprType("1 shr 256", TypeIds.ShortInt);
            AssertExprType("1 shr 65536", TypeIds.ShortInt);
            AssertExprType("1 shr 4294967296", TypeIds.ShortInt);
            AssertExprType("256 shr 1", TypeIds.SmallInt);
            AssertExprType("256 shr 256", TypeIds.SmallInt);
            AssertExprType("256 shr 65536", TypeIds.SmallInt);
            AssertExprType("256 shr 4294967296", TypeIds.SmallInt);
            AssertExprType("65536 shr 1", TypeIds.IntegerType);
            AssertExprType("65536 shr 256", TypeIds.IntegerType);
            AssertExprType("65536 shr 65536", TypeIds.IntegerType);
            AssertExprType("65536 shr 4294967296", TypeIds.IntegerType);
            AssertExprType("4294967296 shr 1", TypeIds.Int64Type);
            AssertExprType("4294967296 shr 256", TypeIds.Int64Type);
            AssertExprType("4294967296 shr 65536", TypeIds.Int64Type);
            AssertExprType("4294967296 shr 4294967296", TypeIds.Int64Type);

            AssertExprType("1 shl 1", TypeIds.ShortInt);
            AssertExprType("1 shl 256", TypeIds.ShortInt);
            AssertExprType("1 shl 65536", TypeIds.ShortInt);
            AssertExprType("1 shl 4294967296", TypeIds.ShortInt);
            AssertExprType("256 shl 1", TypeIds.SmallInt);
            AssertExprType("256 shl 256", TypeIds.SmallInt);
            AssertExprType("256 shl 65536", TypeIds.SmallInt);
            AssertExprType("256 shl 4294967296", TypeIds.SmallInt);
            AssertExprType("65536 shl 1", TypeIds.IntegerType);
            AssertExprType("65536 shl 256", TypeIds.IntegerType);
            AssertExprType("65536 shl 65536", TypeIds.IntegerType);
            AssertExprType("65536 shl 4294967296", TypeIds.IntegerType);
            AssertExprType("4294967296 shl 1", TypeIds.Int64Type);
            AssertExprType("4294967296 shl 256", TypeIds.Int64Type);
            AssertExprType("4294967296 shl 65536", TypeIds.Int64Type);
            AssertExprType("4294967296 shl 4294967296", TypeIds.Int64Type);
        }

        [Fact]
        public void TestBooleanRelationalOperators() {
            AssertExprType("true = true", TypeIds.BooleanType);
            AssertExprType("true <> true", TypeIds.BooleanType);
            AssertExprType("true < true", TypeIds.BooleanType);
            AssertExprType("true > true", TypeIds.BooleanType);
            AssertExprType("true <= true", TypeIds.BooleanType);
            AssertExprType("true >= true", TypeIds.BooleanType);
        }

        [Fact]
        public void TestIntegerRelationalOperators() {
            AssertExprType("1 =  1", TypeIds.BooleanType);
            AssertExprType("1 <> 1", TypeIds.BooleanType);
            AssertExprType("1 <  1", TypeIds.BooleanType);
            AssertExprType("1 >  1", TypeIds.BooleanType);
            AssertExprType("1 <= 1", TypeIds.BooleanType);
            AssertExprType("1 >= 1", TypeIds.BooleanType);
            AssertExprType("256 =  1", TypeIds.BooleanType);
            AssertExprType("256 <> 1", TypeIds.BooleanType);
            AssertExprType("256 <  1", TypeIds.BooleanType);
            AssertExprType("256 >  1", TypeIds.BooleanType);
            AssertExprType("256 <= 1", TypeIds.BooleanType);
            AssertExprType("256 >= 1", TypeIds.BooleanType);
            AssertExprType("1 =  256", TypeIds.BooleanType);
            AssertExprType("1 <> 256", TypeIds.BooleanType);
            AssertExprType("1 <  256", TypeIds.BooleanType);
            AssertExprType("1 >  256", TypeIds.BooleanType);
            AssertExprType("1 <= 256", TypeIds.BooleanType);
            AssertExprType("1 >= 256", TypeIds.BooleanType);
            AssertExprType("65536 =  1", TypeIds.BooleanType);
            AssertExprType("65536 <> 1", TypeIds.BooleanType);
            AssertExprType("65536 <  1", TypeIds.BooleanType);
            AssertExprType("65536 >  1", TypeIds.BooleanType);
            AssertExprType("65536 <= 1", TypeIds.BooleanType);
            AssertExprType("65536 >= 1", TypeIds.BooleanType);
            AssertExprType("1 =  65536", TypeIds.BooleanType);
            AssertExprType("1 <> 65536", TypeIds.BooleanType);
            AssertExprType("1 <  65536", TypeIds.BooleanType);
            AssertExprType("1 >  65536", TypeIds.BooleanType);
            AssertExprType("1 <= 65536", TypeIds.BooleanType);
            AssertExprType("1 >= 65536", TypeIds.BooleanType);
            AssertExprType("4294967296 =  1", TypeIds.BooleanType);
            AssertExprType("4294967296 <> 1", TypeIds.BooleanType);
            AssertExprType("4294967296 <  1", TypeIds.BooleanType);
            AssertExprType("4294967296 >  1", TypeIds.BooleanType);
            AssertExprType("4294967296 <= 1", TypeIds.BooleanType);
            AssertExprType("4294967296 >= 1", TypeIds.BooleanType);
            AssertExprType("1 =  4294967296", TypeIds.BooleanType);
            AssertExprType("1 <> 4294967296", TypeIds.BooleanType);
            AssertExprType("1 <  4294967296", TypeIds.BooleanType);
            AssertExprType("1 >  4294967296", TypeIds.BooleanType);
            AssertExprType("1 <= 4294967296", TypeIds.BooleanType);
            AssertExprType("1 >= 4294967296", TypeIds.BooleanType);
        }

        [Fact]
        public void TestRealRelationalOperators() {
            AssertExprType("1.0 =  1", TypeIds.BooleanType);
            AssertExprType("1.0 <> 1", TypeIds.BooleanType);
            AssertExprType("1.0 <  1", TypeIds.BooleanType);
            AssertExprType("1.0 >  1", TypeIds.BooleanType);
            AssertExprType("1.0 <= 1", TypeIds.BooleanType);
            AssertExprType("1.0 >= 1", TypeIds.BooleanType);
            AssertExprType("1 =  1.0", TypeIds.BooleanType);
            AssertExprType("1 <> 1.0", TypeIds.BooleanType);
            AssertExprType("1 <  1.0", TypeIds.BooleanType);
            AssertExprType("1 >  1.0", TypeIds.BooleanType);
            AssertExprType("1 <= 1.0", TypeIds.BooleanType);
            AssertExprType("1 >= 1.0", TypeIds.BooleanType);
            AssertExprType("1.0 =  1.0", TypeIds.BooleanType);
            AssertExprType("1.0 <> 1.0", TypeIds.BooleanType);
            AssertExprType("1.0 <  1.0", TypeIds.BooleanType);
            AssertExprType("1.0 >  1.0", TypeIds.BooleanType);
            AssertExprType("1.0 >= 1.0", TypeIds.BooleanType);
            AssertExprType("1.0 <= 1.0", TypeIds.BooleanType);

            AssertExprType("1 =  1", TypeIds.BooleanType);
            AssertExprType("1 <> 1", TypeIds.BooleanType);
            AssertExprType("1 <  1", TypeIds.BooleanType);
            AssertExprType("1 >  1", TypeIds.BooleanType);
            AssertExprType("1 <= 1", TypeIds.BooleanType);
            AssertExprType("1 >= 1", TypeIds.BooleanType);
            AssertExprType("256 =  1", TypeIds.BooleanType);
            AssertExprType("256 <> 1", TypeIds.BooleanType);
            AssertExprType("256 <  1", TypeIds.BooleanType);
            AssertExprType("256 >  1", TypeIds.BooleanType);
            AssertExprType("256 <= 1", TypeIds.BooleanType);
            AssertExprType("256 >= 1", TypeIds.BooleanType);
            AssertExprType("1 =  256", TypeIds.BooleanType);
            AssertExprType("1 <> 256", TypeIds.BooleanType);
            AssertExprType("1 <  256", TypeIds.BooleanType);
            AssertExprType("1 >  256", TypeIds.BooleanType);
            AssertExprType("1 <= 256", TypeIds.BooleanType);
            AssertExprType("1 >= 256", TypeIds.BooleanType);
            AssertExprType("65536 =  1", TypeIds.BooleanType);
            AssertExprType("65536 <> 1", TypeIds.BooleanType);
            AssertExprType("65536 <  1", TypeIds.BooleanType);
            AssertExprType("65536 >  1", TypeIds.BooleanType);
            AssertExprType("65536 <= 1", TypeIds.BooleanType);
            AssertExprType("65536 >= 1", TypeIds.BooleanType);
            AssertExprType("1 =  65536", TypeIds.BooleanType);
            AssertExprType("1 <> 65536", TypeIds.BooleanType);
            AssertExprType("1 <  65536", TypeIds.BooleanType);
            AssertExprType("1 >  65536", TypeIds.BooleanType);
            AssertExprType("1 <= 65536", TypeIds.BooleanType);
            AssertExprType("1 >= 65536", TypeIds.BooleanType);
            AssertExprType("4294967296 =  1", TypeIds.BooleanType);
            AssertExprType("4294967296 <> 1", TypeIds.BooleanType);
            AssertExprType("4294967296 <  1", TypeIds.BooleanType);
            AssertExprType("4294967296 >  1", TypeIds.BooleanType);
            AssertExprType("4294967296 <= 1", TypeIds.BooleanType);
            AssertExprType("4294967296 >= 1", TypeIds.BooleanType);
            AssertExprType("1 =  4294967296", TypeIds.BooleanType);
            AssertExprType("1 <> 4294967296", TypeIds.BooleanType);
            AssertExprType("1 <  4294967296", TypeIds.BooleanType);
            AssertExprType("1 >  4294967296", TypeIds.BooleanType);
            AssertExprType("1 <= 4294967296", TypeIds.BooleanType);
            AssertExprType("1 >= 4294967296", TypeIds.BooleanType);
        }

        [Fact]
        public void TestStringRelationalOperators() {
            AssertExprType("'a' =  'b'", TypeIds.BooleanType);
            AssertExprType("'a' <> 'b'", TypeIds.BooleanType);
            AssertExprType("'a' <  'b'", TypeIds.BooleanType);
            AssertExprType("'a' >  'b'", TypeIds.BooleanType);
            AssertExprType("'a' <= 'b'", TypeIds.BooleanType);
            AssertExprType("'a' >= 'b'", TypeIds.BooleanType);

            AssertExprType("'a1' =  'b'", TypeIds.BooleanType);
            AssertExprType("'a1' <> 'b'", TypeIds.BooleanType);
            AssertExprType("'a1' <  'b'", TypeIds.BooleanType);
            AssertExprType("'a1' >  'b'", TypeIds.BooleanType);
            AssertExprType("'a1' <= 'b'", TypeIds.BooleanType);
            AssertExprType("'a1' >= 'b'", TypeIds.BooleanType);

            AssertExprType("'a' =  'b1'", TypeIds.BooleanType);
            AssertExprType("'a' <> 'b1'", TypeIds.BooleanType);
            AssertExprType("'a' <  'b1'", TypeIds.BooleanType);
            AssertExprType("'a' >  'b1'", TypeIds.BooleanType);
            AssertExprType("'a' <= 'b1'", TypeIds.BooleanType);
            AssertExprType("'a' >= 'b1'", TypeIds.BooleanType);
        }


    }
}
