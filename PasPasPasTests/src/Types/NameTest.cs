namespace PasPasPasTests.Types {
    using PasPasPas.Globals.Types;
    using PasPasPasTests.Common;

    /// <summary>
    ///     test names of types
    /// </summary>
    public class NameTest : TypeTest {

        private ISystemUnit KTI =>
            CreateEnvironment().TypeRegistry.SystemUnit;

        (string, string) GetInternalTypeName(ITypeDefinition type) {
            return (type.Name, type.MangledName);
        }


        /// <summary>
        ///     test platform dependent types
        /// </summary>
        [TestMethod]
        public void TestPlatformDependentIntegerTypes() {
            (string, string) g(ITypeDefinition id) => GetInternalTypeName(id);
            Assert.AreEqual(("FixedInt", "i"), g(KTI.FixedIntType));
            Assert.AreEqual(("FixedUInt", "ui"), g(KTI.FixedUIntType));
            Assert.AreEqual(("NativeInt", "i"), g(KTI.NativeIntType));
            Assert.AreEqual(("NativeUInt", "ui"), g(KTI.NativeUIntType));
            Assert.AreEqual(("LongInt", "i"), g(KTI.LongIntType));
            Assert.AreEqual(("LongWord", "ui"), g(KTI.LongWordType));
        }

        /// <summary>
        ///     test names for real numeric types
        /// </summary>
        [TestMethod]
        public void TestRealTypes() {
            (string, string) g(ITypeDefinition id) => GetInternalTypeName(id);
            Assert.AreEqual(("Real48", "6real48"), g(KTI.Real48Type));
            Assert.AreEqual(("Single", "f"), g(KTI.SingleType));
            Assert.AreEqual(("Double", "d"), g(KTI.DoubleType));
            Assert.AreEqual(("Extended", "g"), g(KTI.ExtendedType));
            Assert.AreEqual(("Comp", "System@Comp"), g(KTI.CompType));
            Assert.AreEqual(("Currency", "System@Currency"), g(KTI.CurrencyType));
        }

        /// <summary>
        ///     test names for string numeric types
        /// </summary>
        [TestMethod]
        public void TestStringTypes() {
            (string, string) g(ITypeDefinition id) => GetInternalTypeName(id);
            Assert.AreEqual(("AnsiChar", "c"), g(KTI.AnsiCharType));
            Assert.AreEqual(("WideChar", "b"), g(KTI.WideCharType));
            Assert.AreEqual(("AnsiString", "%AnsiStringT$us$i0$%"), g(KTI.AnsiStringType));
            Assert.AreEqual(("RawByteString", "%AnsiStringT$us$i65535$%"), g(KTI.RawByteStringType));
            Assert.AreEqual(("ShortString", "System@%SmallString$uc$i255$%"), g(KTI.ShortStringType));
            Assert.AreEqual(("WideString", "System@WideString"), g(KTI.WideStringType));
            Assert.AreEqual(("UnicodeString", "System@UnicodeString"), g(KTI.UnicodeStringType));
        }

        /// <summary>
        ///     test platform independent types
        /// </summary>
        [TestMethod]
        public void TestPlatformInDependentIntegerTypes() {
            (string, string) g(ITypeDefinition id) => GetInternalTypeName(id);
            Assert.AreEqual(("ShortInt", "zc"), g(KTI.ShortIntType));
            Assert.AreEqual(("Byte", "uc"), g(KTI.ByteType));
            Assert.AreEqual(("SmallInt", "s"), g(KTI.SmallIntType));
            Assert.AreEqual(("Word", "us"), g(KTI.WordType));
            Assert.AreEqual(("Integer", "i"), g(KTI.IntegerType));
            Assert.AreEqual(("Cardinal", "ui"), g(KTI.CardinalType));
            Assert.AreEqual(("Int64", "j"), g(KTI.Int64Type));
            Assert.AreEqual(("UInt64", "uj"), g(KTI.UInt64Type));
            Assert.AreEqual(("Int8", "zc"), g(KTI.Int8Type));
            Assert.AreEqual(("UInt8", "uc"), g(KTI.UInt8Type));
            Assert.AreEqual(("Int16", "s"), g(KTI.Int16Type));
            Assert.AreEqual(("UInt16", "us"), g(KTI.UInt16Type));
            Assert.AreEqual(("Int32", "i"), g(KTI.Int32Type));
            Assert.AreEqual(("UInt32", "ui"), g(KTI.UInt32Type));
        }

        /// <summary>
        ///     test platform dependent types
        /// </summary>
        [TestMethod]
        public void TestBooleanTypes() {
            (string, string) g(ITypeDefinition id) => GetInternalTypeName(id);
            Assert.AreEqual(("Boolean", "o"), g(KTI.BooleanType));
            Assert.AreEqual(("ByteBool", "uc"), g(KTI.ByteBoolType));
            Assert.AreEqual(("WordBool", "us"), g(KTI.WordBoolType));
            Assert.AreEqual(("LongBool", "i"), g(KTI.LongBoolType));
        }

        /// <summary>
        ///     test pointer names
        /// </summary>
        [TestMethod]
        public void TestPointerTypes() {
            (string, string) g(ITypeDefinition id) => GetInternalTypeName(id);
            Assert.AreEqual(("Pointer", "pv"), g(KTI.GenericPointerType));
            Assert.AreEqual(("PByte", "puc"), g(KTI.PByteType));
            Assert.AreEqual(("PShortInt", "pzc"), g(KTI.PShortIntType));
            Assert.AreEqual(("PWord", "pus"), g(KTI.PWordType));
            Assert.AreEqual(("PSmallInt", "ps"), g(KTI.PSmallIntType));
            Assert.AreEqual(("PCardinal", "pui"), g(KTI.PCardinalType));
            Assert.AreEqual(("PLongword", "pui"), g(KTI.PLongwordType));
            Assert.AreEqual(("PFixedUInt", "pui"), g(KTI.PFixedUIntType));
            Assert.AreEqual(("PInteger", "pi"), g(KTI.PIntegerType));
            Assert.AreEqual(("PLongInt", "pi"), g(KTI.PLongIntType));
            Assert.AreEqual(("PFixedInt", "pi"), g(KTI.PFixedIntType));
            Assert.AreEqual(("PUInt64", "puj"), g(KTI.PUInt64Type));
            Assert.AreEqual(("PInt64", "pj"), g(KTI.PInt64Type));
            Assert.AreEqual(("PNativeInt", "pi"), g(KTI.PNativeIntType));
            Assert.AreEqual(("PNativeUInt", "pui"), g(KTI.PNativeUIntType));
            Assert.AreEqual(("PAnsiChar", "pc"), g(KTI.PAnsiCharType));
            Assert.AreEqual(("PWideChar", "pb"), g(KTI.PWideCharType));
            Assert.AreEqual(("PBoolean", "po"), g(KTI.PBooleanType));
            Assert.AreEqual(("PWordBool", "pus"), g(KTI.PWordBoolType));
            Assert.AreEqual(("PLongBool", "pi"), g(KTI.PLongBoolType));
            Assert.AreEqual(("PPointer", "ppv"), g(KTI.PPointerType));
            Assert.AreEqual(("PCurrency", "pSystem@Currency"), g(KTI.PCurrencyType));
            Assert.AreEqual(("PWideString", "pSystem@WideString"), g(KTI.PWideStringType));
            Assert.AreEqual(("PUnicodeString", "pSystem@UnicodeString"), g(KTI.PUnicodeStringType));
            Assert.AreEqual(("PShortString", "pSystem@%SmallString$uc$i255$%"), g(KTI.PShortStringType));
            Assert.AreEqual(("PRawByteString", "p%AnsiStringT$us$i65535$%"), g(KTI.PRawByteStringType));
        }

    }
}
