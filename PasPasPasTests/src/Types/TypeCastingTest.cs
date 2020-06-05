#nullable disable
using System.Collections.Immutable;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     tests for type casting
    /// </summary>
    public class TypeCastingTest : TypeTest {

        private ISystemUnit KnownTypeIds
            => CreateEnvironment().TypeRegistry.SystemUnit;

        /// <summary>
        ///     test direct type casting
        /// </summary>
        [TestMethod]
        public void TestIntegerCastingDirect() {
            AssertExprValue("ShortInt(384)", GetIntegerValue((sbyte)-128), "", KnownTypeIds.ShortIntType);
            AssertExprValue("Byte(384)", GetIntegerValue((byte)128), "", KnownTypeIds.ByteType);
            AssertExprValue("SmallInt(384)", GetIntegerValue((short)384), "", KnownTypeIds.SmallIntType);
            AssertExprValue("Word(384)", GetIntegerValue((ushort)384), "", KnownTypeIds.WordType);
            AssertExprValue("Integer(384)", GetIntegerValue(384), "", KnownTypeIds.IntegerType);
            AssertExprValue("Cardinal(384)", GetIntegerValue((uint)384), "", KnownTypeIds.CardinalType);
            AssertExprValue("Int64(384)", GetIntegerValue((long)384), "", KnownTypeIds.Int64Type);
            AssertExprValue("UInt64(384)", GetIntegerValue((ulong)384), "", KnownTypeIds.UInt64Type);

            AssertExprValue("WideChar(384)", GetWideCharValue((char)384), "", KnownTypeIds.WideCharType);
            AssertExprValue("Char(384)", GetWideCharValue((char)384), "", KnownTypeIds.CharType);
            AssertExprValue("AnsiChar(384)", GetAnsiCharValue(unchecked((byte)384)), "", KnownTypeIds.AnsiCharType);

            AssertExprValue("Boolean(384)", GetBooleanValue(true), "", KnownTypeIds.BooleanType);
            AssertExprValue("ByteBool(384)", GetByteBooleanValue(unchecked((byte)384)), "", KnownTypeIds.ByteBoolType);
            AssertExprValue("WordBool(384)", GetWordBooleanValue(unchecked(384)), "", KnownTypeIds.WordBoolType);

            var e = CreateEnvironment();
            var tc = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var et = tc.CreateEnumType("e");
            var st = tc.CreateSubrangeType("e", e.TypeRegistry.SystemUnit.IntegerType, GetIntegerValue((sbyte)-2), GetIntegerValue((sbyte)2)); ;
            var v1 = e.Runtime.Types.MakeEnumValue(et, GetIntegerValue(unchecked((sbyte)384)) as IIntegerValue);
            var v2 = e.Runtime.Types.MakeSubrangeValue(st, GetIntegerValue(1));

            AssertExprValue("e(384)", v1, "type e = (e1, e2);", et);

            AssertExprValue("e(257)", v2, "type e = -2..2;", st);
        }

        /// <summary>
        ///     test integer casting
        /// </summary>
        [TestMethod]
        public void TestIntegerCastingIndirect() {
            AssertExprTypeByVar("Integer", "ShortInt(a)", KnownTypeIds.ShortIntType);
            AssertExprTypeByVar("Integer", "Byte(a)", KnownTypeIds.ByteType);
            AssertExprTypeByVar("Integer", "SmallInt(a)", KnownTypeIds.SmallIntType);
            AssertExprTypeByVar("Integer", "Word(a)", KnownTypeIds.WordType);
            AssertExprTypeByVar("Byte", "Integer(a)", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("Byte", "Cardinal(a)", KnownTypeIds.CardinalType);
            AssertExprTypeByVar("Byte", "Int64(a)", KnownTypeIds.Int64Type);
            AssertExprTypeByVar("Byte", "UInt64(a)", KnownTypeIds.UInt64Type);

            AssertExprTypeByVar("Byte", "WideChar(a)", KnownTypeIds.WideCharType);
            AssertExprTypeByVar("Byte", "Char(a)", KnownTypeIds.CharType);
            AssertExprTypeByVar("Byte", "AnsiChar(a)", KnownTypeIds.AnsiCharType);

            AssertExprTypeByVar("Integer", "Boolean(a)", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("Integer", "ByteBool(a)", KnownTypeIds.ByteBoolType);
            AssertExprTypeByVar("Integer", "WordBool(a)", KnownTypeIds.WordBoolType);

            var e = CreateEnvironment();
            var ct = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var t1 = ct.CreateEnumType("e");
            var t2 = ct.CreateSetType(e.TypeRegistry.SystemUnit.IntegerType, "e");

            AssertExprTypeByVar("Integer", "e(a)", t1, false, "type e = (e1, e2);");
            AssertExprTypeByVar("Integer", "e(a)", t2, false, "type e = -2..2;");
        }

        /// <summary>
        ///     test direct string casting
        /// </summary>
        [TestMethod]
        public void TestStringCastingDirect() {
            AssertExprValue("ShortString('a')", GetShortStringValue("a"), "", KnownTypeIds.ShortStringType);
            AssertExprValue("AnsiString('a')", GetAnsiStringValue("a"), "", KnownTypeIds.AnsiStringType);
        }

        /// <summary>
        ///     test direct char casting
        /// </summary>
        [TestMethod]
        public void TestCharCastingDirect() {
            AssertExprValue("ShortInt('a')", GetIntegerValue((sbyte)97), "", KnownTypeIds.ShortIntType);
            AssertExprValue("Byte('a')", GetIntegerValue((byte)97), "", KnownTypeIds.ByteType);
            AssertExprValue("SmallInt('a')", GetIntegerValue((short)97), "", KnownTypeIds.SmallIntType);
            AssertExprValue("Word('a')", GetIntegerValue((ushort)97), "", KnownTypeIds.WordType);
            AssertExprValue("Integer('a')", GetIntegerValue(97), "", KnownTypeIds.IntegerType);
            AssertExprValue("Cardinal('a')", GetIntegerValue((uint)97), "", KnownTypeIds.CardinalType);
            AssertExprValue("Int64('a')", GetIntegerValue((long)97), "", KnownTypeIds.Int64Type);
            AssertExprValue("UInt64('a')", GetIntegerValue((ulong)97), "", KnownTypeIds.UInt64Type);

            AssertExprValue("WideChar('a')", GetWideCharValue('a'), "", KnownTypeIds.WideCharType);
            AssertExprValue("Char('a')", GetWideCharValue('a'), "", KnownTypeIds.CharType);
            AssertExprValue("AnsiChar('a')", GetAnsiCharValue(unchecked((byte)'a')), "", KnownTypeIds.AnsiCharType);

            AssertExprValue("Boolean('a')", GetBooleanValue(true), "", KnownTypeIds.BooleanType);
            AssertExprValue("ByteBool('a')", GetByteBooleanValue(unchecked((byte)384)), "", KnownTypeIds.ByteBoolType);
            AssertExprValue("WordBool('a')", GetWordBooleanValue(unchecked(384)), "", KnownTypeIds.WordBoolType);

            var e = CreateEnvironment();
            var ct = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var t1 = ct.CreateEnumType("e");
            var t2 = ct.CreateSubrangeType("e", KnownTypeIds.IntegerType, GetIntegerValue((sbyte)-2), GetIntegerValue((sbyte)2));

            var ev = e.Runtime.MakeEnumValue(t1, GetIntegerValue((sbyte)97) as IIntegerValue);
            var sr = e.Runtime.Types.MakeSubrangeValue(t2, GetIntegerValue((sbyte)97) as IIntegerValue);

            AssertExprValue("e('a')", ev, "type e = (e1, e2);", t1);
            AssertExprValue("e('a')", sr, "type e = -2..2;", t2);


        }

        /// <summary>
        ///     test direct boolean casting
        /// </summary>
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

        /// <summary>
        ///     test boolean casting
        /// </summary>
        [TestMethod]
        public void TestBooleanCastingIndirect() {
            AssertExprTypeByVar("Boolean", "Boolean(a)", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("Boolean", "ByteBool(a)", KnownTypeIds.ByteBoolType);
            AssertExprTypeByVar("Boolean", "WordBool(a)", KnownTypeIds.WordBoolType);
            AssertExprTypeByVar("Boolean", "LongBool(a)", KnownTypeIds.LongBoolType);
        }

        /// <summary>
        ///     test char casting
        /// </summary>
        [TestMethod]
        public void TestCharCastingIndirectDirect() {
            AssertExprTypeByVar("WideChar", "ShortInt(a)", KnownTypeIds.ShortIntType);
            AssertExprTypeByVar("WideChar", "Byte(a)", KnownTypeIds.ByteType);
            AssertExprTypeByVar("WideChar", "SmallInt(a)", KnownTypeIds.SmallIntType);
            AssertExprTypeByVar("WideChar", "Word(a)", KnownTypeIds.WordType);
            AssertExprTypeByVar("WideChar", "Integer(a)", KnownTypeIds.IntegerType);
            AssertExprTypeByVar("WideChar", "Cardinal(a)", KnownTypeIds.CardinalType);
            AssertExprTypeByVar("WideChar", "Int64(a)", KnownTypeIds.Int64Type);
            AssertExprTypeByVar("WideChar", "UInt64(a)", KnownTypeIds.UInt64Type);

            AssertExprTypeByVar("WideChar", "WideChar(a)", KnownTypeIds.WideCharType);
            AssertExprTypeByVar("WideChar", "Char(a)", KnownTypeIds.CharType);
            AssertExprTypeByVar("WideChar", "AnsiChar(a)", KnownTypeIds.AnsiCharType);

            AssertExprTypeByVar("WideChar", "WideChar(a)", KnownTypeIds.WideCharType);
            AssertExprTypeByVar("WideChar", "Char(a)", KnownTypeIds.CharType);
            AssertExprTypeByVar("WideChar", "AnsiChar(a)", KnownTypeIds.AnsiCharType);

            AssertExprTypeByVar("WideChar", "Boolean(a)", KnownTypeIds.BooleanType);
            AssertExprTypeByVar("WideChar", "ByteBool(a)", KnownTypeIds.ByteBoolType);
            AssertExprTypeByVar("WideChar", "WordBool(a)", KnownTypeIds.WordBoolType);

            var e = CreateEnvironment();
            var ct = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var t1 = ct.CreateEnumType("e");
            var t2 = ct.CreateSubrangeType("e", KnownTypeIds.IntegerType, GetIntegerValue((sbyte)-2), GetIntegerValue((sbyte)2));

            AssertExprTypeByVar("WideChar", "e(a)", t1, false, "type e = (e1, e2);");
            AssertExprTypeByVar("WideChar", "e(a)", t2, false, "type e = -2..2;");

        }

        /// <summary>
        ///     test array casting
        /// </summary>
        [TestMethod]
        public void TestArrayCasting() {

            var e = CreateEnvironment();
            var ct = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var t1 = ct.CreateStaticArrayType(KnownTypeIds.StringType, "", KnownTypeIds.IntegerType, false);
            var t2 = ct.CreateStaticArrayType(KnownTypeIds.AnsiCharType, "", KnownTypeIds.IntegerType, false);

            var a1 = e.Runtime.Structured.CreateArrayValue(t1, KnownTypeIds.StringType, ImmutableArray.Create(GetUnicodeStringValue("a"), GetUnicodeStringValue("a")));
            var a2 = e.Runtime.Structured.CreateArrayValue(t2, KnownTypeIds.AnsiCharType, ImmutableArray.Create(GetAnsiCharValue((byte)'a'), GetAnsiCharValue((byte)'a')));

            AssertExprValue("a", a1, "const a: array[0..1] of string = ('a', 'a');");
            AssertExprValue("a", a2, "const a: array[0..1] of ansichar = 'aa';");
        }

    }
}
