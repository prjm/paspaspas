using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Globals.Types;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test for built in types
    /// </summary>
    public class BuiltInTypesTest : TypeTest {

        private ISystemUnit SystemUnit
           => CreateEnvironment().TypeRegistry.SystemUnit;

        private ISystemUnit SystemUnit64
           => CreateEnvironment(NativeIntSize.All64bit).TypeRegistry.SystemUnit;


        /// <summary>
        ///     test integer types
        /// </summary>
        [TestMethod]
        public void TestIntTypes() {
            AssertDeclType("System.Byte", SystemUnit.ByteType);
            AssertDeclType("System.Word", SystemUnit.WordType);
            AssertDeclType("System.Cardinal", SystemUnit.CardinalType);
            AssertDeclType("System.UInt64", SystemUnit.UInt64Type);
            AssertDeclType("System.ShortInt", SystemUnit.ShortIntType);
            AssertDeclType("System.SmallInt", SystemUnit.SmallIntType);
            AssertDeclType("System.Integer", SystemUnit.IntegerType);
            AssertDeclType("System.Int64", SystemUnit.Int64Type);

            AssertDeclType("Byte", SystemUnit.ByteType);
            AssertDeclType("Word", SystemUnit.WordType);
            AssertDeclType("Cardinal", SystemUnit.CardinalType);
            AssertDeclType("UInt64", SystemUnit.UInt64Type);
            AssertDeclType("ShortInt", SystemUnit.ShortIntType);
            AssertDeclType("SmallInt", SystemUnit.SmallIntType);
            AssertDeclType("Integer", SystemUnit.IntegerType);
            AssertDeclType("Int64", SystemUnit.Int64Type);
        }

        /// <summary>
        ///     test character types
        /// </summary>
        [TestMethod]
        public void TestCharTypes() {
            AssertDeclType("System.Char", SystemUnit.CharType);
            AssertDeclType("System.AnsiChar", SystemUnit.AnsiCharType);
            AssertDeclType("System.WideChar", SystemUnit.WideCharType);
            AssertDeclType("System.UCS2Char", SystemUnit.Ucs2CharType);
            AssertDeclType("System.UCS4Char", SystemUnit.Ucs4CharType);

            AssertDeclType("Char", SystemUnit.CharType);
            AssertDeclType("AnsiChar", SystemUnit.AnsiCharType);
            AssertDeclType("WideChar", SystemUnit.WideCharType);
            AssertDeclType("UCS2Char", SystemUnit.Ucs2CharType);
            AssertDeclType("UCS4Char", SystemUnit.Ucs4CharType);
        }

        /// <summary>
        ///     test boolean types
        /// </summary>
        [TestMethod]
        public void TestBooleanTypes() {
            AssertDeclType("Boolean", SystemUnit.BooleanType);
            AssertDeclType("ByteBool", SystemUnit.ByteBoolType);
            AssertDeclType("WordBool", SystemUnit.WordBoolType);
            AssertDeclType("LongBool", SystemUnit.LongBoolType);

            AssertDeclType("System.Boolean", SystemUnit.BooleanType);
            AssertDeclType("System.ByteBool", SystemUnit.ByteBoolType);
            AssertDeclType("System.WordBool", SystemUnit.WordBoolType);
            AssertDeclType("System.LongBool", SystemUnit.LongBoolType);
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
            AssertDeclType("Pointer", SystemUnit.GenericPointerType);
            AssertDeclType("PByte", SystemUnit.PByteType);
            AssertDeclType("PWord", SystemUnit.PWordType);
            AssertDeclType("PCardinal", SystemUnit.PCardinalType);
            AssertDeclType("PUInt64", SystemUnit.PUInt64Type);
            AssertDeclType("PShortInt", SystemUnit.PShortIntType);
            AssertDeclType("PSmallInt", SystemUnit.PSmallIntType);
            AssertDeclType("PInteger", SystemUnit.PIntegerType);
            AssertDeclType("PInt64", SystemUnit.PInt64Type);
            AssertDeclType("PSingle", SystemUnit.PSingleType);
            AssertDeclType("PDouble", SystemUnit.PDoubleType);
            AssertDeclType("PExtended", SystemUnit.PExtendedType);
            AssertDeclType("PAnsiChar", SystemUnit.PAnsiCharType);
            AssertDeclType("PWideChar", SystemUnit.PWideCharType);
            AssertDeclType("PAnsiString", SystemUnit.PAnsiStringType);
            AssertDeclType("PRawByteString", SystemUnit.PRawByteStringType);
            AssertDeclType("PUnicodeString", SystemUnit.PUnicodeStringType);
            AssertDeclType("PShortString", SystemUnit.PShortStringType);
            AssertDeclType("PWideString", SystemUnit.PWideStringType);
            AssertDeclType("PChar", SystemUnit.PCharType);
            AssertDeclType("PString", SystemUnit.PStringType);
            AssertDeclType("PBoolean", SystemUnit.PBooleanType);
            AssertDeclType("PLongBool", SystemUnit.PLongBoolType);
            AssertDeclType("PWordBool", SystemUnit.PWordBoolType);
            AssertDeclType("PPointer", SystemUnit.PPointerType);
            AssertDeclType("PCurrency", SystemUnit.PCurrencyType);
            AssertDeclType("System.Pointer", SystemUnit.GenericPointerType);
            AssertDeclType("System.PByte", SystemUnit.PByteType);
            AssertDeclType("System.PWord", SystemUnit.PWordType);
            AssertDeclType("System.PCardinal", SystemUnit.PCardinalType);
            AssertDeclType("System.PUInt64", SystemUnit.PUInt64Type);
            AssertDeclType("System.PShortInt", SystemUnit.PShortIntType);
            AssertDeclType("System.PSmallInt", SystemUnit.PSmallIntType);
            AssertDeclType("System.PInteger", SystemUnit.PIntegerType);
            AssertDeclType("System.PInt64", SystemUnit.PInt64Type);
            AssertDeclType("System.PSingle", SystemUnit.PSingleType);
            AssertDeclType("System.PDouble", SystemUnit.PDoubleType);
            AssertDeclType("System.PExtended", SystemUnit.PExtendedType);
            AssertDeclType("System.PAnsiChar", SystemUnit.PAnsiCharType);
            AssertDeclType("System.PWideChar", SystemUnit.PWideCharType);
            AssertDeclType("System.PAnsiString", SystemUnit.PAnsiStringType);
            AssertDeclType("System.PRawByteString", SystemUnit.PRawByteStringType);
            AssertDeclType("System.PUnicodeString", SystemUnit.PUnicodeStringType);
            AssertDeclType("System.PShortString", SystemUnit.PShortStringType);
            AssertDeclType("System.PWideString", SystemUnit.PWideStringType);
            AssertDeclType("System.PChar", SystemUnit.PCharType);
            AssertDeclType("System.PString", SystemUnit.PStringType);
            AssertDeclType("System.PBoolean", SystemUnit.PBooleanType);
            AssertDeclType("System.PLongBool", SystemUnit.PLongBoolType);
            AssertDeclType("System.PWordBool", SystemUnit.PWordBoolType);
            AssertDeclType("System.PPointer", SystemUnit.PPointerType);
            AssertDeclType("System.PCurrency", SystemUnit.PCurrencyType);
        }

        /// <summary>
        ///     test native int types
        /// </summary>
        [TestMethod]
        public void TestNativeIntTypes() {
            AssertDeclType("System.NativeInt", SystemUnit.NativeIntType, NativeIntSize.All32bit, 4);
            AssertDeclType("System.NativeUInt", SystemUnit.NativeUIntType, NativeIntSize.All32bit, 4);
            AssertDeclType("System.LongInt", SystemUnit.LongIntType, NativeIntSize.All32bit, 4);
            AssertDeclType("System.LongWord", SystemUnit.LongWordType, NativeIntSize.All32bit, 4);
            AssertDeclType("NativeInt", SystemUnit.NativeIntType, NativeIntSize.All32bit, 4);
            AssertDeclType("NativeUInt", SystemUnit.NativeUIntType, NativeIntSize.All32bit, 4);
            AssertDeclType("LongInt", SystemUnit.LongIntType, NativeIntSize.All32bit, 4);
            AssertDeclType("LongWord", SystemUnit.LongWordType, NativeIntSize.All32bit, 4);

            AssertDeclType("System.NativeInt", SystemUnit64.NativeIntType, NativeIntSize.All64bit, 8);
            AssertDeclType("System.NativeUInt", SystemUnit64.NativeUIntType, NativeIntSize.All64bit, 8);
            AssertDeclType("System.LongInt", SystemUnit64.LongIntType, NativeIntSize.All64bit, 8);
            AssertDeclType("System.LongWord", SystemUnit64.LongWordType, NativeIntSize.All64bit, 8);
            AssertDeclType("NativeInt", SystemUnit64.NativeIntType, NativeIntSize.All64bit, 8);
            AssertDeclType("NativeUInt", SystemUnit64.NativeUIntType, NativeIntSize.All64bit, 8);
            AssertDeclType("LongInt", SystemUnit64.LongIntType, NativeIntSize.All64bit, 8);
            AssertDeclType("LongWord", SystemUnit64.LongWordType, NativeIntSize.All64bit, 8);

            AssertDeclType("System.NativeInt", SystemUnit64.NativeIntType, NativeIntSize.Windows64bit, 8);
            AssertDeclType("System.NativeUInt", SystemUnit64.NativeUIntType, NativeIntSize.Windows64bit, 8);
            AssertDeclType("System.LongInt", SystemUnit.LongIntType, NativeIntSize.Windows64bit, 4);
            AssertDeclType("System.LongWord", SystemUnit.LongWordType, NativeIntSize.Windows64bit, 4);
            AssertDeclType("NativeInt", SystemUnit64.NativeIntType, NativeIntSize.Windows64bit, 8);
            AssertDeclType("NativeUInt", SystemUnit64.NativeUIntType, NativeIntSize.Windows64bit, 8);
            AssertDeclType("LongInt", SystemUnit.LongIntType, NativeIntSize.Windows64bit, 4);
            AssertDeclType("LongWord", SystemUnit.LongWordType, NativeIntSize.Windows64bit, 4);
        }

        /// <summary>
        ///     test string types
        /// </summary>
        [TestMethod]
        public void TestStringTypes() {
            AssertDeclType("String", SystemUnit.StringType);
            AssertDeclType("String[324]", SystemUnit.ErrorType);
            AssertDeclType("String[80]", GetShortStringType(80));
            AssertDeclType("AnsiString", SystemUnit.AnsiStringType);
            AssertDeclType("UnicodeString", SystemUnit.UnicodeStringType);
            AssertDeclType("WideString", SystemUnit.WideStringType);
            AssertDeclType("ShortString", SystemUnit.ShortStringType);
            AssertDeclType("RawByteString", SystemUnit.RawByteStringType);
            AssertDeclType("System.String", SystemUnit.StringType);
            AssertDeclType("System.AnsiString", SystemUnit.AnsiStringType);
            AssertDeclType("System.UnicodeString", SystemUnit.UnicodeStringType);
            AssertDeclType("System.WideString", SystemUnit.WideStringType);
            AssertDeclType("System.ShortString", SystemUnit.ShortStringType);
            AssertDeclType("System.RawByteString", SystemUnit.RawByteStringType);
        }

        /// <summary>
        ///     test real types
        /// </summary>
        [TestMethod]
        public void TestRealTypes() {
            AssertDeclType("Real48", SystemUnit.Real48Type);
            AssertDeclType("Single", SystemUnit.SingleType);
            AssertDeclType("Double", SystemUnit.DoubleType);
            AssertDeclType("Real", SystemUnit.RealType);
            AssertDeclType("Extended", SystemUnit.ExtendedType);
            AssertDeclType("Comp", SystemUnit.CompType);
            AssertDeclType("Currency", SystemUnit.CurrencyType);
            AssertDeclType("System.Real48", SystemUnit.Real48Type);
            AssertDeclType("System.Single", SystemUnit.SingleType);
            AssertDeclType("System.Double", SystemUnit.DoubleType);
            AssertDeclType("System.Real", SystemUnit.RealType);
            AssertDeclType("System.Extended", SystemUnit.ExtendedType);
            AssertDeclType("System.Comp", SystemUnit.CompType);
            AssertDeclType("System.Currency", SystemUnit.CurrencyType);
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
