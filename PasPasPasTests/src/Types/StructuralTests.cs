using PasPasPas.Globals.Types;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test for structural properties
    /// </summary>
    public class StructuralTests : TypeTest {

        private void AssertExprTypeInProc(string proc, string expression, string typeName = "", string decls = "", int typeId = KnownTypeIds.ErrorType) {
            var file = "SimpleExpr";
            var program = $"program {file};{decls} {proc} begin writeln({expression}); end; begin end. ";
            AssertExprType(file, program, typeId, false, typeName);

        }

        [TestMethod]
        public void TestResultDef() {
            AssertExprTypeInProc("function a: string;", "Result", typeId: KnownTypeIds.StringType);
            AssertExprTypeInProc("function a: string;", "4", typeId: KnownTypeIds.ShortInt);
        }
    }
}
