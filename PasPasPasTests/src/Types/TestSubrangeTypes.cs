using PasPasPas.Globals.Types;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test subrange types
    /// </summary>
    public class TestSubrangeTypes : TypeTest {

        private ISystemUnit KnownTypeIds
            => CreateEnvironment().TypeRegistry.SystemUnit;

        /// <summary>
        ///     test subrange type definitions
        /// </summary>
        [TestMethod]
        public void TestSubrangeTypeDefs() {
            AssertExprTypeByVar("2..126", "a", KnownTypeIds.ShortIntType, true);
            AssertExprTypeByVar("2..1", "a", KnownTypeIds.ErrorType, true);
            AssertExprTypeByVar("2..250", "a", KnownTypeIds.ByteType, true);

            AssertExprTypeByVar("'a'..'z'", "a", KnownTypeIds.WideCharType, true);
            AssertExprTypeByVar("'z'..'a'", "a", KnownTypeIds.ErrorType, true);
            AssertExprTypeByVar("'z'..''", "a", KnownTypeIds.ErrorType, true);
            AssertExprTypeByVar("''..'a'", "a", KnownTypeIds.ErrorType, true);

            AssertExprTypeByVar("false..true", "a", KnownTypeIds.BooleanType, true);
            AssertExprTypeByVar("true..false", "a", KnownTypeIds.ErrorType, true);

            var e = CreateEnvironment();
            var tc = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var et = tc.CreateEnumType("v");

            AssertExprTypeByVar("x..z", "a", et, true, "type v = (x,y,z);");
            AssertExprTypeByVar("z..y", "a", KnownTypeIds.ErrorType, true, "type v = (x,y,z);");

        }

    }
}
