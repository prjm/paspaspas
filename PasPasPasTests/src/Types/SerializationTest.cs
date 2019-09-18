using System;
using System.IO;
using PasPasPas.Api;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Typings.Serialization;
using PasPasPas.Typings.Structured;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    public class SerializationTest : CommonTest {

        protected void TestUnitSerialization(string prg, Action<IUnitType> tester) {
            var env = CreateEnvironment();
            var fle = env.CreateFileReference("a.pas");
            var rsv = CommonApi.CreateResolverForSingleString(fle, prg);
            var opt = Factory.CreateOptions(rsv, env);
            var api = Factory.CreateParserApi(opt);
            using (var p = api.CreateParser(fle)) {
                var cst = p.Parse();
                var ast = api.CreateAbstractSyntraxTree(cst);
                api.AnnotateWithTypes(ast);
                var root = (ast as ProjectItemCollection)?[0]?.TypeInfo;
                var unitType = env.TypeRegistry.GetTypeByIdOrUndefinedType(root.TypeId) as UnitType;
                using (var s = new MemoryStream()) {
                    using (var w = env.CreateTypeWriter(s)) {
                        w.WriteUnit(unitType);
                        s.Seek(0, SeekOrigin.Begin);

                        using (var r = env.CreateTypeReader(s)) {
                            var u = r.ReadUnit() as IUnitType;
                            tester(u);
                        }
                    }
                }
            }

        }

        protected void AssertSerializedConstant(string constant, ITypeReference value) {
            var prg = $"unit a; interface const  B = {constant}; implementation end.";
            void tester(IUnitType t) {
                var c = (t as UnitType).Symbols["B"].Symbol;
                Assert.AreEqual(value, c);
            };

            TestUnitSerialization(prg, tester);
        }


        internal static TypeReader CreateReader(ITypedEnvironment env, MemoryStream stream)
            => env.CreateTypeReader(stream) as TypeReader;

        internal static TypeWriter CreateWriter(ITypedEnvironment env, MemoryStream stream)
            => env.CreateTypeWriter(stream) as TypeWriter;

    }
}
