using Xunit;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test constant propagation
    /// </summary>
    public class ConstantPropagationTest : TypeTest {

        [Fact]
        public void TestIntegerConstants() {
            AssertExprValue("0", (sbyte)0);
            AssertExprValue("-128", (sbyte)-128);
            AssertExprValue("127", (sbyte)127);

            AssertExprValue("128", (byte)128);
            AssertExprValue("255", (byte)255);

            AssertExprValue("256", (short)256);
            AssertExprValue("-129", (short)-129);

        }

    }
}
