using PasPasPas.Globals.Types;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    public class ConstantSerializationTest : SerializationTest {

        [TestMethod]
        public void TestUnitMetadata() {
            void tester(IUnitType u) =>
                Assert.AreEqual("a", u.Name);
            var prg = "unit a; interface implementation end.";
            TestUnitSerialization(prg, tester);
        }

        [TestMethod]
        public void TestIntegerTypes() {
            //AssertSerializedConstant("A = 5", GetIntegerValue((short)5));
        }
    }
}
