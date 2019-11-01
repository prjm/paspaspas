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
            void tester(IExpression value) {
                Assert.IsNotNull(value);

                var typeInfo = value.TypeInfo;
                Assert.IsNotNull(typeInfo);
                Assert.AreEqual(typeId, typeInfo.TypeId);
                Assert.AreEqual(TypeReferenceKind.InvocationResult, typeInfo.ReferenceKind);
            }

            AssertExprType(expr, decls, tester);
        }

        /// <summary>
        ///     helper to verify statements
        /// </summary>
        /// <param name="statement"></param>
        /// <param name="typeId"></param>
        /// <param name="kind"></param>
        /// <param name="decls"></param>
        protected void AssertCallStatementType(string statement, int typeId, string decls, StructuredStatementKind kind) {
            void tester(StructuredStatement value) {
                Assert.IsNotNull(value);

                Assert.AreEqual(value.Kind, kind);

                if (kind == StructuredStatementKind.ExpressionStatement) {
                    Assert.IsTrue(value.Expressions.Count > 0);
                    var e = value.Expressions[0];
                    var typeInfo = e.TypeInfo;
                    Assert.IsNotNull(typeInfo);
                    Assert.AreEqual(typeId, typeInfo.TypeId);
                    Assert.AreEqual(TypeReferenceKind.InvocationResult, typeInfo.ReferenceKind);
                }
            }


            AssertStatementType(statement, decls, tester);
        }

        /// <summary>
        ///     test global method calls
        /// </summary>
        [TestMethod]
        public void TestCallToGlobalMethod() {
            AssertCallExprType("a()", KnownTypeIds.ByteType, "function a: byte; begin Result := 0; end;");
        }

        /// <summary>
        ///     test calling write line
        /// </summary>
        [TestMethod]
        public void TestCallToWriteLn() {
            AssertCallStatementType("WriteLn()", KnownTypeIds.NoType, "", StructuredStatementKind.ExpressionStatement);
        }


    }
}
