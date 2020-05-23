using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Globals.Types;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test for built in types
    /// </summary>
    public class BuiltInTypesTest : TypeTest {

        private ISystemUnit KnownTypeIds
           => CreateEnvironment().TypeRegistry.SystemUnit;

        /// <summary>
        ///     test integer types
        /// </summary>
        [TestMethod]
        public void TestIntTypes() {
            AssertDeclType("System.Byte", KnownTypeIds.ByteType);
            AssertDeclType("System.Word", KnownTypeIds.WordType);
            AssertDeclType("System.Cardinal", KnownTypeIds.CardinalType);
            AssertDeclType("System.UInt64", KnownTypeIds.UInt64Type);
            AssertDeclType("System.ShortInt", KnownTypeIds.ShortIntType);
            AssertDeclType("System.SmallInt", KnownTypeIds.SmallIntType);
            AssertDeclType("System.Integer", KnownTypeIds.IntegerType);
            AssertDeclType("System.Int64", KnownTypeIds.Int64Type);

            AssertDeclType("Byte", KnownTypeIds.ByteType);
            AssertDeclType("Word", KnownTypeIds.WordType);
            AssertDeclType("Cardinal", KnownTypeIds.CardinalType);
            AssertDeclType("UInt64", KnownTypeIds.UInt64Type);
            AssertDeclType("ShortInt", KnownTypeIds.ShortIntType);
            AssertDeclType("SmallInt", KnownTypeIds.SmallIntType);
            AssertDeclType("Integer", KnownTypeIds.IntegerType);
            AssertDeclType("Int64", KnownTypeIds.Int64Type);
        }

        /// <summary>
        ///     test character types
        /// </summary>
        [TestMethod]
        public void TestCharTypes() {
            AssertDeclType("System.Char", KnownTypeIds.WideCharType);
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

        /// <summary>
        ///     test boolean types
        /// </summary>
        [TestMethod]
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

        /// <summary>
        ///     test object types
        /// </summary>
        [TestMethod(Skip = "broken")]
        public void TestTObjectType() {
            //Func<StructuredTypeDeclaration, bool> hasMethod(string q)
            //    => t => t?.HasMethod(q) ?? false;

            //AssertDeclType("TObject", KnownTypeIds.TObject);
            //AssertDeclType("System.TObject", KnownTypeIds.TObject);
            //AssertDeclTypeDef("TObject", hasMethod("Create"));
            //AssertDeclTypeDef("TObject", hasMethod("Free"));
            //AssertDeclTypeDef("TObject", hasMethod("DisposeOf"));
            //AssertDeclTypeDef("TObject", hasMethod("CleanupInstance"));
            //AssertDeclTypeDef("TObject", hasMethod("ClassType"));
            //AssertDeclTypeDef("TObject", hasMethod("FieldAddress"));
        }

        /// <summary>
        ///     test pointer type
        /// </summary>
        [TestMethod]
        public void TestPointerTypes() {
            AssertDeclType("Pointer", KnownTypeIds.GenericPointerType);
            AssertDeclType("PByte", KnownTypeIds.PByteType);
            AssertDeclType("PWord", KnownTypeIds.PWordType);
            AssertDeclType("PCardinal", KnownTypeIds.PCardinalType);
            AssertDeclType("PUInt64", KnownTypeIds.PUInt64Type);
            AssertDeclType("PShortInt", KnownTypeIds.PShortIntType);
            AssertDeclType("PSmallInt", KnownTypeIds.PSmallIntType);
            AssertDeclType("PInteger", KnownTypeIds.PIntegerType);
            AssertDeclType("PInt64", KnownTypeIds.PInt64Type);
            AssertDeclType("PSingle", KnownTypeIds.PSingleType);
            AssertDeclType("PDouble", KnownTypeIds.PDoubleType);
            AssertDeclType("PExtended", KnownTypeIds.PExtendedType);
            AssertDeclType("PAnsiChar", KnownTypeIds.PAnsiCharType);
            AssertDeclType("PWideChar", KnownTypeIds.PWideCharType);
            AssertDeclType("PAnsiString", KnownTypeIds.PAnsiStringType);
            AssertDeclType("PRawByteString", KnownTypeIds.PRawByteStringType);
            AssertDeclType("PUnicodeString", KnownTypeIds.PUnicodeStringType);
            AssertDeclType("PShortString", KnownTypeIds.PShortStringType);
            AssertDeclType("PWideString", KnownTypeIds.PWideStringType);
            AssertDeclType("PChar", KnownTypeIds.PCharType);
            AssertDeclType("PString", KnownTypeIds.PStringType);
            AssertDeclType("PBoolean", KnownTypeIds.PBooleanType);
            AssertDeclType("PLongBool", KnownTypeIds.PLongBoolType);
            AssertDeclType("PWordBool", KnownTypeIds.PWordBoolType);
            AssertDeclType("PPointer", KnownTypeIds.PPointer);
            AssertDeclType("PCurrency", KnownTypeIds.PCurrency);
            AssertDeclType("System.Pointer", KnownTypeIds.GenericPointerType);
            AssertDeclType("System.PByte", KnownTypeIds.PByteType);
            AssertDeclType("System.PWord", KnownTypeIds.PWordType);
            AssertDeclType("System.PCardinal", KnownTypeIds.PCardinalType);
            AssertDeclType("System.PUInt64", KnownTypeIds.PUInt64Type);
            AssertDeclType("System.PShortInt", KnownTypeIds.PShortIntType);
            AssertDeclType("System.PSmallInt", KnownTypeIds.PSmallIntType);
            AssertDeclType("System.PInteger", KnownTypeIds.PIntegerType);
            AssertDeclType("System.PInt64", KnownTypeIds.PInt64Type);
            AssertDeclType("System.PSingle", KnownTypeIds.PSingleType);
            AssertDeclType("System.PDouble", KnownTypeIds.PDoubleType);
            AssertDeclType("System.PExtended", KnownTypeIds.PExtendedType);
            AssertDeclType("System.PAnsiChar", KnownTypeIds.PAnsiCharType);
            AssertDeclType("System.PWideChar", KnownTypeIds.PWideCharType);
            AssertDeclType("System.PAnsiString", KnownTypeIds.PAnsiStringType);
            AssertDeclType("System.PRawByteString", KnownTypeIds.PRawByteStringType);
            AssertDeclType("System.PUnicodeString", KnownTypeIds.PUnicodeStringType);
            AssertDeclType("System.PShortString", KnownTypeIds.PShortStringType);
            AssertDeclType("System.PWideString", KnownTypeIds.PWideStringType);
            AssertDeclType("System.PChar", KnownTypeIds.PCharType);
            AssertDeclType("System.PString", KnownTypeIds.PStringType);
            AssertDeclType("System.PBoolean", KnownTypeIds.PBooleanType);
            AssertDeclType("System.PLongBool", KnownTypeIds.PLongBoolType);
            AssertDeclType("System.PWordBool", KnownTypeIds.PWordBoolType);
            AssertDeclType("System.PPointer", KnownTypeIds.PPointer);
            AssertDeclType("System.PCurrency", KnownTypeIds.PCurrency);
        }

        /// <summary>
        ///     test native int types
        /// </summary>
        [TestMethod]
        public void TestNativeIntTypes() {
            AssertDeclType("System.NativeInt", KnownTypeIds.NativeIntType, NativeIntSize.All32bit, 32);
            AssertDeclType("System.NativeUInt", KnownTypeIds.NativeUIntType, NativeIntSize.All32bit, 32);
            AssertDeclType("System.LongInt", KnownTypeIds.LongIntType, NativeIntSize.All32bit, 32);
            AssertDeclType("System.LongWord", KnownTypeIds.PLongwordType, NativeIntSize.All32bit, 32);
            AssertDeclType("NativeInt", KnownTypeIds.NativeIntType, NativeIntSize.All32bit, 32);
            AssertDeclType("NativeUInt", KnownTypeIds.NativeUIntType, NativeIntSize.All32bit, 32);
            AssertDeclType("LongInt", KnownTypeIds.LongIntType, NativeIntSize.All32bit, 32);
            AssertDeclType("LongWord", KnownTypeIds.LongWordType, NativeIntSize.All32bit, 32);

            AssertDeclType("System.NativeInt", KnownTypeIds.NativeIntType, NativeIntSize.All64bit, 64);
            AssertDeclType("System.NativeUInt", KnownTypeIds.NativeUIntType, NativeIntSize.All64bit, 64);
            AssertDeclType("System.LongInt", KnownTypeIds.LongIntType, NativeIntSize.All64bit, 64);
            AssertDeclType("System.LongWord", KnownTypeIds.LongWordType, NativeIntSize.All64bit, 64);
            AssertDeclType("NativeInt", KnownTypeIds.NativeIntType, NativeIntSize.All64bit, 64);
            AssertDeclType("NativeUInt", KnownTypeIds.NativeUIntType, NativeIntSize.All64bit, 64);
            AssertDeclType("LongInt", KnownTypeIds.LongIntType, NativeIntSize.All64bit, 64);
            AssertDeclType("LongWord", KnownTypeIds.LongWordType, NativeIntSize.All64bit, 64);

            AssertDeclType("System.NativeInt", KnownTypeIds.NativeIntType, NativeIntSize.Windows64bit, 64);
            AssertDeclType("System.NativeUInt", KnownTypeIds.NativeUIntType, NativeIntSize.Windows64bit, 64);
            AssertDeclType("System.LongInt", KnownTypeIds.LongIntType, NativeIntSize.Windows64bit, 32);
            AssertDeclType("System.LongWord", KnownTypeIds.LongWordType, NativeIntSize.Windows64bit, 32);
            AssertDeclType("NativeInt", KnownTypeIds.NativeIntType, NativeIntSize.Windows64bit, 64);
            AssertDeclType("NativeUInt", KnownTypeIds.NativeUIntType, NativeIntSize.Windows64bit, 64);
            AssertDeclType("LongInt", KnownTypeIds.LongIntType, NativeIntSize.Windows64bit, 32);
            AssertDeclType("LongWord", KnownTypeIds.LongWordType, NativeIntSize.Windows64bit, 32);
        }

        /// <summary>
        ///     test string types
        /// </summary>
        [TestMethod]
        public void TestStringTypes() {
            AssertDeclType("String", KnownTypeIds.StringType);
            AssertDeclType("String[324]", KnownTypeIds.StringType);
            AssertDeclType("AnsiString", KnownTypeIds.AnsiStringType);
            AssertDeclType("UnicodeString", KnownTypeIds.UnicodeStringType);
            AssertDeclType("WideString", KnownTypeIds.WideStringType);
            AssertDeclType("ShortString", KnownTypeIds.ShortStringType);
            AssertDeclType("RawByteString", KnownTypeIds.RawByteStringType);
            AssertDeclType("System.String", KnownTypeIds.StringType);
            AssertDeclType("System.AnsiString", KnownTypeIds.AnsiStringType);
            AssertDeclType("System.UnicodeString", KnownTypeIds.UnicodeStringType);
            AssertDeclType("System.WideString", KnownTypeIds.WideStringType);
            AssertDeclType("System.ShortString", KnownTypeIds.ShortStringType);
            AssertDeclType("System.RawByteString", KnownTypeIds.RawByteStringType);
        }

        /// <summary>
        ///     test real types
        /// </summary>
        [TestMethod]
        public void TestRealTypes() {
            AssertDeclType("Real48", KnownTypeIds.Real48Type);
            AssertDeclType("Single", KnownTypeIds.SingleType);
            AssertDeclType("Double", KnownTypeIds.DoubleType);
            AssertDeclType("Real", KnownTypeIds.RealType);
            AssertDeclType("Extended", KnownTypeIds.ExtendedType);
            AssertDeclType("Comp", KnownTypeIds.CompType);
            AssertDeclType("Currency", KnownTypeIds.CurrencyType);
            AssertDeclType("System.Real48", KnownTypeIds.Real48Type);
            AssertDeclType("System.Single", KnownTypeIds.SingleType);
            AssertDeclType("System.Double", KnownTypeIds.DoubleType);
            AssertDeclType("System.Real", KnownTypeIds.RealType);
            AssertDeclType("System.Extended", KnownTypeIds.ExtendedType);
            AssertDeclType("System.Comp", KnownTypeIds.CompType);
            AssertDeclType("System.Currency", KnownTypeIds.CurrencyType);
        }

        /// <summary>
        ///     test array types
        /// </summary>
        [TestMethod]
        public void TestArrayTypes() {
            var e = CreateEnvironment();
            var tc = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var a1 = tc.CreateDynamicArrayType(e.TypeRegistry.SystemUnit.IntegerType, string.Empty, false);
            var a2 = tc.CreateDynamicArrayType(e.TypeRegistry.SystemUnit.StringType, string.Empty, false);
            AssertDeclType("TArray<Integer>", a1, typeKind: BaseType.Array);
            AssertDeclType("TArray<String>", a2, typeKind: BaseType.Array);
        }

    }
}
