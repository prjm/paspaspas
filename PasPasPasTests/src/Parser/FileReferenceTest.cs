using System;
using System.IO;
using System.Linq;
using PasPasPas.Api;
using PasPasPas.Globals.Api;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Log;
using PasPasPas.Globals.Options;
using PasPasPas.Globals.Parsing;
using PasPasPas.Infrastructure.Log;
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
            public ListLogTarget LogTarget { get; }
            public ILogSource Log { get; }

            public TestBundle() {
                LogTarget = new ListLogTarget();
                SysEnv = CreateEnvironment();
                Log = new LogSource(SysEnv.Log, 0x9999);
                SysEnv.Log.RegisterTarget(LogTarget);
                Files = new FilesAndPaths();
                Resolver = Files.CreateResolver();
                Options = Factory.CreateOptions(Resolver, SysEnv);
                ParserFactory = Factory.CreateParserApi(Options);
                Rsvlr = new IncludeFilePathResolver(Options);
            }

            public ISyntaxPart Parse(string file) {
                var f = ParserFactory.Tokenizer.Readers;
                var c = Path.Combine(Directory.GetCurrentDirectory(), file);
                var q = f.SystemEnvironment.CreateFileReference(c);
                var p = ParserFactory.CreateParser(q);
                return p.Parse();
            }

            internal IFileReference Ref(string v)
                => ParserFactory.Tokenizer.Environment.CreateFileReference(v);

            internal IFileReference GetCurrentDir()
                => Ref(Directory.GetCurrentDirectory());

            public bool HasMessage(uint messageNumber)
                => LogTarget.Messages.FirstOrDefault(t => t.MessageID == messageNumber) != default;

            public RequiredUnitsFinder CreateRequiredUnitsFinder()
                => new RequiredUnitsFinder(GetCurrentDir(), Options.Meta.IncludePathResolver, Log);
        }

        [TestMethod]
        public void TestProgramUnitResolver() {
            var t = new TestBundle();
            t.Files.Add("a.dpr", "program a; uses b; begin end.");
            t.Files.Add("b.pas", "unit b; interface implementation end.");
            var p = t.Parse("a.dpr");
            var f = t.CreateRequiredUnitsFinder();
            f.FindRequiredUnits(p);
            Assert.AreEqual(1, f.RequiredUnits.Count);
            Assert.IsFalse(t.HasMessage(MessageNumbers.MissingFile));
            Assert.IsFalse(f.HasMissingFiles);
        }

        [TestMethod]
        public void TestLibraryUnitResolver() {
            var t = new TestBundle();
            t.Files.Add("a.dpr", "library a; uses b; begin end.");
            t.Files.Add("b.pas", "unit b; interface implementation end.");
            var p = t.Parse("a.dpr");
            var f = t.CreateRequiredUnitsFinder();
            f.FindRequiredUnits(p);
            Assert.AreEqual(1, f.RequiredUnits.Count);
            Assert.IsFalse(t.HasMessage(MessageNumbers.MissingFile));
            Assert.IsFalse(f.HasMissingFiles);
        }

        [TestMethod]
        public void TestUnitUnitResolver() {
            var t = new TestBundle();
            t.Files.Add("a.pas", "unit a; interface uses b; implementation end.");
            t.Files.Add("b.pas", "unit b; interface implementation end.");
            var p = t.Parse("a.pas");
            var f = t.CreateRequiredUnitsFinder();
            f.FindRequiredUnits(p);
            Assert.AreEqual(1, f.RequiredUnits.Count);
            Assert.IsFalse(t.HasMessage(MessageNumbers.MissingFile));
            Assert.IsFalse(f.HasMissingFiles);
        }

        [TestMethod]
        public void TestPackageUnitResolver() {
            var t = new TestBundle();
            t.Files.Add("a.dpk", "package a; contains b in 'b.pas'; end.");
            t.Files.Add("b.pas", "unit b; interface implementation end.");
            var p = t.Parse("a.dpk");
            var f = t.CreateRequiredUnitsFinder();
            f.FindRequiredUnits(p);
            Assert.AreEqual(1, f.RequiredUnits.Count);
            Assert.IsFalse(t.HasMessage(MessageNumbers.MissingFile));
            Assert.IsFalse(f.HasMissingFiles);
        }

        [TestMethod]
        public void TestUnitNotFoundInProgram() {
            var t = new TestBundle();
            t.Files.Add("a.dpr", "program a; uses b; begin end.");
            var p = t.Parse("a.dpr");
            var f = t.CreateRequiredUnitsFinder();
            f.FindRequiredUnits(p);
            Assert.AreEqual(0, f.RequiredUnits.Count);
            Assert.IsTrue(t.HasMessage(MessageNumbers.MissingFile));
            Assert.IsTrue(f.HasMissingFiles);
        }

        [TestMethod]
        public void TestSimpleReferencedConstant() {
            var t = new TestBundle();
            var a = "unit a; interface const x = 5; implementation end.";
            var b = "unit b; interface uses a; const b = a.x; implementation end.";
            t.Files.Add("a.pas", a);
            t.Files.Add("b.pas", b);
            var s = t.Parse("b.pas");
            var l = t.ParserFactory.CreateAbstractSyntraxTree(s);
            t.ParserFactory.AnnotateWithTypes(l);
            var v = new AstVisitor<ConstantDeclaration>(z => (z is ConstantDeclaration u && string.Equals(u.Name.Name, "b", StringComparison.Ordinal)) ? u : default);
            l.Accept(v);
            Assert.AreEqual(GetIntegerValue(5), v.Result.Value.TypeInfo);
        }

    }
}
