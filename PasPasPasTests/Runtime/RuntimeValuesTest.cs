using PasPasPas.Typings.Common;
using PasPasPasTests.Common;

namespace PasPasPasTests.Runtime {

    public class RuntimeValuesTest : CommonTest {

        [Xunit.Fact]
        public void TestIntegers() {
            Assert.AreEqual(new byte[] { 1 }, GetIntegerValue((sbyte)1).Data);
            Assert.AreEqual("1", GetIntegerValue((sbyte)1).ToString());
            Assert.AreEqual(TypeIds.ShortInt, GetIntegerValue((sbyte)1).TypeId);
        }

    }
}

