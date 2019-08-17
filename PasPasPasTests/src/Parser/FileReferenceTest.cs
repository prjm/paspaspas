using System;
using System.IO;
using PasPasPas.Api;
using PasPasPas.Globals.Api;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Options;
using PasPasPas.Globals.Parsing;
using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.Parser;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPasTests.Common;

namespace PasPasPasTests.Parser {

    public class FileReferenceTest : ParserTestBase {

        internal class TestBundle {
            public IAssemblyBuilderEnvironment SysEnv { get; }
            public IInputResolver Resolver { get; }
            public IOptionSet Options { get; }
            public IParserApi ParserFactory { get; }
            public FilesAndPaths Files { get; }
            public SearchPathResolver Rsvlr { get; }

            public TestBundle() {
                SysEnv = CreateEnvironment();
                Files = new FilesAndPaths();
                Resolver = Files.CreateResolver();
                Options = Factory.CreateOptions(Resolver, SysEnv);
                ParserFactory = Factory.CreateParserApi(Options);
                Rsvlr = new IncludeFilePathResolver(Options);
            }

            public ISyntaxPart Parse(string file) {
                var f = ParserFactory.Tokenizer.Readers;
                var c = Path.Combine(Directory.GetCurrentDirectory(), file);
                var q = f.CreateFileRef(c);
                var p = ParserFactory.CreateParser(q);
                return p.Parse();
            }

            internal FileReference Ref(string v)
                => ParserFactory.Tokenizer.Readers.CreateFileRef(v);

            internal FileReference GetCurrentDir()
                => Ref(Directory.GetCurrentDirectory());
        }

        [TestMethod]
        public void TestProgramUnitResolver() {
            var t = new TestBundle();
            t.Files.Add("a.dpr", "program x; uses b; begin end.");
            t.Files.Add("b.pas", "unit b; interface implementation end.");
            var p = t.Parse("a.dpr");
            var f = new RequiredUnitsFinder(t.GetCurrentDir(), t.Options.Meta.IncludePathResolver);
            f.FindRequiredUnits(p);
            Assert.AreEqual(1, f.RequiredUnits.Count);
        }

        //[TestMethod]
        public void TestSimpleReferencedConstant() {
            var e = CreateEnvironment();
            var a = "unit a; interface const x = 5; implementation end.";
            var b = "unit b; interface uses a; const b = a.x; implementation end.";
            var t = new FilesAndPaths(("a.pas", a), ("b.pas", b));
            var i = t.CreateResolver();
            var o = Factory.CreateOptions(i, e);
            var x = Factory.CreateParserApi(o);
            var q = x.Tokenizer.Readers.CreateFileRef("b.pas");
            var p = x.CreateParser(q);
            var s = p.Parse();
            var l = x.CreateAbstractSyntraxTree(s);
            var v = new AstVisitor<ConstantDeclaration>(z => (z is ConstantDeclaration u && string.Equals(u.Name.Name, "b", StringComparison.Ordinal)) ? u : default);
            l.Accept(v);
            Assert.AreEqual(v.Result.Value.TypeInfo, GetIntegerValue(5));
        }

    }
}
