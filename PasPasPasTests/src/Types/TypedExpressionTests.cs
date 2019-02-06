﻿using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test cases for typed expressions
    /// </summary>
    public class TypedExpressionTest : TypeTest {

        [TestMethod]
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

        [TestMethod]
        public void TestBooleanLiteralTypes() {
            AssertExprType("true", KnownTypeIds.BooleanType);
            AssertExprType("false", KnownTypeIds.BooleanType);
        }

        [TestMethod]
        public void TestCharLiteralTypes() {
            AssertExprType("'C'", KnownTypeIds.WideCharType);
            AssertExprType("#9", KnownTypeIds.WideCharType);
        }

        [TestMethod]
        public void TestStringLiteralTypes() {
            AssertExprType("'CD'", KnownTypeIds.UnicodeStringType);
            AssertExprType("#9#9", KnownTypeIds.UnicodeStringType);
        }

        [TestMethod]
        public void TestExtendedLiteralTypes() {
            AssertExprType("3.5", KnownTypeIds.Extended);
            AssertExprType("2.33434343", KnownTypeIds.Extended);
        }

        [TestMethod]
        public void TestBooleanOperators() {
            AssertExprType("not true", KnownTypeIds.BooleanType);
            AssertExprType("not false", KnownTypeIds.BooleanType);
            AssertExprType("true and false", KnownTypeIds.BooleanType);
            AssertExprType("true or false", KnownTypeIds.BooleanType);
            AssertExprType("true xor false", KnownTypeIds.BooleanType);
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void TestConcatOperator() {
            AssertExprType("'a' + 'b'", KnownTypeIds.UnicodeStringType);
            AssertExprType("'ac' + 'b'", KnownTypeIds.UnicodeStringType);
            AssertExprType("'a' + 'bc'", KnownTypeIds.UnicodeStringType);
            AssertExprType("'ac' + 'bc'", KnownTypeIds.UnicodeStringType);
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void TestBooleanRelationalOperators() {
            AssertExprType("true = true", KnownTypeIds.BooleanType);
            AssertExprType("true <> true", KnownTypeIds.BooleanType);
            AssertExprType("true < true", KnownTypeIds.BooleanType);
            AssertExprType("true > true", KnownTypeIds.BooleanType);
            AssertExprType("true <= true", KnownTypeIds.BooleanType);
            AssertExprType("true >= true", KnownTypeIds.BooleanType);
        }

        [TestMethod]
        public void TestEnumRelationalOperators() {
            AssertExprType("a = b", KnownTypeIds.BooleanType, "type te = (a, b, c)");
            AssertExprType("a <> b", KnownTypeIds.BooleanType, "type te = (a, b, c)");
        }

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

        [TestMethod]
        public void TestArithmeticOperatorsIndirect() {
            var d = new[] {
                Tuple.Create("Byte", KnownTypeIds.IntegerType),
                Tuple.Create("Word", KnownTypeIds.IntegerType),
                Tuple.Create("Cardinal", KnownTypeIds.CardinalType),
                Tuple.Create("UInt64", KnownTypeIds.Uint64Type),
                Tuple.Create("SmallInt", KnownTypeIds.IntegerType),
                Tuple.Create("ShortInt", KnownTypeIds.IntegerType),
                Tuple.Create("Integer", KnownTypeIds.IntegerType),
                Tuple.Create("Int64", KnownTypeIds.Int64Type),
            };

            // subrange types
            AssertExprTypeByVar("-1..1", "+ a", RegisteredTypes.SmallestUserTypeId);
            AssertExprTypeByVar("-1..1", "- a", RegisteredTypes.SmallestUserTypeId);

            AssertExprTypeByVar("Byte", "+ a", KnownTypeIds.ByteType);
            AssertExprTypeByVar("Word", "+ a", KnownTypeIds.WordType);
            AssertExprTypeByVar("Cardinal", "+ a", KnownTypeIds.CardinalType);
            AssertExprTypeByVar("UInt64", "+ a", KnownTypeIds.Uint64Type);
            AssertExprTypeByVar("ShortInt", "+ a", KnownTypeIds.ShortInt);
            AssertExprTypeByVar("SmallInt", "+ a", KnownTypeIds.SmallInt);
            AssertExprTypeByVar("Integer", "+ a", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("Int64", "+ a", KnownTypeIds.Int64Type);

            AssertExprTypeByVar("Byte", "- a", KnownTypeIds.ByteType);
            AssertExprTypeByVar("Word", "- a", KnownTypeIds.WordType);
            AssertExprTypeByVar("Cardinal", "- a", KnownTypeIds.CardinalType);
            AssertExprTypeByVar("UInt64", "- a", KnownTypeIds.Uint64Type);
            AssertExprTypeByVar("ShortInt", "- a", KnownTypeIds.ShortInt);
            AssertExprTypeByVar("SmallInt", "- a", KnownTypeIds.SmallInt);
            AssertExprTypeByVar("Integer", "- a", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("Int64", "- a", KnownTypeIds.Int64Type);


            foreach (var e in d) {
                AssertExprTypeByVar(e.Item1, "a + b", e.Item2);
                AssertExprTypeByVar(e.Item1, "a - b", e.Item2);
                AssertExprTypeByVar(e.Item1, "a div b", e.Item2);
                AssertExprTypeByVar(e.Item1, "a * b", e.Item2);
                AssertExprTypeByVar(e.Item1, "a mod b", e.Item2);
                AssertExprTypeByVar(e.Item1, "a / b", KnownTypeIds.Extended);
            }

            // subrange type
            AssertExprTypeByVar("-1..1", "a + b", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("-1..1", "a - b", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("-1..1", "a div b", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("-1..1", "a * b", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("-1..1", "a mod b", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("-1..1", "a / b", KnownTypeIds.Extended);

        }

        [TestMethod]
        public void TestLogicOperatorsIntIndirect() {
            var d = new[] {
                Tuple.Create("Byte", KnownTypeIds.ByteType),
                Tuple.Create("Word", KnownTypeIds.WordType),
                Tuple.Create("Cardinal", KnownTypeIds.CardinalType),
                Tuple.Create("UInt64", KnownTypeIds.Uint64Type),
                Tuple.Create("SmallInt", KnownTypeIds.SmallInt),
                Tuple.Create("ShortInt", KnownTypeIds.ShortInt),
                Tuple.Create("Integer", KnownTypeIds.IntegerType),
                Tuple.Create("Int64", KnownTypeIds.Int64Type),
            };

            // subrange types
            AssertExprTypeByVar("-1..1", "not a", RegisteredTypes.SmallestUserTypeId);
            AssertExprTypeByVar("-1..1", "not a", RegisteredTypes.SmallestUserTypeId);

            AssertExprTypeByVar("Byte", "not a", KnownTypeIds.ByteType);
            AssertExprTypeByVar("Word", "not a", KnownTypeIds.WordType);
            AssertExprTypeByVar("Cardinal", "not a", KnownTypeIds.CardinalType);
            AssertExprTypeByVar("UInt64", "not a", KnownTypeIds.Uint64Type);
            AssertExprTypeByVar("ShortInt", "not a", KnownTypeIds.ShortInt);
            AssertExprTypeByVar("SmallInt", "not a", KnownTypeIds.SmallInt);
            AssertExprTypeByVar("Integer", "not a", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("Int64", "not a", KnownTypeIds.Int64Type);

            foreach (var e in d) {
                AssertExprTypeByVar(e.Item1, "a or b", e.Item2);
                AssertExprTypeByVar(e.Item1, "a and b", e.Item2);
                AssertExprTypeByVar(e.Item1, "a xor b", e.Item2);
            }

            // subrange type
            AssertExprTypeByVar("-1..1", "a or b", KnownTypeIds.ShortInt);
            AssertExprTypeByVar("-1..1", "a xor b", KnownTypeIds.ShortInt);
            AssertExprTypeByVar("-1..1", "a and b", KnownTypeIds.ShortInt);
        }

        [TestMethod]
        public void TestLogicOperatorsBoolIndirect() {
            var d = new[] {
                Tuple.Create("Boolean", KnownTypeIds.BooleanType),
                Tuple.Create("ByteBool", KnownTypeIds.ByteBoolType),
                Tuple.Create("WordBool", KnownTypeIds.WordBoolType),
            };

            // subrange types
            AssertExprTypeByVar("-1..1", "not a", RegisteredTypes.SmallestUserTypeId);
            AssertExprTypeByVar("-1..1", "not a", RegisteredTypes.SmallestUserTypeId);

            AssertExprTypeByVar("Boolean", "not a", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("ByteBool", "not a", KnownTypeIds.ByteBoolType);
            AssertExprTypeByVar("WordBool", "not a", KnownTypeIds.WordBoolType);

            foreach (var e in d) {
                AssertExprTypeByVar(e.Item1, "a or b", e.Item2);
                AssertExprTypeByVar(e.Item1, "a and b", e.Item2);
                AssertExprTypeByVar(e.Item1, "a xor b", e.Item2);
            }

            // subrange type
            AssertExprTypeByVar("false..true", "a or b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("false..true", "a xor b", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("false..true", "a and b", KnownTypeIds.BooleanType);
        }

        [TestMethod]
        public void TestPointerOperator() {
            AssertExprValue("@a", GetPointerValue(GetUnkownValue(KnownTypeIds.IntegerType, PasPasPas.Globals.Runtime.CommonTypeKind.IntegerType)), "var a: integer;", isConstant: false);
            AssertExprValue("@a", GetPointerValue(GetIntegerValue(4)), "const a = 4;");
        }

        [TestMethod]
        public void TestShiftOperators() {
            AssertExprTypeByVar("Byte", "a shl 33", KnownTypeIds.ErrorType);
            AssertExprTypeByVar("Word", "a shl 33", KnownTypeIds.ErrorType);
            AssertExprTypeByVar("Cardinal", "a shl 33", KnownTypeIds.ErrorType);
            AssertExprTypeByVar("UInt64", "a shl 33", KnownTypeIds.Uint64Type);
            AssertExprTypeByVar("ShortInt", "a shl 33", KnownTypeIds.ErrorType);
            AssertExprTypeByVar("SmallInt", "a shl 33", KnownTypeIds.ErrorType);
            AssertExprTypeByVar("Integer", "a shl 33", KnownTypeIds.ErrorType);
            AssertExprTypeByVar("Int64", "a shl 33", KnownTypeIds.Int64Type);
            AssertExprTypeByVar("Byte", "a shl 32", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("Word", "a shl 32", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("Cardinal", "a shl 32", KnownTypeIds.CardinalType);
            AssertExprTypeByVar("UInt64", "a shl 32", KnownTypeIds.Uint64Type);
            AssertExprTypeByVar("ShortInt", "a shl 32", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("SmallInt", "a shl 32", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("Integer", "a shl 32", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("Int64", "a shl 32", KnownTypeIds.Int64Type);
            AssertExprTypeByVar("Byte", "a shr 33", KnownTypeIds.ErrorType);
            AssertExprTypeByVar("Word", "a shr 33", KnownTypeIds.ErrorType);
            AssertExprTypeByVar("Cardinal", "a shr 33", KnownTypeIds.ErrorType);
            AssertExprTypeByVar("UInt64", "a shr 33", KnownTypeIds.Uint64Type);
            AssertExprTypeByVar("ShortInt", "a shr 33", KnownTypeIds.ErrorType);
            AssertExprTypeByVar("SmallInt", "a shr 33", KnownTypeIds.ErrorType);
            AssertExprTypeByVar("Integer", "a shr 33", KnownTypeIds.ErrorType);
            AssertExprTypeByVar("Int64", "a shr 33", KnownTypeIds.Int64Type);
            AssertExprTypeByVar("Byte", "a shr 32", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("Word", "a shr 32", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("Cardinal", "a shr 32", KnownTypeIds.CardinalType);
            AssertExprTypeByVar("UInt64", "a shr 32", KnownTypeIds.Uint64Type);
            AssertExprTypeByVar("ShortInt", "a shr 32", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("SmallInt", "a shr 32", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("Integer", "a shr 32", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("Int64", "a shr 32", KnownTypeIds.Int64Type);
        }

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

        [TestMethod]
        public void TestRelationalOperatorsIntIndirect() {
            var d = new[] {
                Tuple.Create("Byte", KnownTypeIds.ByteType),
                Tuple.Create("Word", KnownTypeIds.WordType),
                Tuple.Create("Cardinal", KnownTypeIds.CardinalType),
                Tuple.Create("UInt64", KnownTypeIds.Uint64Type),
                Tuple.Create("SmallInt", KnownTypeIds.SmallInt),
                Tuple.Create("ShortInt", KnownTypeIds.ShortInt),
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

        [TestMethod]
        public void TestConcatOperatorIndirect() {
            AssertExprTypeByVar("UnicodeString", "a + b", KnownTypeIds.UnicodeStringType);
            AssertExprTypeByVar("AnsiString", "a + b", KnownTypeIds.AnsiStringType);
        }

        [TestMethod]
        public void TestConstantArrayTypes() {
            AssertExprTypeByConst("(1,2)", KnownTypeIds.UnspecifiedType, false, "array [1001] of int8");
            AssertExprTypeByConst("('a','b')", KnownTypeIds.UnspecifiedType, false, "array [1001] of WideChar");
            AssertExprTypeByConst("('aa','b')", KnownTypeIds.UnspecifiedType, false, "array [1001] of UnicodeString");
            AssertExprTypeByConst("(1.0, 1.4)", KnownTypeIds.UnspecifiedType, false, "array [1001] of extended");
            AssertExprTypeByConst("(a, b)", KnownTypeIds.UnspecifiedType, false, "array [1002] of enum Int8", "type ta = (a,b);");
            AssertExprValue("a", GetArrayValue(KnownTypeIds.StringType, GetUnicodeStringValue("11"), GetUnicodeStringValue("2")), "const a: array [0 .. 1] of String = ('11', '2');");
        }

        [TestMethod]
        public void TestSetAddOperator() {
            var i1 = GetIntegerValue(1);
            var i2 = GetIntegerValue(2);
            var c1 = GetWideCharValue('1');
            var c2 = GetWideCharValue('2');
            var b1 = GetBooleanValue(true);
            var b2 = GetBooleanValue(false);

            AssertExprValue("a + b", GetSetValue(RegisteredTypes.SmallestUserTypeId, i1, i2), "type Ta = set of integer; const a: Ta = [1]; b: Ta = [2];");
            AssertExprValue("a + b", GetSetValue(RegisteredTypes.SmallestUserTypeId, i1, i2), "type Ta = set of integer; const a: Ta = [1,2]; b: Ta = [2];");
            AssertExprValue("a + b", GetSetValue(RegisteredTypes.SmallestUserTypeId, i1, i2), "type Ta = set of integer; const a: Ta = [1]; b: Ta = [1,2];");
            AssertExprValue("a + b", GetSetValue(RegisteredTypes.SmallestUserTypeId, i1, i2), "type Ta = set of integer; const a: Ta = [1,2]; b: Ta = [1,2];");

            AssertExprValue("a + b", GetSetValue(RegisteredTypes.SmallestUserTypeId, c1, c2), "type Ta = set of char; const a: Ta = ['1']; b: Ta = ['2'];");
            AssertExprValue("a + b", GetSetValue(RegisteredTypes.SmallestUserTypeId, c1, c2), "type Ta = set of char; const a: Ta = ['1','2']; b: Ta = ['2'];");
            AssertExprValue("a + b", GetSetValue(RegisteredTypes.SmallestUserTypeId, c1, c2), "type Ta = set of char; const a: Ta = ['1']; b: Ta = ['1','2'];");
            AssertExprValue("a + b", GetSetValue(RegisteredTypes.SmallestUserTypeId, c1, c2), "type Ta = set of char; const a: Ta = ['1','2']; b: Ta = ['1','2'];");

            AssertExprValue("a + b", GetSetValue(RegisteredTypes.SmallestUserTypeId, b1, b2), "type Ta = set of boolean; const a: Ta = [true]; b: Ta = [false];");
            AssertExprValue("a + b", GetSetValue(RegisteredTypes.SmallestUserTypeId, b1, b2), "type Ta = set of boolean; const a: Ta = [true,false]; b: Ta = [false];");
            AssertExprValue("a + b", GetSetValue(RegisteredTypes.SmallestUserTypeId, b1, b2), "type Ta = set of boolean; const a: Ta = [true]; b: Ta = [true,false];");
            AssertExprValue("a + b", GetSetValue(RegisteredTypes.SmallestUserTypeId, b1, b2), "type Ta = set of boolean; const a: Ta = [true,false]; b: Ta = [true,false];");

        }

        [TestMethod]
        public void TestSetDifferenceOperator() {
            var i1 = GetIntegerValue(1);
            var i2 = GetIntegerValue(2);
            var c1 = GetWideCharValue('1');
            var c2 = GetWideCharValue('2');
            var b1 = GetBooleanValue(true);
            var b2 = GetBooleanValue(false);

            AssertExprValue("a - b", GetSetValue(RegisteredTypes.SmallestUserTypeId, i1), "type Ta = set of integer; const a: Ta = [1,2]; b: Ta = [2];");
            AssertExprValue("a - b", GetSetValue(RegisteredTypes.SmallestUserTypeId, i1), "type Ta = set of integer; const a: Ta = [1]; b: Ta = [2];");
            AssertExprValue("a - b", GetSetValue(RegisteredTypes.SmallestUserTypeId), "type Ta = set of integer; const a: Ta = [2]; b: Ta = [2];");
            AssertExprValue("a - b", GetSetValue(RegisteredTypes.SmallestUserTypeId, i2), "type Ta = set of integer; const a: Ta = [2]; b: Ta = [1];");

            AssertExprValue("a - b", GetSetValue(RegisteredTypes.SmallestUserTypeId, c1), "type Ta = set of char; const a: Ta = ['1','2']; b: Ta = ['2'];");
            AssertExprValue("a - b", GetSetValue(RegisteredTypes.SmallestUserTypeId, c1), "type Ta = set of char; const a: Ta = ['1']; b: Ta = ['2'];");
            AssertExprValue("a - b", GetSetValue(RegisteredTypes.SmallestUserTypeId), "type Ta = set of char; const a: Ta = ['2']; b: Ta = ['2'];");
            AssertExprValue("a - b", GetSetValue(RegisteredTypes.SmallestUserTypeId, c2), "type Ta = set of char; const a: Ta = ['2']; b: Ta = ['1'];");

            AssertExprValue("a - b", GetSetValue(RegisteredTypes.SmallestUserTypeId, b1), "type Ta = set of Boolean; const a: Ta = [true,false]; b: Ta = [false];");
            AssertExprValue("a - b", GetSetValue(RegisteredTypes.SmallestUserTypeId, b1), "type Ta = set of Boolean; const a: Ta = [true]; b: Ta = [false];");
            AssertExprValue("a - b", GetSetValue(RegisteredTypes.SmallestUserTypeId), "type Ta = set of Boolean; const a: Ta = [true]; b: Ta = [true];");
            AssertExprValue("a - b", GetSetValue(RegisteredTypes.SmallestUserTypeId, b2), "type Ta = set of Boolean; const a: Ta = [false]; b: Ta = [true];");
        }

        [TestMethod]
        public void TestSetIntersectOperator() {
            var i1 = GetIntegerValue(1);
            var i2 = GetIntegerValue(2);
            var c1 = GetWideCharValue('1');
            var c2 = GetWideCharValue('2');
            var b1 = GetBooleanValue(true);
            var b2 = GetBooleanValue(false);

            AssertExprValue("a * b", GetSetValue(RegisteredTypes.SmallestUserTypeId, i2), "type Ta = set of integer; const a: Ta = [1,2]; b: Ta = [2];");
            AssertExprValue("a * b", GetSetValue(RegisteredTypes.SmallestUserTypeId), "type Ta = set of integer; const a: Ta = [1]; b: Ta = [2];");
            AssertExprValue("a * b", GetSetValue(RegisteredTypes.SmallestUserTypeId, i1), "type Ta = set of integer; const a: Ta = [1]; b: Ta = [1..5];");
            AssertExprValue("a * b", GetSetValue(RegisteredTypes.SmallestUserTypeId, i1, i2), "type Ta = set of integer; const a: Ta = [1,2]; b: Ta = [2,1];");

            AssertExprValue("a * b", GetSetValue(RegisteredTypes.SmallestUserTypeId, c2), "type Ta = set of char; const a: Ta = ['1','2']; b: Ta = ['2'];");
            AssertExprValue("a * b", GetSetValue(RegisteredTypes.SmallestUserTypeId), "type Ta = set of char; const a: Ta = ['1']; b: Ta = ['2'];");
            AssertExprValue("a * b", GetSetValue(RegisteredTypes.SmallestUserTypeId, c2), "type Ta = set of char; const a: Ta = ['2']; b: Ta = ['1'..'5'];");
            AssertExprValue("a * b", GetSetValue(RegisteredTypes.SmallestUserTypeId, c1, c2), "type Ta = set of char; const a: Ta = ['2','1']; b: Ta = ['1','2'];");

            AssertExprValue("a * b", GetSetValue(RegisteredTypes.SmallestUserTypeId, b2), "type Ta = set of Boolean; const a: Ta = [true,false]; b: Ta = [false];");
            AssertExprValue("a * b", GetSetValue(RegisteredTypes.SmallestUserTypeId), "type Ta = set of Boolean; const a: Ta = [true]; b: Ta = [false];");
            AssertExprValue("a * b", GetSetValue(RegisteredTypes.SmallestUserTypeId, b2), "type Ta = set of Boolean; const a: Ta = [false]; b: Ta = [true,false];");
            AssertExprValue("a * b", GetSetValue(RegisteredTypes.SmallestUserTypeId, b1, b2), "type Ta = set of Boolean; const a: Ta = [false,true]; b: Ta = [true,false];");
        }

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


        [TestMethod]
        public void TestSetOperatorsIndirect() {
            AssertExprValue("a + b", GetUnkownValue(RegisteredTypes.SmallestUserTypeId, CommonTypeKind.SetType), "type Ta = set of integer; var a, b: Ta;", isConstant: false);
            AssertExprValue("a + b", GetUnkownValue(RegisteredTypes.SmallestUserTypeId, CommonTypeKind.SetType), "type Ta = set of char; var a, b: Ta;", isConstant: false);
            AssertExprValue("a + b", GetUnkownValue(RegisteredTypes.SmallestUserTypeId, CommonTypeKind.SetType), "type Ta = set of boolean; var a, b: Ta;", isConstant: false);

            AssertExprValue("a - b", GetUnkownValue(RegisteredTypes.SmallestUserTypeId, CommonTypeKind.SetType), "type Ta = set of integer; var a, b: Ta;", isConstant: false);
            AssertExprValue("a - b", GetUnkownValue(RegisteredTypes.SmallestUserTypeId, CommonTypeKind.SetType), "type Ta = set of char; var a, b: Ta;", isConstant: false);
            AssertExprValue("a - b", GetUnkownValue(RegisteredTypes.SmallestUserTypeId, CommonTypeKind.SetType), "type Ta = set of boolean; var a, b: Ta;", isConstant: false);

            AssertExprValue("a * b", GetUnkownValue(RegisteredTypes.SmallestUserTypeId, CommonTypeKind.SetType), "type Ta = set of integer; var a, b: Ta;", isConstant: false);
            AssertExprValue("a * b", GetUnkownValue(RegisteredTypes.SmallestUserTypeId, CommonTypeKind.SetType), "type Ta = set of char; var a, b: Ta;", isConstant: false);
            AssertExprValue("a * b", GetUnkownValue(RegisteredTypes.SmallestUserTypeId, CommonTypeKind.SetType), "type Ta = set of boolean; var a, b: Ta;", isConstant: false);

            AssertExprValue("a = b", GetUnkownValue(KnownTypeIds.BooleanType, CommonTypeKind.BooleanType), "type Ta = set of integer; var a, b: Ta;", isConstant: false);
            AssertExprValue("a = b", GetUnkownValue(KnownTypeIds.BooleanType, CommonTypeKind.BooleanType), "type Ta = set of char; var a, b: Ta;", isConstant: false);
            AssertExprValue("a = b", GetUnkownValue(KnownTypeIds.BooleanType, CommonTypeKind.BooleanType), "type Ta = set of boolean; var a, b: Ta;", isConstant: false);

            AssertExprValue("a >= b", GetUnkownValue(KnownTypeIds.BooleanType, CommonTypeKind.BooleanType), "type Ta = set of integer; var a, b: Ta;", isConstant: false);
            AssertExprValue("a >= b", GetUnkownValue(KnownTypeIds.BooleanType, CommonTypeKind.BooleanType), "type Ta = set of char; var a, b: Ta;", isConstant: false);
            AssertExprValue("a >= b", GetUnkownValue(KnownTypeIds.BooleanType, CommonTypeKind.BooleanType), "type Ta = set of boolean; var a, b: Ta;", isConstant: false);

            AssertExprValue("a <= b", GetUnkownValue(KnownTypeIds.BooleanType, CommonTypeKind.BooleanType), "type Ta = set of integer; var a, b: Ta;", isConstant: false);
            AssertExprValue("a <= b", GetUnkownValue(KnownTypeIds.BooleanType, CommonTypeKind.BooleanType), "type Ta = set of char; var a, b: Ta;", isConstant: false);
            AssertExprValue("a <= b", GetUnkownValue(KnownTypeIds.BooleanType, CommonTypeKind.BooleanType), "type Ta = set of boolean; var a, b: Ta;", isConstant: false);

            AssertExprValue("a <> b", GetUnkownValue(KnownTypeIds.BooleanType, CommonTypeKind.BooleanType), "type Ta = set of integer; var a, b: Ta;", isConstant: false);
            AssertExprValue("a <> b", GetUnkownValue(KnownTypeIds.BooleanType, CommonTypeKind.BooleanType), "type Ta = set of char; var a, b: Ta;", isConstant: false);
            AssertExprValue("a <> b", GetUnkownValue(KnownTypeIds.BooleanType, CommonTypeKind.BooleanType), "type Ta = set of boolean; var a, b: Ta;", isConstant: false);
        }


    }
}