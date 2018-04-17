using PasPasPas.Global.Constants;
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
            AssertExprType("-128", KnownTypeIds.ShortInt);
            AssertExprType("0", KnownTypeIds.ShortInt);
            AssertExprType("127", KnownTypeIds.ShortInt);
            AssertExprType("128", KnownTypeIds.ByteType);
            AssertExprType("255", KnownTypeIds.ByteType);
            AssertExprType("-129", KnownTypeIds.SmallInt);
            AssertExprType("256", KnownTypeIds.SmallInt);
            AssertExprType("-32768", KnownTypeIds.SmallInt);
            AssertExprType("-32769", KnownTypeIds.IntegerType);
            AssertExprType("32767", KnownTypeIds.SmallInt);
            AssertExprType("32768", KnownTypeIds.WordType);
            AssertExprType("65535", KnownTypeIds.WordType);
            AssertExprType("65536", KnownTypeIds.IntegerType);
            AssertExprType("2147483648", KnownTypeIds.CardinalType);
            AssertExprType("-2147483648", KnownTypeIds.IntegerType);
            AssertExprType("-2147483649", KnownTypeIds.Int64Type);
            AssertExprType("4294967295", KnownTypeIds.CardinalType);
            AssertExprType("4294967296", KnownTypeIds.Int64Type);
            AssertExprType("9223372036854775807", KnownTypeIds.Int64Type);
            AssertExprType("18446744073709551615", KnownTypeIds.Uint64Type);
        }

        [Fact]
        public void TestBooleanLiteralTypes() {
            AssertExprType("true", KnownTypeIds.BooleanType);
            AssertExprType("false", KnownTypeIds.BooleanType);
        }

        [Fact]
        public void TestCharLiteralTypes() {
            AssertExprType("'C'", KnownTypeIds.WideCharType);
            AssertExprType("#9", KnownTypeIds.WideCharType);
        }

        [Fact]
        public void TestStringLiteralTypes() {
            AssertExprType("'CD'", KnownTypeIds.UnicodeStringType);
            AssertExprType("#9#9", KnownTypeIds.UnicodeStringType);
        }

        [Fact]
        public void TestExtendedLiteralTypes() {
            AssertExprType("3.5", KnownTypeIds.Extended);
            AssertExprType("2.33434343", KnownTypeIds.Extended);
        }

        [Fact]
        public void TestBooleanOperators() {
            AssertExprType("not true", KnownTypeIds.BooleanType);
            AssertExprType("not false", KnownTypeIds.BooleanType);
            AssertExprType("true and false", KnownTypeIds.BooleanType);
            AssertExprType("true or false", KnownTypeIds.BooleanType);
            AssertExprType("true xor false", KnownTypeIds.BooleanType);
        }

        [Fact]
        public void TestArithmetikOperatorsInteger() {
            AssertExprType("+ 1", KnownTypeIds.ShortInt);
            AssertExprType("- 1", KnownTypeIds.ShortInt);
            AssertExprType("1 + 1", KnownTypeIds.ShortInt);
            AssertExprType("1 - 1", KnownTypeIds.ShortInt);
            AssertExprType("1 * 1", KnownTypeIds.ShortInt);
            AssertExprType("1 div 1", KnownTypeIds.ShortInt);
            AssertExprType("1 mod 1", KnownTypeIds.ShortInt);
            AssertExprType("1 / 1", KnownTypeIds.Extended);
        }

        [Fact]
        public void TestArithmetikOperatorsInt64() {
            AssertExprType("+ 4294967296", KnownTypeIds.Int64Type);
            AssertExprType("- 4294967296", KnownTypeIds.Int64Type);
            AssertExprType("4294967296 + 1", KnownTypeIds.Int64Type);
            AssertExprType("4294967296 - 1", KnownTypeIds.CardinalType);
            AssertExprType("4294967296 * 1", KnownTypeIds.Int64Type);
            AssertExprType("4294967296 div 1", KnownTypeIds.Int64Type);
            AssertExprType("4294967296 mod 1", KnownTypeIds.ShortInt);
            AssertExprType("4294967296 / 1", KnownTypeIds.Extended);
            AssertExprType("1 + 4294967296", KnownTypeIds.Int64Type);
            AssertExprType("1 - 4294967296", KnownTypeIds.Int64Type);
            AssertExprType("1 * 4294967296", KnownTypeIds.Int64Type);
            AssertExprType("1 div 4294967296", KnownTypeIds.ShortInt);
            AssertExprType("1 mod 4294967296", KnownTypeIds.ShortInt);
            AssertExprType("1 / 4294967296", KnownTypeIds.Extended);
            AssertExprType("4294967296 + 4294967296", KnownTypeIds.Int64Type);
            AssertExprType("4294967296 - 4294967296", KnownTypeIds.ShortInt);
            AssertExprType("4294967296 * 3", KnownTypeIds.Int64Type);
            AssertExprType("4294967296 div 4294967296", KnownTypeIds.ShortInt);
            AssertExprType("4294967296 mod 4294967296", KnownTypeIds.ShortInt);
            AssertExprType("4294967296 / 4294967296", KnownTypeIds.Extended);
        }

        [Fact]
        public void TestArithmetikOperatorsReal() {
            AssertExprType("+ 1.0", KnownTypeIds.Extended);
            AssertExprType("- 1.0", KnownTypeIds.Extended);
            AssertExprType("1.0 + 1", KnownTypeIds.Extended);
            AssertExprType("1.0 - 1", KnownTypeIds.Extended);
            AssertExprType("1.0 * 1", KnownTypeIds.Extended);
            AssertExprType("1.0 / 1", KnownTypeIds.Extended);
            AssertExprType("1 + 1.0", KnownTypeIds.Extended);
            AssertExprType("1 - 1.0", KnownTypeIds.Extended);
            AssertExprType("1 * 1.0", KnownTypeIds.Extended);
            AssertExprType("1 / 1.0", KnownTypeIds.Extended);
            AssertExprType("1.0 + 1.0", KnownTypeIds.Extended);
            AssertExprType("1.0 - 1.0", KnownTypeIds.Extended);
            AssertExprType("1.0 * 1.0", KnownTypeIds.Extended);
            AssertExprType("1.0 / 1.0", KnownTypeIds.Extended);
            AssertExprType("1.0 + 4294967296", KnownTypeIds.Extended);
            AssertExprType("1.0 - 4294967296", KnownTypeIds.Extended);
            AssertExprType("1.0 * 4294967296", KnownTypeIds.Extended);
            AssertExprType("1.0 / 4294967296", KnownTypeIds.Extended);
            AssertExprType("4294967296 + 1.0", KnownTypeIds.Extended);
            AssertExprType("4294967296 - 1.0", KnownTypeIds.Extended);
            AssertExprType("4294967296 * 1.0", KnownTypeIds.Extended);
            AssertExprType("4294967296 / 1.0", KnownTypeIds.Extended);
        }

        [Fact]
        public void TestConcatOperator() {
            AssertExprType("'a' + 'b'", KnownTypeIds.UnicodeStringType);
            AssertExprType("'ac' + 'b'", KnownTypeIds.UnicodeStringType);
            AssertExprType("'a' + 'bc'", KnownTypeIds.UnicodeStringType);
            AssertExprType("'ac' + 'bc'", KnownTypeIds.UnicodeStringType);
        }

        [Fact]
        public void TestBitwiseOperators() {
            AssertExprType("not 1", KnownTypeIds.ShortInt);
            AssertExprType("not 256", KnownTypeIds.SmallInt);
            AssertExprType("not 4294967295", KnownTypeIds.CardinalType);
            AssertExprType("not 4294967296", KnownTypeIds.Int64Type);

            AssertExprType("1 and 1", KnownTypeIds.ShortInt);
            AssertExprType("1 and 256", KnownTypeIds.ShortInt);
            AssertExprType("277 and 256", KnownTypeIds.SmallInt);
            AssertExprType("1 and 65536", KnownTypeIds.ShortInt);
            AssertExprType("1 and 4294967296", KnownTypeIds.ShortInt);
            AssertExprType("1 and 1", KnownTypeIds.ShortInt);
            AssertExprType("256 and 1", KnownTypeIds.ShortInt);
            AssertExprType("65536 and 1", KnownTypeIds.ShortInt);
            AssertExprType("4294967296 and 1", KnownTypeIds.ShortInt);

            AssertExprType("1 or 1", KnownTypeIds.ShortInt);
            AssertExprType("1 or 256", KnownTypeIds.SmallInt);
            AssertExprType("1 or 65536", KnownTypeIds.IntegerType);
            AssertExprType("1 or 4294967296", KnownTypeIds.Int64Type);
            AssertExprType("1 or 1", KnownTypeIds.ShortInt);
            AssertExprType("256 or 1", KnownTypeIds.SmallInt);
            AssertExprType("65536 or 1", KnownTypeIds.IntegerType);
            AssertExprType("4294967296 or 1", KnownTypeIds.Int64Type);

            AssertExprType("1 xor 1", KnownTypeIds.ShortInt);
            AssertExprType("1 xor 256", KnownTypeIds.SmallInt);
            AssertExprType("1 xor 65536", KnownTypeIds.IntegerType);
            AssertExprType("1 xor 4294967296", KnownTypeIds.Int64Type);
            AssertExprType("1 xor 1", KnownTypeIds.ShortInt);
            AssertExprType("256 xor 1", KnownTypeIds.SmallInt);
            AssertExprType("65536 xor 1", KnownTypeIds.IntegerType);
            AssertExprType("4294967296 xor 1", KnownTypeIds.Int64Type);
        }

        [Fact]
        public void TestShiftingOperators() {
            AssertExprType("1 shr 1", KnownTypeIds.ShortInt);
            AssertExprType("1 shr 256", KnownTypeIds.ShortInt);
            AssertExprType("1 shr 65536", KnownTypeIds.ShortInt);
            AssertExprType("1 shr 4294967296", KnownTypeIds.ShortInt);
            AssertExprType("256 shr 1", KnownTypeIds.ByteType);
            AssertExprType("256 shr 256", KnownTypeIds.SmallInt);
            AssertExprType("256 shr 65536", KnownTypeIds.SmallInt);
            AssertExprType("256 shr 4294967296", KnownTypeIds.SmallInt);
            AssertExprType("65536 shr 1", KnownTypeIds.WordType);
            AssertExprType("65536 shr 256", KnownTypeIds.IntegerType);
            AssertExprType("65536 shr 65536", KnownTypeIds.IntegerType);
            AssertExprType("65536 shr 4294967296", KnownTypeIds.IntegerType);
            AssertExprType("4294967296 shr 1", KnownTypeIds.CardinalType);
            AssertExprType("4294967296 shr 256", KnownTypeIds.Int64Type);
            AssertExprType("4294967296 shr 65536", KnownTypeIds.Int64Type);
            AssertExprType("4294967296 shr 4294967296", KnownTypeIds.Int64Type);

            AssertExprType("1 shl 1", KnownTypeIds.ShortInt);
            AssertExprType("1 shl 256", KnownTypeIds.ShortInt);
            AssertExprType("1 shl 65536", KnownTypeIds.ShortInt);
            AssertExprType("1 shl 4294967296", KnownTypeIds.ShortInt);
            AssertExprType("256 shl 1", KnownTypeIds.SmallInt);
            AssertExprType("256 shl 256", KnownTypeIds.SmallInt);
            AssertExprType("256 shl 65536", KnownTypeIds.SmallInt);
            AssertExprType("256 shl 4294967296", KnownTypeIds.SmallInt);
            AssertExprType("65536 shl 1", KnownTypeIds.IntegerType);
            AssertExprType("65536 shl 256", KnownTypeIds.IntegerType);
            AssertExprType("65536 shl 65536", KnownTypeIds.IntegerType);
            AssertExprType("65536 shl 4294967296", KnownTypeIds.IntegerType);
            AssertExprType("4294967296 shl 1", KnownTypeIds.Int64Type);
            AssertExprType("4294967296 shl 256", KnownTypeIds.Int64Type);
            AssertExprType("4294967296 shl 65536", KnownTypeIds.Int64Type);
            AssertExprType("4294967296 shl 4294967296", KnownTypeIds.Int64Type);
        }

        [Fact]
        public void TestBooleanRelationalOperators() {
            AssertExprType("true = true", KnownTypeIds.BooleanType);
            AssertExprType("true <> true", KnownTypeIds.BooleanType);
            AssertExprType("true < true", KnownTypeIds.BooleanType);
            AssertExprType("true > true", KnownTypeIds.BooleanType);
            AssertExprType("true <= true", KnownTypeIds.BooleanType);
            AssertExprType("true >= true", KnownTypeIds.BooleanType);
        }

        [Fact]
        public void TestIntegerRelationalOperators() {
            AssertExprType("1 =  1", KnownTypeIds.BooleanType);
            AssertExprType("1 <> 1", KnownTypeIds.BooleanType);
            AssertExprType("1 <  1", KnownTypeIds.BooleanType);
            AssertExprType("1 >  1", KnownTypeIds.BooleanType);
            AssertExprType("1 <= 1", KnownTypeIds.BooleanType);
            AssertExprType("1 >= 1", KnownTypeIds.BooleanType);
            AssertExprType("256 =  1", KnownTypeIds.BooleanType);
            AssertExprType("256 <> 1", KnownTypeIds.BooleanType);
            AssertExprType("256 <  1", KnownTypeIds.BooleanType);
            AssertExprType("256 >  1", KnownTypeIds.BooleanType);
            AssertExprType("256 <= 1", KnownTypeIds.BooleanType);
            AssertExprType("256 >= 1", KnownTypeIds.BooleanType);
            AssertExprType("1 =  256", KnownTypeIds.BooleanType);
            AssertExprType("1 <> 256", KnownTypeIds.BooleanType);
            AssertExprType("1 <  256", KnownTypeIds.BooleanType);
            AssertExprType("1 >  256", KnownTypeIds.BooleanType);
            AssertExprType("1 <= 256", KnownTypeIds.BooleanType);
            AssertExprType("1 >= 256", KnownTypeIds.BooleanType);
            AssertExprType("65536 =  1", KnownTypeIds.BooleanType);
            AssertExprType("65536 <> 1", KnownTypeIds.BooleanType);
            AssertExprType("65536 <  1", KnownTypeIds.BooleanType);
            AssertExprType("65536 >  1", KnownTypeIds.BooleanType);
            AssertExprType("65536 <= 1", KnownTypeIds.BooleanType);
            AssertExprType("65536 >= 1", KnownTypeIds.BooleanType);
            AssertExprType("1 =  65536", KnownTypeIds.BooleanType);
            AssertExprType("1 <> 65536", KnownTypeIds.BooleanType);
            AssertExprType("1 <  65536", KnownTypeIds.BooleanType);
            AssertExprType("1 >  65536", KnownTypeIds.BooleanType);
            AssertExprType("1 <= 65536", KnownTypeIds.BooleanType);
            AssertExprType("1 >= 65536", KnownTypeIds.BooleanType);
            AssertExprType("4294967296 =  1", KnownTypeIds.BooleanType);
            AssertExprType("4294967296 <> 1", KnownTypeIds.BooleanType);
            AssertExprType("4294967296 <  1", KnownTypeIds.BooleanType);
            AssertExprType("4294967296 >  1", KnownTypeIds.BooleanType);
            AssertExprType("4294967296 <= 1", KnownTypeIds.BooleanType);
            AssertExprType("4294967296 >= 1", KnownTypeIds.BooleanType);
            AssertExprType("1 =  4294967296", KnownTypeIds.BooleanType);
            AssertExprType("1 <> 4294967296", KnownTypeIds.BooleanType);
            AssertExprType("1 <  4294967296", KnownTypeIds.BooleanType);
            AssertExprType("1 >  4294967296", KnownTypeIds.BooleanType);
            AssertExprType("1 <= 4294967296", KnownTypeIds.BooleanType);
            AssertExprType("1 >= 4294967296", KnownTypeIds.BooleanType);
        }

        [Fact]
        public void TestRealRelationalOperators() {
            AssertExprType("1.0 =  1", KnownTypeIds.BooleanType);
            AssertExprType("1.0 <> 1", KnownTypeIds.BooleanType);
            AssertExprType("1.0 <  1", KnownTypeIds.BooleanType);
            AssertExprType("1.0 >  1", KnownTypeIds.BooleanType);
            AssertExprType("1.0 <= 1", KnownTypeIds.BooleanType);
            AssertExprType("1.0 >= 1", KnownTypeIds.BooleanType);
            AssertExprType("1 =  1.0", KnownTypeIds.BooleanType);
            AssertExprType("1 <> 1.0", KnownTypeIds.BooleanType);
            AssertExprType("1 <  1.0", KnownTypeIds.BooleanType);
            AssertExprType("1 >  1.0", KnownTypeIds.BooleanType);
            AssertExprType("1 <= 1.0", KnownTypeIds.BooleanType);
            AssertExprType("1 >= 1.0", KnownTypeIds.BooleanType);
            AssertExprType("1.0 =  1.0", KnownTypeIds.BooleanType);
            AssertExprType("1.0 <> 1.0", KnownTypeIds.BooleanType);
            AssertExprType("1.0 <  1.0", KnownTypeIds.BooleanType);
            AssertExprType("1.0 >  1.0", KnownTypeIds.BooleanType);
            AssertExprType("1.0 >= 1.0", KnownTypeIds.BooleanType);
            AssertExprType("1.0 <= 1.0", KnownTypeIds.BooleanType);

            AssertExprType("1 =  1", KnownTypeIds.BooleanType);
            AssertExprType("1 <> 1", KnownTypeIds.BooleanType);
            AssertExprType("1 <  1", KnownTypeIds.BooleanType);
            AssertExprType("1 >  1", KnownTypeIds.BooleanType);
            AssertExprType("1 <= 1", KnownTypeIds.BooleanType);
            AssertExprType("1 >= 1", KnownTypeIds.BooleanType);
            AssertExprType("256 =  1", KnownTypeIds.BooleanType);
            AssertExprType("256 <> 1", KnownTypeIds.BooleanType);
            AssertExprType("256 <  1", KnownTypeIds.BooleanType);
            AssertExprType("256 >  1", KnownTypeIds.BooleanType);
            AssertExprType("256 <= 1", KnownTypeIds.BooleanType);
            AssertExprType("256 >= 1", KnownTypeIds.BooleanType);
            AssertExprType("1 =  256", KnownTypeIds.BooleanType);
            AssertExprType("1 <> 256", KnownTypeIds.BooleanType);
            AssertExprType("1 <  256", KnownTypeIds.BooleanType);
            AssertExprType("1 >  256", KnownTypeIds.BooleanType);
            AssertExprType("1 <= 256", KnownTypeIds.BooleanType);
            AssertExprType("1 >= 256", KnownTypeIds.BooleanType);
            AssertExprType("65536 =  1", KnownTypeIds.BooleanType);
            AssertExprType("65536 <> 1", KnownTypeIds.BooleanType);
            AssertExprType("65536 <  1", KnownTypeIds.BooleanType);
            AssertExprType("65536 >  1", KnownTypeIds.BooleanType);
            AssertExprType("65536 <= 1", KnownTypeIds.BooleanType);
            AssertExprType("65536 >= 1", KnownTypeIds.BooleanType);
            AssertExprType("1 =  65536", KnownTypeIds.BooleanType);
            AssertExprType("1 <> 65536", KnownTypeIds.BooleanType);
            AssertExprType("1 <  65536", KnownTypeIds.BooleanType);
            AssertExprType("1 >  65536", KnownTypeIds.BooleanType);
            AssertExprType("1 <= 65536", KnownTypeIds.BooleanType);
            AssertExprType("1 >= 65536", KnownTypeIds.BooleanType);
            AssertExprType("4294967296 =  1", KnownTypeIds.BooleanType);
            AssertExprType("4294967296 <> 1", KnownTypeIds.BooleanType);
            AssertExprType("4294967296 <  1", KnownTypeIds.BooleanType);
            AssertExprType("4294967296 >  1", KnownTypeIds.BooleanType);
            AssertExprType("4294967296 <= 1", KnownTypeIds.BooleanType);
            AssertExprType("4294967296 >= 1", KnownTypeIds.BooleanType);
            AssertExprType("1 =  4294967296", KnownTypeIds.BooleanType);
            AssertExprType("1 <> 4294967296", KnownTypeIds.BooleanType);
            AssertExprType("1 <  4294967296", KnownTypeIds.BooleanType);
            AssertExprType("1 >  4294967296", KnownTypeIds.BooleanType);
            AssertExprType("1 <= 4294967296", KnownTypeIds.BooleanType);
            AssertExprType("1 >= 4294967296", KnownTypeIds.BooleanType);
        }

        [Fact]
        public void TestStringRelationalOperators() {
            AssertExprType("'a' =  'b'", KnownTypeIds.BooleanType);
            AssertExprType("'a' <> 'b'", KnownTypeIds.BooleanType);
            AssertExprType("'a' <  'b'", KnownTypeIds.BooleanType);
            AssertExprType("'a' >  'b'", KnownTypeIds.BooleanType);
            AssertExprType("'a' <= 'b'", KnownTypeIds.BooleanType);
            AssertExprType("'a' >= 'b'", KnownTypeIds.BooleanType);

            AssertExprType("'a1' =  'b'", KnownTypeIds.BooleanType);
            AssertExprType("'a1' <> 'b'", KnownTypeIds.BooleanType);
            AssertExprType("'a1' <  'b'", KnownTypeIds.BooleanType);
            AssertExprType("'a1' >  'b'", KnownTypeIds.BooleanType);
            AssertExprType("'a1' <= 'b'", KnownTypeIds.BooleanType);
            AssertExprType("'a1' >= 'b'", KnownTypeIds.BooleanType);

            AssertExprType("'a' =  'b1'", KnownTypeIds.BooleanType);
            AssertExprType("'a' <> 'b1'", KnownTypeIds.BooleanType);
            AssertExprType("'a' <  'b1'", KnownTypeIds.BooleanType);
            AssertExprType("'a' >  'b1'", KnownTypeIds.BooleanType);
            AssertExprType("'a' <= 'b1'", KnownTypeIds.BooleanType);
            AssertExprType("'a' >= 'b1'", KnownTypeIds.BooleanType);
        }


    }
}
