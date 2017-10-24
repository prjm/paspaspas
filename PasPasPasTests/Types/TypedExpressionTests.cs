using PasPasPas.Typings.Common;
using PasPasPasTests.Common;
using Xunit;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test cases for typed expressions
    /// </summary>
    public class TypedExpressionTest : TypeTest {

        [Fact]
        public void TestIntegerLiteralTypes() {
            AssertExprType("5", TypeIds.ByteType);
            //AssertExprType("$5", TypeIds.ByteType);
        }

    }
}
