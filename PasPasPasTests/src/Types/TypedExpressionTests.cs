#nullable disable
using System;
using System.Collections.Immutable;
using PasPasPas.Globals.Types;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test cases for typed expressions
    /// </summary>
    public class TypedExpressionTest : TypeTest {

        private ISystemUnit KnownTypeIds
            => CreateEnvironment().TypeRegistry.SystemUnit;

        /// <summary>
        ///     test integer literals
        /// </summary>
        [TestMethod]
        public void TestIntegerLiteralTypes() {
            AssertExprType("-128", KnownTypeIds.ShortIntType);
            AssertExprType("0", KnownTypeIds.ShortIntType);
            AssertExprType("127", KnownTypeIds.ShortIntType);
            AssertExprType("128", KnownTypeIds.ByteType);
            AssertExprType("255", KnownTypeIds.ByteType);
            AssertExprType("-129", KnownTypeIds.SmallIntType);
            AssertExprType("256", KnownTypeIds.SmallIntType);
            AssertExprType("-32768", KnownTypeIds.SmallIntType);
            AssertExprType("-32769", KnownTypeIds.IntegerType);
            AssertExprType("32767", KnownTypeIds.SmallIntType);
            AssertExprType("32768", KnownTypeIds.WordType);
            AssertExprType("65535", KnownTypeIds.WordType);
            AssertExprType("65536", KnownTypeIds.IntegerType);
            AssertExprType("2147483648", KnownTypeIds.CardinalType);
            AssertExprType("-2147483648", KnownTypeIds.IntegerType);
            AssertExprType("-2147483649", KnownTypeIds.Int64Type);
            AssertExprType("4294967295", KnownTypeIds.CardinalType);
            AssertExprType("4294967296", KnownTypeIds.Int64Type);
            AssertExprType("9223372036854775807", KnownTypeIds.Int64Type);
            AssertExprType("18446744073709551615", KnownTypeIds.UInt64Type);
        }

        /// <summary>
        ///     test boolean literals
        /// </summary>
        [TestMethod]
        public void TestBooleanLiteralTypes() {
            AssertExprType("true", KnownTypeIds.BooleanType);
            AssertExprType("false", KnownTypeIds.BooleanType);
        }

        /// <summary>
        ///     test char literals
        /// </summary>
        [TestMethod]
        public void TestCharLiteralTypes() {
            AssertExprType("'C'", KnownTypeIds.WideCharType);
            AssertExprType("#9", KnownTypeIds.WideCharType);
        }

        /// <summary>
        ///     test string literals
        /// </summary>
        [TestMethod]
        public void TestStringLiteralTypes() {
            AssertExprType("'CD'", KnownTypeIds.UnicodeStringType);
            AssertExprType("#9#9", KnownTypeIds.UnicodeStringType);
        }

        /// <summary>
        ///     test extended literals
        /// </summary>
        [TestMethod]
        public void TestExtendedLiteralTypes() {
            AssertExprType("3.5", KnownTypeIds.ExtendedType);
            AssertExprType("2.33434343", KnownTypeIds.ExtendedType);
        }

        /// <summary>
        ///     test boolean operators
        /// </summary>
        [TestMethod]
        public void TestBooleanOperators() {
            AssertExprType("not true", KnownTypeIds.BooleanType);
            AssertExprType("not false", KnownTypeIds.BooleanType);
            AssertExprType("true and false", KnownTypeIds.BooleanType);
            AssertExprType("true or false", KnownTypeIds.BooleanType);
            AssertExprType("true xor false", KnownTypeIds.BooleanType);
        }

        /// <summary>
        ///     test arithmetic operators
        /// </summary>
        [TestMethod]
        public void TestArithmeticOperatorsInteger() {
            AssertExprType("+ 1", KnownTypeIds.ShortIntType);
            AssertExprType("- 1", KnownTypeIds.ShortIntType);
            AssertExprType("1 + 1", KnownTypeIds.ShortIntType);
            AssertExprType("1 - 1", KnownTypeIds.ShortIntType);
            AssertExprType("1 * 1", KnownTypeIds.ShortIntType);
            AssertExprType("1 div 1", KnownTypeIds.ShortIntType);
            AssertExprType("1 mod 1", KnownTypeIds.ShortIntType);
            AssertExprType("1 / 1", KnownTypeIds.ExtendedType);
        }

        /// <summary>
        ///     test arithmetic operators
        /// </summary>
        [TestMethod]
        public void TestArithmeticOperatorsInt64() {
            AssertExprType("+ 4294967296", KnownTypeIds.Int64Type);
            AssertExprType("- 4294967296", KnownTypeIds.Int64Type);
            AssertExprType("4294967296 + 1", KnownTypeIds.Int64Type);
            AssertExprType("4294967296 - 1", KnownTypeIds.CardinalType);
            AssertExprType("4294967296 * 1", KnownTypeIds.Int64Type);
            AssertExprType("4294967296 div 1", KnownTypeIds.Int64Type);
            AssertExprType("4294967296 mod 1", KnownTypeIds.ShortIntType);
            AssertExprType("4294967296 / 1", KnownTypeIds.ExtendedType);
            AssertExprType("1 + 4294967296", KnownTypeIds.Int64Type);
            AssertExprType("1 - 4294967296", KnownTypeIds.Int64Type);
            AssertExprType("1 * 4294967296", KnownTypeIds.Int64Type);
            AssertExprType("1 div 4294967296", KnownTypeIds.ShortIntType);
            AssertExprType("1 mod 4294967296", KnownTypeIds.ShortIntType);
            AssertExprType("1 / 4294967296", KnownTypeIds.ExtendedType);
            AssertExprType("4294967296 + 4294967296", KnownTypeIds.Int64Type);
            AssertExprType("4294967296 - 4294967296", KnownTypeIds.ShortIntType);
            AssertExprType("4294967296 * 3", KnownTypeIds.Int64Type);
            AssertExprType("4294967296 div 4294967296", KnownTypeIds.ShortIntType);
            AssertExprType("4294967296 mod 4294967296", KnownTypeIds.ShortIntType);
            AssertExprType("4294967296 / 4294967296", KnownTypeIds.ExtendedType);
        }

        /// <summary>
        ///     test arithmetic operators
        /// </summary>
        [TestMethod]
        public void TestArithmeticOperatorsReal() {
            AssertExprType("+ 1.0", KnownTypeIds.ExtendedType);
            AssertExprType("- 1.0", KnownTypeIds.ExtendedType);
            AssertExprType("1.0 + 1", KnownTypeIds.ExtendedType);
            AssertExprType("1.0 - 1", KnownTypeIds.ExtendedType);
            AssertExprType("1.0 * 1", KnownTypeIds.ExtendedType);
            AssertExprType("1.0 / 1", KnownTypeIds.ExtendedType);
            AssertExprType("1 + 1.0", KnownTypeIds.ExtendedType);
            AssertExprType("1 - 1.0", KnownTypeIds.ExtendedType);
            AssertExprType("1 * 1.0", KnownTypeIds.ExtendedType);
            AssertExprType("1 / 1.0", KnownTypeIds.ExtendedType);
            AssertExprType("1.0 + 1.0", KnownTypeIds.ExtendedType);
            AssertExprType("1.0 - 1.0", KnownTypeIds.ExtendedType);
            AssertExprType("1.0 * 1.0", KnownTypeIds.ExtendedType);
            AssertExprType("1.0 / 1.0", KnownTypeIds.ExtendedType);
            AssertExprType("1.0 + 4294967296", KnownTypeIds.ExtendedType);
            AssertExprType("1.0 - 4294967296", KnownTypeIds.ExtendedType);
            AssertExprType("1.0 * 4294967296", KnownTypeIds.ExtendedType);
            AssertExprType("1.0 / 4294967296", KnownTypeIds.ExtendedType);
            AssertExprType("4294967296 + 1.0", KnownTypeIds.ExtendedType);
            AssertExprType("4294967296 - 1.0", KnownTypeIds.ExtendedType);
            AssertExprType("4294967296 * 1.0", KnownTypeIds.ExtendedType);
            AssertExprType("4294967296 / 1.0", KnownTypeIds.ExtendedType);
        }

        /// <summary>
        ///     test concatenate operator
        /// </summary>
        [TestMethod]
        public void TestConcatOperator() {
            AssertExprType("'a' + 'b'", KnownTypeIds.UnicodeStringType);
            AssertExprType("'ac' + 'b'", KnownTypeIds.UnicodeStringType);
            AssertExprType("'a' + 'bc'", KnownTypeIds.UnicodeStringType);
            AssertExprType("'ac' + 'bc'", KnownTypeIds.UnicodeStringType);
        }

        /// <summary>
        ///     test bitwise operators
        /// </summary>
        [TestMethod]
        public void TestBitwiseOperators() {
            AssertExprType("not 1", KnownTypeIds.ShortIntType);
            AssertExprType("not 256", KnownTypeIds.SmallIntType);
            AssertExprType("not 4294967295", KnownTypeIds.ShortIntType);
            AssertExprType("not 4294967296", KnownTypeIds.Int64Type);

            AssertExprType("1 and 1", KnownTypeIds.ShortIntType);
            AssertExprType("1 and 256", KnownTypeIds.ShortIntType);
            AssertExprType("277 and 256", KnownTypeIds.SmallIntType);
            AssertExprType("1 and 65536", KnownTypeIds.ShortIntType);
            AssertExprType("1 and 4294967296", KnownTypeIds.ShortIntType);
            AssertExprType("1 and 1", KnownTypeIds.ShortIntType);
            AssertExprType("256 and 1", KnownTypeIds.ShortIntType);
            AssertExprType("65536 and 1", KnownTypeIds.ShortIntType);
            AssertExprType("4294967296 and 1", KnownTypeIds.ShortIntType);

            AssertExprType("1 or 1", KnownTypeIds.ShortIntType);
            AssertExprType("1 or 256", KnownTypeIds.SmallIntType);
            AssertExprType("1 or 65536", KnownTypeIds.IntegerType);
            AssertExprType("1 or 4294967296", KnownTypeIds.Int64Type);
            AssertExprType("1 or 1", KnownTypeIds.ShortIntType);
            AssertExprType("256 or 1", KnownTypeIds.SmallIntType);
            AssertExprType("65536 or 1", KnownTypeIds.IntegerType);
            AssertExprType("4294967296 or 1", KnownTypeIds.Int64Type);

            AssertExprType("1 xor 1", KnownTypeIds.ShortIntType);
            AssertExprType("1 xor 256", KnownTypeIds.SmallIntType);
            AssertExprType("1 xor 65536", KnownTypeIds.IntegerType);
            AssertExprType("1 xor 4294967296", KnownTypeIds.Int64Type);
            AssertExprType("1 xor 1", KnownTypeIds.ShortIntType);
            AssertExprType("256 xor 1", KnownTypeIds.SmallIntType);
            AssertExprType("65536 xor 1", KnownTypeIds.IntegerType);
            AssertExprType("4294967296 xor 1", KnownTypeIds.Int64Type);
        }

        /// <summary>
        ///     test shifting operators
        /// </summary>
        [TestMethod]
        public void TestShiftingOperators() {
            AssertExprType("1 shr 1", KnownTypeIds.ShortIntType);
            AssertExprType("1 shr 256", KnownTypeIds.ShortIntType);
            AssertExprType("1 shr 65536", KnownTypeIds.ShortIntType);
            AssertExprType("1 shr 4294967296", KnownTypeIds.ShortIntType);
            AssertExprType("256 shr 1", KnownTypeIds.ByteType);
            AssertExprType("256 shr 256", KnownTypeIds.SmallIntType);
            AssertExprType("256 shr 65536", KnownTypeIds.SmallIntType);
            AssertExprType("256 shr 4294967296", KnownTypeIds.SmallIntType);
            AssertExprType("65536 shr 1", KnownTypeIds.WordType);
            AssertExprType("65536 shr 256", KnownTypeIds.IntegerType);
            AssertExprType("65536 shr 65536", KnownTypeIds.IntegerType);
            AssertExprType("65536 shr 4294967296", KnownTypeIds.IntegerType);
            AssertExprType("4294967296 shr 1", KnownTypeIds.CardinalType);
            AssertExprType("4294967296 shr 256", KnownTypeIds.Int64Type);
            AssertExprType("4294967296 shr 65536", KnownTypeIds.Int64Type);
            AssertExprType("4294967296 shr 4294967296", KnownTypeIds.Int64Type);

            AssertExprType("1 shl 1", KnownTypeIds.ShortIntType);
            AssertExprType("1 shl 256", KnownTypeIds.ShortIntType);
            AssertExprType("1 shl 65536", KnownTypeIds.ShortIntType);
            AssertExprType("1 shl 4294967296", KnownTypeIds.ShortIntType);
            AssertExprType("256 shl 1", KnownTypeIds.SmallIntType);
            AssertExprType("256 shl 256", KnownTypeIds.SmallIntType);
            AssertExprType("256 shl 65536", KnownTypeIds.SmallIntType);
            AssertExprType("256 shl 4294967296", KnownTypeIds.SmallIntType);
            AssertExprType("65536 shl 1", KnownTypeIds.IntegerType);
            AssertExprType("65536 shl 256", KnownTypeIds.IntegerType);
            AssertExprType("65536 shl 65536", KnownTypeIds.IntegerType);
            AssertExprType("65536 shl 4294967296", KnownTypeIds.IntegerType);
            AssertExprType("4294967296 shl 1", KnownTypeIds.Int64Type);
            AssertExprType("4294967296 shl 256", KnownTypeIds.Int64Type);
            AssertExprType("4294967296 shl 65536", KnownTypeIds.Int64Type);
            AssertExprType("4294967296 shl 4294967296", KnownTypeIds.Int64Type);
        }

        /// <summary>
        ///     test boolean relational operators
        /// </summary>
        [TestMethod]
        public void TestBooleanRelationalOperators() {
            AssertExprType("true = true", KnownTypeIds.BooleanType);
            AssertExprType("true <> true", KnownTypeIds.BooleanType);
            AssertExprType("true < true", KnownTypeIds.BooleanType);
            AssertExprType("true > true", KnownTypeIds.BooleanType);
            AssertExprType("true <= true", KnownTypeIds.BooleanType);
            AssertExprType("true >= true", KnownTypeIds.BooleanType);
        }

        /// <summary>
        ///     test enumerated relational operators
        /// </summary>
        [TestMethod]
        public void TestEnumRelationalOperators() {
            AssertExprType("a = b", KnownTypeIds.BooleanType, "type te = (a, b, c)");
            AssertExprType("a <> b", KnownTypeIds.BooleanType, "type te = (a, b, c)");
        }

        /// <summary>
        ///     test integer relation operators
        /// </summary>
        [TestMethod]
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

        /// <summary>
        ///     test real relational operators
        /// </summary>
        [TestMethod]
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

        /// <summary>
        ///     test string relational operators
        /// </summary>
        [TestMethod]
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

        /// <summary>
        ///     test arithmetic operators
        /// </summary>
        [TestMethod]
        public void TestArithmeticOperatorsIndirect() {
            var d = new[] {
                Tuple.Create("Byte", KnownTypeIds.IntegerType),
                Tuple.Create("Word", KnownTypeIds.IntegerType),
                Tuple.Create("Cardinal", KnownTypeIds.CardinalType),
                Tuple.Create("UInt64", KnownTypeIds.UInt64Type),
                Tuple.Create("SmallInt", KnownTypeIds.IntegerType),
                Tuple.Create("ShortInt", KnownTypeIds.IntegerType),
                Tuple.Create("Integer", KnownTypeIds.IntegerType),
                Tuple.Create("Int64", KnownTypeIds.Int64Type),
            };

            // subrange types
            var e = CreateEnvironment();
            var ct = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var sr = ct.CreateSubrangeType(string.Empty, KnownTypeIds.IntegerType, GetIntegerValue((sbyte)-1), GetIntegerValue((sbyte)1));
            AssertExprTypeByVar("-1..1", "+ a", sr);
            AssertExprTypeByVar("-1..1", "- a", sr);

            AssertExprTypeByVar("Byte", "+ a", KnownTypeIds.ByteType);
            AssertExprTypeByVar("Word", "+ a", KnownTypeIds.WordType);
            AssertExprTypeByVar("Cardinal", "+ a", KnownTypeIds.CardinalType);
            AssertExprTypeByVar("UInt64", "+ a", KnownTypeIds.UInt64Type);
            AssertExprTypeByVar("ShortInt", "+ a", KnownTypeIds.ShortIntType);
            AssertExprTypeByVar("SmallInt", "+ a", KnownTypeIds.SmallIntType);
            AssertExprTypeByVar("Integer", "+ a", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("Int64", "+ a", KnownTypeIds.Int64Type);

            AssertExprTypeByVar("Byte", "- a", KnownTypeIds.ByteType);
            AssertExprTypeByVar("Word", "- a", KnownTypeIds.WordType);
            AssertExprTypeByVar("Cardinal", "- a", KnownTypeIds.CardinalType);
            AssertExprTypeByVar("UInt64", "- a", KnownTypeIds.UInt64Type);
            AssertExprTypeByVar("ShortInt", "- a", KnownTypeIds.ShortIntType);
            AssertExprTypeByVar("SmallInt", "- a", KnownTypeIds.SmallIntType);
            AssertExprTypeByVar("Integer", "- a", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("Int64", "- a", KnownTypeIds.Int64Type);


            foreach (var e1 in d) {
                AssertExprTypeByVar(e1.Item1, "a + b", e1.Item2);
                AssertExprTypeByVar(e1.Item1, "a - b", e1.Item2);
                AssertExprTypeByVar(e1.Item1, "a div b", e1.Item2);
                AssertExprTypeByVar(e1.Item1, "a * b", e1.Item2);
                AssertExprTypeByVar(e1.Item1, "a mod b", e1.Item2);
                AssertExprTypeByVar(e1.Item1, "a / b", KnownTypeIds.ExtendedType);
            }

            // subrange type
            AssertExprTypeByVar("-1..1", "a + b", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("-1..1", "a - b", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("-1..1", "a div b", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("-1..1", "a * b", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("-1..1", "a mod b", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("-1..1", "a / b", KnownTypeIds.ExtendedType);

        }

        /// <summary>
        ///     test logical operators
        /// </summary>
        [TestMethod]
        public void TestLogicOperatorsIntIndirect() {
            var d = new[] {
                Tuple.Create("Byte", KnownTypeIds.ByteType),
                Tuple.Create("Word", KnownTypeIds.WordType),
                Tuple.Create("Cardinal", KnownTypeIds.CardinalType),
                Tuple.Create("UInt64", KnownTypeIds.UInt64Type),
                Tuple.Create("SmallInt", KnownTypeIds.SmallIntType),
                Tuple.Create("ShortInt", KnownTypeIds.ShortIntType),
                Tuple.Create("Integer", KnownTypeIds.IntegerType),
                Tuple.Create("Int64", KnownTypeIds.Int64Type),
            };

            // subrange types
            var e = CreateEnvironment();
            var ct = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var sr = ct.CreateSubrangeType(string.Empty, KnownTypeIds.IntegerType, GetIntegerValue((sbyte)-1), GetIntegerValue((sbyte)1));

            AssertExprTypeByVar("-1..1", "not a", sr);
            AssertExprTypeByVar("-1..1", "not a", sr);

            AssertExprTypeByVar("Byte", "not a", KnownTypeIds.ByteType);
            AssertExprTypeByVar("Word", "not a", KnownTypeIds.WordType);
            AssertExprTypeByVar("Cardinal", "not a", KnownTypeIds.CardinalType);
            AssertExprTypeByVar("UInt64", "not a", KnownTypeIds.UInt64Type);
            AssertExprTypeByVar("ShortInt", "not a", KnownTypeIds.ShortIntType);
            AssertExprTypeByVar("SmallInt", "not a", KnownTypeIds.SmallIntType);
            AssertExprTypeByVar("Integer", "not a", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("Int64", "not a", KnownTypeIds.Int64Type);

            foreach (var e1 in d) {
                AssertExprTypeByVar(e1.Item1, "a or b", e1.Item2);
                AssertExprTypeByVar(e1.Item1, "a and b", e1.Item2);
                AssertExprTypeByVar(e1.Item1, "a xor b", e1.Item2);
            }

            // subrange type
            AssertExprTypeByVar("-1..1", "a or b", KnownTypeIds.ShortIntType);
            AssertExprTypeByVar("-1..1", "a xor b", KnownTypeIds.ShortIntType);
            AssertExprTypeByVar("-1..1", "a and b", KnownTypeIds.ShortIntType);
        }

        /// <summary>
        ///     test logical operators
        /// </summary>
        [TestMethod]
        public void TestLogicOperatorsBoolIndirect() {
            var d = new[] {
                Tuple.Create("Boolean", KnownTypeIds.BooleanType),
                Tuple.Create("ByteBool", KnownTypeIds.ByteBoolType),
                Tuple.Create("WordBool", KnownTypeIds.WordBoolType),
            };

            // subrange types
            var e = CreateEnvironment();
            var ct = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var sr = ct.CreateSubrangeType(string.Empty, KnownTypeIds.IntegerType, GetIntegerValue((sbyte)-1), GetIntegerValue((sbyte)1));
            AssertExprTypeByVar("-1..1", "not a", sr);
            AssertExprTypeByVar("-1..1", "not a", sr);

            AssertExprTypeByVar("Boolean", "not a", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("ByteBool", "not a", KnownTypeIds.ByteBoolType);
            AssertExprTypeByVar("WordBool", "not a", KnownTypeIds.WordBoolType);

            foreach (var e1 in d) {
                AssertExprTypeByVar(e1.Item1, "a or b", e1.Item2);
                AssertExprTypeByVar(e1.Item1, "a and b", e1.Item2);
                AssertExprTypeByVar(e1.Item1, "a xor b", e1.Item2);
            }

            // subrange type
            AssertExprTypeByVar("false..true", "a or b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("false..true", "a xor b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("false..true", "a and b", KnownTypeIds.BooleanType);
        }

        /// <summary>
        ///     test pointer operator
        /// </summary>
        [TestMethod]
        public void TestPointerOperator() {
            var r = CreateEnvironment().Runtime;
            var pv = r.Types.MakeOperatorResult(OperatorKind.AtOperator, KnownTypeIds.PIntegerType.Reference, r.Types.MakeSignature(KnownTypeIds.ErrorType.Reference, KnownTypeIds.IntegerType.Reference));
            AssertExprValue("@a", pv, "var a: integer;", isConstant: false);
            AssertExprValue("@a", GetPointerValue(GetIntegerValue(4)), "const a = 4;");
        }

        /// <summary>
        ///     test shift operator
        /// </summary>
        [TestMethod]
        public void TestShiftOperators() {
            AssertExprTypeByVar("Byte", "a shl 33", KnownTypeIds.ErrorType);
            AssertExprTypeByVar("Word", "a shl 33", KnownTypeIds.ErrorType);
            AssertExprTypeByVar("Cardinal", "a shl 33", KnownTypeIds.ErrorType);
            AssertExprTypeByVar("UInt64", "a shl 33", KnownTypeIds.UInt64Type);
            AssertExprTypeByVar("ShortInt", "a shl 33", KnownTypeIds.ErrorType);
            AssertExprTypeByVar("SmallInt", "a shl 33", KnownTypeIds.ErrorType);
            AssertExprTypeByVar("Integer", "a shl 33", KnownTypeIds.ErrorType);
            AssertExprTypeByVar("Int64", "a shl 33", KnownTypeIds.Int64Type);
            AssertExprTypeByVar("Byte", "a shl 32", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("Word", "a shl 32", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("Cardinal", "a shl 32", KnownTypeIds.CardinalType);
            AssertExprTypeByVar("UInt64", "a shl 32", KnownTypeIds.UInt64Type);
            AssertExprTypeByVar("ShortInt", "a shl 32", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("SmallInt", "a shl 32", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("Integer", "a shl 32", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("Int64", "a shl 32", KnownTypeIds.Int64Type);
            AssertExprTypeByVar("Byte", "a shr 33", KnownTypeIds.ErrorType);
            AssertExprTypeByVar("Word", "a shr 33", KnownTypeIds.ErrorType);
            AssertExprTypeByVar("Cardinal", "a shr 33", KnownTypeIds.ErrorType);
            AssertExprTypeByVar("UInt64", "a shr 33", KnownTypeIds.UInt64Type);
            AssertExprTypeByVar("ShortInt", "a shr 33", KnownTypeIds.ErrorType);
            AssertExprTypeByVar("SmallInt", "a shr 33", KnownTypeIds.ErrorType);
            AssertExprTypeByVar("Integer", "a shr 33", KnownTypeIds.ErrorType);
            AssertExprTypeByVar("Int64", "a shr 33", KnownTypeIds.Int64Type);
            AssertExprTypeByVar("Byte", "a shr 32", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("Word", "a shr 32", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("Cardinal", "a shr 32", KnownTypeIds.CardinalType);
            AssertExprTypeByVar("UInt64", "a shr 32", KnownTypeIds.UInt64Type);
            AssertExprTypeByVar("ShortInt", "a shr 32", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("SmallInt", "a shr 32", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("Integer", "a shr 32", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("Int64", "a shr 32", KnownTypeIds.Int64Type);
        }

        /// <summary>
        ///     test relational operators
        /// </summary>
        [TestMethod]
        public void TestRelationalOperatorsStringIndirect() {
            AssertExprTypeByVar("String", "a = b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("String", "a <> b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("String", "a < b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("String", "a > b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("String", "a <= b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("String", "a >= b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("Char", "a = b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("Char", "a <> b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("Char", "a < b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("Char", "a > b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("Char", "a <= b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("Char", "a >= b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("AnsiChar", "a = b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("AnsiChar", "a <> b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("AnsiChar", "a < b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("AnsiChar", "a > b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("AnsiChar", "a <= b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("AnsiChar", "a >= b", KnownTypeIds.BooleanType);


        }

        /// <summary>
        ///     test relational operators
        /// </summary>
        [TestMethod]
        public void TestRelationalOperatorsIntIndirect() {
            var d = new[] {
                Tuple.Create("Byte", KnownTypeIds.ByteType),
                Tuple.Create("Word", KnownTypeIds.WordType),
                Tuple.Create("Cardinal", KnownTypeIds.CardinalType),
                Tuple.Create("UInt64", KnownTypeIds.UInt64Type),
                Tuple.Create("SmallInt", KnownTypeIds.SmallIntType),
                Tuple.Create("ShortInt", KnownTypeIds.ShortIntType),
                Tuple.Create("Integer", KnownTypeIds.IntegerType),
                Tuple.Create("Int64", KnownTypeIds.Int64Type),
            };

            foreach (var dd in d) {
                AssertExprTypeByVar(dd.Item1, "a = b", KnownTypeIds.BooleanType);
                AssertExprTypeByVar(dd.Item1, "a <> b", KnownTypeIds.BooleanType);
                AssertExprTypeByVar(dd.Item1, "a < b", KnownTypeIds.BooleanType);
                AssertExprTypeByVar(dd.Item1, "a > b", KnownTypeIds.BooleanType);
                AssertExprTypeByVar(dd.Item1, "a <= b", KnownTypeIds.BooleanType);
                AssertExprTypeByVar(dd.Item1, "a >= b", KnownTypeIds.BooleanType);
            }

            // subrange
            AssertExprTypeByVar("-1..1", "a = b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("-1..1", "a <> b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("-1..1", "a < b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("-1..1", "a > b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("-1..1", "a <= b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("-1..1", "a >= b", KnownTypeIds.BooleanType);

        }

        /// <summary>
        ///     test relational operators
        /// </summary>
        [TestMethod]
        public void TestRelationalOperatorsBoolIndirect() {
            AssertExprTypeByVar("Boolean", "a = b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("Boolean", "a <> b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("Boolean", "a < b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("Boolean", "a > b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("Boolean", "a <= b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("Boolean", "a >= b", KnownTypeIds.BooleanType);

            AssertExprTypeByVar("ByteBool", "a = b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("ByteBool", "a <> b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("ByteBool", "a < b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("ByteBool", "a > b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("ByteBool", "a <= b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("ByteBool", "a >= b", KnownTypeIds.BooleanType);

            AssertExprTypeByVar("WordBool", "a = b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("WordBool", "a <> b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("WordBool", "a < b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("WordBool", "a > b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("WordBool", "a <= b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("WordBool", "a >= b", KnownTypeIds.BooleanType);
        }

        /// <summary>
        ///     test concatenate operators
        /// </summary>
        [TestMethod]
        public void TestConcatOperatorIndirect() {
            AssertExprTypeByVar("UnicodeString", "a + b", KnownTypeIds.UnicodeStringType);
            AssertExprTypeByVar("AnsiString", "a + b", KnownTypeIds.AnsiStringType);
        }

        /// <summary>
        ///     test array operators
        /// </summary>
        [TestMethod]
        public void TestConstantArrayTypes() {
            AssertExprTypeByConst("(1,2)", KnownTypeIds.UnspecifiedType, false, "static array [Subrange i] of zc");
            AssertExprTypeByConst("('a','b')", KnownTypeIds.UnspecifiedType, false, "static array [Subrange i] of b");
            AssertExprTypeByConst("('aa','b')", KnownTypeIds.UnspecifiedType, false, "static array [Subrange i] of System@UnicodeString");
            AssertExprTypeByConst("(1.0, 1.4)", KnownTypeIds.UnspecifiedType, false, "static array [Subrange i] of g");
            AssertExprTypeByConst("(a, b)", KnownTypeIds.UnspecifiedType, false, "static array [Subrange i] of enum zc", "type ta = (a,b);");

            var c = CreateEnvironment();
            var ct = c.TypeRegistry.CreateTypeFactory(c.TypeRegistry.SystemUnit);
            var at = ct.CreateStaticArrayType(KnownTypeIds.StringType, string.Empty, KnownTypeIds.IntegerType, false);
            var av = c.Runtime.Structured.CreateArrayValue(at, KnownTypeIds.StringType, ImmutableArray.Create(GetUnicodeStringValue("11"), GetUnicodeStringValue("2")));
            AssertExprValue("a", av, "const a: array [0 .. 1] of String = ('11', '2');");
        }

        /// <summary>
        ///     test set add operator
        /// </summary>
        [TestMethod]
        public void TestSetAddOperator() {
            var i1 = GetIntegerValue(1);
            var i2 = GetIntegerValue(2);
            var c1 = GetWideCharValue('1');
            var c2 = GetWideCharValue('2');
            var b1 = GetBooleanValue(true);
            var b2 = GetBooleanValue(false);

            var e = CreateEnvironment();
            var tc = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var soi = tc.CreateSetType(KnownTypeIds.IntegerType, "Ta");
            var soc = tc.CreateSetType(KnownTypeIds.WideCharType, "Ta");
            var sob = tc.CreateSetType(KnownTypeIds.BooleanType, "Ta");

            AssertExprValue("a + b", GetSetValue(soi, i1, i2), "type Ta = set of integer; const a: Ta = [1]; b: Ta = [2];");
            AssertExprValue("a + b", GetSetValue(soi, i1, i2), "type Ta = set of integer; const a: Ta = [1,2]; b: Ta = [2];");
            AssertExprValue("a + b", GetSetValue(soi, i1, i2), "type Ta = set of integer; const a: Ta = [1]; b: Ta = [1,2];");
            AssertExprValue("a + b", GetSetValue(soi, i1, i2), "type Ta = set of integer; const a: Ta = [1,2]; b: Ta = [1,2];");

            AssertExprValue("a + b", GetSetValue(soc, c1, c2), "type Ta = set of char; const a: Ta = ['1']; b: Ta = ['2'];");
            AssertExprValue("a + b", GetSetValue(soc, c1, c2), "type Ta = set of char; const a: Ta = ['1','2']; b: Ta = ['2'];");
            AssertExprValue("a + b", GetSetValue(soc, c1, c2), "type Ta = set of char; const a: Ta = ['1']; b: Ta = ['1','2'];");
            AssertExprValue("a + b", GetSetValue(soc, c1, c2), "type Ta = set of char; const a: Ta = ['1','2']; b: Ta = ['1','2'];");

            AssertExprValue("a + b", GetSetValue(sob, b1, b2), "type Ta = set of boolean; const a: Ta = [true]; b: Ta = [false];");
            AssertExprValue("a + b", GetSetValue(sob, b1, b2), "type Ta = set of boolean; const a: Ta = [true,false]; b: Ta = [false];");
            AssertExprValue("a + b", GetSetValue(sob, b1, b2), "type Ta = set of boolean; const a: Ta = [true]; b: Ta = [true,false];");
            AssertExprValue("a + b", GetSetValue(sob, b1, b2), "type Ta = set of boolean; const a: Ta = [true,false]; b: Ta = [true,false];");

        }

        /// <summary>
        ///     test set difference operator
        /// </summary>
        [TestMethod]
        public void TestSetDifferenceOperator() {
            var i1 = GetIntegerValue(1);
            var i2 = GetIntegerValue(2);
            var c1 = GetWideCharValue('1');
            var c2 = GetWideCharValue('2');
            var b1 = GetBooleanValue(true);
            var b2 = GetBooleanValue(false);

            var e = CreateEnvironment();
            var tc = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var soi = tc.CreateSetType(KnownTypeIds.IntegerType, "Ta");
            var soc = tc.CreateSetType(KnownTypeIds.WideCharType, "Ta");
            var sob = tc.CreateSetType(KnownTypeIds.BooleanType, "Ta");


            AssertExprValue("a - b", GetSetValue(soi, i1), "type Ta = set of integer; const a: Ta = [1,2]; b: Ta = [2];");
            AssertExprValue("a - b", GetSetValue(soi, i1), "type Ta = set of integer; const a: Ta = [1]; b: Ta = [2];");
            AssertExprValue("a - b", GetSetValue(soi), "type Ta = set of integer; const a: Ta = [2]; b: Ta = [2];");
            AssertExprValue("a - b", GetSetValue(soi, i2), "type Ta = set of integer; const a: Ta = [2]; b: Ta = [1];");

            AssertExprValue("a - b", GetSetValue(soc, c1), "type Ta = set of char; const a: Ta = ['1','2']; b: Ta = ['2'];");
            AssertExprValue("a - b", GetSetValue(soc, c1), "type Ta = set of char; const a: Ta = ['1']; b: Ta = ['2'];");
            AssertExprValue("a - b", GetSetValue(soc), "type Ta = set of char; const a: Ta = ['2']; b: Ta = ['2'];");
            AssertExprValue("a - b", GetSetValue(soc, c2), "type Ta = set of char; const a: Ta = ['2']; b: Ta = ['1'];");

            AssertExprValue("a - b", GetSetValue(sob, b1), "type Ta = set of Boolean; const a: Ta = [true,false]; b: Ta = [false];");
            AssertExprValue("a - b", GetSetValue(sob, b1), "type Ta = set of Boolean; const a: Ta = [true]; b: Ta = [false];");
            AssertExprValue("a - b", GetSetValue(sob), "type Ta = set of Boolean; const a: Ta = [true]; b: Ta = [true];");
            AssertExprValue("a - b", GetSetValue(sob, b2), "type Ta = set of Boolean; const a: Ta = [false]; b: Ta = [true];");
        }

        /// <summary>
        ///     test set intersect operator
        /// </summary>
        [TestMethod]
        public void TestSetIntersectOperator() {
            var i1 = GetIntegerValue(1);
            var i2 = GetIntegerValue(2);
            var c1 = GetWideCharValue('1');
            var c2 = GetWideCharValue('2');
            var b1 = GetBooleanValue(true);
            var b2 = GetBooleanValue(false);

            var e = CreateEnvironment();
            var tc = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var soi = tc.CreateSetType(KnownTypeIds.IntegerType, "Ta");
            var soc = tc.CreateSetType(KnownTypeIds.WideCharType, "Ta");
            var sob = tc.CreateSetType(KnownTypeIds.BooleanType, "Ta");

            AssertExprValue("a * b", GetSetValue(soi, i2), "type Ta = set of integer; const a: Ta = [1,2]; b: Ta = [2];");
            AssertExprValue("a * b", GetSetValue(soi), "type Ta = set of integer; const a: Ta = [1]; b: Ta = [2];");
            AssertExprValue("a * b", GetSetValue(soi, i1), "type Ta = set of integer; const a: Ta = [1]; b: Ta = [1..5];");
            AssertExprValue("a * b", GetSetValue(soi, i1, i2), "type Ta = set of integer; const a: Ta = [1,2]; b: Ta = [2,1];");

            AssertExprValue("a * b", GetSetValue(soc, c2), "type Ta = set of char; const a: Ta = ['1','2']; b: Ta = ['2'];");
            AssertExprValue("a * b", GetSetValue(soc), "type Ta = set of char; const a: Ta = ['1']; b: Ta = ['2'];");
            AssertExprValue("a * b", GetSetValue(soc, c2), "type Ta = set of char; const a: Ta = ['2']; b: Ta = ['1'..'5'];");
            AssertExprValue("a * b", GetSetValue(soc, c1, c2), "type Ta = set of char; const a: Ta = ['2','1']; b: Ta = ['1','2'];");

            AssertExprValue("a * b", GetSetValue(sob, b2), "type Ta = set of Boolean; const a: Ta = [true,false]; b: Ta = [false];");
            AssertExprValue("a * b", GetSetValue(sob), "type Ta = set of Boolean; const a: Ta = [true]; b: Ta = [false];");
            AssertExprValue("a * b", GetSetValue(sob, b2), "type Ta = set of Boolean; const a: Ta = [false]; b: Ta = [true,false];");
            AssertExprValue("a * b", GetSetValue(sob, b1, b2), "type Ta = set of Boolean; const a: Ta = [false,true]; b: Ta = [true,false];");
        }

        /// <summary>
        ///     test set equals operator
        /// </summary>
        [TestMethod]
        public void TestSetEqualsOperator() {
            AssertExprValue("a = b", GetBooleanValue(false), "type Ta = set of integer; const a: Ta = [1]; b: Ta = [2];");
            AssertExprValue("a = b", GetBooleanValue(false), "type Ta = set of integer; const a: Ta = [2]; b: Ta = [1];");
            AssertExprValue("a = b", GetBooleanValue(false), "type Ta = set of integer; const a: Ta = []; b: Ta = [1..5];");
            AssertExprValue("a = b", GetBooleanValue(true), "type Ta = set of integer; const a: Ta = [1,2]; b: Ta = [2,1];");

            AssertExprValue("a = b", GetBooleanValue(false), "type Ta = set of char; const a: Ta = ['1']; b: Ta = ['2'];");
            AssertExprValue("a = b", GetBooleanValue(false), "type Ta = set of char; const a: Ta = ['2']; b: Ta = ['1'];");
            AssertExprValue("a = b", GetBooleanValue(false), "type Ta = set of char; const a: Ta = []; b: Ta = ['1'..'5'];");
            AssertExprValue("a = b", GetBooleanValue(true), "type Ta = set of char; const a: Ta = ['1','2']; b: Ta = ['2','1'];");

            AssertExprValue("a = b", GetBooleanValue(false), "type Ta = set of boolean; const a: Ta = [false]; b: Ta = [true, true];");
            AssertExprValue("a = b", GetBooleanValue(false), "type Ta = set of boolean; const a: Ta = [true]; b: Ta = [false];");
            AssertExprValue("a = b", GetBooleanValue(false), "type Ta = set of boolean; const a: Ta = []; b: Ta = [false,true];");
            AssertExprValue("a = b", GetBooleanValue(true), "type Ta = set of boolean; const a: Ta = [true,false]; b: Ta = [false,true];");
        }

        /// <summary>
        ///     test set is superset operator
        /// </summary>
        [TestMethod]
        public void TestSetIsSuperset() {
            AssertExprValue("a >= b", GetBooleanValue(false), "type Ta = set of integer; const a: Ta = [1]; b: Ta = [2];");
            AssertExprValue("a >= b", GetBooleanValue(false), "type Ta = set of integer; const a: Ta = [2]; b: Ta = [1];");
            AssertExprValue("a >= b", GetBooleanValue(false), "type Ta = set of integer; const a: Ta = []; b: Ta = [1..5];");
            AssertExprValue("a >= b", GetBooleanValue(true), "type Ta = set of integer; const a: Ta = [1,2]; b: Ta = [2,1];");
            AssertExprValue("a >= b", GetBooleanValue(true), "type Ta = set of integer; const a: Ta = [1,2]; b: Ta = [2];");

            AssertExprValue("a >= b", GetBooleanValue(false), "type Ta = set of char; const a: Ta = ['1']; b: Ta = ['2'];");
            AssertExprValue("a >= b", GetBooleanValue(false), "type Ta = set of char; const a: Ta = ['2']; b: Ta = ['1'];");
            AssertExprValue("a >= b", GetBooleanValue(false), "type Ta = set of char; const a: Ta = []; b: Ta = ['1'..'5'];");
            AssertExprValue("a >= b", GetBooleanValue(true), "type Ta = set of char; const a: Ta = ['1','2']; b: Ta = ['2','1'];");
            AssertExprValue("a >= b", GetBooleanValue(true), "type Ta = set of char; const a: Ta = ['1','2']; b: Ta = ['2'];");

            AssertExprValue("a >= b", GetBooleanValue(false), "type Ta = set of boolean; const a: Ta = [false]; b: Ta = [true, true];");
            AssertExprValue("a >= b", GetBooleanValue(false), "type Ta = set of boolean; const a: Ta = [true]; b: Ta = [false];");
            AssertExprValue("a >= b", GetBooleanValue(false), "type Ta = set of boolean; const a: Ta = []; b: Ta = [false,true];");
            AssertExprValue("a >= b", GetBooleanValue(true), "type Ta = set of boolean; const a: Ta = [true,false]; b: Ta = [false,true];");
            AssertExprValue("a >= b", GetBooleanValue(true), "type Ta = set of boolean; const a: Ta = [true,false]; b: Ta = [false];");
        }

        /// <summary>
        ///     test subset operator
        /// </summary>
        [TestMethod]
        public void TestSetIsSubset() {
            AssertExprValue("a <= b", GetBooleanValue(false), "type Ta = set of integer; const a: Ta = [1]; b: Ta = [2];");
            AssertExprValue("a <= b", GetBooleanValue(false), "type Ta = set of integer; const a: Ta = [2]; b: Ta = [1];");
            AssertExprValue("a <= b", GetBooleanValue(true), "type Ta = set of integer; const a: Ta = []; b: Ta = [1..5];");
            AssertExprValue("a <= b", GetBooleanValue(true), "type Ta = set of integer; const a: Ta = [1,2]; b: Ta = [2,1];");
            AssertExprValue("a <= b", GetBooleanValue(false), "type Ta = set of integer; const a: Ta = [1,2]; b: Ta = [2];");

            AssertExprValue("a <= b", GetBooleanValue(false), "type Ta = set of char; const a: Ta = ['1']; b: Ta = ['2'];");
            AssertExprValue("a <= b", GetBooleanValue(false), "type Ta = set of char; const a: Ta = ['2']; b: Ta = ['1'];");
            AssertExprValue("a <= b", GetBooleanValue(true), "type Ta = set of char; const a: Ta = []; b: Ta = ['1'..'5'];");
            AssertExprValue("a <= b", GetBooleanValue(true), "type Ta = set of char; const a: Ta = ['1','2']; b: Ta = ['2','1'];");
            AssertExprValue("a <= b", GetBooleanValue(false), "type Ta = set of char; const a: Ta = ['1','2']; b: Ta = ['2'];");

            AssertExprValue("a <= b", GetBooleanValue(false), "type Ta = set of boolean; const a: Ta = [false]; b: Ta = [true, true];");
            AssertExprValue("a <= b", GetBooleanValue(false), "type Ta = set of boolean; const a: Ta = [true]; b: Ta = [false];");
            AssertExprValue("a <= b", GetBooleanValue(true), "type Ta = set of boolean; const a: Ta = []; b: Ta = [false,true];");
            AssertExprValue("a <= b", GetBooleanValue(true), "type Ta = set of boolean; const a: Ta = [true,false]; b: Ta = [false,true];");
            AssertExprValue("a <= b", GetBooleanValue(false), "type Ta = set of boolean; const a: Ta = [true,false]; b: Ta = [false];");
        }

        /// <summary>
        ///     test in operator
        /// </summary>
        [TestMethod]
        public void TestSetInOperator() {
            AssertExprValue("a in b", GetBooleanValue(false), "type Ta = set of boolean; const a = true; b: Ta = [false];");
            AssertExprValue("a in b", GetBooleanValue(true), "type Ta = set of boolean; const a = true; b: Ta = [true];");
            AssertExprValue("a in b", GetBooleanValue(true), "type Ta = set of boolean; const a = true; b: Ta = [false,true];");
            AssertExprValue("a in b", GetBooleanValue(false), "type Ta = set of boolean; const a = true; b: Ta = [];");

            AssertExprValue("a in b", GetBooleanValue(false), "type Ta = set of word; const a = 1; b: Ta = [2];");
            AssertExprValue("a in b", GetBooleanValue(true), "type Ta = set of word; const a = 1; b: Ta = [1];");
            AssertExprValue("a in b", GetBooleanValue(true), "type Ta = set of word; const a = 1; b: Ta = [1,2];");
            AssertExprValue("a in b", GetBooleanValue(false), "type Ta = set of word; const a = 1; b: Ta = [];");

            AssertExprValue("a in b", GetBooleanValue(false), "type Ta = set of char; const a = '1'; b: Ta = ['2'];");
            AssertExprValue("a in b", GetBooleanValue(true), "type Ta = set of char; const a = '1'; b: Ta = ['1'];");
            AssertExprValue("a in b", GetBooleanValue(true), "type Ta = set of char; const a = '1'; b: Ta = ['1','2'];");
            AssertExprValue("a in b", GetBooleanValue(false), "type Ta = set of char; const a = '1'; b: Ta = [];");

        }

        /// <summary>
        ///     test set operators
        /// </summary>
        [TestMethod]
        public void TestSetOperatorsIndirect() {
            var e = CreateEnvironment();
            var tc = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var soi = tc.CreateSetType(KnownTypeIds.IntegerType, "Ta");
            var soc = tc.CreateSetType(KnownTypeIds.WideCharType, "Ta");
            var sob = tc.CreateSetType(KnownTypeIds.BooleanType, "Ta");

            ITypeSymbol goc(OperatorKind kind, ITypeDefinition d) {
                return e.Runtime.Types.MakeOperatorResult(kind, d.Reference, e.Runtime.Types.MakeSignature(d.Reference, d.Reference)); ;
            };

            AssertExprValue("a + b", goc(OperatorKind.SetAddOperator, soi), "type Ta = set of integer; var a, b: Ta;", isConstant: false);
            AssertExprValue("a + b", goc(OperatorKind.SetAddOperator, soc), "type Ta = set of char; var a, b: Ta;", isConstant: false);
            AssertExprValue("a + b", goc(OperatorKind.SetAddOperator, sob), "type Ta = set of boolean; var a, b: Ta;", isConstant: false);

            AssertExprValue("a - b", goc(OperatorKind.SetDifferenceOperator, soi), "type Ta = set of integer; var a, b: Ta;", isConstant: false);
            AssertExprValue("a - b", goc(OperatorKind.SetDifferenceOperator, soc), "type Ta = set of char; var a, b: Ta;", isConstant: false);
            AssertExprValue("a - b", goc(OperatorKind.SetDifferenceOperator, sob), "type Ta = set of boolean; var a, b: Ta;", isConstant: false);

            AssertExprValue("a * b", goc(OperatorKind.SetAddOperator, soi), "type Ta = set of integer; var a, b: Ta;", isConstant: false);
            AssertExprValue("a * b", goc(OperatorKind.SetAddOperator, soc), "type Ta = set of char; var a, b: Ta;", isConstant: false);
            AssertExprValue("a * b", goc(OperatorKind.SetAddOperator, sob), "type Ta = set of boolean; var a, b: Ta;", isConstant: false);

            AssertExprValue("a = b", goc(OperatorKind.EqualsOperator, KnownTypeIds.BooleanType), "type Ta = set of integer; var a, b: Ta;", isConstant: false);
            AssertExprValue("a = b", goc(OperatorKind.EqualsOperator, KnownTypeIds.BooleanType), "type Ta = set of char; var a, b: Ta;", isConstant: false);
            AssertExprValue("a = b", goc(OperatorKind.EqualsOperator, KnownTypeIds.BooleanType), "type Ta = set of boolean; var a, b: Ta;", isConstant: false);

            AssertExprValue("a >= b", goc(OperatorKind.GreaterThanOrEqual, KnownTypeIds.BooleanType), "type Ta = set of integer; var a, b: Ta;", isConstant: false);
            AssertExprValue("a >= b", goc(OperatorKind.GreaterThanOrEqual, KnownTypeIds.BooleanType), "type Ta = set of char; var a, b: Ta;", isConstant: false);
            AssertExprValue("a >= b", goc(OperatorKind.GreaterThanOrEqual, KnownTypeIds.BooleanType), "type Ta = set of boolean; var a, b: Ta;", isConstant: false);

            AssertExprValue("a <= b", goc(OperatorKind.LessThanOrEqual, KnownTypeIds.BooleanType), "type Ta = set of integer; var a, b: Ta;", isConstant: false);
            AssertExprValue("a <= b", goc(OperatorKind.LessThanOrEqual, KnownTypeIds.BooleanType), "type Ta = set of char; var a, b: Ta;", isConstant: false);
            AssertExprValue("a <= b", goc(OperatorKind.LessThanOrEqual, KnownTypeIds.BooleanType), "type Ta = set of boolean; var a, b: Ta;", isConstant: false);

            AssertExprValue("a <> b", goc(OperatorKind.NotEqualsOperator, KnownTypeIds.BooleanType), "type Ta = set of integer; var a, b: Ta;", isConstant: false);
            AssertExprValue("a <> b", goc(OperatorKind.NotEqualsOperator, KnownTypeIds.BooleanType), "type Ta = set of char; var a, b: Ta;", isConstant: false);
            AssertExprValue("a <> b", goc(OperatorKind.NotEqualsOperator, KnownTypeIds.BooleanType), "type Ta = set of boolean; var a, b: Ta;", isConstant: false);
        }

        /// <summary>
        ///     test class operator
        /// </summary>
        [TestMethod]
        public void TestClassOperators() {
            AssertExprType("a is TObject", KnownTypeIds.BooleanType, "var a: TObject;");
            AssertExprType("a is system.TObject", KnownTypeIds.BooleanType, "var a: TObject;");
            AssertExprType("a is TObject", KnownTypeIds.BooleanType, "type ta = class end; var a: ta;");
            AssertExprType("a is tb", KnownTypeIds.ErrorType, "type ta = class end; tb = class end; var a: ta;");
            AssertExprType("a is tb", KnownTypeIds.BooleanType, "type ta = class end; tb = class(ta) end; var a: ta;");
            AssertExprType("a is ta", KnownTypeIds.BooleanType, "type ta = class end; tb = class(ta) end; var a: tb;");
            AssertExprType("a is tb", KnownTypeIds.BooleanType, "type ta = class end; tc = ta; tb = class(tc) end; var a: ta;");
            AssertExprType("a is ta", KnownTypeIds.BooleanType, "type ta = class end; tc = ta; tb = class(tc) end; var a: tb;");

            AssertExprType("a as TObject", KnownTypeIds.TObjectType, "var a: TObject;");
            AssertExprType("a as System.TObject", KnownTypeIds.TObjectType, "var a: TObject;");
            AssertExprType("a as TObject", KnownTypeIds.TObjectType, "type ta = class end; var a: ta;");
            AssertExprType("a as tb", KnownTypeIds.ErrorType, "type ta = class end; tb = class end; var a: ta;");

            var e = CreateEnvironment();
            var tc = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var ta = tc.CreateStructuredType("Ta", StructuredTypeKind.Class);
            var tb = tc.CreateStructuredType("Tb", StructuredTypeKind.Class);
            var ia = tc.CreateStructuredType("Ia", StructuredTypeKind.Interface);

            AssertExprType("a as tb", tb, "type ta = class end; tb = class(ta) end; var a: ta;");
            AssertExprType("a as ta", ta, "type ta = class end; tb = class(ta) end; var a: tb;");
            AssertExprType("a as tb", ta, "type ta = class end; tc = ta; tb = class(tc) end; var a: ta;");
            AssertExprType("a as ta", ta, "type ta = class end; tc = ta; tb = class(tc) end; var a: tb;");

            AssertExprType("a is TObject", KnownTypeIds.BooleanType, "type ia = interface end; var a: ia;");
            AssertExprType("a as ia", ia, "type ia = interface end; ta = class(ia) end; var a: ta;");
            AssertExprType("a as ia", ia, "type ia = interface end; ta = class(tobject, ia) end; var a: ta;");
            AssertExprType("a as ia", KnownTypeIds.ErrorType, "type ia = interface end; ta = class end; var a: ta;");
        }

        /// <summary>
        ///     test formatted expressions
        /// </summary>
        [TestMethod]
        public void TestFormattedExressions() {
            AssertExprValue("'a' : -1", GetUnicodeStringValue("a"));
            AssertExprValue("'a' : 4", GetUnicodeStringValue("   a"));
            AssertExprValue("true : 5", GetUnicodeStringValue(" TRUE"));
            AssertExprValue("faLsE : 7", GetUnicodeStringValue("  FALSE"));
            AssertExprValue("'aba' : -1", GetUnicodeStringValue("aba"));
            AssertExprValue("'aca' : 4", GetUnicodeStringValue(" aca"));
            AssertExprValue("-12 : -1", GetUnicodeStringValue("-12"));
            AssertExprValue("-12 : 5", GetUnicodeStringValue("  -12"));
            AssertExprValue("pi : 5 : 2", GetUnicodeStringValue(" 3.14"));
            AssertExprValue("5.2 : 8", GetUnicodeStringValue("5.20000000e+0000"));
        }

        /// <summary>
        ///     test formatted expressions
        /// </summary>
        [TestMethod]
        public void TestFormattedExressionsIndirect() {
            ITypeSymbol goc(ITypeDefinition p) {
                var s = CreateEnvironment().Runtime.Types.MakeSignature(KnownTypeIds.StringType.Reference, p.Reference);
                return CreateEnvironment().Runtime.Types.MakeInvocationResultFromIntrinsic(KnownTypeIds.FormatExpression, s);
            }

            AssertExprValue("a : -1", goc(KnownTypeIds.CharType), "var a: Char;", isConstant: false);
            AssertExprValue("a : 4", goc(KnownTypeIds.CharType), "var a: Char;", isConstant: false);
            AssertExprValue("a : 5", goc(KnownTypeIds.BooleanType), "var a: Boolean;", isConstant: false);
            AssertExprValue("a : 7", goc(KnownTypeIds.BooleanType), "var a: Boolean;", isConstant: false);
            AssertExprValue("a : -1", goc(KnownTypeIds.StringType), "var a: String;", isConstant: false);
            AssertExprValue("a : 4", goc(KnownTypeIds.StringType), "var a: String;", isConstant: false);
            AssertExprValue("a : -1", goc(KnownTypeIds.ByteType), "var a: Byte;", isConstant: false);
            AssertExprValue("a : 5", goc(KnownTypeIds.ByteType), "var a: Byte;", isConstant: false);
            AssertExprValue("a : 5 : 2", goc(KnownTypeIds.StringType), "var a: String;", isConstant: false);
            AssertExprValue("a : 8", goc(KnownTypeIds), "var a: String;", isConstant: false);
        }


    }
}
