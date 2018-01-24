using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test constant propagation
    /// </summary>
    public class ConstantPropagationTest : TypeTest {

        [TestCase]
        public void TestIntegerConstants() {
            AssertExprValue("0", GetIntegerValue(0));
            AssertExprValue("-128", GetIntegerValue((sbyte)-128));
            AssertExprValue("127", GetIntegerValue((sbyte)127));

            AssertExprValue("128", GetIntegerValue((byte)128));
            AssertExprValue("255", GetIntegerValue((byte)255));

            AssertExprValue("256", GetIntegerValue((short)256));
            AssertExprValue("-129", GetIntegerValue((short)-129));
        }

        [TestCase]
        public void TestIntegerOperations() {
            AssertExprValue("4 + 5", GetIntegerValue(9));
        }

    }
}
