using System;
using System.IO;
using System.Linq;
using PasPasPas.Api;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Typings.Serialization;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test unit serialization
    /// </summary>
    public class SerializationTest : CommonTest {

        /// <summary>
        ///     test a simple unit
        /// </summary>
        /// <param name="prg"></param>
        /// <param name="tester"></param>
        protected void TestUnitSerialization(string prg, Action<IUnitType?> tester) {
            var env = CreateEnvironment();
            var fle = env.CreateFileReference("a.pas");
            var rsv = CommonApi.CreateResolverForSingleString(fle, prg);
            var opt = Factory.CreateOptions(rsv, env);
            var api = Factory.CreateParserApi(opt);
            using var p = api.CreateParser(fle);
            var cst = p.Parse();
            var ast = api.CreateAbstractSyntraxTree(cst);
            api.AnnotateWithTypes(ast);
            var root = (ast as ProjectItemCollection)?[0]?.TypeInfo;
            var unitType = root?.TypeDefinition as IUnitType;
            using var s = new MemoryStream();
            using var w = env.CreateTypeWriter(s);
            w.WriteUnit(unitType);
            s.Seek(0, SeekOrigin.Begin);

            using var r = env.CreateTypeReader(s);
            var u = r.ReadUnit() as IUnitType;
            tester(u);

        }

        /// <summary>
        ///     test a serialized constant
        /// </summary>
        /// <param name="constant"></param>
        /// <param name="value"></param>
        protected void AssertSerializedConstant(string constant, IValue value) {
            var prg = $"unit a; interface const  B = {constant}; implementation end.";
            void tester(IUnitType? t) {
                var c = t?.Symbols.Where(t1 => string.Equals(t1.Name, "B", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                Assert.AreEqual(value, c as IValue);
            };

            TestUnitSerialization(prg, tester);
        }


        internal static TypeReader CreateReader(ITypedEnvironment env, MemoryStream stream)
            => env.CreateTypeReader(stream) as TypeReader ?? throw new InvalidOperationException();

        internal static TypeWriter CreateWriter(ITypedEnvironment env, MemoryStream stream)
            => env.CreateTypeWriter(stream) as TypeWriter ?? throw new InvalidOperationException();

    }
}
