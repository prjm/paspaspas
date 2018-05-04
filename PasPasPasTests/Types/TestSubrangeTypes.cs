using PasPasPas.Global.Constants;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test subrange types
    /// </summary>
    public class TestSubrangeTypes : TypeTest {

        [TestCase]
        public void TestSubrangeTypeDefs() {
            AssertExprTypeByVar("2..126", "a", KnownTypeIds.ShortInt, true);
            AssertExprTypeByVar("2..1", "a", KnownTypeIds.ErrorType, true);
        }

    }
}
