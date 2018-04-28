using System;
using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;
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
            AssertDeclType("System.Byte", KnownTypeIds.ByteType);
            AssertDeclType("System.Word", KnownTypeIds.WordType);
            AssertDeclType("System.Cardinal", KnownTypeIds.CardinalType);
            AssertDeclType("System.UInt64", KnownTypeIds.Uint64Type);
            AssertDeclType("System.ShortInt", KnownTypeIds.ShortInt);
            AssertDeclType("System.SmallInt", KnownTypeIds.SmallInt);
            AssertDeclType("System.Integer", KnownTypeIds.IntegerType);
            AssertDeclType("System.Int64", KnownTypeIds.Int64Type);

            AssertDeclType("Byte", KnownTypeIds.ByteType);
            AssertDeclType("Word", KnownTypeIds.WordType);
            AssertDeclType("Cardinal", KnownTypeIds.CardinalType);
            AssertDeclType("UInt64", KnownTypeIds.Uint64Type);
            AssertDeclType("ShortInt", KnownTypeIds.ShortInt);
            AssertDeclType("SmallInt", KnownTypeIds.SmallInt);
            AssertDeclType("Integer", KnownTypeIds.IntegerType);
            AssertDeclType("Int64", KnownTypeIds.Int64Type);
        }

        [Fact]
        public void TestCharTypes() {
            AssertDeclType("System.Char", KnownTypeIds.CharType);
            AssertDeclType("System.AnsiChar", KnownTypeIds.AnsiCharType);
            AssertDeclType("System.WideChar", KnownTypeIds.WideCharType);
            AssertDeclType("System.UCS2Char", KnownTypeIds.Ucs2CharType);
            AssertDeclType("System.UCS4Char", KnownTypeIds.Ucs4CharType);

            AssertDeclType("Char", KnownTypeIds.CharType);
            AssertDeclType("AnsiChar", KnownTypeIds.AnsiCharType);
            AssertDeclType("WideChar", KnownTypeIds.WideCharType);
            AssertDeclType("UCS2Char", KnownTypeIds.Ucs2CharType);
            AssertDeclType("UCS4Char", KnownTypeIds.Ucs4CharType);
        }

        [Fact]
        public void TestBooleanTypes() {
            AssertDeclType("Boolean", KnownTypeIds.BooleanType);
            AssertDeclType("ByteBool", KnownTypeIds.ByteBoolType);
            AssertDeclType("WordBool", KnownTypeIds.WordBoolType);
            AssertDeclType("LongBool", KnownTypeIds.LongBoolType);

            AssertDeclType("System.Boolean", KnownTypeIds.BooleanType);
            AssertDeclType("System.ByteBool", KnownTypeIds.ByteBoolType);
            AssertDeclType("System.WordBool", KnownTypeIds.WordBoolType);
            AssertDeclType("System.LongBool", KnownTypeIds.LongBoolType);
        }

        [Fact]
        public void TestTObjectType() {
            Func<StructuredTypeDeclaration, bool> hasMethod(string q) => (t => t?.HasMethod(q) ?? false);

            AssertDeclType("TObject", KnownTypeIds.TObject);
            AssertDeclType("System.TObject", KnownTypeIds.TObject);
            AssertDeclTypeDef("TObject", hasMethod("Create"));
            AssertDeclTypeDef("TObject", hasMethod("Free"));
            AssertDeclTypeDef("TObject", hasMethod("DisposeOf"));
            AssertDeclTypeDef("TObject", hasMethod("CleanupInstance"));
            AssertDeclTypeDef("TObject", hasMethod("ClassType"));
            AssertDeclTypeDef("TObject", hasMethod("FieldAddress"));
        }

        [Fact]
        public void TestPointerTypes() {
            AssertDeclType("Pointer", KnownTypeIds.GenericPointer);
            AssertDeclType("PByte", KnownTypeIds.PByte);
            AssertDeclType("PWord", KnownTypeIds.PWord);
            AssertDeclType("PCardinal", KnownTypeIds.PCardinal);
            AssertDeclType("PUInt64", KnownTypeIds.PUInt64);
            AssertDeclType("PShortInt", KnownTypeIds.PShortInt);
            AssertDeclType("PSmallInt", KnownTypeIds.PSmallInt);
            AssertDeclType("PInteger", KnownTypeIds.PInteger);
            AssertDeclType("PInt64", KnownTypeIds.PInt64);
            AssertDeclType("PSingle", KnownTypeIds.PSingle);
            AssertDeclType("PDouble", KnownTypeIds.PDouble);
            AssertDeclType("PExtended", KnownTypeIds.PExtended);
            AssertDeclType("PAnsiChar", KnownTypeIds.PAnsiChar);
            AssertDeclType("PWideChar", KnownTypeIds.PWideChar);
            AssertDeclType("PAnsiString", KnownTypeIds.PAnsiString);
            AssertDeclType("PRawByteString", KnownTypeIds.PRawByteString);
            AssertDeclType("PUnicodeString", KnownTypeIds.PUnicodeString);
            AssertDeclType("PShortString", KnownTypeIds.PShortString);
            AssertDeclType("PWideString", KnownTypeIds.PWideString);
            AssertDeclType("PChar", KnownTypeIds.PChar);
            AssertDeclType("PString", KnownTypeIds.PString);
            AssertDeclType("PBoolean", KnownTypeIds.PBoolean);
            AssertDeclType("PLongBool", KnownTypeIds.PLongBool);
            AssertDeclType("PWordBool", KnownTypeIds.PWordBool);
            AssertDeclType("PPointer", KnownTypeIds.PPointer);
            AssertDeclType("PCurrency", KnownTypeIds.PCurrency);
            AssertDeclType("System.Pointer", KnownTypeIds.GenericPointer);
            AssertDeclType("System.PByte", KnownTypeIds.PByte);
            AssertDeclType("System.PWord", KnownTypeIds.PWord);
            AssertDeclType("System.PCardinal", KnownTypeIds.PCardinal);
            AssertDeclType("System.PUInt64", KnownTypeIds.PUInt64);
            AssertDeclType("System.PShortInt", KnownTypeIds.PShortInt);
            AssertDeclType("System.PSmallInt", KnownTypeIds.PSmallInt);
            AssertDeclType("System.PInteger", KnownTypeIds.PInteger);
            AssertDeclType("System.PInt64", KnownTypeIds.PInt64);
            AssertDeclType("System.PSingle", KnownTypeIds.PSingle);
            AssertDeclType("System.PDouble", KnownTypeIds.PDouble);
            AssertDeclType("System.PExtended", KnownTypeIds.PExtended);
            AssertDeclType("System.PAnsiChar", KnownTypeIds.PAnsiChar);
            AssertDeclType("System.PWideChar", KnownTypeIds.PWideChar);
            AssertDeclType("System.PAnsiString", KnownTypeIds.PAnsiString);
            AssertDeclType("System.PRawByteString", KnownTypeIds.PRawByteString);
            AssertDeclType("System.PUnicodeString", KnownTypeIds.PUnicodeString);
            AssertDeclType("System.PShortString", KnownTypeIds.PShortString);
            AssertDeclType("System.PWideString", KnownTypeIds.PWideString);
            AssertDeclType("System.PChar", KnownTypeIds.PChar);
            AssertDeclType("System.PString", KnownTypeIds.PString);
            AssertDeclType("System.PBoolean", KnownTypeIds.PBoolean);
            AssertDeclType("System.PLongBool", KnownTypeIds.PLongBool);
            AssertDeclType("System.PWordBool", KnownTypeIds.PWordBool);
            AssertDeclType("System.PPointer", KnownTypeIds.PPointer);
            AssertDeclType("System.PCurrency", KnownTypeIds.PCurrency);
        }

        [Fact]
        public void TestNativeIntTypes() {
            AssertDeclType("System.NativeInt", KnownTypeIds.NativeInt, NativeIntSize.All32bit, 32);
            AssertDeclType("System.NativeUInt", KnownTypeIds.NativeUInt, NativeIntSize.All32bit, 32);
            AssertDeclType("System.LongInt", KnownTypeIds.LongInt, NativeIntSize.All32bit, 32);
            AssertDeclType("System.LongWord", KnownTypeIds.LongWord, NativeIntSize.All32bit, 32);
            AssertDeclType("NativeInt", KnownTypeIds.NativeInt, NativeIntSize.All32bit, 32);
            AssertDeclType("NativeUInt", KnownTypeIds.NativeUInt, NativeIntSize.All32bit, 32);
            AssertDeclType("LongInt", KnownTypeIds.LongInt, NativeIntSize.All32bit, 32);
            AssertDeclType("LongWord", KnownTypeIds.LongWord, NativeIntSize.All32bit, 32);

            AssertDeclType("System.NativeInt", KnownTypeIds.NativeInt, NativeIntSize.All64bit, 64);
            AssertDeclType("System.NativeUInt", KnownTypeIds.NativeUInt, NativeIntSize.All64bit, 64);
            AssertDeclType("System.LongInt", KnownTypeIds.LongInt, NativeIntSize.All64bit, 64);
            AssertDeclType("System.LongWord", KnownTypeIds.LongWord, NativeIntSize.All64bit, 64);
            AssertDeclType("NativeInt", KnownTypeIds.NativeInt, NativeIntSize.All64bit, 64);
            AssertDeclType("NativeUInt", KnownTypeIds.NativeUInt, NativeIntSize.All64bit, 64);
            AssertDeclType("LongInt", KnownTypeIds.LongInt, NativeIntSize.All64bit, 64);
            AssertDeclType("LongWord", KnownTypeIds.LongWord, NativeIntSize.All64bit, 64);

            AssertDeclType("System.NativeInt", KnownTypeIds.NativeInt, NativeIntSize.Windows64bit, 64);
            AssertDeclType("System.NativeUInt", KnownTypeIds.NativeUInt, NativeIntSize.Windows64bit, 64);
            AssertDeclType("System.LongInt", KnownTypeIds.LongInt, NativeIntSize.Windows64bit, 32);
            AssertDeclType("System.LongWord", KnownTypeIds.LongWord, NativeIntSize.Windows64bit, 32);
            AssertDeclType("NativeInt", KnownTypeIds.NativeInt, NativeIntSize.Windows64bit, 64);
            AssertDeclType("NativeUInt", KnownTypeIds.NativeUInt, NativeIntSize.Windows64bit, 64);
            AssertDeclType("LongInt", KnownTypeIds.LongInt, NativeIntSize.Windows64bit, 32);
            AssertDeclType("LongWord", KnownTypeIds.LongWord, NativeIntSize.Windows64bit, 32);
        }

        [Fact]
        public void TestStringTypes() {
            AssertDeclType("String", KnownTypeIds.StringType);
            AssertDeclType("String[324]", typeKind: CommonTypeKind.ShortStringType);
            AssertDeclType("AnsiString", KnownTypeIds.AnsiStringType);
            AssertDeclType("UnicodeString", KnownTypeIds.UnicodeStringType);
            AssertDeclType("WideString", KnownTypeIds.WideStringType);
            AssertDeclType("ShortString", KnownTypeIds.ShortStringType);
            AssertDeclType("RawByteString", KnownTypeIds.RawByteString);
            AssertDeclType("System.String", KnownTypeIds.StringType);
            AssertDeclType("System.AnsiString", KnownTypeIds.AnsiStringType);
            AssertDeclType("System.UnicodeString", KnownTypeIds.UnicodeStringType);
            AssertDeclType("System.WideString", KnownTypeIds.WideStringType);
            AssertDeclType("System.ShortString", KnownTypeIds.ShortStringType);
            AssertDeclType("System.RawByteString", KnownTypeIds.RawByteString);
        }

        [Fact]
        public void TestRealTypes() {
            AssertDeclType("Real48", KnownTypeIds.Real48Type);
            AssertDeclType("Single", KnownTypeIds.SingleType);
            AssertDeclType("Double", KnownTypeIds.Double);
            AssertDeclType("Real", KnownTypeIds.Real);
            AssertDeclType("Extended", KnownTypeIds.Extended);
            AssertDeclType("Comp", KnownTypeIds.Comp);
            AssertDeclType("Currency", KnownTypeIds.Currency);
            AssertDeclType("System.Real48", KnownTypeIds.Real48Type);
            AssertDeclType("System.Single", KnownTypeIds.SingleType);
            AssertDeclType("System.Double", KnownTypeIds.Double);
            AssertDeclType("System.Real", KnownTypeIds.Real);
            AssertDeclType("System.Extended", KnownTypeIds.Extended);
            AssertDeclType("System.Comp", KnownTypeIds.Comp);
            AssertDeclType("System.Currency", KnownTypeIds.Currency);
        }

    }
}
