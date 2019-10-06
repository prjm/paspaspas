using PasPasPas.Globals.Types;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test routines and type
    /// </summary>
    public class RoutinesAndTypesTest : TypeTest {

        /// <summary>
        ///     test global method calls
        /// </summary>
        [TestMethod]
        public void TestCallToGlobalMethod() {
            AssertExprType("a()", KnownTypeIds.ByteType, "function a: byte; begin Result := 0; end;");
        }


    }
}
