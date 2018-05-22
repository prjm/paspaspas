using PasPasPas.Infrastructure.Files;
using PasPasPas.Parsing.SyntaxTree.Standard;
using PasPasPasTests.Common;

namespace PasPasPasTests.Parser {

    public class CstTest : ParserTestBase {

        [TestCase]
        public void TestAbstract() {
            var s = RunEmptyCstTest(p => p.ParseAbstractDirective());
            Assert.IsNotNull(s.Directive);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestReintroduce() {
            var s = RunEmptyCstTest(p => p.ParseReintroduceDirective());
            Assert.IsNotNull(s.Directive);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestOverload() {
            var s = RunEmptyCstTest(p => p.ParseOverloadDirective());
            Assert.IsNotNull(s.Directive);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestInline() {
            var s = RunEmptyCstTest(p => p.ParseInlineDirective());
            Assert.IsNotNull(s.Directive);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestBinding() {
            var s = RunEmptyCstTest(p => p.ParseBindingDirective());
            Assert.IsNotNull(s.Directive);
            Assert.IsNotNull(s.MessageExpression);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestCallConvention() {
            var s = RunEmptyCstTest(p => p.ParseCallConvention());
            Assert.IsNotNull(s.Directive);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestDispIdDirective() {
            var s = RunEmptyCstTest(p => p.ParseDispIdDirective());
            Assert.IsNotNull(s.DispId);
            Assert.IsNotNull(s.DispExpression);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestHintDirective() {
            var s = RunEmptyCstTest(p => p.ParseHint(), "library");
            Assert.IsNotNull(s.Symbol);
            Assert.IsNotNull(s.DeprecatedComment);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual("library".Length, s.Length);
        }


        [TestCase]
        public void TestProgram() {
            var s = RunEmptyCstTest(p => p.ParseProgram(new FileReference(CstPath)));
            Assert.IsNotNull(s.ProgramHead);
            Assert.IsNotNull(s.Uses);
            Assert.IsNotNull(s.MainBlock);
            Assert.IsNotNull(s.Dot);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestLibrary() {
            var s = RunEmptyCstTest(p => p.ParseLibrary(new FileReference(CstPath)));
            Assert.IsNotNull(s.LibraryHead);
            Assert.IsNotNull(s.Uses);
            Assert.IsNotNull(s.MainBlock);
            Assert.IsNotNull(s.Dot);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestLibraryHead() {
            var s = RunEmptyCstTest(p => p.ParseLibraryHead());
            Assert.IsNotNull(s.LibrarySymbol);
            Assert.IsNotNull(s.LibraryName);
            Assert.IsNotNull(s.Hints);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestProgramHeader() {
            var s = RunEmptyCstTest(p => p.ParseProgramHead());
            Assert.IsNotNull(s.ProgramSymbol);
            Assert.IsNotNull(s.Name);
            Assert.IsNotNull(s.Parameters);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(0, s.Length);
        }


        [TestCase]
        public void TestUnit() {
            var s = RunEmptyCstTest(p => p.ParseUnit(new FileReference(CstPath)));
            Assert.IsNotNull(s.UnitHead);
            Assert.IsNotNull(s.UnitInterface);
            Assert.IsNotNull(s.UnitImplementation);
            Assert.IsNotNull(s.UnitBlock);
            Assert.IsNotNull(s.DotSymbol);
            Assert.AreEqual(0, s.Length);
        }

    }
}
