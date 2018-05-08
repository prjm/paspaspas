using PasPasPas.Global.Types;
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
            AssertExprTypeByVar("2..250", "a", KnownTypeIds.ByteType, true);

            AssertExprTypeByVar("'a'..'z'", "a", KnownTypeIds.WideCharType, true);
            AssertExprTypeByVar("'z'..'a'", "a", KnownTypeIds.ErrorType, true);

            AssertExprTypeByVar("false..true", "a", KnownTypeIds.BooleanType, true);
            AssertExprTypeByVar("true..false", "a", KnownTypeIds.ErrorType, true);
        }

    }
}
