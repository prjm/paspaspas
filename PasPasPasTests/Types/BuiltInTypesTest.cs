using PasPasPas.Options.DataTypes;
using PasPasPas.Typings.Common;
using Xunit;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test for built in types
    /// </summary>
    public class BuiltInTypesTest : TypeTest {

        [Fact]
        public void TestIntTypes() {
            AssertDeclType("System.Byte", TypeIds.ByteType);
            AssertDeclType("System.Word", TypeIds.WordType);
            AssertDeclType("System.Cardinal", TypeIds.CardinalType);
            AssertDeclType("System.UInt64", TypeIds.Uint64Type);
            AssertDeclType("System.ShortInt", TypeIds.ShortInt);
            AssertDeclType("System.SmallInt", TypeIds.SmallInt);
            AssertDeclType("System.Integer", TypeIds.IntegerType);
            AssertDeclType("System.Int64", TypeIds.Int64Type);

            AssertDeclType("Byte", TypeIds.ByteType);
            AssertDeclType("Word", TypeIds.WordType);
            AssertDeclType("Cardinal", TypeIds.CardinalType);
            AssertDeclType("UInt64", TypeIds.Uint64Type);
            AssertDeclType("ShortInt", TypeIds.ShortInt);
            AssertDeclType("SmallInt", TypeIds.SmallInt);
            AssertDeclType("Integer", TypeIds.IntegerType);
            AssertDeclType("Int64", TypeIds.Int64Type);
        }

        [Fact]
        public void TestCharTypes() {
            AssertDeclType("System.Char", TypeIds.CharType);
            AssertDeclType("System.AnsiChar", TypeIds.AnsiCharType);
            AssertDeclType("System.WideChar", TypeIds.WideCharType);
            AssertDeclType("System.UCS2Char", TypeIds.Ucs2CharType);
            AssertDeclType("System.UCS4Char", TypeIds.Ucs4CharType);

            AssertDeclType("Char", TypeIds.CharType);
            AssertDeclType("AnsiChar", TypeIds.AnsiCharType);
            AssertDeclType("WideChar", TypeIds.WideCharType);
            AssertDeclType("UCS2Char", TypeIds.Ucs2CharType);
            AssertDeclType("UCS4Char", TypeIds.Ucs4CharType);
        }

        [Fact]
        public void TestBooleanTypes() {
            AssertDeclType("Boolean", TypeIds.BooleanType);
            AssertDeclType("ByteBool", TypeIds.ByteBoolType);
            AssertDeclType("WordBool", TypeIds.WordBoolType);
            AssertDeclType("LongBool", TypeIds.LongBoolType);

            AssertDeclType("System.Boolean", TypeIds.BooleanType);
            AssertDeclType("System.ByteBool", TypeIds.ByteBoolType);
            AssertDeclType("System.WordBool", TypeIds.WordBoolType);
            AssertDeclType("System.LongBool", TypeIds.LongBoolType);
        }

        [Fact]
        public void TestNativeIntTypes() {
            AssertDeclType("System.NativeInt", TypeIds.NativeInt, NativeIntSize.All32bit, 32);
            AssertDeclType("System.NativeUInt", TypeIds.NativeUInt, NativeIntSize.All32bit, 32);
            AssertDeclType("System.LongInt", TypeIds.LongInt, NativeIntSize.All32bit, 32);
            AssertDeclType("System.LongWord", TypeIds.LongWord, NativeIntSize.All32bit, 32);
            AssertDeclType("NativeInt", TypeIds.NativeInt, NativeIntSize.All32bit, 32);
            AssertDeclType("NativeUInt", TypeIds.NativeUInt, NativeIntSize.All32bit, 32);
            AssertDeclType("LongInt", TypeIds.LongInt, NativeIntSize.All32bit, 32);
            AssertDeclType("LongWord", TypeIds.LongWord, NativeIntSize.All32bit, 32);

            AssertDeclType("System.NativeInt", TypeIds.NativeInt, NativeIntSize.All64bit, 64);
            AssertDeclType("System.NativeUInt", TypeIds.NativeUInt, NativeIntSize.All64bit, 64);
            AssertDeclType("System.LongInt", TypeIds.LongInt, NativeIntSize.All64bit, 64);
            AssertDeclType("System.LongWord", TypeIds.LongWord, NativeIntSize.All64bit, 64);
            AssertDeclType("NativeInt", TypeIds.NativeInt, NativeIntSize.All64bit, 64);
            AssertDeclType("NativeUInt", TypeIds.NativeUInt, NativeIntSize.All64bit, 64);
            AssertDeclType("LongInt", TypeIds.LongInt, NativeIntSize.All64bit, 64);
            AssertDeclType("LongWord", TypeIds.LongWord, NativeIntSize.All64bit, 64);

            AssertDeclType("System.NativeInt", TypeIds.NativeInt, NativeIntSize.Windows64bit, 64);
            AssertDeclType("System.NativeUInt", TypeIds.NativeUInt, NativeIntSize.Windows64bit, 64);
            AssertDeclType("System.LongInt", TypeIds.LongInt, NativeIntSize.Windows64bit, 32);
            AssertDeclType("System.LongWord", TypeIds.LongWord, NativeIntSize.Windows64bit, 32);
            AssertDeclType("NativeInt", TypeIds.NativeInt, NativeIntSize.Windows64bit, 64);
            AssertDeclType("NativeUInt", TypeIds.NativeUInt, NativeIntSize.Windows64bit, 64);
            AssertDeclType("LongInt", TypeIds.LongInt, NativeIntSize.Windows64bit, 32);
            AssertDeclType("LongWord", TypeIds.LongWord, NativeIntSize.Windows64bit, 32);
        }

        [Fact]
        public void TestStringTypes() {
            AssertDeclType("String", TypeIds.StringType);
            AssertDeclType("AnsiString", TypeIds.AnsiStringType);
            AssertDeclType("UnicodeString", TypeIds.UnicodeStringType);
            AssertDeclType("WideString", TypeIds.WideStringType);
            AssertDeclType("ShortString", TypeIds.ShortStringType);
            AssertDeclType("System.String", TypeIds.StringType);
            AssertDeclType("System.AnsiString", TypeIds.AnsiStringType);
            AssertDeclType("System.UnicodeString", TypeIds.UnicodeStringType);
            AssertDeclType("System.WideString", TypeIds.WideStringType);
            AssertDeclType("System.ShortString", TypeIds.ShortStringType);
        }

        [Fact]
        public void TestRealTypes() {
            AssertDeclType("Real48", TypeIds.Real48Type);
            AssertDeclType("Single", TypeIds.SingleType);
            AssertDeclType("Double", TypeIds.Double);
            AssertDeclType("Real", TypeIds.Real);
            AssertDeclType("Extended", TypeIds.Extended);
            AssertDeclType("Comp", TypeIds.Comp);
            AssertDeclType("Currency", TypeIds.Currency);
            AssertDeclType("System.Real48", TypeIds.Real48Type);
            AssertDeclType("System.Single", TypeIds.SingleType);
            AssertDeclType("System.Double", TypeIds.Double);
            AssertDeclType("System.Real", TypeIds.Real);
            AssertDeclType("System.Extended", TypeIds.Extended);
            AssertDeclType("System.Comp", TypeIds.Comp);
            AssertDeclType("System.Currency", TypeIds.Currency);
        }

    }
}
