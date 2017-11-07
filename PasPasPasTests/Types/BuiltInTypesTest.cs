using PasPasPas.Typings.Common;
using Xunit;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test for built in types
    /// </summary>
    public class BuiltInTypesTest : TypeTest {

        [Fact]
        public void TestIntTypes() {
            AssertDeclType("System.Byte", TypeIds.ByteType);
            AssertDeclType("System.Word", TypeIds.WordType);
            AssertDeclType("System.Cardinal", TypeIds.CardinalType);
            AssertDeclType("System.UInt64", TypeIds.Uint64Type);
        }

    }
}
