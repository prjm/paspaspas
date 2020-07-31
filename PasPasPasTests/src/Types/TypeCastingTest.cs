using System;
using System.Collections.Immutable;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     tests for type casting
    /// </summary>
    public class TypeCastingTest : TypeTest {

        private ISystemUnit SystemUnit
            => CreateEnvironment().TypeRegistry.SystemUnit;

        /// <summary>
        ///     test direct type casting
        /// </summary>
        [TestMethod]
        public void TestIntegerCastingDirect() {
            AssertExprValue("ShortInt(384)", GetIntegerValue((sbyte)-128), "", SystemUnit.ShortIntType);
            AssertExprValue("Byte(384)", GetIntegerValue((byte)128), "", SystemUnit.ByteType);
            AssertExprValue("SmallInt(384)", GetIntegerValue((short)384), "", SystemUnit.SmallIntType);
            AssertExprValue("Word(384)", GetIntegerValue((ushort)384), "", SystemUnit.WordType);
            AssertExprValue("Integer(384)", GetIntegerValue(384), "", SystemUnit.IntegerType);
            AssertExprValue("Cardinal(384)", GetIntegerValue((uint)384), "", SystemUnit.CardinalType);
            AssertExprValue("Int64(384)", GetIntegerValue((long)384), "", SystemUnit.Int64Type);
            AssertExprValue("UInt64(384)", GetIntegerValue((ulong)384), "", SystemUnit.UInt64Type);

            AssertExprValue("WideChar(384)", GetWideCharValue((char)384), "", SystemUnit.WideCharType);
            AssertExprValue("Char(384)", GetWideCharValue((char)384), "", SystemUnit.CharType);
            AssertExprValue("AnsiChar(384)", GetAnsiCharValue(unchecked((byte)384)), "", SystemUnit.AnsiCharType);

            AssertExprValue("Boolean(384)", GetBooleanValue(true), "", SystemUnit.BooleanType);
            AssertExprValue("ByteBool(384)", GetByteBooleanValue(unchecked((byte)384)), "", SystemUnit.ByteBoolType);
            AssertExprValue("WordBool(384)", GetWordBooleanValue(unchecked(384)), "", SystemUnit.WordBoolType);

            var e = CreateEnvironment();
            var tc = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var et = tc.CreateEnumType("e");
            var st = tc.CreateSubrangeType(e.TypeRegistry.SystemUnit.IntegerType, GetIntegerValue((sbyte)-2), GetIntegerValue((sbyte)2)); ;
            var v1 = e.Runtime.MakeEnumValue(et, GetIntegerValue(unchecked((sbyte)384)) as IIntegerValue ?? throw new InvalidOperationException(), string.Empty);
            var v2 = e.Runtime.Types.MakeSubrangeValue(st, GetIntegerValue(1));

            AssertExprValue("e(384)", v1, "type e = (e1, e2);", et);

            AssertExprValue("e(257)", v2, "type e = -2..2;", st);
        }

        /// <summary>
        ///     test integer casting
        /// </summary>
        [TestMethod]
        public void TestIntegerCastingIndirect() {
            AssertExprTypeByVar("Integer", "ShortInt(a)", SystemUnit.ShortIntType);
            AssertExprTypeByVar("Integer", "Byte(a)", SystemUnit.ByteType);
            AssertExprTypeByVar("Integer", "SmallInt(a)", SystemUnit.SmallIntType);
            AssertExprTypeByVar("Integer", "Word(a)", SystemUnit.WordType);
            AssertExprTypeByVar("Byte", "Integer(a)", SystemUnit.IntegerType);
            AssertExprTypeByVar("Byte", "Cardinal(a)", SystemUnit.CardinalType);
            AssertExprTypeByVar("Byte", "Int64(a)", SystemUnit.Int64Type);
            AssertExprTypeByVar("Byte", "UInt64(a)", SystemUnit.UInt64Type);

            AssertExprTypeByVar("Byte", "WideChar(a)", SystemUnit.WideCharType);
            AssertExprTypeByVar("Byte", "Char(a)", SystemUnit.CharType);
            AssertExprTypeByVar("Byte", "AnsiChar(a)", SystemUnit.AnsiCharType);

            AssertExprTypeByVar("Integer", "Boolean(a)", SystemUnit.BooleanType);
            AssertExprTypeByVar("Integer", "ByteBool(a)", SystemUnit.ByteBoolType);
            AssertExprTypeByVar("Integer", "WordBool(a)", SystemUnit.WordBoolType);

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
            AssertExprValue("ShortString('a')", GetShortStringValue("a"), "", SystemUnit.ShortStringType);
            AssertExprValue("AnsiString('a')", GetAnsiStringValue("a"), "", SystemUnit.AnsiStringType);
        }

        /// <summary>
        ///     test direct char casting
        /// </summary>
        [TestMethod]
        public void TestCharCastingDirect() {
            AssertExprValue("ShortInt('a')", GetIntegerValue((sbyte)97), "", SystemUnit.ShortIntType);
            AssertExprValue("Byte('a')", GetIntegerValue((byte)97), "", SystemUnit.ByteType);
            AssertExprValue("SmallInt('a')", GetIntegerValue((short)97), "", SystemUnit.SmallIntType);
            AssertExprValue("Word('a')", GetIntegerValue((ushort)97), "", SystemUnit.WordType);
            AssertExprValue("Integer('a')", GetIntegerValue(97), "", SystemUnit.IntegerType);
            AssertExprValue("Cardinal('a')", GetIntegerValue((uint)97), "", SystemUnit.CardinalType);
            AssertExprValue("Int64('a')", GetIntegerValue((long)97), "", SystemUnit.Int64Type);
            AssertExprValue("UInt64('a')", GetIntegerValue((ulong)97), "", SystemUnit.UInt64Type);

            AssertExprValue("WideChar('a')", GetWideCharValue('a'), "", SystemUnit.WideCharType);
            AssertExprValue("Char('a')", GetWideCharValue('a'), "", SystemUnit.CharType);
            AssertExprValue("AnsiChar('a')", GetAnsiCharValue(unchecked((byte)'a')), "", SystemUnit.AnsiCharType);

            AssertExprValue("Boolean('a')", GetBooleanValue(true), "", SystemUnit.BooleanType);
            AssertExprValue("ByteBool('a')", GetByteBooleanValue(unchecked((byte)384)), "", SystemUnit.ByteBoolType);
            AssertExprValue("WordBool('a')", GetWordBooleanValue(unchecked(384)), "", SystemUnit.WordBoolType);

            var e = CreateEnvironment();
            var ct = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var t1 = ct.CreateEnumType("e");
            var t2 = ct.CreateSubrangeType(SystemUnit.IntegerType, GetIntegerValue((sbyte)-2), GetIntegerValue((sbyte)2));

            var ev = e.Runtime.MakeEnumValue(t1, GetIntegerValue((sbyte)1) as IIntegerValue ?? throw new InvalidOperationException(), string.Empty);
            var sr = e.Runtime.Types.MakeSubrangeValue(t2, GetIntegerValue((sbyte)97) as IIntegerValue ?? throw new InvalidOperationException());

            AssertExprValue("e('a')", ev, "type e = (e1, e2);", t1);
            AssertExprValue("e('a')", sr, "type e = -2..2;", t2);


        }

        /// <summary>
        ///     test direct boolean casting
        /// </summary>
        [TestMethod]
        public void TestBooleanCastingDirect() {
            AssertExprValue(" Boolean(true)", GetBooleanValue(true), "", SystemUnit.BooleanType);
            AssertExprValue("ByteBool(true)", GetByteBooleanValue(0xff), "", SystemUnit.ByteBoolType);
            AssertExprValue("WordBool(true)", GetWordBooleanValue(0xffff), "", SystemUnit.WordBoolType);
            AssertExprValue("LongBool(true)", GetLongBooleanValue(0xffff_ffff), "", SystemUnit.LongBoolType);
            AssertExprValue(" Boolean(false)", GetBooleanValue(false), "", SystemUnit.BooleanType);
            AssertExprValue("ByteBool(false)", GetByteBooleanValue(0x0), "", SystemUnit.ByteBoolType);
            AssertExprValue("WordBool(false)", GetWordBooleanValue(0x0), "", SystemUnit.WordBoolType);
            AssertExprValue("LongBool(false)", GetLongBooleanValue(0x0), "", SystemUnit.LongBoolType);
        }

        /// <summary>
        ///     test boolean casting
        /// </summary>
        [TestMethod]
        public void TestBooleanCastingIndirect() {
            AssertExprTypeByVar("Boolean", "Boolean(a)", SystemUnit.BooleanType);
            AssertExprTypeByVar("Boolean", "ByteBool(a)", SystemUnit.ByteBoolType);
            AssertExprTypeByVar("Boolean", "WordBool(a)", SystemUnit.WordBoolType);
            AssertExprTypeByVar("Boolean", "LongBool(a)", SystemUnit.LongBoolType);
        }

        /// <summary>
        ///     test char casting
        /// </summary>
        [TestMethod]
        public void TestCharCastingIndirectDirect() {
            AssertExprTypeByVar("WideChar", "ShortInt(a)", SystemUnit.ShortIntType);
            AssertExprTypeByVar("WideChar", "Byte(a)", SystemUnit.ByteType);
            AssertExprTypeByVar("WideChar", "SmallInt(a)", SystemUnit.SmallIntType);
            AssertExprTypeByVar("WideChar", "Word(a)", SystemUnit.WordType);
            AssertExprTypeByVar("WideChar", "Integer(a)", SystemUnit.IntegerType);
            AssertExprTypeByVar("WideChar", "Cardinal(a)", SystemUnit.CardinalType);
            AssertExprTypeByVar("WideChar", "Int64(a)", SystemUnit.Int64Type);
            AssertExprTypeByVar("WideChar", "UInt64(a)", SystemUnit.UInt64Type);

            AssertExprTypeByVar("WideChar", "WideChar(a)", SystemUnit.WideCharType);
            AssertExprTypeByVar("WideChar", "Char(a)", SystemUnit.CharType);
            AssertExprTypeByVar("WideChar", "AnsiChar(a)", SystemUnit.AnsiCharType);

            AssertExprTypeByVar("WideChar", "WideChar(a)", SystemUnit.WideCharType);
            AssertExprTypeByVar("WideChar", "Char(a)", SystemUnit.CharType);
            AssertExprTypeByVar("WideChar", "AnsiChar(a)", SystemUnit.AnsiCharType);

            AssertExprTypeByVar("WideChar", "Boolean(a)", SystemUnit.BooleanType);
            AssertExprTypeByVar("WideChar", "ByteBool(a)", SystemUnit.ByteBoolType);
            AssertExprTypeByVar("WideChar", "WordBool(a)", SystemUnit.WordBoolType);

            var e = CreateEnvironment();
            var ct = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var t1 = ct.CreateEnumType("e");
            var t2 = ct.CreateSubrangeType(SystemUnit.IntegerType, GetIntegerValue((sbyte)-2), GetIntegerValue((sbyte)2));

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
            var t1 = ct.CreateStaticArrayType(SystemUnit.StringType, "", SystemUnit.IntegerType, false);
            var t2 = ct.CreateStaticArrayType(SystemUnit.AnsiCharType, "", SystemUnit.IntegerType, false);

            var a1 = e.Runtime.Structured.CreateArrayValue(t1, SystemUnit.StringType, ImmutableArray.Create(GetUnicodeStringValue("a"), GetUnicodeStringValue("a")));
            var a2 = e.Runtime.Structured.CreateArrayValue(t2, SystemUnit.AnsiCharType, ImmutableArray.Create(GetAnsiCharValue((byte)'a'), GetAnsiCharValue((byte)'a')));

            AssertExprValue("a", a1, "const a: array[0..1] of string = ('a', 'a');");
            AssertExprValue("a", a2, "const a: array[0..1] of ansichar = 'aa';");
        }

    }
}
