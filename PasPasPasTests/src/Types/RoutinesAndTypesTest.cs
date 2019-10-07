using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test routines and type
    /// </summary>
    public class RoutinesAndTypesTest : TypeTest {

        /// <summary>
        ///     helper method to verify expressions
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="typeId"></param>
        /// <param name="decls"></param>
        protected void AssertCallExprType(string expr, int typeId, string decls) {
            Action<IExpression> tester = (IExpression value) => {
                Assert.IsNotNull(value);

                var typeInfo = value.TypeInfo;
                Assert.IsNotNull(typeInfo);
                Assert.AreEqual(typeId, typeInfo.TypeId);
                Assert.AreEqual(TypeReferenceKind.InvocationResult, typeInfo.ReferenceKind);
            };

            AssertExprType(expr, decls, tester);
        }

        /// <summary>
        ///     test global method calls
        /// </summary>
        [TestMethod]
        public void TestCallToGlobalMethod() {
            AssertCallExprType("a()", KnownTypeIds.ByteType, "function a: byte; begin Result := 0; end;");
        }


    }
}
