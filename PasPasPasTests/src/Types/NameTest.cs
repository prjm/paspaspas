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

    }
}
