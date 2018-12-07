﻿using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test constant propagation
    /// </summary>
    public class ConstantPropagationTest : TypeTest {

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

        [TestMethod]
        public void TestIntegerOperations() {
            AssertExprValue("4 + 5", GetIntegerValue(9));
            AssertExprValue("4 - 3", GetIntegerValue(1));
            AssertExprValue("4 - 5", GetIntegerValue(-1));

            AssertExprValue("4 * 5", GetIntegerValue(20));
            AssertExprValue("20 div 4", GetIntegerValue(5));
            AssertExprValue("9 mod 4", GetIntegerValue(1));
        }

        [TestMethod]
        public void TestAbs() {
            AssertExprValue("Abs(5)", GetIntegerValue(5));
            AssertExprValue("Abs(0)", GetIntegerValue(0));
            AssertExprValue("Abs(-3)", GetIntegerValue(3));
            AssertExprValue("Abs(a)", GetSubrangeValue(1000, GetIntegerValue(5)), "type Ta=-4..8; const a: Ta = 5;");
            AssertExprValue("Abs(a)", GetSubrangeValue(1000, GetIntegerValue(0)), "type Ta=-4..8; const a: Ta = 0;");
            AssertExprValue("Abs(a)", GetSubrangeValue(1000, GetIntegerValue(3)), "type Ta=-4..8; const a: Ta = -3;");
            AssertExprValue("Abs(a)", GetUnkownValue(KnownTypeIds.IntegerType, CommonTypeKind.IntegerType), "var a: integer;", isConstant: false);
            AssertExprValue("Abs(-a)", GetUnkownValue(1000, CommonTypeKind.SubrangeType), "type Ta = -4..4; var a: Ta;", isConstant: false);
            AssertExprValue("Abs(a)", GetUnkownValue(1000, CommonTypeKind.SubrangeType), "type Ta = -4..4; var a: Ta;", isConstant: false);
            AssertExprValue("Abs(-a)", GetUnkownValue(1000, CommonTypeKind.SubrangeType), "type Ta = -4..4; var a: Ta;", isConstant: false);


            AssertExprValue("Abs(5.4)", GetExtendedValue("5.4"));
            AssertExprValue("Abs(0.0)", GetExtendedValue(0));
            AssertExprValue("Abs(-3.3)", GetExtendedValue("3.3"));

            AssertExprValue("Abs(a)", GetSubrangeValue(1000, GetIntegerValue(3)), "type Ta = 1..9; const a: Ta = 3;");
        }

        [TestMethod]
        public void TestChr() {
            AssertExprValue("Chr(5)", GetWideCharValue((char)5));
            AssertExprValue("Chr(0)", GetWideCharValue((char)0));
            AssertExprValue("Chr(-3)", GetWideCharValue((char)(ushort.MaxValue - 3 + 1)));

            AssertExprValue("Chr(a)", GetWideCharValue((char)5), "type ta = 2..9; const a: ta = 5; ");
        }

        [TestMethod]
        public void TestHi() {
            AssertExprValue("Hi($FF)", GetIntegerValue(0x00));
            AssertExprValue("Hi($FFFF)", GetIntegerValue(0xff));
            AssertExprValue("Hi($FFFFFF)", GetIntegerValue(0xff));

            AssertExprValue("Hi(a)", GetIntegerValue(0xf0), "type ta = 3..$FFFF; const a: ta = $F0FF;");
        }

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

        [TestMethod]
        public void TestLength() {
            AssertExprValue("Length('')", GetIntegerValue(0));
            AssertExprValue("Length('a')", GetIntegerValue(1));
            AssertExprValue("Length('aaa')", GetIntegerValue(3));

            AssertExprValue("Length(a)", GetIntegerValue(3), "const a = (1,2,3);");
        }

        [TestMethod]
        public void TestLo() {
            AssertExprValue("Lo($FF)", GetIntegerValue(0xff));
            AssertExprValue("Lo($FF00)", GetIntegerValue(0x00));
            AssertExprValue("Lo($FFFF0F)", GetIntegerValue(0x0f));

            AssertExprValue("Lo(a)", GetIntegerValue(0x0f), "type ta = 3..$FFFF; const a: ta = $F00F;");
        }

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
            AssertExprValue("Low(Byte)", GetIntegerValue(byte.MinValue));
            AssertExprValue("Low(SmallInt)", GetIntegerValue(short.MinValue));
            AssertExprValue("Low(Word)", GetIntegerValue(ushort.MinValue));
            AssertExprValue("Low(Integer)", GetIntegerValue(int.MinValue));
            AssertExprValue("Low(Cardinal)", GetIntegerValue(uint.MinValue));
            AssertExprValue("Low(Int64)", GetIntegerValue(long.MinValue));
            AssertExprValue("Low(UInt64)", GetIntegerValue(ulong.MinValue));

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



        [TestMethod]
        public void TestConcat() {
            AssertExprValue("Concat('a')", GetWideCharValue('a'));
            AssertExprValue("Concat('a', 'b')", GetUnicodeStringValue("ab"));
            AssertExprValue("Concat('a', '')", GetUnicodeStringValue("a"));
            AssertExprValue("Concat('', 'b')", GetUnicodeStringValue("b"));
            AssertExprValue("Concat('', '')", GetUnicodeStringValue(""));
            AssertExprValue("Concat('a', 'b', 'c')", GetUnicodeStringValue("abc"));
        }

        [TestMethod]
        public void TestSucc() {

            // integers
            AssertExprValue("Succ(1)", GetIntegerValue(2));
            AssertExprValue("Succ(-3)", GetIntegerValue(-2));

            // chars
            AssertExprValue("Succ('a')", GetWideCharValue('b'));
            AssertExprValue("Succ('b')", GetWideCharValue('c'));

            // booleans
            AssertExprValue("Succ(true)", GetBooleanValue(false));
            AssertExprValue("Succ(false)", GetBooleanValue(true));

            // enumerations
            AssertExprValue("Succ(a)", GetIntegerValue(1), "type Te = (a,b);");

            // subrange types
            AssertExprValue("Succ(a)", GetSubrangeValue(1000, GetIntegerValue(-1)), "type Te = -3..3; const a: Te = -2;");
        }

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

    }
}
