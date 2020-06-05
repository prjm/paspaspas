#nullable disable
using System;
using System.Collections.Immutable;
using System.Reflection;
using System.Reflection.Emit;
using PasPasPas.AssemblyBuilder.Builder.Net;
using PasPasPas.Globals.CodeGen;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Structured;
using PasPasPasTests.Common;

namespace PasPasPasTests.CodeGen {

    using POpCode = PasPasPas.Globals.CodeGen.OpCode;

    /// <summary>
    ///     test op code generation
    /// </summary>
    public class NetOpCodeTests : CommonTest {

        private void OpCodeTest(IAssemblyBuilderEnvironment env, POpCode code) {
            var r = new Random();
            var n = $"P3T{r}";
            var t = "T" + n;
            var name = new AssemblyName("n");
            var asmBuilder = AssemblyBuilder.DefineDynamicAssembly(name, AssemblyBuilderAccess.RunAndCollect);
            var mBuilder = asmBuilder.DefineDynamicModule(n + ".dll");
            var tBuilder = mBuilder.DefineType(t);
            var mmBuilder = tBuilder.DefineMethod("A", MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard);
            var tMapper = new TypeMapper(env.TypeRegistry);
            var m = new NetMethodBuilder(mmBuilder, tMapper);
            var rt = new RoutineGroup(env.TypeRegistry.SystemUnit, "A");
            var p = new Routine(rt, RoutineKind.Procedure, default);
            m.ReturnType = env.TypeRegistry.SystemUnit.NoType;
            p.Code = ImmutableArray.Create<POpCode>(code);
            m.DefineMethodBody();
            m.FinishMethod();

            var td = tBuilder.CreateType();
            var mt = td.GetMethod("A", BindingFlags.Public | BindingFlags.Static);
            mt.Invoke(default, Array.Empty<object>());
        }

        /// <summary>
        ///     test a call to an intrinsic method
        /// </summary>
        [TestMethod]
        public void TestCallIntrinsinc() {
            var env = CreateEnvironment();
            var r = default(IRoutineGroup);
            var p = new Routine(r, RoutineKind.Procedure, default);
            var i = env.Runtime.Types.MakeInvocationResultFromIntrinsic(r, env.Runtime.Types.MakeSignature(env.TypeRegistry.SystemUnit.NoType.Reference));
            var c = new POpCode(OpCodeId.Call);
            OpCodeTest(env, c);
        }

    }
}
