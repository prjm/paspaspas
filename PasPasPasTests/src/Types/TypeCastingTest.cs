using PasPasPas.Globals.Types;
using PasPasPas.Runtime.Values;
using PasPasPas.Runtime.Values.IntValues;
using PasPasPas.Typings.Common;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     tests for type casting
    /// </summary>
    public class TypeCastingTest : TypeTest {

        [TestMethod]
        public void TestIntegerCastingDirect() {
            AssertExprValue("ShortInt(384)", new ShortIntValue(-128), "", KnownTypeIds.ShortInt);
            AssertExprValue("Byte(384)", new ByteValue(128), "", KnownTypeIds.ByteType);
            AssertExprValue("SmallInt(384)", new SmallIntValue(384), "", KnownTypeIds.SmallInt);
            AssertExprValue("Word(384)", new WordValue(384), "", KnownTypeIds.WordType);
            AssertExprValue("Integer(384)", new IntegerValue(384), "", KnownTypeIds.IntegerType);
            AssertExprValue("Cardinal(384)", new CardinalValue(384), "", KnownTypeIds.CardinalType);
            AssertExprValue("Int64(384)", new Int64Value(384), "", KnownTypeIds.Int64Type);
            AssertExprValue("UInt64(384)", new UInt64Value(384), "", KnownTypeIds.Uint64Type);

            AssertExprValue("WideChar(384)", GetWideCharValue((char)384), "", KnownTypeIds.WideCharType);
            AssertExprValue("Char(384)", GetWideCharValue((char)384), "", KnownTypeIds.CharType);
            AssertExprValue("AnsiChar(384)", GetAnsiCharValue(unchecked((byte)384)), "", KnownTypeIds.AnsiCharType);

            AssertExprValue("Boolean(384)", GetBooleanValue(true), "", KnownTypeIds.BooleanType);
            AssertExprValue("ByteBool(384)", GetByteBooleanValue(unchecked((byte)384)), "", KnownTypeIds.ByteBoolType);
            AssertExprValue("WordBool(384)", GetWordBooleanValue(unchecked(384)), "", KnownTypeIds.WordBoolType);

            AssertExprValue("e(384)",
                new EnumeratedValue(1 + RegisteredTypes.SmallestUserTypeId, new ShortIntValue(unchecked((sbyte)384))),
                "type e = (e1, e2);", 1 + RegisteredTypes.SmallestUserTypeId);

            AssertExprValue("e(257)",
                GetSubrangeValue(RegisteredTypes.SmallestUserTypeId + 1, new ShortIntValue(1)), "type e = -2..2;", 1001);
        }

        [TestMethod]
        public void TestIntegerCastingIndirect() {
            AssertExprTypeByVar("Integer", "ShortInt(a)", KnownTypeIds.ShortInt);
            AssertExprTypeByVar("Integer", "Byte(a)", KnownTypeIds.ByteType);
            AssertExprTypeByVar("Integer", "SmallInt(a)", KnownTypeIds.SmallInt);
            AssertExprTypeByVar("Integer", "Word(a)", KnownTypeIds.WordType);
            AssertExprTypeByVar("Byte", "Integer(a)", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("Byte", "Cardinal(a)", KnownTypeIds.CardinalType);
            AssertExprTypeByVar("Byte", "Int64(a)", KnownTypeIds.Int64Type);
            AssertExprTypeByVar("Byte", "UInt64(a)", KnownTypeIds.Uint64Type);

            AssertExprTypeByVar("Byte", "WideChar(a)", KnownTypeIds.WideCharType);
            AssertExprTypeByVar("Byte", "Char(a)", KnownTypeIds.CharType);
            AssertExprTypeByVar("Byte", "AnsiChar(a)", KnownTypeIds.AnsiCharType);

            AssertExprTypeByVar("Integer", "Boolean(a)", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("Integer", "ByteBool(a)", KnownTypeIds.ByteBoolType);
            AssertExprTypeByVar("Integer", "WordBool(a)", KnownTypeIds.WordBoolType);

            AssertExprTypeByVar("Integer", "e(a)", 1 + RegisteredTypes.SmallestUserTypeId, false, "type e = (e1, e2);");
            AssertExprTypeByVar("Integer", "e(a)", 1 + RegisteredTypes.SmallestUserTypeId, false, "type e = -2..2;");
        }

        [TestMethod]
        public void TestStringCastingDirect() {
            AssertExprValue("ShortString('a')", GetShortStringValue("a"), "", KnownTypeIds.ShortStringType);
            AssertExprValue("AnsiString('a')", GetAnsiStringValue("a"), "", KnownTypeIds.AnsiStringType);
        }

        [TestMethod]
        public void TestCharCastingDirect() {
            AssertExprValue("ShortInt('a')", new ShortIntValue(97), "", KnownTypeIds.ShortInt);
            AssertExprValue("Byte('a')", new ByteValue(97), "", KnownTypeIds.ByteType);
            AssertExprValue("SmallInt('a')", new SmallIntValue(97), "", KnownTypeIds.SmallInt);
            AssertExprValue("Word('a')", new WordValue(97), "", KnownTypeIds.WordType);
            AssertExprValue("Integer('a')", new IntegerValue(97), "", KnownTypeIds.IntegerType);
            AssertExprValue("Cardinal('a')", new CardinalValue(97), "", KnownTypeIds.CardinalType);
            AssertExprValue("Int64('a')", new Int64Value(97), "", KnownTypeIds.Int64Type);
            AssertExprValue("UInt64('a')", new UInt64Value(97), "", KnownTypeIds.Uint64Type);

            AssertExprValue("WideChar('a')", GetWideCharValue('a'), "", KnownTypeIds.WideCharType);
            AssertExprValue("Char('a')", GetWideCharValue('a'), "", KnownTypeIds.CharType);
            AssertExprValue("AnsiChar('a')", GetAnsiCharValue(unchecked((byte)'a')), "", KnownTypeIds.AnsiCharType);

            AssertExprValue("Boolean('a')", GetBooleanValue(true), "", KnownTypeIds.BooleanType);
            AssertExprValue("ByteBool('a')", GetByteBooleanValue(unchecked((byte)384)), "", KnownTypeIds.ByteBoolType);
            AssertExprValue("WordBool('a')", GetWordBooleanValue(unchecked(384)), "", KnownTypeIds.WordBoolType);

            AssertExprValue("e('a')",
                new EnumeratedValue(1 + RegisteredTypes.SmallestUserTypeId, new ShortIntValue(unchecked(97))),
                "type e = (e1, e2);", 1 + RegisteredTypes.SmallestUserTypeId);

            AssertExprValue("e('a')",
                GetSubrangeValue(1 + RegisteredTypes.SmallestUserTypeId, new ShortIntValue(97)), "type e = -2..2;", 1001);


        }

        [TestMethod]
        public void TestBooleanCastingDirect() {
            AssertExprValue(" Boolean(true)", GetBooleanValue(true), "", KnownTypeIds.BooleanType);
            AssertExprValue("ByteBool(true)", GetByteBooleanValue(0xff), "", KnownTypeIds.ByteBoolType);
            AssertExprValue("WordBool(true)", GetWordBooleanValue(0xffff), "", KnownTypeIds.WordBoolType);
            AssertExprValue("LongBool(true)", GetLongBooleanValue(0xffff_ffff), "", KnownTypeIds.LongBoolType);
            AssertExprValue(" Boolean(false)", GetBooleanValue(false), "", KnownTypeIds.BooleanType);
            AssertExprValue("ByteBool(false)", GetByteBooleanValue(0x0), "", KnownTypeIds.ByteBoolType);
            AssertExprValue("WordBool(false)", GetWordBooleanValue(0x0), "", KnownTypeIds.WordBoolType);
            AssertExprValue("LongBool(false)", GetLongBooleanValue(0x0), "", KnownTypeIds.LongBoolType);
        }

        [TestMethod]
        public void TestBooleanCastingIndirect() {
            AssertExprTypeByVar("Boolean", "Boolean(a)", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("Boolean", "ByteBool(a)", KnownTypeIds.ByteBoolType);
            AssertExprTypeByVar("Boolean", "WordBool(a)", KnownTypeIds.WordBoolType);
            AssertExprTypeByVar("Boolean", "LongBool(a)", KnownTypeIds.LongBoolType);
        }

        [TestMethod]
        public void TestCharCastingIndirectDirect() {
            AssertExprTypeByVar("WideChar", "ShortInt(a)", KnownTypeIds.ShortInt);
            AssertExprTypeByVar("WideChar", "Byte(a)", KnownTypeIds.ByteType);
            AssertExprTypeByVar("WideChar", "SmallInt(a)", KnownTypeIds.SmallInt);
            AssertExprTypeByVar("WideChar", "Word(a)", KnownTypeIds.WordType);
            AssertExprTypeByVar("WideChar", "Integer(a)", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("WideChar", "Cardinal(a)", KnownTypeIds.CardinalType);
            AssertExprTypeByVar("WideChar", "Int64(a)", KnownTypeIds.Int64Type);
            AssertExprTypeByVar("WideChar", "UInt64(a)", KnownTypeIds.Uint64Type);

            AssertExprTypeByVar("WideChar", "WideChar(a)", KnownTypeIds.WideCharType);
            AssertExprTypeByVar("WideChar", "Char(a)", KnownTypeIds.CharType);
            AssertExprTypeByVar("WideChar", "AnsiChar(a)", KnownTypeIds.AnsiCharType);

            AssertExprTypeByVar("WideChar", "WideChar(a)", KnownTypeIds.WideCharType);
            AssertExprTypeByVar("WideChar", "Char(a)", KnownTypeIds.CharType);
            AssertExprTypeByVar("WideChar", "AnsiChar(a)", KnownTypeIds.AnsiCharType);

            AssertExprTypeByVar("WideChar", "Boolean(a)", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("WideChar", "ByteBool(a)", KnownTypeIds.ByteBoolType);
            AssertExprTypeByVar("WideChar", "WordBool(a)", KnownTypeIds.WordBoolType);

            AssertExprTypeByVar("WideChar", "e(a)", 1 + RegisteredTypes.SmallestUserTypeId, false, "type e = (e1, e2);");
            AssertExprTypeByVar("WideChar", "e(a)", 1 + RegisteredTypes.SmallestUserTypeId, false, "type e = -2..2;");

        }

        [TestMethod]
        public void TestArrayCasting() {
            AssertExprValue("a", GetArrayValue(RegisteredTypes.SmallestUserTypeId + 1, KnownTypeIds.StringType, GetUnicodeStringValue("a"), GetUnicodeStringValue("a")), "const a: array[0..1] of string = ('a', 'a');");
            AssertExprValue("a", GetArrayValue(RegisteredTypes.SmallestUserTypeId + 1, KnownTypeIds.AnsiCharType, GetAnsiCharValue((byte)'a'), GetAnsiCharValue((byte)'a')), "const a: array[0..1] of ansichar = 'aa';");
        }

    }
}
