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

        [TestMethod]
        public void TestVariablesAndTypes() {
            const string typeName = "P3.x_z.<UnitClass>";
            Func<Type, bool> d(string a) => t => Assert.AreEqual(a, t?.GetField("a", BindingFlags.Static | BindingFlags.NonPublic)?.FieldType?.Name);

            RunTypeTest("x.z", "program x.z; var a: ShortInt; begin end.", d("SByte"), typeName);
            RunTypeTest("x.z", "program x.z; var a: SmallInt; begin end.", d("Int16"), typeName);
            RunTypeTest("x.z", "program x.z; var a: Integer; begin end.", d("Int32"), typeName);
            RunTypeTest("x.z", "program x.z; var a: Int64; begin end.", d("Int64"), typeName);
            RunTypeTest("x.z", "program x.z; var a: Byte; begin end.", d("Byte"), typeName);
            RunTypeTest("x.z", "program x.z; var a: Word; begin end.", d("UInt16"), typeName);
            RunTypeTest("x.z", "program x.z; var a: Cardinal; begin end.", d("UInt32"), typeName);
            RunTypeTest("x.z", "program x.z; var a: UInt64; begin end.", d("UInt64"), typeName);

            RunTypeTest("x.z", "program x.z; var a: NativeInt; begin end.", d("Int32"), typeName);
            RunTypeTest("x.z", "program x.z; var a: NativeUInt; begin end.", d("UInt32"), typeName);
            RunTypeTest("x.z", "program x.z; var a: LongInt; begin end.", d("Int32"), typeName);
            RunTypeTest("x.z", "program x.z; var a: LongWord; begin end.", d("UInt32"), typeName);

            RunTypeTest("x.z", "program x.z; var a: AnsiChar; begin end.", d("Byte"), typeName);
            RunTypeTest("x.z", "program x.z; var a: WideChar; begin end.", d("Char"), typeName);
            RunTypeTest("x.z", "program x.z; var a: Char; begin end.", d("Char"), typeName);

            RunTypeTest("x.z", "program x.z; var a: Boolean; begin end.", d("Boolean"), typeName);
            RunTypeTest("x.z", "program x.z; var a: ByteBool; begin end.", d("Byte"), typeName);
            RunTypeTest("x.z", "program x.z; var a: WordBool; begin end.", d("UInt16"), typeName);
            RunTypeTest("x.z", "program x.z; var a: LongBool; begin end.", d("UInt32"), typeName);
        }

    }
}
