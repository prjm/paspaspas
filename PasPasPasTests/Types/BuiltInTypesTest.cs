using System;
using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;
using PasPasPas.Typings.Structured;
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
        public void TestTObjectType() {
            Func<StructuredTypeDeclaration, bool> hasMethod(string q) => (t => t?.HasMethod(q) ?? false);

            AssertDeclType("TObject", TypeIds.TObject);
            AssertDeclType("System.TObject", TypeIds.TObject);
            AssertDeclTypeDef("TObject", hasMethod("Create"));
            AssertDeclTypeDef("TObject", hasMethod("Free"));
            AssertDeclTypeDef("TObject", hasMethod("DisposeOf"));
            AssertDeclTypeDef("TObject", hasMethod("CleanupInstance"));
            AssertDeclTypeDef("TObject", hasMethod("ClassType"));
            AssertDeclTypeDef("TObject", hasMethod("FieldAddress"));
        }

        [Fact]
        public void TestPointerTypes() {
            AssertDeclType("Pointer", TypeIds.GenericPointer);
            AssertDeclType("PByte", TypeIds.PByte);
            AssertDeclType("PWord", TypeIds.PWord);
            AssertDeclType("PCardinal", TypeIds.PCardinal);
            AssertDeclType("PUInt64", TypeIds.PUInt64);
            AssertDeclType("PShortInt", TypeIds.PShortInt);
            AssertDeclType("PSmallInt", TypeIds.PSmallInt);
            AssertDeclType("PInteger", TypeIds.PInteger);
            AssertDeclType("PInt64", TypeIds.PInt64);
            AssertDeclType("PSingle", TypeIds.PSingle);
            AssertDeclType("PDouble", TypeIds.PDouble);
            AssertDeclType("PExtended", TypeIds.PExtended);
            AssertDeclType("PAnsiChar", TypeIds.PAnsiChar);
            AssertDeclType("PWideChar", TypeIds.PWideChar);
            AssertDeclType("PAnsiString", TypeIds.PAnsiString);
            AssertDeclType("PRawByteString", TypeIds.PRawByteString);
            AssertDeclType("PUnicodeString", TypeIds.PUnicodeString);
            AssertDeclType("PShortString", TypeIds.PShortString);
            AssertDeclType("PWideString", TypeIds.PWideString);
            AssertDeclType("PChar", TypeIds.PChar);
            AssertDeclType("PString", TypeIds.PString);
            AssertDeclType("PBoolean", TypeIds.PBoolean);
            AssertDeclType("PLongBool", TypeIds.PLongBool);
            AssertDeclType("PWordBool", TypeIds.PWordBool);
            AssertDeclType("PPointer", TypeIds.PPointer);
            AssertDeclType("PCurrency", TypeIds.PCurrency);
            AssertDeclType("System.Pointer", TypeIds.GenericPointer);
            AssertDeclType("System.PByte", TypeIds.PByte);
            AssertDeclType("System.PWord", TypeIds.PWord);
            AssertDeclType("System.PCardinal", TypeIds.PCardinal);
            AssertDeclType("System.PUInt64", TypeIds.PUInt64);
            AssertDeclType("System.PShortInt", TypeIds.PShortInt);
            AssertDeclType("System.PSmallInt", TypeIds.PSmallInt);
            AssertDeclType("System.PInteger", TypeIds.PInteger);
            AssertDeclType("System.PInt64", TypeIds.PInt64);
            AssertDeclType("System.PSingle", TypeIds.PSingle);
            AssertDeclType("System.PDouble", TypeIds.PDouble);
            AssertDeclType("System.PExtended", TypeIds.PExtended);
            AssertDeclType("System.PAnsiChar", TypeIds.PAnsiChar);
            AssertDeclType("System.PWideChar", TypeIds.PWideChar);
            AssertDeclType("System.PAnsiString", TypeIds.PAnsiString);
            AssertDeclType("System.PRawByteString", TypeIds.PRawByteString);
            AssertDeclType("System.PUnicodeString", TypeIds.PUnicodeString);
            AssertDeclType("System.PShortString", TypeIds.PShortString);
            AssertDeclType("System.PWideString", TypeIds.PWideString);
            AssertDeclType("System.PChar", TypeIds.PChar);
            AssertDeclType("System.PString", TypeIds.PString);
            AssertDeclType("System.PBoolean", TypeIds.PBoolean);
            AssertDeclType("System.PLongBool", TypeIds.PLongBool);
            AssertDeclType("System.PWordBool", TypeIds.PWordBool);
            AssertDeclType("System.PPointer", TypeIds.PPointer);
            AssertDeclType("System.PCurrency", TypeIds.PCurrency);
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
            AssertDeclType("String[324]", typeKind: CommonTypeKind.ShortStringType);
            AssertDeclType("AnsiString", TypeIds.AnsiStringType);
            AssertDeclType("UnicodeString", TypeIds.UnicodeStringType);
            AssertDeclType("WideString", TypeIds.WideStringType);
            AssertDeclType("ShortString", TypeIds.ShortStringType);
            AssertDeclType("RawByteString", TypeIds.RawByteString);
            AssertDeclType("System.String", TypeIds.StringType);
            AssertDeclType("System.AnsiString", TypeIds.AnsiStringType);
            AssertDeclType("System.UnicodeString", TypeIds.UnicodeStringType);
            AssertDeclType("System.WideString", TypeIds.WideStringType);
            AssertDeclType("System.ShortString", TypeIds.ShortStringType);
            AssertDeclType("System.RawByteString", TypeIds.RawByteString);
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
