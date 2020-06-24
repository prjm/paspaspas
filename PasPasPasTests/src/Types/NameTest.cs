namespace PasPasPasTests.Types {
    using PasPasPas.Globals.Types;
    using PasPasPasTests.Common;

    /// <summary>
    ///     test names of types
    /// </summary>
    public class NameTest : TypeTest {

        private ISystemUnit SystemUnit =>
            CreateEnvironment().TypeRegistry.SystemUnit;

        (string name, string mangledName) GetInternalTypeName(ITypeDefinition type)
            => (type.Name, type.MangledName);


        /// <summary>
        ///     test platform dependent types
        /// </summary>
        [TestMethod]
        public void TestPlatformDependentIntegerTypes() {
            (string, string) g(ITypeDefinition id) => GetInternalTypeName(id);
            Assert.AreEqual(("FixedInt", "i"), g(SystemUnit.FixedIntType));
            Assert.AreEqual(("FixedUInt", "ui"), g(SystemUnit.FixedUIntType));
            Assert.AreEqual(("NativeInt", "i"), g(SystemUnit.NativeIntType));
            Assert.AreEqual(("NativeUInt", "ui"), g(SystemUnit.NativeUIntType));
            Assert.AreEqual(("LongInt", "i"), g(SystemUnit.LongIntType));
            Assert.AreEqual(("LongWord", "ui"), g(SystemUnit.LongWordType));
        }

        /// <summary>
        ///     test names for real numeric types
        /// </summary>
        [TestMethod]
        public void TestRealTypes() {
            (string, string) g(ITypeDefinition id) => GetInternalTypeName(id);
            Assert.AreEqual(("Real48", "6real48"), g(SystemUnit.Real48Type));
            Assert.AreEqual(("Single", "f"), g(SystemUnit.SingleType));
            Assert.AreEqual(("Double", "d"), g(SystemUnit.DoubleType));
            Assert.AreEqual(("Extended", "g"), g(SystemUnit.ExtendedType));
            Assert.AreEqual(("Comp", "System@Comp"), g(SystemUnit.CompType));
            Assert.AreEqual(("Currency", "System@Currency"), g(SystemUnit.CurrencyType));
        }

        /// <summary>
        ///     test names for string numeric types
        /// </summary>
        [TestMethod]
        public void TestStringTypes() {
            (string, string) g(ITypeDefinition id) => GetInternalTypeName(id);
            Assert.AreEqual(("AnsiChar", "c"), g(SystemUnit.AnsiCharType));
            Assert.AreEqual(("WideChar", "b"), g(SystemUnit.WideCharType));
            Assert.AreEqual(("AnsiString", "%AnsiStringT$us$i0$%"), g(SystemUnit.AnsiStringType));
            Assert.AreEqual(("RawByteString", "%AnsiStringT$us$i65535$%"), g(SystemUnit.RawByteStringType));
            Assert.AreEqual(("ShortString", "System@%SmallString$uc$i255$%"), g(SystemUnit.ShortStringType));
            Assert.AreEqual(("WideString", "System@WideString"), g(SystemUnit.WideStringType));
            Assert.AreEqual(("UnicodeString", "System@UnicodeString"), g(SystemUnit.UnicodeStringType));
        }

        /// <summary>
        ///     test platform independent types
        /// </summary>
        [TestMethod]
        public void TestPlatformInDependentIntegerTypes() {
            (string, string) g(ITypeDefinition id) => GetInternalTypeName(id);
            Assert.AreEqual(("ShortInt", "zc"), g(SystemUnit.ShortIntType));
            Assert.AreEqual(("Byte", "uc"), g(SystemUnit.ByteType));
            Assert.AreEqual(("SmallInt", "s"), g(SystemUnit.SmallIntType));
            Assert.AreEqual(("Word", "us"), g(SystemUnit.WordType));
            Assert.AreEqual(("Integer", "i"), g(SystemUnit.IntegerType));
            Assert.AreEqual(("Cardinal", "ui"), g(SystemUnit.CardinalType));
            Assert.AreEqual(("Int64", "j"), g(SystemUnit.Int64Type));
            Assert.AreEqual(("UInt64", "uj"), g(SystemUnit.UInt64Type));
            Assert.AreEqual(("Int8", "zc"), g(SystemUnit.Int8Type));
            Assert.AreEqual(("UInt8", "uc"), g(SystemUnit.UInt8Type));
            Assert.AreEqual(("Int16", "s"), g(SystemUnit.Int16Type));
            Assert.AreEqual(("UInt16", "us"), g(SystemUnit.UInt16Type));
            Assert.AreEqual(("Int32", "i"), g(SystemUnit.Int32Type));
            Assert.AreEqual(("UInt32", "ui"), g(SystemUnit.UInt32Type));
        }

        /// <summary>
        ///     test platform dependent types
        /// </summary>
        [TestMethod]
        public void TestBooleanTypes() {
            (string, string) g(ITypeDefinition id) => GetInternalTypeName(id);
            Assert.AreEqual(("Boolean", "o"), g(SystemUnit.BooleanType));
            Assert.AreEqual(("ByteBool", "uc"), g(SystemUnit.ByteBoolType));
            Assert.AreEqual(("WordBool", "us"), g(SystemUnit.WordBoolType));
            Assert.AreEqual(("LongBool", "i"), g(SystemUnit.LongBoolType));
        }

        /// <summary>
        ///     test pointer names
        /// </summary>
        [TestMethod]
        public void TestPointerTypes() {
            (string, string) g(ITypeDefinition id) => GetInternalTypeName(id);
            Assert.AreEqual(("Pointer", "pv"), g(SystemUnit.GenericPointerType));
            Assert.AreEqual(("PByte", "puc"), g(SystemUnit.PByteType));
            Assert.AreEqual(("PShortInt", "pzc"), g(SystemUnit.PShortIntType));
            Assert.AreEqual(("PWord", "pus"), g(SystemUnit.PWordType));
            Assert.AreEqual(("PSmallInt", "ps"), g(SystemUnit.PSmallIntType));
            Assert.AreEqual(("PCardinal", "pui"), g(SystemUnit.PCardinalType));
            Assert.AreEqual(("PLongword", "pui"), g(SystemUnit.PLongwordType));
            Assert.AreEqual(("PFixedUInt", "pui"), g(SystemUnit.PFixedUIntType));
            Assert.AreEqual(("PInteger", "pi"), g(SystemUnit.PIntegerType));
            Assert.AreEqual(("PLongInt", "pi"), g(SystemUnit.PLongIntType));
            Assert.AreEqual(("PFixedInt", "pi"), g(SystemUnit.PFixedIntType));
            Assert.AreEqual(("PUInt64", "puj"), g(SystemUnit.PUInt64Type));
            Assert.AreEqual(("PInt64", "pj"), g(SystemUnit.PInt64Type));
            Assert.AreEqual(("PNativeInt", "pi"), g(SystemUnit.PNativeIntType));
            Assert.AreEqual(("PNativeUInt", "pui"), g(SystemUnit.PNativeUIntType));
            Assert.AreEqual(("PAnsiChar", "pc"), g(SystemUnit.PAnsiCharType));
            Assert.AreEqual(("PWideChar", "pb"), g(SystemUnit.PWideCharType));
            Assert.AreEqual(("PBoolean", "po"), g(SystemUnit.PBooleanType));
            Assert.AreEqual(("PWordBool", "pus"), g(SystemUnit.PWordBoolType));
            Assert.AreEqual(("PLongBool", "pi"), g(SystemUnit.PLongBoolType));
            Assert.AreEqual(("PPointer", "ppv"), g(SystemUnit.PPointerType));
            Assert.AreEqual(("PCurrency", "pSystem@Currency"), g(SystemUnit.PCurrencyType));
            Assert.AreEqual(("PWideString", "pSystem@WideString"), g(SystemUnit.PWideStringType));
            Assert.AreEqual(("PUnicodeString", "pSystem@UnicodeString"), g(SystemUnit.PUnicodeStringType));
            Assert.AreEqual(("PShortString", "pSystem@%SmallString$uc$i255$%"), g(SystemUnit.PShortStringType));
            Assert.AreEqual(("PRawByteString", "p%AnsiStringT$us$i65535$%"), g(SystemUnit.PRawByteStringType));
        }

    }
}
