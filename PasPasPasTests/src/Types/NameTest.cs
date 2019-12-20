namespace PasPasPasTests.Types {
    using PasPasPasTests.Common;
    using KTI = PasPasPas.Globals.Types.KnownTypeIds;

    /// <summary>
    ///     test names of types
    /// </summary>
    public class NameTest : TypeTest {

        (string, string) GetInternalTypeName(int typeId) {
            var env = CreateEnvironment();
            var type = env.TypeRegistry.GetTypeByIdOrUndefinedType(typeId);
            return (type.LongName, type.ShortName);
        }


        /// <summary>
        ///     test platform dependent types
        /// </summary>
        [TestMethod]
        public void TestPlatformDependentIntegerTypes() {
            (string, string) g(int id) => GetInternalTypeName(id);
            Assert.AreEqual(("FixedInt", "i"), g(KTI.FixedInt));
            Assert.AreEqual(("FixedUInt", "ui"), g(KTI.FixedUInt));
            Assert.AreEqual(("NativeInt", "i"), g(KTI.NativeInt));
            Assert.AreEqual(("NativeUInt", "ui"), g(KTI.NativeUInt));
            Assert.AreEqual(("LongInt", "i"), g(KTI.LongInt));
            Assert.AreEqual(("LongWord", "ui"), g(KTI.LongWord));
        }

        /// <summary>
        ///     test names for real numeric types
        /// </summary>
        [TestMethod]
        public void TestRealTypes() {
            (string, string) g(int id) => GetInternalTypeName(id);
            Assert.AreEqual(("Real48", "6real48"), g(KTI.Real48Type));
            Assert.AreEqual(("Single", "f"), g(KTI.SingleType));
            Assert.AreEqual(("Double", "d"), g(KTI.DoubleType));
            Assert.AreEqual(("Extended", "g"), g(KTI.Extended));
            Assert.AreEqual(("Comp", "System@Comp"), g(KTI.Comp));
            Assert.AreEqual(("Currency", "System@Currency"), g(KTI.Currency));
        }

        /// <summary>
        ///     test names for string numeric types
        /// </summary>
        [TestMethod]
        public void TestStringTypes() {
            (string, string) g(int id) => GetInternalTypeName(id);
            Assert.AreEqual(("AnsiChar", "c"), g(KTI.AnsiCharType));
            Assert.AreEqual(("WideChar", "b"), g(KTI.WideCharType));

        }

        /// <summary>
        ///     test platform independent types
        /// </summary>
        [TestMethod]
        public void TestPlatformInDependentIntegerTypes() {
            (string, string) g(int id) => GetInternalTypeName(id);
            Assert.AreEqual(("ShortInt", "zc"), g(KTI.ShortInt));
            Assert.AreEqual(("Byte", "uc"), g(KTI.ByteType));
            Assert.AreEqual(("SmallInt", "s"), g(KTI.SmallInt));
            Assert.AreEqual(("Word", "us"), g(KTI.WordType));
            Assert.AreEqual(("Integer", "i"), g(KTI.IntegerType));
            Assert.AreEqual(("Cardinal", "ui"), g(KTI.CardinalType));
            Assert.AreEqual(("Int64", "j"), g(KTI.Int64Type));
            Assert.AreEqual(("UInt64", "uj"), g(KTI.UInt64Type));
            Assert.AreEqual(("Int8", "zc"), g(KTI.Signed8BitInteger));
            Assert.AreEqual(("UInt8", "uc"), g(KTI.Unsigned8BitInteger));
            Assert.AreEqual(("Int16", "s"), g(KTI.Signed16BitInteger));
            Assert.AreEqual(("UInt16", "us"), g(KTI.Unsigned16BitInteger));
            Assert.AreEqual(("Int32", "i"), g(KTI.Signed32BitInteger));
            Assert.AreEqual(("UInt32", "ui"), g(KTI.Unsigned32BitInteger));
        }

        /// <summary>
        ///     test platform dependent types
        /// </summary>
        [TestMethod]
        public void TestBooleanTypes() {
            (string, string) g(int id) => GetInternalTypeName(id);
            Assert.AreEqual(("Boolean", "o"), g(KTI.BooleanType));
            Assert.AreEqual(("ByteBool", "uc"), g(KTI.ByteBoolType));
            Assert.AreEqual(("WordBool", "us"), g(KTI.WordBoolType));
            Assert.AreEqual(("LongBool", "i"), g(KTI.LongBoolType));
        }

    }
}
