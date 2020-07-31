using System;
using System.Collections.Immutable;
using PasPasPas.Globals.Types;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test cases for typed expressions
    /// </summary>
    public class TypedExpressionTest : TypeTest {

        private ISystemUnit SystemUnit
            => CreateEnvironment().TypeRegistry.SystemUnit;

        /// <summary>
        ///     test integer literals
        /// </summary>
        [TestMethod]
        public void TestIntegerLiteralTypes() {
            AssertExprType("-128", SystemUnit.ShortIntType);
            AssertExprType("0", SystemUnit.ShortIntType);
            AssertExprType("127", SystemUnit.ShortIntType);
            AssertExprType("128", SystemUnit.ByteType);
            AssertExprType("255", SystemUnit.ByteType);
            AssertExprType("-129", SystemUnit.SmallIntType);
            AssertExprType("256", SystemUnit.SmallIntType);
            AssertExprType("-32768", SystemUnit.SmallIntType);
            AssertExprType("-32769", SystemUnit.IntegerType);
            AssertExprType("32767", SystemUnit.SmallIntType);
            AssertExprType("32768", SystemUnit.WordType);
            AssertExprType("65535", SystemUnit.WordType);
            AssertExprType("65536", SystemUnit.IntegerType);
            AssertExprType("2147483648", SystemUnit.CardinalType);
            AssertExprType("-2147483648", SystemUnit.IntegerType);
            AssertExprType("-2147483649", SystemUnit.Int64Type);
            AssertExprType("4294967295", SystemUnit.CardinalType);
            AssertExprType("4294967296", SystemUnit.Int64Type);
            AssertExprType("9223372036854775807", SystemUnit.Int64Type);
            AssertExprType("18446744073709551615", SystemUnit.UInt64Type);
        }

        /// <summary>
        ///     test boolean literals
        /// </summary>
        [TestMethod]
        public void TestBooleanLiteralTypes() {
            AssertExprType("true", SystemUnit.BooleanType);
            AssertExprType("false", SystemUnit.BooleanType);
        }

        /// <summary>
        ///     test char literals
        /// </summary>
        [TestMethod]
        public void TestCharLiteralTypes() {
            AssertExprType("'C'", SystemUnit.WideCharType);
            AssertExprType("#9", SystemUnit.WideCharType);
        }

        /// <summary>
        ///     test string literals
        /// </summary>
        [TestMethod]
        public void TestStringLiteralTypes() {
            AssertExprType("'CD'", SystemUnit.UnicodeStringType);
            AssertExprType("#9#9", SystemUnit.UnicodeStringType);
        }

        /// <summary>
        ///     test extended literals
        /// </summary>
        [TestMethod]
        public void TestExtendedLiteralTypes() {
            AssertExprType("3.5", SystemUnit.ExtendedType);
            AssertExprType("2.33434343", SystemUnit.ExtendedType);
        }

        /// <summary>
        ///     test boolean operators
        /// </summary>
        [TestMethod]
        public void TestBooleanOperators() {
            AssertExprType("not true", SystemUnit.BooleanType);
            AssertExprType("not false", SystemUnit.BooleanType);
            AssertExprType("true and false", SystemUnit.BooleanType);
            AssertExprType("true or false", SystemUnit.BooleanType);
            AssertExprType("true xor false", SystemUnit.BooleanType);
        }

        /// <summary>
        ///     test arithmetic operators
        /// </summary>
        [TestMethod]
        public void TestArithmeticOperatorsInteger() {
            AssertExprType("+ 1", SystemUnit.ShortIntType);
            AssertExprType("- 1", SystemUnit.ShortIntType);
            AssertExprType("1 + 1", SystemUnit.ShortIntType);
            AssertExprType("1 - 1", SystemUnit.ShortIntType);
            AssertExprType("1 * 1", SystemUnit.ShortIntType);
            AssertExprType("1 div 1", SystemUnit.ShortIntType);
            AssertExprType("1 mod 1", SystemUnit.ShortIntType);
            AssertExprType("1 / 1", SystemUnit.ExtendedType);
        }

        /// <summary>
        ///     test arithmetic operators
        /// </summary>
        [TestMethod]
        public void TestArithmeticOperatorsInt64() {
            AssertExprType("+ 4294967296", SystemUnit.Int64Type);
            AssertExprType("- 4294967296", SystemUnit.Int64Type);
            AssertExprType("4294967296 + 1", SystemUnit.Int64Type);
            AssertExprType("4294967296 - 1", SystemUnit.CardinalType);
            AssertExprType("4294967296 * 1", SystemUnit.Int64Type);
            AssertExprType("4294967296 div 1", SystemUnit.Int64Type);
            AssertExprType("4294967296 mod 1", SystemUnit.ShortIntType);
            AssertExprType("4294967296 / 1", SystemUnit.ExtendedType);
            AssertExprType("1 + 4294967296", SystemUnit.Int64Type);
            AssertExprType("1 - 4294967296", SystemUnit.Int64Type);
            AssertExprType("1 * 4294967296", SystemUnit.Int64Type);
            AssertExprType("1 div 4294967296", SystemUnit.ShortIntType);
            AssertExprType("1 mod 4294967296", SystemUnit.ShortIntType);
            AssertExprType("1 / 4294967296", SystemUnit.ExtendedType);
            AssertExprType("4294967296 + 4294967296", SystemUnit.Int64Type);
            AssertExprType("4294967296 - 4294967296", SystemUnit.ShortIntType);
            AssertExprType("4294967296 * 3", SystemUnit.Int64Type);
            AssertExprType("4294967296 div 4294967296", SystemUnit.ShortIntType);
            AssertExprType("4294967296 mod 4294967296", SystemUnit.ShortIntType);
            AssertExprType("4294967296 / 4294967296", SystemUnit.ExtendedType);
        }

        /// <summary>
        ///     test arithmetic operators
        /// </summary>
        [TestMethod]
        public void TestArithmeticOperatorsReal() {
            AssertExprType("+ 1.0", SystemUnit.ExtendedType);
            AssertExprType("- 1.0", SystemUnit.ExtendedType);
            AssertExprType("1.0 + 1", SystemUnit.ExtendedType);
            AssertExprType("1.0 - 1", SystemUnit.ExtendedType);
            AssertExprType("1.0 * 1", SystemUnit.ExtendedType);
            AssertExprType("1.0 / 1", SystemUnit.ExtendedType);
            AssertExprType("1 + 1.0", SystemUnit.ExtendedType);
            AssertExprType("1 - 1.0", SystemUnit.ExtendedType);
            AssertExprType("1 * 1.0", SystemUnit.ExtendedType);
            AssertExprType("1 / 1.0", SystemUnit.ExtendedType);
            AssertExprType("1.0 + 1.0", SystemUnit.ExtendedType);
            AssertExprType("1.0 - 1.0", SystemUnit.ExtendedType);
            AssertExprType("1.0 * 1.0", SystemUnit.ExtendedType);
            AssertExprType("1.0 / 1.0", SystemUnit.ExtendedType);
            AssertExprType("1.0 + 4294967296", SystemUnit.ExtendedType);
            AssertExprType("1.0 - 4294967296", SystemUnit.ExtendedType);
            AssertExprType("1.0 * 4294967296", SystemUnit.ExtendedType);
            AssertExprType("1.0 / 4294967296", SystemUnit.ExtendedType);
            AssertExprType("4294967296 + 1.0", SystemUnit.ExtendedType);
            AssertExprType("4294967296 - 1.0", SystemUnit.ExtendedType);
            AssertExprType("4294967296 * 1.0", SystemUnit.ExtendedType);
            AssertExprType("4294967296 / 1.0", SystemUnit.ExtendedType);
        }

        /// <summary>
        ///     test concatenate operator
        /// </summary>
        [TestMethod]
        public void TestConcatOperator() {
            AssertExprType("'a' + 'b'", SystemUnit.UnicodeStringType);
            AssertExprType("'ac' + 'b'", SystemUnit.UnicodeStringType);
            AssertExprType("'a' + 'bc'", SystemUnit.UnicodeStringType);
            AssertExprType("'ac' + 'bc'", SystemUnit.UnicodeStringType);
        }

        /// <summary>
        ///     test bitwise operators
        /// </summary>
        [TestMethod]
        public void TestBitwiseOperators() {
            AssertExprType("not 1", SystemUnit.ShortIntType);
            AssertExprType("not 256", SystemUnit.SmallIntType);
            AssertExprType("not 4294967295", SystemUnit.ShortIntType);
            AssertExprType("not 4294967296", SystemUnit.Int64Type);

            AssertExprType("1 and 1", SystemUnit.ShortIntType);
            AssertExprType("1 and 256", SystemUnit.ShortIntType);
            AssertExprType("277 and 256", SystemUnit.SmallIntType);
            AssertExprType("1 and 65536", SystemUnit.ShortIntType);
            AssertExprType("1 and 4294967296", SystemUnit.ShortIntType);
            AssertExprType("1 and 1", SystemUnit.ShortIntType);
            AssertExprType("256 and 1", SystemUnit.ShortIntType);
            AssertExprType("65536 and 1", SystemUnit.ShortIntType);
            AssertExprType("4294967296 and 1", SystemUnit.ShortIntType);

            AssertExprType("1 or 1", SystemUnit.ShortIntType);
            AssertExprType("1 or 256", SystemUnit.SmallIntType);
            AssertExprType("1 or 65536", SystemUnit.IntegerType);
            AssertExprType("1 or 4294967296", SystemUnit.Int64Type);
            AssertExprType("1 or 1", SystemUnit.ShortIntType);
            AssertExprType("256 or 1", SystemUnit.SmallIntType);
            AssertExprType("65536 or 1", SystemUnit.IntegerType);
            AssertExprType("4294967296 or 1", SystemUnit.Int64Type);

            AssertExprType("1 xor 1", SystemUnit.ShortIntType);
            AssertExprType("1 xor 256", SystemUnit.SmallIntType);
            AssertExprType("1 xor 65536", SystemUnit.IntegerType);
            AssertExprType("1 xor 4294967296", SystemUnit.Int64Type);
            AssertExprType("1 xor 1", SystemUnit.ShortIntType);
            AssertExprType("256 xor 1", SystemUnit.SmallIntType);
            AssertExprType("65536 xor 1", SystemUnit.IntegerType);
            AssertExprType("4294967296 xor 1", SystemUnit.Int64Type);
        }

        /// <summary>
        ///     test shifting operators
        /// </summary>
        [TestMethod]
        public void TestShiftingOperators() {
            AssertExprType("1 shr 1", SystemUnit.ShortIntType);
            AssertExprType("1 shr 256", SystemUnit.ShortIntType);
            AssertExprType("1 shr 65536", SystemUnit.ShortIntType);
            AssertExprType("1 shr 4294967296", SystemUnit.ShortIntType);
            AssertExprType("256 shr 1", SystemUnit.ByteType);
            AssertExprType("256 shr 256", SystemUnit.SmallIntType);
            AssertExprType("256 shr 65536", SystemUnit.SmallIntType);
            AssertExprType("256 shr 4294967296", SystemUnit.SmallIntType);
            AssertExprType("65536 shr 1", SystemUnit.WordType);
            AssertExprType("65536 shr 256", SystemUnit.IntegerType);
            AssertExprType("65536 shr 65536", SystemUnit.IntegerType);
            AssertExprType("65536 shr 4294967296", SystemUnit.IntegerType);
            AssertExprType("4294967296 shr 1", SystemUnit.CardinalType);
            AssertExprType("4294967296 shr 256", SystemUnit.Int64Type);
            AssertExprType("4294967296 shr 65536", SystemUnit.Int64Type);
            AssertExprType("4294967296 shr 4294967296", SystemUnit.Int64Type);

            AssertExprType("1 shl 1", SystemUnit.ShortIntType);
            AssertExprType("1 shl 256", SystemUnit.ShortIntType);
            AssertExprType("1 shl 65536", SystemUnit.ShortIntType);
            AssertExprType("1 shl 4294967296", SystemUnit.ShortIntType);
            AssertExprType("256 shl 1", SystemUnit.SmallIntType);
            AssertExprType("256 shl 256", SystemUnit.SmallIntType);
            AssertExprType("256 shl 65536", SystemUnit.SmallIntType);
            AssertExprType("256 shl 4294967296", SystemUnit.SmallIntType);
            AssertExprType("65536 shl 1", SystemUnit.IntegerType);
            AssertExprType("65536 shl 256", SystemUnit.IntegerType);
            AssertExprType("65536 shl 65536", SystemUnit.IntegerType);
            AssertExprType("65536 shl 4294967296", SystemUnit.IntegerType);
            AssertExprType("4294967296 shl 1", SystemUnit.Int64Type);
            AssertExprType("4294967296 shl 256", SystemUnit.Int64Type);
            AssertExprType("4294967296 shl 65536", SystemUnit.Int64Type);
            AssertExprType("4294967296 shl 4294967296", SystemUnit.Int64Type);
        }

        /// <summary>
        ///     test boolean relational operators
        /// </summary>
        [TestMethod]
        public void TestBooleanRelationalOperators() {
            AssertExprType("true = true", SystemUnit.BooleanType);
            AssertExprType("true <> true", SystemUnit.BooleanType);
            AssertExprType("true < true", SystemUnit.BooleanType);
            AssertExprType("true > true", SystemUnit.BooleanType);
            AssertExprType("true <= true", SystemUnit.BooleanType);
            AssertExprType("true >= true", SystemUnit.BooleanType);
        }

        /// <summary>
        ///     test enumerated relational operators
        /// </summary>
        [TestMethod]
        public void TestEnumRelationalOperators() {
            AssertExprType("a = b", SystemUnit.BooleanType, "type te = (a, b, c)");
            AssertExprType("a <> b", SystemUnit.BooleanType, "type te = (a, b, c)");
        }

        /// <summary>
        ///     test integer relation operators
        /// </summary>
        [TestMethod]
        public void TestIntegerRelationalOperators() {
            AssertExprType("1 =  1", SystemUnit.BooleanType);
            AssertExprType("1 <> 1", SystemUnit.BooleanType);
            AssertExprType("1 <  1", SystemUnit.BooleanType);
            AssertExprType("1 >  1", SystemUnit.BooleanType);
            AssertExprType("1 <= 1", SystemUnit.BooleanType);
            AssertExprType("1 >= 1", SystemUnit.BooleanType);
            AssertExprType("256 =  1", SystemUnit.BooleanType);
            AssertExprType("256 <> 1", SystemUnit.BooleanType);
            AssertExprType("256 <  1", SystemUnit.BooleanType);
            AssertExprType("256 >  1", SystemUnit.BooleanType);
            AssertExprType("256 <= 1", SystemUnit.BooleanType);
            AssertExprType("256 >= 1", SystemUnit.BooleanType);
            AssertExprType("1 =  256", SystemUnit.BooleanType);
            AssertExprType("1 <> 256", SystemUnit.BooleanType);
            AssertExprType("1 <  256", SystemUnit.BooleanType);
            AssertExprType("1 >  256", SystemUnit.BooleanType);
            AssertExprType("1 <= 256", SystemUnit.BooleanType);
            AssertExprType("1 >= 256", SystemUnit.BooleanType);
            AssertExprType("65536 =  1", SystemUnit.BooleanType);
            AssertExprType("65536 <> 1", SystemUnit.BooleanType);
            AssertExprType("65536 <  1", SystemUnit.BooleanType);
            AssertExprType("65536 >  1", SystemUnit.BooleanType);
            AssertExprType("65536 <= 1", SystemUnit.BooleanType);
            AssertExprType("65536 >= 1", SystemUnit.BooleanType);
            AssertExprType("1 =  65536", SystemUnit.BooleanType);
            AssertExprType("1 <> 65536", SystemUnit.BooleanType);
            AssertExprType("1 <  65536", SystemUnit.BooleanType);
            AssertExprType("1 >  65536", SystemUnit.BooleanType);
            AssertExprType("1 <= 65536", SystemUnit.BooleanType);
            AssertExprType("1 >= 65536", SystemUnit.BooleanType);
            AssertExprType("4294967296 =  1", SystemUnit.BooleanType);
            AssertExprType("4294967296 <> 1", SystemUnit.BooleanType);
            AssertExprType("4294967296 <  1", SystemUnit.BooleanType);
            AssertExprType("4294967296 >  1", SystemUnit.BooleanType);
            AssertExprType("4294967296 <= 1", SystemUnit.BooleanType);
            AssertExprType("4294967296 >= 1", SystemUnit.BooleanType);
            AssertExprType("1 =  4294967296", SystemUnit.BooleanType);
            AssertExprType("1 <> 4294967296", SystemUnit.BooleanType);
            AssertExprType("1 <  4294967296", SystemUnit.BooleanType);
            AssertExprType("1 >  4294967296", SystemUnit.BooleanType);
            AssertExprType("1 <= 4294967296", SystemUnit.BooleanType);
            AssertExprType("1 >= 4294967296", SystemUnit.BooleanType);
        }

        /// <summary>
        ///     test real relational operators
        /// </summary>
        [TestMethod]
        public void TestRealRelationalOperators() {
            AssertExprType("1.0 =  1", SystemUnit.BooleanType);
            AssertExprType("1.0 <> 1", SystemUnit.BooleanType);
            AssertExprType("1.0 <  1", SystemUnit.BooleanType);
            AssertExprType("1.0 >  1", SystemUnit.BooleanType);
            AssertExprType("1.0 <= 1", SystemUnit.BooleanType);
            AssertExprType("1.0 >= 1", SystemUnit.BooleanType);
            AssertExprType("1 =  1.0", SystemUnit.BooleanType);
            AssertExprType("1 <> 1.0", SystemUnit.BooleanType);
            AssertExprType("1 <  1.0", SystemUnit.BooleanType);
            AssertExprType("1 >  1.0", SystemUnit.BooleanType);
            AssertExprType("1 <= 1.0", SystemUnit.BooleanType);
            AssertExprType("1 >= 1.0", SystemUnit.BooleanType);
            AssertExprType("1.0 =  1.0", SystemUnit.BooleanType);
            AssertExprType("1.0 <> 1.0", SystemUnit.BooleanType);
            AssertExprType("1.0 <  1.0", SystemUnit.BooleanType);
            AssertExprType("1.0 >  1.0", SystemUnit.BooleanType);
            AssertExprType("1.0 >= 1.0", SystemUnit.BooleanType);
            AssertExprType("1.0 <= 1.0", SystemUnit.BooleanType);

            AssertExprType("1 =  1", SystemUnit.BooleanType);
            AssertExprType("1 <> 1", SystemUnit.BooleanType);
            AssertExprType("1 <  1", SystemUnit.BooleanType);
            AssertExprType("1 >  1", SystemUnit.BooleanType);
            AssertExprType("1 <= 1", SystemUnit.BooleanType);
            AssertExprType("1 >= 1", SystemUnit.BooleanType);
            AssertExprType("256 =  1", SystemUnit.BooleanType);
            AssertExprType("256 <> 1", SystemUnit.BooleanType);
            AssertExprType("256 <  1", SystemUnit.BooleanType);
            AssertExprType("256 >  1", SystemUnit.BooleanType);
            AssertExprType("256 <= 1", SystemUnit.BooleanType);
            AssertExprType("256 >= 1", SystemUnit.BooleanType);
            AssertExprType("1 =  256", SystemUnit.BooleanType);
            AssertExprType("1 <> 256", SystemUnit.BooleanType);
            AssertExprType("1 <  256", SystemUnit.BooleanType);
            AssertExprType("1 >  256", SystemUnit.BooleanType);
            AssertExprType("1 <= 256", SystemUnit.BooleanType);
            AssertExprType("1 >= 256", SystemUnit.BooleanType);
            AssertExprType("65536 =  1", SystemUnit.BooleanType);
            AssertExprType("65536 <> 1", SystemUnit.BooleanType);
            AssertExprType("65536 <  1", SystemUnit.BooleanType);
            AssertExprType("65536 >  1", SystemUnit.BooleanType);
            AssertExprType("65536 <= 1", SystemUnit.BooleanType);
            AssertExprType("65536 >= 1", SystemUnit.BooleanType);
            AssertExprType("1 =  65536", SystemUnit.BooleanType);
            AssertExprType("1 <> 65536", SystemUnit.BooleanType);
            AssertExprType("1 <  65536", SystemUnit.BooleanType);
            AssertExprType("1 >  65536", SystemUnit.BooleanType);
            AssertExprType("1 <= 65536", SystemUnit.BooleanType);
            AssertExprType("1 >= 65536", SystemUnit.BooleanType);
            AssertExprType("4294967296 =  1", SystemUnit.BooleanType);
            AssertExprType("4294967296 <> 1", SystemUnit.BooleanType);
            AssertExprType("4294967296 <  1", SystemUnit.BooleanType);
            AssertExprType("4294967296 >  1", SystemUnit.BooleanType);
            AssertExprType("4294967296 <= 1", SystemUnit.BooleanType);
            AssertExprType("4294967296 >= 1", SystemUnit.BooleanType);
            AssertExprType("1 =  4294967296", SystemUnit.BooleanType);
            AssertExprType("1 <> 4294967296", SystemUnit.BooleanType);
            AssertExprType("1 <  4294967296", SystemUnit.BooleanType);
            AssertExprType("1 >  4294967296", SystemUnit.BooleanType);
            AssertExprType("1 <= 4294967296", SystemUnit.BooleanType);
            AssertExprType("1 >= 4294967296", SystemUnit.BooleanType);
        }

        /// <summary>
        ///     test string relational operators
        /// </summary>
        [TestMethod]
        public void TestStringRelationalOperators() {
            AssertExprType("'a' =  'b'", SystemUnit.BooleanType);
            AssertExprType("'a' <> 'b'", SystemUnit.BooleanType);
            AssertExprType("'a' <  'b'", SystemUnit.BooleanType);
            AssertExprType("'a' >  'b'", SystemUnit.BooleanType);
            AssertExprType("'a' <= 'b'", SystemUnit.BooleanType);
            AssertExprType("'a' >= 'b'", SystemUnit.BooleanType);

            AssertExprType("'a1' =  'b'", SystemUnit.BooleanType);
            AssertExprType("'a1' <> 'b'", SystemUnit.BooleanType);
            AssertExprType("'a1' <  'b'", SystemUnit.BooleanType);
            AssertExprType("'a1' >  'b'", SystemUnit.BooleanType);
            AssertExprType("'a1' <= 'b'", SystemUnit.BooleanType);
            AssertExprType("'a1' >= 'b'", SystemUnit.BooleanType);

            AssertExprType("'a' =  'b1'", SystemUnit.BooleanType);
            AssertExprType("'a' <> 'b1'", SystemUnit.BooleanType);
            AssertExprType("'a' <  'b1'", SystemUnit.BooleanType);
            AssertExprType("'a' >  'b1'", SystemUnit.BooleanType);
            AssertExprType("'a' <= 'b1'", SystemUnit.BooleanType);
            AssertExprType("'a' >= 'b1'", SystemUnit.BooleanType);
        }

        /// <summary>
        ///     test arithmetic operators
        /// </summary>
        [TestMethod]
        public void TestArithmeticOperatorsIndirect() {
            var d = new[] {
                Tuple.Create("Byte", SystemUnit.IntegerType),
                Tuple.Create("Word", SystemUnit.IntegerType),
                Tuple.Create("Cardinal", SystemUnit.CardinalType),
                Tuple.Create("UInt64", SystemUnit.UInt64Type),
                Tuple.Create("SmallInt", SystemUnit.IntegerType),
                Tuple.Create("ShortInt", SystemUnit.IntegerType),
                Tuple.Create("Integer", SystemUnit.IntegerType),
                Tuple.Create("Int64", SystemUnit.Int64Type),
            };

            // subrange types
            var e = CreateEnvironment();
            var ct = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var sr = ct.CreateSubrangeType(SystemUnit.IntegerType, GetIntegerValue((sbyte)-1), GetIntegerValue((sbyte)1));
            AssertExprTypeByVar("-1..1", "+ a", sr);
            AssertExprTypeByVar("-1..1", "- a", sr);

            AssertExprTypeByVar("Byte", "+ a", SystemUnit.ByteType);
            AssertExprTypeByVar("Word", "+ a", SystemUnit.WordType);
            AssertExprTypeByVar("Cardinal", "+ a", SystemUnit.CardinalType);
            AssertExprTypeByVar("UInt64", "+ a", SystemUnit.UInt64Type);
            AssertExprTypeByVar("ShortInt", "+ a", SystemUnit.ShortIntType);
            AssertExprTypeByVar("SmallInt", "+ a", SystemUnit.SmallIntType);
            AssertExprTypeByVar("Integer", "+ a", SystemUnit.IntegerType);
            AssertExprTypeByVar("Int64", "+ a", SystemUnit.Int64Type);

            AssertExprTypeByVar("Byte", "- a", SystemUnit.ByteType);
            AssertExprTypeByVar("Word", "- a", SystemUnit.WordType);
            AssertExprTypeByVar("Cardinal", "- a", SystemUnit.CardinalType);
            AssertExprTypeByVar("UInt64", "- a", SystemUnit.UInt64Type);
            AssertExprTypeByVar("ShortInt", "- a", SystemUnit.ShortIntType);
            AssertExprTypeByVar("SmallInt", "- a", SystemUnit.SmallIntType);
            AssertExprTypeByVar("Integer", "- a", SystemUnit.IntegerType);
            AssertExprTypeByVar("Int64", "- a", SystemUnit.Int64Type);


            foreach (var e1 in d) {
                AssertExprTypeByVar(e1.Item1, "a + b", e1.Item2);
                AssertExprTypeByVar(e1.Item1, "a - b", e1.Item2);
                AssertExprTypeByVar(e1.Item1, "a div b", e1.Item2);
                AssertExprTypeByVar(e1.Item1, "a * b", e1.Item2);
                AssertExprTypeByVar(e1.Item1, "a mod b", e1.Item2);
                AssertExprTypeByVar(e1.Item1, "a / b", SystemUnit.ExtendedType);
            }

            // subrange type
            AssertExprTypeByVar("-1..1", "a + b", SystemUnit.IntegerType);
            AssertExprTypeByVar("-1..1", "a - b", SystemUnit.IntegerType);
            AssertExprTypeByVar("-1..1", "a div b", SystemUnit.IntegerType);
            AssertExprTypeByVar("-1..1", "a * b", SystemUnit.IntegerType);
            AssertExprTypeByVar("-1..1", "a mod b", SystemUnit.IntegerType);
            AssertExprTypeByVar("-1..1", "a / b", SystemUnit.ExtendedType);

        }

        /// <summary>
        ///     test logical operators
        /// </summary>
        [TestMethod]
        public void TestLogicOperatorsIntIndirect() {
            var d = new[] {
                Tuple.Create("Byte", SystemUnit.ByteType),
                Tuple.Create("Word", SystemUnit.WordType),
                Tuple.Create("Cardinal", SystemUnit.CardinalType),
                Tuple.Create("UInt64", SystemUnit.UInt64Type),
                Tuple.Create("SmallInt", SystemUnit.SmallIntType),
                Tuple.Create("ShortInt", SystemUnit.ShortIntType),
                Tuple.Create("Integer", SystemUnit.IntegerType),
                Tuple.Create("Int64", SystemUnit.Int64Type),
            };

            // subrange types
            var e = CreateEnvironment();
            var ct = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var sr = ct.CreateSubrangeType(SystemUnit.IntegerType, GetIntegerValue((sbyte)-1), GetIntegerValue((sbyte)1));

            AssertExprTypeByVar("-1..1", "not a", sr);
            AssertExprTypeByVar("-1..1", "not a", sr);

            AssertExprTypeByVar("Byte", "not a", SystemUnit.ByteType);
            AssertExprTypeByVar("Word", "not a", SystemUnit.WordType);
            AssertExprTypeByVar("Cardinal", "not a", SystemUnit.CardinalType);
            AssertExprTypeByVar("UInt64", "not a", SystemUnit.UInt64Type);
            AssertExprTypeByVar("ShortInt", "not a", SystemUnit.ShortIntType);
            AssertExprTypeByVar("SmallInt", "not a", SystemUnit.SmallIntType);
            AssertExprTypeByVar("Integer", "not a", SystemUnit.IntegerType);
            AssertExprTypeByVar("Int64", "not a", SystemUnit.Int64Type);

            foreach (var e1 in d) {
                AssertExprTypeByVar(e1.Item1, "a or b", e1.Item2);
                AssertExprTypeByVar(e1.Item1, "a and b", e1.Item2);
                AssertExprTypeByVar(e1.Item1, "a xor b", e1.Item2);
            }

            // subrange type
            AssertExprTypeByVar("-1..1", "a or b", SystemUnit.ShortIntType);
            AssertExprTypeByVar("-1..1", "a xor b", SystemUnit.ShortIntType);
            AssertExprTypeByVar("-1..1", "a and b", SystemUnit.ShortIntType);
        }

        /// <summary>
        ///     test logical operators
        /// </summary>
        [TestMethod]
        public void TestLogicOperatorsBoolIndirect() {
            var d = new[] {
                Tuple.Create("Boolean", SystemUnit.BooleanType),
                Tuple.Create("ByteBool", SystemUnit.ByteBoolType),
                Tuple.Create("WordBool", SystemUnit.WordBoolType),
            };

            // subrange types
            var e = CreateEnvironment();
            var ct = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var sr = ct.CreateSubrangeType(SystemUnit.IntegerType, GetIntegerValue((sbyte)-1), GetIntegerValue((sbyte)1));
            AssertExprTypeByVar("-1..1", "not a", sr);
            AssertExprTypeByVar("-1..1", "not a", sr);

            AssertExprTypeByVar("Boolean", "not a", SystemUnit.BooleanType);
            AssertExprTypeByVar("ByteBool", "not a", SystemUnit.ByteBoolType);
            AssertExprTypeByVar("WordBool", "not a", SystemUnit.WordBoolType);

            foreach (var e1 in d) {
                AssertExprTypeByVar(e1.Item1, "a or b", e1.Item2);
                AssertExprTypeByVar(e1.Item1, "a and b", e1.Item2);
                AssertExprTypeByVar(e1.Item1, "a xor b", e1.Item2);
            }

            // subrange type
            AssertExprTypeByVar("false..true", "a or b", SystemUnit.BooleanType);
            AssertExprTypeByVar("false..true", "a xor b", SystemUnit.BooleanType);
            AssertExprTypeByVar("false..true", "a and b", SystemUnit.BooleanType);
        }

        /// <summary>
        ///     test pointer operator
        /// </summary>
        [TestMethod]
        public void TestPointerOperator() {
            var r = CreateEnvironment().Runtime;
            var pv = r.Types.MakeOperatorResult(OperatorKind.AtOperator, SystemUnit.PIntegerType.Reference, r.Types.MakeSignature(SystemUnit.ErrorType.Reference, SystemUnit.IntegerType.Reference));
            AssertExprValue("@a", pv, "var a: integer;", isConstant: false);
            AssertExprValue("@a", GetPointerValue(GetIntegerValue(4)), "const a = 4;");
        }

        /// <summary>
        ///     test shift operator
        /// </summary>
        [TestMethod]
        public void TestShiftOperators() {
            AssertExprTypeByVar("Byte", "a shl 33", SystemUnit.ErrorType);
            AssertExprTypeByVar("Word", "a shl 33", SystemUnit.ErrorType);
            AssertExprTypeByVar("Cardinal", "a shl 33", SystemUnit.ErrorType);
            AssertExprTypeByVar("UInt64", "a shl 33", SystemUnit.UInt64Type);
            AssertExprTypeByVar("ShortInt", "a shl 33", SystemUnit.ErrorType);
            AssertExprTypeByVar("SmallInt", "a shl 33", SystemUnit.ErrorType);
            AssertExprTypeByVar("Integer", "a shl 33", SystemUnit.ErrorType);
            AssertExprTypeByVar("Int64", "a shl 33", SystemUnit.Int64Type);
            AssertExprTypeByVar("Byte", "a shl 32", SystemUnit.IntegerType);
            AssertExprTypeByVar("Word", "a shl 32", SystemUnit.IntegerType);
            AssertExprTypeByVar("Cardinal", "a shl 32", SystemUnit.CardinalType);
            AssertExprTypeByVar("UInt64", "a shl 32", SystemUnit.UInt64Type);
            AssertExprTypeByVar("ShortInt", "a shl 32", SystemUnit.IntegerType);
            AssertExprTypeByVar("SmallInt", "a shl 32", SystemUnit.IntegerType);
            AssertExprTypeByVar("Integer", "a shl 32", SystemUnit.IntegerType);
            AssertExprTypeByVar("Int64", "a shl 32", SystemUnit.Int64Type);
            AssertExprTypeByVar("Byte", "a shr 33", SystemUnit.ErrorType);
            AssertExprTypeByVar("Word", "a shr 33", SystemUnit.ErrorType);
            AssertExprTypeByVar("Cardinal", "a shr 33", SystemUnit.ErrorType);
            AssertExprTypeByVar("UInt64", "a shr 33", SystemUnit.UInt64Type);
            AssertExprTypeByVar("ShortInt", "a shr 33", SystemUnit.ErrorType);
            AssertExprTypeByVar("SmallInt", "a shr 33", SystemUnit.ErrorType);
            AssertExprTypeByVar("Integer", "a shr 33", SystemUnit.ErrorType);
            AssertExprTypeByVar("Int64", "a shr 33", SystemUnit.Int64Type);
            AssertExprTypeByVar("Byte", "a shr 32", SystemUnit.IntegerType);
            AssertExprTypeByVar("Word", "a shr 32", SystemUnit.IntegerType);
            AssertExprTypeByVar("Cardinal", "a shr 32", SystemUnit.CardinalType);
            AssertExprTypeByVar("UInt64", "a shr 32", SystemUnit.UInt64Type);
            AssertExprTypeByVar("ShortInt", "a shr 32", SystemUnit.IntegerType);
            AssertExprTypeByVar("SmallInt", "a shr 32", SystemUnit.IntegerType);
            AssertExprTypeByVar("Integer", "a shr 32", SystemUnit.IntegerType);
            AssertExprTypeByVar("Int64", "a shr 32", SystemUnit.Int64Type);
        }

        /// <summary>
        ///     test relational operators
        /// </summary>
        [TestMethod]
        public void TestRelationalOperatorsStringIndirect() {
            AssertExprTypeByVar("String", "a = b", SystemUnit.BooleanType);
            AssertExprTypeByVar("String", "a <> b", SystemUnit.BooleanType);
            AssertExprTypeByVar("String", "a < b", SystemUnit.BooleanType);
            AssertExprTypeByVar("String", "a > b", SystemUnit.BooleanType);
            AssertExprTypeByVar("String", "a <= b", SystemUnit.BooleanType);
            AssertExprTypeByVar("String", "a >= b", SystemUnit.BooleanType);
            AssertExprTypeByVar("Char", "a = b", SystemUnit.BooleanType);
            AssertExprTypeByVar("Char", "a <> b", SystemUnit.BooleanType);
            AssertExprTypeByVar("Char", "a < b", SystemUnit.BooleanType);
            AssertExprTypeByVar("Char", "a > b", SystemUnit.BooleanType);
            AssertExprTypeByVar("Char", "a <= b", SystemUnit.BooleanType);
            AssertExprTypeByVar("Char", "a >= b", SystemUnit.BooleanType);
            AssertExprTypeByVar("AnsiChar", "a = b", SystemUnit.BooleanType);
            AssertExprTypeByVar("AnsiChar", "a <> b", SystemUnit.BooleanType);
            AssertExprTypeByVar("AnsiChar", "a < b", SystemUnit.BooleanType);
            AssertExprTypeByVar("AnsiChar", "a > b", SystemUnit.BooleanType);
            AssertExprTypeByVar("AnsiChar", "a <= b", SystemUnit.BooleanType);
            AssertExprTypeByVar("AnsiChar", "a >= b", SystemUnit.BooleanType);


        }

        /// <summary>
        ///     test relational operators
        /// </summary>
        [TestMethod]
        public void TestRelationalOperatorsIntIndirect() {
            var d = new[] {
                Tuple.Create("Byte", SystemUnit.ByteType),
                Tuple.Create("Word", SystemUnit.WordType),
                Tuple.Create("Cardinal", SystemUnit.CardinalType),
                Tuple.Create("UInt64", SystemUnit.UInt64Type),
                Tuple.Create("SmallInt", SystemUnit.SmallIntType),
                Tuple.Create("ShortInt", SystemUnit.ShortIntType),
                Tuple.Create("Integer", SystemUnit.IntegerType),
                Tuple.Create("Int64", SystemUnit.Int64Type),
            };

            foreach (var dd in d) {
                AssertExprTypeByVar(dd.Item1, "a = b", SystemUnit.BooleanType);
                AssertExprTypeByVar(dd.Item1, "a <> b", SystemUnit.BooleanType);
                AssertExprTypeByVar(dd.Item1, "a < b", SystemUnit.BooleanType);
                AssertExprTypeByVar(dd.Item1, "a > b", SystemUnit.BooleanType);
                AssertExprTypeByVar(dd.Item1, "a <= b", SystemUnit.BooleanType);
                AssertExprTypeByVar(dd.Item1, "a >= b", SystemUnit.BooleanType);
            }

            // subrange
            AssertExprTypeByVar("-1..1", "a = b", SystemUnit.BooleanType);
            AssertExprTypeByVar("-1..1", "a <> b", SystemUnit.BooleanType);
            AssertExprTypeByVar("-1..1", "a < b", SystemUnit.BooleanType);
            AssertExprTypeByVar("-1..1", "a > b", SystemUnit.BooleanType);
            AssertExprTypeByVar("-1..1", "a <= b", SystemUnit.BooleanType);
            AssertExprTypeByVar("-1..1", "a >= b", SystemUnit.BooleanType);

        }

        /// <summary>
        ///     test relational operators
        /// </summary>
        [TestMethod]
        public void TestRelationalOperatorsBoolIndirect() {
            AssertExprTypeByVar("Boolean", "a = b", SystemUnit.BooleanType);
            AssertExprTypeByVar("Boolean", "a <> b", SystemUnit.BooleanType);
            AssertExprTypeByVar("Boolean", "a < b", SystemUnit.BooleanType);
            AssertExprTypeByVar("Boolean", "a > b", SystemUnit.BooleanType);
            AssertExprTypeByVar("Boolean", "a <= b", SystemUnit.BooleanType);
            AssertExprTypeByVar("Boolean", "a >= b", SystemUnit.BooleanType);

            AssertExprTypeByVar("ByteBool", "a = b", SystemUnit.BooleanType);
            AssertExprTypeByVar("ByteBool", "a <> b", SystemUnit.BooleanType);
            AssertExprTypeByVar("ByteBool", "a < b", SystemUnit.BooleanType);
            AssertExprTypeByVar("ByteBool", "a > b", SystemUnit.BooleanType);
            AssertExprTypeByVar("ByteBool", "a <= b", SystemUnit.BooleanType);
            AssertExprTypeByVar("ByteBool", "a >= b", SystemUnit.BooleanType);

            AssertExprTypeByVar("WordBool", "a = b", SystemUnit.BooleanType);
            AssertExprTypeByVar("WordBool", "a <> b", SystemUnit.BooleanType);
            AssertExprTypeByVar("WordBool", "a < b", SystemUnit.BooleanType);
            AssertExprTypeByVar("WordBool", "a > b", SystemUnit.BooleanType);
            AssertExprTypeByVar("WordBool", "a <= b", SystemUnit.BooleanType);
            AssertExprTypeByVar("WordBool", "a >= b", SystemUnit.BooleanType);
        }

        /// <summary>
        ///     test concatenate operators
        /// </summary>
        [TestMethod]
        public void TestConcatOperatorIndirect() {
            AssertExprTypeByVar("UnicodeString", "a + b", SystemUnit.UnicodeStringType);
            AssertExprTypeByVar("AnsiString", "a + b", SystemUnit.AnsiStringType);
        }

        /// <summary>
        ///     test array operators
        /// </summary>
        [TestMethod]
        public void TestConstantArrayTypes() {
            AssertExprTypeByConst("(1,2)", SystemUnit.UnspecifiedType, false, "static array [Subrange i] of zc");
            AssertExprTypeByConst("('a','b')", SystemUnit.UnspecifiedType, false, "static array [Subrange i] of b");
            AssertExprTypeByConst("('aa','b')", SystemUnit.UnspecifiedType, false, "static array [Subrange i] of System@UnicodeString");
            AssertExprTypeByConst("(1.0, 1.4)", SystemUnit.UnspecifiedType, false, "static array [Subrange i] of g");
            AssertExprTypeByConst("(a, b)", SystemUnit.UnspecifiedType, false, "static array [Subrange i] of enum zc", "type ta = (a,b);");

            var c = CreateEnvironment();
            var ct = c.TypeRegistry.CreateTypeFactory(c.TypeRegistry.SystemUnit);
            var at = ct.CreateStaticArrayType(SystemUnit.StringType, string.Empty, SystemUnit.IntegerType, false);
            var av = c.Runtime.Structured.CreateArrayValue(at, SystemUnit.StringType, ImmutableArray.Create(GetUnicodeStringValue("11"), GetUnicodeStringValue("2")));
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
            var soi = tc.CreateSetType(SystemUnit.IntegerType, "Ta");
            var soc = tc.CreateSetType(SystemUnit.WideCharType, "Ta");
            var sob = tc.CreateSetType(SystemUnit.BooleanType, "Ta");

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
            var soi = tc.CreateSetType(SystemUnit.IntegerType, "Ta");
            var soc = tc.CreateSetType(SystemUnit.WideCharType, "Ta");
            var sob = tc.CreateSetType(SystemUnit.BooleanType, "Ta");


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
            var soi = tc.CreateSetType(SystemUnit.IntegerType, "Ta");
            var soc = tc.CreateSetType(SystemUnit.WideCharType, "Ta");
            var sob = tc.CreateSetType(SystemUnit.BooleanType, "Ta");

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
            var soi = tc.CreateSetType(SystemUnit.IntegerType, "Ta");
            var soc = tc.CreateSetType(SystemUnit.WideCharType, "Ta");
            var sob = tc.CreateSetType(SystemUnit.BooleanType, "Ta");

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

            AssertExprValue("a = b", goc(OperatorKind.EqualsOperator, SystemUnit.BooleanType), "type Ta = set of integer; var a, b: Ta;", isConstant: false);
            AssertExprValue("a = b", goc(OperatorKind.EqualsOperator, SystemUnit.BooleanType), "type Ta = set of char; var a, b: Ta;", isConstant: false);
            AssertExprValue("a = b", goc(OperatorKind.EqualsOperator, SystemUnit.BooleanType), "type Ta = set of boolean; var a, b: Ta;", isConstant: false);

            AssertExprValue("a >= b", goc(OperatorKind.GreaterThanOrEqual, SystemUnit.BooleanType), "type Ta = set of integer; var a, b: Ta;", isConstant: false);
            AssertExprValue("a >= b", goc(OperatorKind.GreaterThanOrEqual, SystemUnit.BooleanType), "type Ta = set of char; var a, b: Ta;", isConstant: false);
            AssertExprValue("a >= b", goc(OperatorKind.GreaterThanOrEqual, SystemUnit.BooleanType), "type Ta = set of boolean; var a, b: Ta;", isConstant: false);

            AssertExprValue("a <= b", goc(OperatorKind.LessThanOrEqual, SystemUnit.BooleanType), "type Ta = set of integer; var a, b: Ta;", isConstant: false);
            AssertExprValue("a <= b", goc(OperatorKind.LessThanOrEqual, SystemUnit.BooleanType), "type Ta = set of char; var a, b: Ta;", isConstant: false);
            AssertExprValue("a <= b", goc(OperatorKind.LessThanOrEqual, SystemUnit.BooleanType), "type Ta = set of boolean; var a, b: Ta;", isConstant: false);

            AssertExprValue("a <> b", goc(OperatorKind.NotEqualsOperator, SystemUnit.BooleanType), "type Ta = set of integer; var a, b: Ta;", isConstant: false);
            AssertExprValue("a <> b", goc(OperatorKind.NotEqualsOperator, SystemUnit.BooleanType), "type Ta = set of char; var a, b: Ta;", isConstant: false);
            AssertExprValue("a <> b", goc(OperatorKind.NotEqualsOperator, SystemUnit.BooleanType), "type Ta = set of boolean; var a, b: Ta;", isConstant: false);
        }

        /// <summary>
        ///     test class operator
        /// </summary>
        [TestMethod]
        public void TestClassOperators() {
            AssertExprType("a is TObject", SystemUnit.BooleanType, "var a: TObject;");
            AssertExprType("a is system.TObject", SystemUnit.BooleanType, "var a: TObject;");
            AssertExprType("a is TObject", SystemUnit.BooleanType, "type ta = class end; var a: ta;");
            AssertExprType("a is tb", SystemUnit.ErrorType, "type ta = class end; tb = class end; var a: ta;");
            AssertExprType("a is tb", SystemUnit.BooleanType, "type ta = class end; tb = class(ta) end; var a: ta;");
            AssertExprType("a is ta", SystemUnit.BooleanType, "type ta = class end; tb = class(ta) end; var a: tb;");
            AssertExprType("a is tb", SystemUnit.BooleanType, "type ta = class end; tc = ta; tb = class(tc) end; var a: ta;");
            AssertExprType("a is ta", SystemUnit.BooleanType, "type ta = class end; tc = ta; tb = class(tc) end; var a: tb;");

            AssertExprType("a as TObject", SystemUnit.TObjectType, "var a: TObject;");
            AssertExprType("a as System.TObject", SystemUnit.TObjectType, "var a: TObject;");
            AssertExprType("a as TObject", SystemUnit.TObjectType, "type ta = class end; var a: ta;");
            AssertExprType("a as tb", SystemUnit.ErrorType, "type ta = class end; tb = class end; var a: ta;");

            var e = CreateEnvironment();
            var tc = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var ta = tc.CreateStructuredType("Ta", StructuredTypeKind.Class);
            var tb = tc.CreateStructuredType("Tb", StructuredTypeKind.Class);
            var ia = tc.CreateStructuredType("Ia", StructuredTypeKind.Interface);

            AssertExprType("a as tb", tb, "type ta = class end; tb = class(ta) end; var a: ta;");
            AssertExprType("a as ta", ta, "type ta = class end; tb = class(ta) end; var a: tb;");
            AssertExprType("a as tb", ta, "type ta = class end; tc = ta; tb = class(tc) end; var a: ta;");
            AssertExprType("a as ta", ta, "type ta = class end; tc = ta; tb = class(tc) end; var a: tb;");

            AssertExprType("a is TObject", SystemUnit.BooleanType, "type ia = interface end; var a: ia;");
            AssertExprType("a as ia", ia, "type ia = interface end; ta = class(ia) end; var a: ta;");
            AssertExprType("a as ia", ia, "type ia = interface end; ta = class(tobject, ia) end; var a: ta;");
            AssertExprType("a as ia", SystemUnit.ErrorType, "type ia = interface end; ta = class end; var a: ta;");
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
                var s = CreateEnvironment().Runtime.Types.MakeSignature(SystemUnit.StringType.Reference, p.Reference);
                return CreateEnvironment().Runtime.Types.MakeInvocationResultFromIntrinsic(SystemUnit.FormatExpression, s);
            }

            AssertExprValue("a : -1", goc(SystemUnit.CharType), "var a: Char;", isConstant: false);
            AssertExprValue("a : 4", goc(SystemUnit.CharType), "var a: Char;", isConstant: false);
            AssertExprValue("a : 5", goc(SystemUnit.BooleanType), "var a: Boolean;", isConstant: false);
            AssertExprValue("a : 7", goc(SystemUnit.BooleanType), "var a: Boolean;", isConstant: false);
            AssertExprValue("a : -1", goc(SystemUnit.StringType), "var a: String;", isConstant: false);
            AssertExprValue("a : 4", goc(SystemUnit.StringType), "var a: String;", isConstant: false);
            AssertExprValue("a : -1", goc(SystemUnit.ByteType), "var a: Byte;", isConstant: false);
            AssertExprValue("a : 5", goc(SystemUnit.ByteType), "var a: Byte;", isConstant: false);
            AssertExprValue("a : 5 : 2", goc(SystemUnit.StringType), "var a: String;", isConstant: false);
            AssertExprValue("a : 8", goc(SystemUnit), "var a: String;", isConstant: false);
        }


    }
}
