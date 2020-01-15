using System.Collections.Generic;
using System.Linq;
using PasPasPas.Api;
using PasPasPas.AssemblyBuilder.Builder;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPasTests.Common;

namespace PasPasPasTests.CodeGen {

    /// <summary>
    ///     tests for opcodes
    /// </summary>
    public class POpCodeTests : CommonTest {

        private void RunOpCodeTest(string decls, string input, string[] output) {
            var env = CreateEnvironment();
            var p = $"program a; {decls} begin {input} end.";
            var path = env.CreateFileReference("a.dpr");
            var resolver = CommonApi.CreateResolverForSingleString(path, p);
            var opts = Factory.CreateOptions(resolver, env);
            var api = Factory.CreateParserApi(opts);
            var pa = api.CreateParser(path);
            var cst = pa.Parse();
            var ast = api.CreateAbstractSyntraxTree(cst);
            api.AnnotateWithTypes(ast);
            var pf = env.TypeRegistry.RegisteredTypeDefinitions.Where(t => t is IUnitType && t.TypeId != KnownTypeIds.SystemUnit).FirstOrDefault() as IUnitType;
            var mr = pf.Symbols[KnownNames.MainMethod];
            var r = mr.Symbol as IRoutineGroup;
            var pr = r.Items[0];
            var c = pr.Code;
            var ce = new ConstantEncoder(env);
            Assert.IsNotNull(c);
            var codes = new List<string>();

            foreach (var code in c) {
                codes.Add(code.ToOpCodeString(ce));
            }

            Assert.AreEqualSequences(output, codes);
        }

        /// <summary>
        ///     test the store p-opcode
        /// </summary>
        [TestMethod]
        public void TestStore() {
            var d = "var _i: integer;";
            var i = "_i:=5;";
            var o = new[] { "ldc 5", "st 1" };
            RunOpCodeTest(d, i, o);
        }

    }
}
