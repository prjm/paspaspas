using Xunit;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test constant propagation
    /// </summary>
    public class ConstantPropagationTest : TypeTest {

        [Fact]
        public void TestIntegerConstants() {
            AssertExprValue("0", 0);
            AssertExprValue("-128", -128);
            AssertExprValue("127", 127);

            AssertExprValue("128", 128);
            AssertExprValue("255", 255);

            AssertExprValue("256", 256);
            AssertExprValue("-129", -129);
        }

    }
}
