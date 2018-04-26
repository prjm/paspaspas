using System;
using PasPasPas.Global.Constants;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test cases for typed expressions
    /// </summary>
    public class TypedExpressionTest : TypeTest {

        [TestCase]
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

        [TestCase]
        public void TestBooleanLiteralTypes() {
            AssertExprType("true", KnownTypeIds.BooleanType);
            AssertExprType("false", KnownTypeIds.BooleanType);
        }

        [TestCase]
        public void TestCharLiteralTypes() {
            AssertExprType("'C'", KnownTypeIds.WideCharType);
            AssertExprType("#9", KnownTypeIds.WideCharType);
        }

        [TestCase]
        public void TestStringLiteralTypes() {
            AssertExprType("'CD'", KnownTypeIds.UnicodeStringType);
            AssertExprType("#9#9", KnownTypeIds.UnicodeStringType);
        }

        [TestCase]
        public void TestExtendedLiteralTypes() {
            AssertExprType("3.5", KnownTypeIds.Extended);
            AssertExprType("2.33434343", KnownTypeIds.Extended);
        }

        [TestCase]
        public void TestBooleanOperators() {
            AssertExprType("not true", KnownTypeIds.BooleanType);
            AssertExprType("not false", KnownTypeIds.BooleanType);
            AssertExprType("true and false", KnownTypeIds.BooleanType);
            AssertExprType("true or false", KnownTypeIds.BooleanType);
            AssertExprType("true xor false", KnownTypeIds.BooleanType);
        }

        [TestCase]
        public void TestArithmeticOperatorsInteger() {
            AssertExprType("+ 1", KnownTypeIds.ShortInt);
            AssertExprType("- 1", KnownTypeIds.ShortInt);
            AssertExprType("1 + 1", KnownTypeIds.ShortInt);
            AssertExprType("1 - 1", KnownTypeIds.ShortInt);
            AssertExprType("1 * 1", KnownTypeIds.ShortInt);
            AssertExprType("1 div 1", KnownTypeIds.ShortInt);
            AssertExprType("1 mod 1", KnownTypeIds.ShortInt);
            AssertExprType("1 / 1", KnownTypeIds.Extended);
        }

        [TestCase]
        public void TestArithmeticOperatorsInt64() {
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

        [TestCase]
        public void TestArithmeticOperatorsReal() {
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

        [TestCase]
        public void TestConcatOperator() {
            AssertExprType("'a' + 'b'", KnownTypeIds.UnicodeStringType);
            AssertExprType("'ac' + 'b'", KnownTypeIds.UnicodeStringType);
            AssertExprType("'a' + 'bc'", KnownTypeIds.UnicodeStringType);
            AssertExprType("'ac' + 'bc'", KnownTypeIds.UnicodeStringType);
        }

        [TestCase]
        public void TestBitwiseOperators() {
            AssertExprType("not 1", KnownTypeIds.ShortInt);
            AssertExprType("not 256", KnownTypeIds.SmallInt);
            AssertExprType("not 4294967295", KnownTypeIds.ShortInt);
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

        [TestCase]
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

        [TestCase]
        public void TestBooleanRelationalOperators() {
            AssertExprType("true = true", KnownTypeIds.BooleanType);
            AssertExprType("true <> true", KnownTypeIds.BooleanType);
            AssertExprType("true < true", KnownTypeIds.BooleanType);
            AssertExprType("true > true", KnownTypeIds.BooleanType);
            AssertExprType("true <= true", KnownTypeIds.BooleanType);
            AssertExprType("true >= true", KnownTypeIds.BooleanType);
        }

        [TestCase]
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

        [TestCase]
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

        [TestCase]
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

        [TestCase]
        public void TestArithmeticOperatorsIndirect() {
            var d = new[] {
                Tuple.Create("Byte", KnownTypeIds.IntegerType),
                Tuple.Create("Word", KnownTypeIds.IntegerType),
                Tuple.Create("Cardinal", KnownTypeIds.CardinalType),
                Tuple.Create("UInt64", KnownTypeIds.Uint64Type),
                Tuple.Create("SmallInt", KnownTypeIds.SmallInt),
                Tuple.Create("ShortInt", KnownTypeIds.ShortInt),
                Tuple.Create("Integer", KnownTypeIds.IntegerType),
                Tuple.Create("Int64", KnownTypeIds.Int64Type),
            };

            foreach (var e in d) {
                AssertExprTypeByVar(e.Item1, "a + b", e.Item2);
                AssertExprTypeByVar(e.Item1, "a - b", e.Item2);
                AssertExprTypeByVar(e.Item1, "a div b", e.Item2);
                AssertExprTypeByVar(e.Item1, "a * b", e.Item2);
                AssertExprTypeByVar(e.Item1, "a mod b", e.Item2);
                AssertExprTypeByVar(e.Item1, "a / b", KnownTypeIds.Extended);
            }
        }

    }
}
