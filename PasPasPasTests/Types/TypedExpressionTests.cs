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
            AssertExprType("1", TypeIds.ByteType);
            AssertExprType("255", TypeIds.ByteType);
            AssertExprType("256", TypeIds.WordType);
            AssertExprType("65535", TypeIds.WordType);
            AssertExprType("65536", TypeIds.CardinalType);
            AssertExprType("4294967295", TypeIds.CardinalType);
            AssertExprType("4294967296", TypeIds.Uint64Type);
            AssertExprType("18446744073709551615", TypeIds.Uint64Type);
        }

        [Fact]
        public void TestBooleanLiteralTypes() {
            AssertExprType("true", TypeIds.BooleanType);
            AssertExprType("false", TypeIds.BooleanType);
        }

        [Fact]
        public void TestCharLiteralTypes() {
            AssertExprType("'C'", TypeIds.CharType);
            AssertExprType("#9", TypeIds.CharType);
        }

        [Fact]
        public void TestStringLiteralTypes() {
            AssertExprType("'CD'", TypeIds.StringType);
            AssertExprType("#9#9", TypeIds.StringType);
        }

        [Fact]
        public void TestExtendedLiteralTypes() {
            AssertExprType("3.5", TypeIds.Extended);
            AssertExprType("2.33434343", TypeIds.Extended);
        }

    }
}
