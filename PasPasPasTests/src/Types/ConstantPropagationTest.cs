﻿using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Structured;
using PasPasPasTests.Common;
using SharpFloat.FloatingPoint;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test constant propagation
    /// </summary>
    public class ConstantPropagationTest : TypeTest {

        private ISystemUnit SystemUnit
            => CreateEnvironment().TypeRegistry.SystemUnit;

        /// <summary>
        ///     test integer constants
        /// </summary>
        [TestMethod]
        public void TestIntegerConstants() {
            AssertExprValue("0", GetIntegerValue(0));
            AssertExprValue("-128", GetIntegerValue((sbyte)-128));
            AssertExprValue("127", GetIntegerValue((sbyte)127));

            AssertExprValue("128", GetIntegerValue((byte)128));
            AssertExprValue("255", GetIntegerValue((byte)255));

            AssertExprValue("256", GetIntegerValue((short)256));
            AssertExprValue("-129", GetIntegerValue((short)-129));
        }

        /// <summary>
        ///     test integer operations
        /// </summary>
        [TestMethod]
        public void TestIntegerOperations() {
            AssertExprValue("4 + 5", GetIntegerValue(9));
            AssertExprValue("4 - 3", GetIntegerValue(1));
            AssertExprValue("4 - 5", GetIntegerValue(-1));

            AssertExprValue("4 * 5", GetIntegerValue(20));
            AssertExprValue("20 div 4", GetIntegerValue(5));
            AssertExprValue("9 mod 4", GetIntegerValue(1));
        }

        /// <summary>
        ///     test abs function
        /// </summary>
        [TestMethod]
        public void TestAbs() {
            var e = CreateEnvironment();
            var tc = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var et = tc.CreateEnumType("e");
            var st = tc.CreateSubrangeType(e.TypeRegistry.SystemUnit.ShortIntType, GetIntegerValue((sbyte)-4), GetIntegerValue((sbyte)8));
            var st2 = tc.CreateSubrangeType(e.TypeRegistry.SystemUnit.ShortIntType, GetIntegerValue((sbyte)-4), GetIntegerValue((sbyte)4));
            var st3 = tc.CreateSubrangeType(e.TypeRegistry.SystemUnit.ShortIntType, GetIntegerValue((sbyte)1), GetIntegerValue((sbyte)9));

            IValue sv(int x)
                => e.Runtime.Types.MakeSubrangeValue(st, e.Runtime.Integers.ToScaledIntegerValue(x));

            AssertExprValue("Abs(5)", GetIntegerValue(5));
            AssertExprValue("Abs(0)", GetIntegerValue(0));
            AssertExprValue("Abs(-3)", GetIntegerValue(3));
            AssertExprValue("Abs(a)", sv(5), "type Ta=-4..8; const a: Ta = 5;");
            AssertExprValue("Abs(a)", sv(0), "type Ta=-4..8; const a: Ta = 0;");
            AssertExprValue("Abs(a)", sv(3), "type Ta=-4..8; const a: Ta = -3;");
            AssertExprValue("Abs(a)", GetInvocationValue(IntrinsicRoutineId.Abs, SystemUnit.IntegerType, SystemUnit.IntegerType), "var a: integer;", isConstant: false);
            AssertExprValue("Abs(-a)", GetInvocationValue(IntrinsicRoutineId.Abs, st2, st2), "type Ta = -4..4; var a: Ta;", isConstant: false);
            AssertExprValue("Abs(a)", GetInvocationValue(IntrinsicRoutineId.Abs, st2, st2), "type Ta = -4..4; var a: Ta;", isConstant: false);
            AssertExprValue("Abs(-a)", GetInvocationValue(IntrinsicRoutineId.Abs, st2, st2), "type Ta = -4..4; var a: Ta;", isConstant: false);


            AssertExprValue("Abs(5.4)", GetExtendedValue("5.4"));
            AssertExprValue("Abs(0.0)", GetExtendedValue(0));
            AssertExprValue("Abs(-3.3)", GetExtendedValue("3.3"));

            AssertExprValue("Abs(a)", GetSubrangeValue(st3, GetIntegerValue(3)), "type Ta = 1..9; const a: Ta = 3;");
        }


        /// <summary>
        ///     test the <c>chr</c> function
        /// </summary>
        [TestMethod]
        public void TestChr() {
            AssertExprValue("Chr(5)", GetWideCharValue((char)5));
            AssertExprValue("Chr(0)", GetWideCharValue((char)0));
            AssertExprValue("Chr(-3)", GetWideCharValue((char)(ushort.MaxValue - 3 + 1)));

            AssertExprValue("Chr(a)", GetWideCharValue((char)5), "type ta = 2..9; const a: ta = 5; ");
        }

        /// <summary>
        ///     test <c>hi</c> function
        /// </summary>
        [TestMethod]
        public void TestHi() {
            AssertExprValue("Hi($FF)", GetByteValue(0x00));
            AssertExprValue("Hi($FFFF)", GetByteValue(0xff));
            AssertExprValue("Hi($FFFFFF)", GetByteValue(0xff));

            AssertExprValue("Hi(a)", GetByteValue(0xf0), "type ta = 3..$FFFF; const a: ta = $F0FF;");
            AssertExprValue("Hi(a)", GetInvocationValue(IntrinsicRoutineId.Hi, SystemUnit.ByteType, SystemUnit.IntegerType), "type ta = 3..$FFFF; var a: ta;", isConstant: false);
        }

        /// <summary>
        ///     test high function
        /// </summary>
        [TestMethod]
        public void TestHigh() {

            // ordinal types
            AssertExprValue("High(ta)", GetIntegerValue(2), "type ta = 1..2;");
            AssertExprValue("High(ta)", GetIntegerValue(-2), "type ta = -4..-2;");
            AssertExprValue("High(ta)", GetIntegerValue(2), "type ta = (aa, bb, cc);");
            AssertExprValue("High(Boolean)", GetBooleanValue(true));
            AssertExprValue("High(AnsiChar)", GetAnsiCharValue(byte.MaxValue));
            AssertExprValue("High(WideChar)", GetWideCharValue(char.MaxValue));
            AssertExprValue("High(ShortInt)", GetIntegerValue(sbyte.MaxValue));
            AssertExprValue("High(Byte)", GetIntegerValue(byte.MaxValue));
            AssertExprValue("High(SmallInt)", GetIntegerValue(short.MaxValue));
            AssertExprValue("High(Word)", GetIntegerValue(ushort.MaxValue));
            AssertExprValue("High(Integer)", GetIntegerValue(int.MaxValue));
            AssertExprValue("High(Cardinal)", GetIntegerValue(uint.MaxValue));
            AssertExprValue("High(Int64)", GetIntegerValue(long.MaxValue));
            AssertExprValue("High(UInt64)", GetIntegerValue(ulong.MaxValue));

            // short string types
            AssertExprValue("High(string[20])", GetIntegerValue(20));
            AssertExprValue("High(ShortString)", GetIntegerValue(255));

            // constant arrays
            AssertExprValue("High(a)", GetIntegerValue(2), "const a: array[0..2] of string = ('a','b','c');");
            AssertExprValue("High(a)", GetWideCharValue('c'), "const a: array['a'..'c'] of string = ('a','b','c');");

            // array types
            AssertExprValue("High(Ta)", GetIntegerValue(2), "type Ta = array[0..2];");
            AssertExprValue("High(Ta)", GetWideCharValue('c'), "type Ta = array['a'..'c'];");
        }

        /// <summary>
        ///     test <c>length</c>function
        /// </summary>
        [TestMethod]
        public void TestLength() {
            AssertExprValue("Length('')", GetIntegerValue(0));
            AssertExprValue("Length('a')", GetIntegerValue(1));
            AssertExprValue("Length('aaa')", GetIntegerValue(3));

            AssertExprValue("Length(a)", GetIntegerValue(3), "const a = (1,2,3);");
            AssertExprValue("Length(a)", GetIntegerValue(1), "type Ta = 'a'..'c'; const a: Ta = 'a';");
            AssertExprValue("Length(a)", GetInvocationValue(IntrinsicRoutineId.Length, SystemUnit.IntegerType, SystemUnit.IntegerType), "type Ta = 'a'..'c'; var a: Ta;", isConstant: false);
        }

        /// <summary>
        ///     test lo function
        /// </summary>
        [TestMethod]
        public void TestLo() {
            AssertExprValue("Lo($FF)", GetIntegerValue(0xff));
            AssertExprValue("Lo($FF00)", GetByteValue(0x00));
            AssertExprValue("Lo($FFFF0F)", GetByteValue(0x0f));

            AssertExprValue("Lo(a)", GetByteValue(0x0f), "type ta = 3..$FFFF; const a: ta = $F00F;");
        }

        /// <summary>
        ///     test low function
        /// </summary>
        [TestMethod]
        public void TestLow() {

            // ordinal types
            AssertExprValue("Low(ta)", GetIntegerValue(1), "type ta = 1..2;");
            AssertExprValue("Low(ta)", GetIntegerValue(-4), "type ta = -4..-2;");
            AssertExprValue("Low(ta)", GetIntegerValue(0), "type ta = (aa, bb, cc);");
            AssertExprValue("Low(Boolean)", GetBooleanValue(false));
            AssertExprValue("Low(AnsiChar)", GetAnsiCharValue(byte.MinValue));
            AssertExprValue("Low(WideChar)", GetWideCharValue(char.MinValue));
            AssertExprValue("Low(ShortInt)", GetIntegerValue(sbyte.MinValue));
            AssertExprValue("Low(Byte)", GetByteValue(byte.MinValue));
            AssertExprValue("Low(SmallInt)", GetSmallIntValue(short.MinValue));
            AssertExprValue("Low(Word)", GetWordValue(ushort.MinValue));
            AssertExprValue("Low(Integer)", GetIntValue(int.MinValue));
            AssertExprValue("Low(Cardinal)", GetCardinalValue(uint.MinValue));
            AssertExprValue("Low(Int64)", GetInt64Value(long.MinValue));
            AssertExprValue("Low(UInt64)", GetUInt64Value(ulong.MinValue));

            // short string types
            AssertExprValue("Low(string[20])", GetIntegerValue(1));
            AssertExprValue("Low(ShortString)", GetIntegerValue(1));

            // constant arrays
            AssertExprValue("Low(a)", GetIntegerValue(0), "const a: array[0..2] of string = ('a','b','c');");
            AssertExprValue("Low(a)", GetWideCharValue('a'), "const a: array['a'..'c'] of string = ('a','b','c');");

            // array types
            AssertExprValue("Low(Ta)", GetIntegerValue(0), "type Ta = array[0..2];");
            AssertExprValue("Low(Ta)", GetWideCharValue('a'), "type Ta = array['a'..'c'];");
        }

        /// <summary>
        ///     test odd function
        /// </summary>
        [TestMethod]
        public void TestOdd() {
            AssertExprValue("Odd(-3)", GetBooleanValue(true));
            AssertExprValue("Odd(4)", GetBooleanValue(false));
            AssertExprValue("Odd(a)", GetInvocationValue(IntrinsicRoutineId.Odd, SystemUnit.BooleanType, SystemUnit.ShortIntType), "var a: ShortInt;", isConstant: false);
            AssertExprValue("Odd(a)", GetInvocationValue(SystemUnit.BooleanType, SystemUnit.ShortIntType), "type Ta = -3..3; var a: Ta;", isConstant: false);
        }

        /// <summary>
        ///     test <c>ord</c> function
        /// </summary>
        [TestMethod]
        public void TestOrd() {
            AssertExprValue("Ord(-3)", GetIntegerValue(-3));
            AssertExprValue("Ord(3)", GetIntegerValue(3));
            AssertExprValue("Ord(xa)", GetIntegerValue(2), "type Ta = (xc,xb,xa);");
            AssertExprValue("Ord(a)", GetIntegerValue(2), "type Ta = (xc,xb,xa); const a = xa;");
            AssertExprValue("Ord('a')", GetIntegerValue(97));
            AssertExprValue("Ord('▒')", GetIntegerValue(9618));
            AssertExprValue("Ord(true)", GetIntegerValue(1));
            AssertExprValue("Ord(false)", GetIntegerValue(0));
            AssertExprValue("Ord(ByteBool(true))", GetIntegerValue(0xff));
            AssertExprValue("Ord(ByteBool(false))", GetIntegerValue(0));
            AssertExprValue("Ord(WordBool(true))", GetIntegerValue(0xffff));
            AssertExprValue("Ord(WordBool(false))", GetIntegerValue(0));
            AssertExprValue("Ord(LongBool(true))", GetIntegerValue(0xffffffff));
            AssertExprValue("Ord(LongBool(false))", GetIntegerValue(0));
            AssertExprValue("Ord(a)", GetIntegerValue(-3), "type Ta = -5..5; const a: Ta = -3;");
            AssertExprValue("Ord(a)", GetIntegerValue(1), "type Ta = false..true; const a: Ta = true;");
            AssertExprValue("Ord(a)", GetIntegerValue(116), "type Ta = 's'..'z'; const a: Ta = 't';");
            AssertExprValue("Ord(a)", GetIntegerValue(1), "type Ta = (xa, xb, cx); Tb = xa..xb; const a: Tb = xb;");

            AssertExprValue("Ord(a)", GetInvocationValue(IntrinsicRoutineId.Ord, SystemUnit.WordType, SystemUnit.WideCharType), "var a: WideChar;", isConstant: false);
            AssertExprValue("Ord(a)", GetInvocationValue(IntrinsicRoutineId.Ord, SystemUnit.ByteType, SystemUnit.AnsiCharType), "var a: AnsiChar;", isConstant: false);

            AssertExprValue("Ord(q)", GetInvocationValue(IntrinsicRoutineId.Ord, SystemUnit.ShortIntType, SystemUnit.ShortIntType), "var q: ShortInt;", isConstant: false);
            AssertExprValue("Ord(q)", GetInvocationValue(IntrinsicRoutineId.Ord, SystemUnit.ByteType, SystemUnit.BooleanType), "var q: Boolean;", isConstant: false);

            var e = CreateEnvironment();
            var ct = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var et = ct.CreateEnumType("Ta");
            AssertExprValue("Ord(q)", GetInvocationValue(IntrinsicRoutineId.Ord, SystemUnit.ShortIntType, et), "type Ta = (xc,xb,xa); var q: Ta;", isConstant: false);
        }

        /// <summary>
        ///     test muldiv64 function
        /// </summary>
        [TestMethod]
        public void TestMulDiv64() {
            AssertExprValue("MulDivInt64(10, 4, 2)", GetIntegerValue(20));
            AssertExprValue("MulDivInt64(-10, 4, 2)", GetIntegerValue(-20));
            AssertExprValue("MulDivInt64(a, 4, 2)", GetInvocationValue(IntrinsicRoutineId.MulDiv64, SystemUnit.Int64Type, SystemUnit.Int64Type), "var a: byte;", isConstant: false);
            AssertExprValue("MulDivInt64(a, 4, 2)", GetInvocationValue(IntrinsicRoutineId.MulDiv64, SystemUnit.Int64Type, SystemUnit.Int64Type), "type Ta = 3..8; var a: Ta;", isConstant: false);
        }

        /// <summary>
        ///     evaluate pi function
        /// </summary>
        [TestMethod]
        public void TestPi()
            => AssertExprValue("Pi()", GetExtendedValue(ExtF80.Pi));


        /// <summary>
        ///     test concatenation operator
        /// </summary>
        [TestMethod]
        public void TestConcat() {
            AssertExprValue("Concat('a')", GetWideCharValue('a'));
            AssertExprValue("Concat('a', 'b')", GetUnicodeStringValue("ab"));
            AssertExprValue("Concat('a', '')", GetUnicodeStringValue("a"));
            AssertExprValue("Concat('', 'b')", GetUnicodeStringValue("b"));
            AssertExprValue("Concat('', '')", GetUnicodeStringValue(""));
            AssertExprValue("Concat('a', 'b', 'c')", GetUnicodeStringValue("abc"));
            AssertExprValue("Concat(a, a, a)", GetUnicodeStringValue("aaa"), "type Ta = 'a'..'b'; const a: Ta = 'a';");
            AssertExprValue("Concat(a)", GetWideCharValue('a'), "type Ta = 'a'..'b'; const a: Ta = 'a';");

            var e = CreateEnvironment();
            var ct = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var sr = ct.CreateSubrangeType(SystemUnit.WideCharType, GetWideCharValue('a'), GetWideCharValue('b'));

            AssertExprValue("Concat(a, a, a)", GetInvocationValue(IntrinsicRoutineId.Concat, SystemUnit.UnicodeStringType, SystemUnit.WideCharType), "type Ta = 'a'..'b'; var a: Ta;", isConstant: false);
            AssertExprValue("Concat(a)", GetInvocationValue(IntrinsicRoutineId.Concat, sr, sr), "type Ta = 'a'..'b'; var a: Ta;", isConstant: false);
        }

        /// <summary>
        ///     test <c>pred</c> operator
        /// </summary>
        [TestMethod]
        public void TestPred() {

            // integers
            AssertExprValue("Pred(1)", GetIntegerValue(0));
            AssertExprValue("Pred(-3)", GetIntegerValue(-4));

            // chars
            AssertExprValue("Pred('b')", GetWideCharValue('a'));
            AssertExprValue("Pred('c')", GetWideCharValue('b'));

            // booleans
            AssertExprValue("Pred(true)", GetBooleanValue(false));
            AssertExprValue("Pred(false)", GetBooleanValue(false));

            // enumerations
            AssertExprValue("Pred(b)", GetIntegerValue(0), "type Te = (a,b);");

            // subrange types
            var e = CreateEnvironment();
            var tc = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var st = tc.CreateSubrangeType(SystemUnit.ShortIntType, GetIntegerValue(-3), GetIntegerValue(3));
            AssertExprValue("Pred(a)", GetSubrangeValue(st, GetIntegerValue(-3)), "type Te = -3..3; const a: Te = -2;");
        }

        /// <summary>
        ///     test pointer cast
        /// </summary>
        [TestMethod]
        public void TestPtr()
            => AssertExprValue("Ptr(4)", GetPointerValue(GetIntValue(4)));

        /// <summary>
        ///     test round function
        /// </summary>
        [TestMethod]
        public void TestRound() {
            AssertExprValue("Round(2.5)", GetIntegerValue(2));
            AssertExprValue("Round(3.5)", GetIntegerValue(4));
            AssertExprValue("Round(-2.5)", GetIntegerValue(-2));
            AssertExprValue("Round(-3.5)", GetIntegerValue(-4));
            AssertExprValue("Round(2)", GetIntegerValue(2));
            AssertExprValue("Round(3)", GetIntegerValue(3));
            AssertExprValue("Round(-2)", GetIntegerValue(-2));
            AssertExprValue("Round(-3)", GetIntegerValue(-3));
            AssertExprValue("Round(2.33)", GetIntegerValue(2));
            AssertExprValue("Round(3.33)", GetIntegerValue(3));
            AssertExprValue("Round(-2.33)", GetIntegerValue(-2));
            AssertExprValue("Round(-3.33)", GetIntegerValue(-3));
            AssertExprValue("Round(2.77)", GetIntegerValue(3));
            AssertExprValue("Round(3.77)", GetIntegerValue(4));
            AssertExprValue("Round(-2.77)", GetIntegerValue(-3));
            AssertExprValue("Round(-3.77)", GetIntegerValue(-4));

            AssertExprValue("Round(Pi()", GetIntegerValue(3));
        }

        /// <summary>
        ///     test <c>succ</c> function
        /// </summary>
        [TestMethod]
        public void TestSucc() {

            // integers
            AssertExprValue("Succ(1)", GetIntegerValue(2));
            AssertExprValue("Succ(-3)", GetIntegerValue(-2));

            // chars
            AssertExprValue("Succ('a')", GetWideCharValue('b'));
            AssertExprValue("Succ('b')", GetWideCharValue('c'));

            // booleans
            AssertExprValue("Succ(true)", GetBooleanValue(true));
            AssertExprValue("Succ(false)", GetBooleanValue(true));
            AssertExprValue("Succ(ByteBool(false))", GetBooleanValue(1));
            AssertExprValue("Succ(ByteBool(true))", GetBooleanValue(0xff));

            // enumerations
            AssertExprValue("Succ(a)", GetIntegerValue(1), "type Te = (a,b);");

            // subrange types
            var e = CreateEnvironment();
            var tc = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var st = tc.CreateSubrangeType(SystemUnit.ShortIntType, GetIntegerValue(-3), GetIntegerValue(3));
            var sv = e.Runtime.Types.MakeSubrangeValue(st, GetIntegerValue(-1));
            AssertExprValue("Succ(a)", sv, "type Te = -3..3; const a: Te = -2;");
        }

        /// <summary>
        ///     test boolean operations
        /// </summary>
        [TestMethod]
        public void TestBooleanOperations() {
            AssertExprValue("false and false", GetBooleanValue(false));
            AssertExprValue("false and true", GetBooleanValue(false));
            AssertExprValue("true and false", GetBooleanValue(false));
            AssertExprValue("true and true", GetBooleanValue(true));

            AssertExprValue("false or false", GetBooleanValue(false));
            AssertExprValue("false or true", GetBooleanValue(true));
            AssertExprValue("true or false", GetBooleanValue(true));
            AssertExprValue("true or true", GetBooleanValue(true));

            AssertExprValue("false xor false", GetBooleanValue(false));
            AssertExprValue("false xor true", GetBooleanValue(true));
            AssertExprValue("true xor false", GetBooleanValue(true));
            AssertExprValue("true xor true", GetBooleanValue(false));

            AssertExprValue("false and a", GetBooleanValue(false), "var a: Boolean");
            AssertExprValue("true or a", GetBooleanValue(true), "var a: Boolean");
        }

        /// <summary>
        ///     test sizeof operator
        /// </summary>
        [TestMethod]
        public void TestSizeOf() {
            AssertExprValue("SizeOf(AnsiChar)", GetIntegerValue(1));
            AssertExprValue("SizeOf(AnsiString)", GetIntegerValue(4));
            AssertExprValue("SizeOf(AnsiChar(a))", GetIntegerValue(1), "var a: AnsiChar;");

            AssertExprValue("SizeOf(Char)", GetIntegerValue(2));
            AssertExprValue("SizeOf(WideChar)", GetIntegerValue(2));

            AssertExprValue("SizeOf(Ta)", GetIntegerValue(1), "type Ta = (xa,xb,xc); ");

            AssertExprValue("SizeOf(Boolean)", GetIntegerValue(1));
            AssertExprValue("SizeOf(ByteBool)", GetIntegerValue(1));
            AssertExprValue("SizeOf(WordBool)", GetIntegerValue(2));
            AssertExprValue("SizeOf(LongBool)", GetIntegerValue(4));

            AssertExprValue("SizeOf(Byte)", GetIntegerValue(1));
            AssertExprValue("SizeOf(Uint8)", GetIntegerValue(1));
            AssertExprValue("SizeOf(ShortInt)", GetIntegerValue(1));
            AssertExprValue("SizeOf(int8)", GetIntegerValue(1));

            AssertExprValue("SizeOf(Word)", GetIntegerValue(2));
            AssertExprValue("SizeOf(Uint16)", GetIntegerValue(2));
            AssertExprValue("SizeOf(SmallInt)", GetIntegerValue(2));
            AssertExprValue("SizeOf(Int16)", GetIntegerValue(2));

            AssertExprValue("SizeOf(FixedUint)", GetIntegerValue(4));
            AssertExprValue("SizeOf(Cardinal)", GetIntegerValue(4));
            AssertExprValue("SizeOf(UInt32)", GetIntegerValue(4));
            AssertExprValue("SizeOf(LongWord)", GetIntegerValue(4));
            AssertExprValue("SizeOf(NativeUInt)", GetIntegerValue(4));
            AssertExprValue("SizeOf(Fixedint)", GetIntegerValue(4));
            AssertExprValue("SizeOf(Integer)", GetIntegerValue(4));
            AssertExprValue("SizeOf(Int32)", GetIntegerValue(4));
            AssertExprValue("SizeOf(LongInt)", GetIntegerValue(4));
            AssertExprValue("SizeOf(NativeInt)", GetIntegerValue(4));

            AssertExprValue("SizeOf(Int64)", GetIntegerValue(8));
            AssertExprValue("SizeOf(UInt64)", GetIntegerValue(8));

            AssertExprValue("SizeOf(Ta)", GetIntegerValue(1), "type Ta = -3..3;");
            AssertExprValue("SizeOf(Ta)", GetIntegerValue(1), "type Ta = 3..180;");
            AssertExprValue("SizeOf(Ta)", GetIntegerValue(2), "type Ta = -180..3;");
            AssertExprValue("SizeOf(Ta)", GetIntegerValue(2), "type Ta = 3..380;");
            AssertExprValue("SizeOf(Ta)", GetIntegerValue(4), "type Ta = -33555..3;");
            AssertExprValue("SizeOf(Ta)", GetIntegerValue(4), "type Ta = 3..4294967295;");
            AssertExprValue("SizeOf(Ta)", GetIntegerValue(8), "type Ta = -4294967295..3;");
            AssertExprValue("SizeOf(Ta)", GetIntegerValue(8), "type Ta = 3..18446744073709551614;");

            AssertExprValue("SizeOf(Real48)", GetIntegerValue(6));
            AssertExprValue("SizeOf(Single)", GetIntegerValue(4));
            AssertExprValue("SizeOf(Real)", GetIntegerValue(8));
            AssertExprValue("SizeOf(Double)", GetIntegerValue(8));
            AssertExprValue("SizeOf(Extended)", GetIntegerValue(10));
            AssertExprValue("SizeOf(Comp)", GetIntegerValue(8));
            AssertExprValue("SizeOf(Currency)", GetIntegerValue(8));

            AssertExprValue("SizeOf(Pointer)", GetIntegerValue(4));
        }

        /// <summary>
        ///     test sqr function
        /// </summary>
        [TestMethod]
        public void TestSqr() {
            AssertExprValue("Sqr(2)", GetIntegerValue(4));
            AssertExprValue("Sqr(3)", GetIntegerValue(9));
            AssertExprValue("Sqr(2.25)", GetExtendedValue(5.0625));
        }

        /// <summary>
        ///     test swap function
        /// </summary>
        [TestMethod]
        public void TestSwap() {
            AssertExprValue("Swap($ff)", GetIntegerValue(0x00));
            AssertExprValue("Swap($ff00)", GetIntegerValue(0xff));
            AssertExprValue("Swap($aaff00)", GetIntegerValue(0xaa00ff));
        }

        /// <summary>
        ///     test trunc function
        /// </summary>
        [TestMethod]
        public void TestTrunc() {
            AssertExprValue("Trunc(0.0)", GetIntegerValue(0));
            AssertExprValue("Trunc(1.0)", GetIntegerValue(1));
            AssertExprValue("Trunc(-1.0)", GetIntegerValue(-1));
            AssertExprValue("Trunc(1.9998)", GetIntegerValue(1));
            AssertExprValue("Trunc(-1.9998)", GetIntegerValue(-1));
            AssertExprValue("Trunc(1)", GetIntegerValue(1));
            AssertExprValue("Trunc(-1)", GetIntegerValue(-1));
        }

        /// <summary>
        ///     test record constants
        /// </summary>
        [TestMethod]
        public void TestRecordConstants() {
            var e = CreateEnvironment();
            var tc = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var rt1 = tc.CreateStructuredType(string.Empty, StructuredTypeKind.Record);
            rt1.AddField(new Variable() { Name = "a", TypeDefinition = e.TypeRegistry.SystemUnit.ShortIntType });
            rt1.AddField(new Variable() { Name = "b", TypeDefinition = e.TypeRegistry.SystemUnit.WideCharType });
            var rt2 = tc.CreateStructuredType(string.Empty, StructuredTypeKind.Record);
            rt2.AddField(new Variable() { Name = "a", TypeDefinition = e.TypeRegistry.SystemUnit.ExtendedType });
            rt2.AddField(new Variable() { Name = "b", TypeDefinition = e.TypeRegistry.SystemUnit.UnicodeStringType });

            AssertExprValue("a", GetRecordValue(rt1, GetIntegerValue(1), GetWideCharValue('2')), "const a = (a: 1; b: '2');");
            AssertExprValue("a", GetRecordValue(rt2, GetExtendedValue(1.0), GetUnicodeStringValue("22")), "const a = (a: 1.0; b: '22');");
        }

        /// <summary>
        ///     test set constants
        /// </summary>
        [TestMethod]
        public void TestSetConstants() {
            var e = CreateEnvironment();
            var ct = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var sr1 = ct.CreateSubrangeType(SystemUnit.ShortIntType, GetIntegerValue(1), GetIntegerValue(5));
            var sr2 = ct.CreateSubrangeType(SystemUnit.ShortIntType, GetIntegerValue(1), GetIntegerValue(3));
            var st1 = ct.CreateSetType(sr1, "tb");
            var st2 = ct.CreateSetType(sr2, "tb");
            var v1 = GetSubrangeValue(sr1, GetIntegerValue(1));
            var v2 = GetSubrangeValue(sr1, GetIntegerValue(2));
            var v3 = GetSubrangeValue(sr1, GetIntegerValue(3));
            var v4 = GetSubrangeValue(sr2, GetIntegerValue(1));
            var v5 = GetSubrangeValue(sr2, GetIntegerValue(2));
            var v6 = GetSubrangeValue(sr2, GetIntegerValue(3));
            AssertExprValue("b", GetSetValue(st1, v1, v3), "type Ta = 1..5; Tb = set of Ta; const b : Tb = [1,3];");
            AssertExprValue("b", GetSetValue(st2, v4, v5, v6), "type Ta = 1..3; Tb = set of Ta; const b : Tb = [Low(Ta)..High(Ta)];");
            AssertExprValue("b", e.TypeRegistry.SystemUnit.ErrorType.Reference, "type Ta = 1..3333; Tb = set of Ta; const b : Tb = [Low(Ta)..High(Ta)];", isConstant: false);
            AssertExprValue("b", e.TypeRegistry.SystemUnit.ErrorType.Reference, "type Ta = 1..3; Tb = set of Ta; const b : Tb = [High(Ta)..Low(Ta)];", isConstant: false);
            AssertExprValue("b", e.TypeRegistry.SystemUnit.ErrorType.Reference, "type Tb = set of string; const b : Tb = [High(Ta)..Low(Ta)];", isConstant: false);
            AssertExprValue("b", e.TypeRegistry.SystemUnit.ErrorType.Reference, "const b = ['aa','b'];", isConstant: false);
        }

        /// <summary>
        ///     test array constants
        /// </summary>
        [TestMethod]
        public void TestArrayConstants() {

            var ev = CreateEnvironment();
            var tc = ev.TypeRegistry.CreateTypeFactory(ev.TypeRegistry.SystemUnit);
            var sr = tc.CreateSubrangeType(SystemUnit.ShortIntType, GetIntegerValue(0), GetIntegerValue(1));
            var rt = tc.CreateStructuredType("ta", StructuredTypeKind.Record);
            rt.Fields.Add(new Variable() { Name = "a", TypeDefinition = SystemUnit.StringType });
            rt.Fields.Add(new Variable() { Name = "b", TypeDefinition = SystemUnit.IntegerType });
            var at2 = tc.CreateDynamicArrayType(SystemUnit.StringType, string.Empty, false);
            var at3 = tc.CreateStaticArrayType(rt, string.Empty, sr, false);
            var r1 = GetRecordValue(rt, GetUnicodeStringValue("a"), GetIntValue(2));
            var r2 = GetRecordValue(rt, GetUnicodeStringValue("b"), GetIntValue(4));
            var v = GetArrayValue(at3, rt, r1, r2);
            var a = GetArrayValue(at2, SystemUnit.StringType, GetUnicodeStringValue("aa"), GetUnicodeStringValue("b"), GetUnicodeStringValue("cc"));

            AssertExprValue("c", a, "const c: array of string = ['aa','b','cc']");
            AssertExprValue("c", a, "const c: array of string = ['aa','b']+['cc']");
            AssertExprValue("c", v, "type Ta = record a: string; b: integer; end; const c: array[0..1] of Ta = ((a: 'a';b:2),(a: 'b';b: 4));");
            AssertExprValue("c", GetInvalidCastValue(), "type Ta = record a: string; b: integer; end; const c: array[0..1] of Ta = ((a: 2;b:'b'),(a: 'b';b: 4));", isConstant: false);
            AssertExprValue("c", GetInvalidCastValue(), "type Ta = record a: integer; b: string; end; const c: array[0..1] of Ta = ((a: 'a';b:2),(a: 'b';b: 4));", isConstant: false);

            var it = tc.CreateEnumType("Ta");
            it.DefineEnumValue(MakeRuntime(), "a1", true, GetIntegerValue(0));
            it.DefineEnumValue(MakeRuntime(), "a2", true, GetIntegerValue(1));
            var at1 = tc.CreateStaticArrayType(SystemUnit.StringType, string.Empty, it, false);
            v = GetArrayValue(at1, SystemUnit.StringType, GetUnicodeStringValue("a"), GetUnicodeStringValue("b"));
            AssertExprValue("c", v, "type Ta = (a1, a2); const c: array[Ta] of string = ('a','b'); ");
        }

    }
}
