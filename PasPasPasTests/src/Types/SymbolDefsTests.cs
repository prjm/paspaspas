using System;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test for symbol types
    /// </summary>
    public class SymbolDefsTests : TypeTest {

        private void TestOnUnitType(Func<IUnitType, bool> tester) {
            var env = default(ITypedEnvironment);
            Func<object, CompilationUnit> t =
                x => {
                    var unit = x as CompilationUnit;
                    if (unit == default)
                        return default;

                    var unitType = env.TypeRegistry.GetTypeByIdOrUndefinedType(unit?.TypeInfo?.TypeId ?? KnownTypeIds.ErrorType) as IUnitType;
                    Assert.IsNotNull(unitType);
                    Assert.AreEqual(CommonTypeKind.Unit, unitType.TypeKind);
                    Assert.IsTrue(tester(unitType));
                    return unit;
                };

            EvaluateExpressionType("a", "program a; begin end.", t, NativeIntSize.All32bit, out env);
        }

        /// <summary>
        ///     test
        /// </summary>
        [TestMethod]
        public void TestMainMethodExistence() {
            Func<IUnitType, bool> t = u => {
                var s = u.Symbols[KnownTypeNames.MainMethod];
                return s != default;
            };
            TestOnUnitType(t);
        }

    }
}
