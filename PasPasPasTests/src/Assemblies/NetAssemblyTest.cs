using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using PasPasPas.Api;
using PasPasPas.AssemblyBuilder.Builder;
using PasPasPas.Globals.Log;
using PasPasPas.Infrastructure.Log;
using PasPasPasTests.Common;

namespace PasPasPasTests.Assemblies {

    /// <summary>
    ///     test assembly generation
    /// </summary>
    public class NetAssemblyTest : CommonTest {

        private void RunAssemblyTest(string file, string program, Func<Assembly, bool> tester, params uint[] errorMessages) {
            var env = CreateEnvironment();
            var msgs = new List<ILogMessage>();
            var log = new LogTarget();
            var hasError = false;
            var errorText = string.Empty;

            env.Log.RegisterTarget(log);

            log.ProcessMessage += (x, y) => {
                msgs.Add(y.Message);
                errorText += y.Message.MessageID.ToString(MessageNumbers.NumberFormat, CultureInfo.InvariantCulture) + Environment.NewLine;
                hasError = hasError ||
                y.Message.Severity == MessageSeverity.Error ||
                y.Message.Severity == MessageSeverity.FatalError;
            };

            var api = new AssemblyBuilderApi(env);
            var asm = api.CreateAssemblyForString($"{file}.dpr", program);

            if (tester != default) {
                Assert.IsNotNull(asm);
                Assert.IsTrue(tester(asm.GeneratedAssembly));
            }

            if (errorMessages.Length < 1) {
                Assert.AreEqual(string.Empty, errorText);
                Assert.IsFalse(hasError);
            }
            Assert.AreEqual(errorMessages.Length, msgs.Count);
            foreach (var guid in errorMessages)
                Assert.IsTrue(msgs.Where(t => t.MessageID == guid).Any());

        }

        private void RunTypeTest(string file, string program, Func<Type, bool> tester, string typeName, params uint[] errorMessages) {
            bool v(Assembly a) => tester(a?.GetType(typeName));
            RunAssemblyTest(file, program, v, errorMessages);
        }

        [TestMethod]
        public void TestAssemblyName() {
            bool t(Assembly a) => string.Equals(a.GetName().Name, "x.z", StringComparison.OrdinalIgnoreCase);
            RunAssemblyTest("x.z", "program x.z; begin end.", t);
            RunAssemblyTest("x.z", "unit x.z; interface implementation end.", default, BuilderErrorMessages.UndefinedProjectName);
        }

        [TestMethod]
        public void TestUnitType() {
            const string typeName = "P3.x_z.<UnitClass>";
            bool d(Type t) => string.Equals(t?.FullName, typeName, StringComparison.OrdinalIgnoreCase);
            RunTypeTest("x.z", "program x.z; begin end.", d, typeName);
        }

    }
}
