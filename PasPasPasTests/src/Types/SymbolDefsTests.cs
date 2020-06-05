#nullable disable
using System;
using System.Linq;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test for symbol types
    /// </summary>
    public class SymbolDefsTests : TypeTest {



        private void TestOnUnitType(Func<IUnitType, bool> tester, string decls = "") {
            var env = default(ITypedEnvironment);
            CompilationUnit t(object x) {
                var unit = x as CompilationUnit;
                if (unit == default)
                    return default;

                var unitType = (unit?.TypeInfo?.TypeDefinition ?? env.TypeRegistry.SystemUnit.ErrorType) as IUnitType;
                Assert.IsNotNull(unitType);
                Assert.AreEqual(BaseType.Unit, unitType.BaseType);
                Assert.IsTrue(tester(unitType));
                return unit;
            }

            EvaluateExpressionType("a", $"program a; {decls} begin end.", t, NativeIntSize.All32bit, out env);
        }

        /// <summary>
        ///     test the definition of the main method
        /// </summary>
        [TestMethod]
        public void TestMainMethodExistence() {
            bool t(IUnitType u) {
                var s = u.Symbols.Where(x => string.Equals(x.Name, KnownNames.MainMethod, StringComparison.OrdinalIgnoreCase));
                return s != default;
            }
            TestOnUnitType(t);
        }

        /// <summary>
        ///     test the definition of a global variable
        /// </summary>
        [TestMethod]
        public void TestUnitVariable() {
            bool t(IUnitType u) {
                var s = u.Symbols.Where(x => string.Equals(x.Name, "a", StringComparison.OrdinalIgnoreCase));
                return s != default;
            }
            TestOnUnitType(t, "var a: string;");
        }

    }
}
