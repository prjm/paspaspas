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
        public void TestArrayIndex() {
            var s = RunEmptyCstTest(p => p.ParseArrayIndex());
            Assert.IsNotNull(s.StartIndex);
            Assert.IsNotNull(s.DotDot);
            Assert.IsNotNull(s.EndIndex);
            Assert.IsNotNull(s.Comma);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestArrayType() {
            var s = RunEmptyCstTest(p => p.ParseArrayType());
            Assert.IsNotNull(s.Array);
            Assert.IsNotNull(s.OpenBraces);
            Assert.IsNotNull(s.CloseBraces);
            Assert.IsNotNull(s.OfSymbol);
            Assert.IsNotNull(s.ConstSymbol);
            Assert.IsNotNull(s.TypeSpecification);
            Assert.AreEqual(0, s.Length);
        }


        [TestCase]
        public void TestInterfaceSection() {
            var s = RunEmptyCstTest(p => p.ParseUnitInterface());
            Assert.IsNotNull(s.InterfaceSymbol);
            Assert.IsNotNull(s.UsesClause);
            Assert.IsNotNull(s.InterfaceDeclaration);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestInterfaceDeclaration() {
            var s = RunEmptyCstTest(p => p.ParseInterfaceDeclaration());
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestAsmBlock() {
            var s = RunEmptyCstTest(p => p.ParseAsmBlock());
            Assert.IsNotNull(s.AsmSymbol);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestBlock() {
            var s = RunEmptyCstTest(p => p.ParseBlock());
            Assert.IsNotNull(s.DeclarationSections);
            Assert.IsNotNull(s.Body);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestAsmFactor() {
            var s = RunEmptyCstTest(p => p.ParseAssemblyFactor());
            Assert.IsNotNull(s.SegmentPrefix);
            Assert.IsNotNull(s.ColonSymbol);
            Assert.IsNotNull(s.SegmentExpression);
            Assert.IsNotNull(s.OpenParen);
            Assert.IsNotNull(s.Subexpression);
            Assert.IsNotNull(s.CloseParen);
            Assert.IsNotNull(s.OpenBraces);
            Assert.IsNotNull(s.MemorySubexpression);
            Assert.IsNotNull(s.CloseBraces);
            Assert.IsNotNull(s.Identifier);
            Assert.IsNotNull(s.Number);
            Assert.IsNotNull(s.RealNumber);
            Assert.IsNotNull(s.HexNumber);
            Assert.IsNotNull(s.QuotedString);
            Assert.IsNotNull(s.Label);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestAsmLabel() {
            var s = RunEmptyCstTest(p => p.ParseAssemblyLabel());
            Assert.IsNotNull(s.Label);
            Assert.IsNotNull(s.LocalLabel);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestAsmOpCodeSymbol() {
            var s = RunEmptyCstTest(p => p.ParseAssemblyOpcode() as AsmOpCodeSymbol);
            Assert.IsNotNull(s.OpCode);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestAsmPrefixymbol() {
            var s = RunEmptyCstTest(p => p.ParseAssemblyPrefix() as AsmPrefixSymbol, "lock");
            Assert.IsNotNull(s.LockPrefix);
            Assert.IsNotNull(s.SegmentPrefix);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestCaseStatement() {
            var s = RunEmptyCstTest(p => p.ParseCaseStatement(), "");
            Assert.IsNotNull(s.CaseSymbol);
            Assert.IsNotNull(s.CaseExpression);
            Assert.IsNotNull(s.OfSymbol);
            Assert.IsNotNull(s.ElseSymbol);
            Assert.IsNotNull(s.Else);
            Assert.IsNotNull(s.Semicolon);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestCaseItem() {
            var s = RunEmptyCstTest(p => p.ParseCaseItem(), "5: ;");
            Assert.IsNotNull(s.ColonSymbol);
            Assert.IsNotNull(s.CaseStatement);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(3, s.Length);
        }

        [TestCase]
        public void TestClassDeclaration() {
            var s = RunEmptyCstTest(p => p.ParseClassDeclaration(), "5: ;");
            Assert.IsNotNull(s.ClassOf);
            Assert.IsNotNull(s.ClassHelper);
            Assert.IsNotNull(s.ClassDef);
            Assert.IsNotNull(s.InterfaceDef);
            Assert.IsNotNull(s.ObjectDecl);
            Assert.IsNotNull(s.RecordDecl);
            Assert.IsNotNull(s.RecordHelper);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestCaseLabel() {
            var s = RunEmptyCstTest(p => p.ParseCaseLabel(), "");
            Assert.IsNotNull(s.StartExpression);
            Assert.IsNotNull(s.Dots);
            Assert.IsNotNull(s.EndExpression);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestBlockBody() {
            var s = RunEmptyCstTest(p => p.ParseBlockBody() as BlockBodySymbol, "");
            Assert.IsNotNull(s.AssemblerBlock);
            Assert.IsNotNull(s.Body);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestAsmPseudoOpSymbol() {
            var s = RunEmptyCstTest(p => p.ParseAsmPseudoOp(), "");
            Assert.IsNotNull(s.DotSymbol);
            Assert.IsNotNull(s.Kind);
            Assert.IsNotNull(s.NumberOfParams);
            Assert.IsNotNull(s.Register);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestAsmStatement() {
            var s = RunEmptyCstTest(p => p.ParseAsmStatement(), "");
            Assert.IsNotNull(s.Label);
            Assert.IsNotNull(s.ColonSymbol);
            Assert.IsNotNull(s.OpCode);
            Assert.IsNotNull(s.Prefix);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestAsmTerm() {
            var s = RunEmptyCstTest(p => p.ParseAssemblyTerm(), "");
            Assert.IsNotNull(s.DotSymbol);
            Assert.IsNotNull(s.Subtype);
            Assert.IsNotNull(s.LeftOperand);
            Assert.IsNotNull(s.Operator);
            Assert.IsNotNull(s.RightOperand);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestAssemblySymbol() {
            var s = RunEmptyCstTest(p => p.ParseAssemblyAttribute(), "");
            Assert.IsNotNull(s.OpenBraces);
            Assert.IsNotNull(s.AssemblySymbol);
            Assert.IsNotNull(s.ColonSymbol);
            Assert.IsNotNull(s.Attribute);
            Assert.IsNotNull(s.CloseBraces);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestAsmOperand() {
            var s = RunEmptyCstTest(p => p.ParseAssemblyOperand());
            Assert.IsNotNull(s.NotSymbol);
            Assert.IsNotNull(s.NotExpression);
            Assert.IsNotNull(s.Operand);
            Assert.IsNotNull(s.LeftTerm);
            Assert.IsNotNull(s.RightTerm);
            Assert.IsNotNull(s.Comma);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestAsmExpression() {
            var s = RunEmptyCstTest(p => p.ParseAssemblyExpression());
            Assert.IsNotNull(s.OffsetSymbol);
            Assert.IsNotNull(s.Offset);
            Assert.IsNotNull(s.BytePtrKind);
            Assert.IsNotNull(s.BytePtr);
            Assert.IsNotNull(s.TypeSymbol);
            Assert.IsNotNull(s.TypeExpression);
            Assert.IsNotNull(s.LeftOperand);
            Assert.IsNotNull(s.RightOperand);
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

        [TestCase]
        public void TestClassDeclarationItem() {
            var mode = ClassDeclarationMode.Other;
            var s = RunEmptyCstTest(p => p.ParseClassDeclarationItem(ref mode));
            Assert.IsNotNull(s.Attributes1);
            Assert.IsNotNull(s.ClassSymbol);
            Assert.IsNotNull(s.Attributes2);
            Assert.IsNotNull(s.VarSymbol);
            Assert.IsNotNull(s.StrictSymbol);
            Assert.IsNotNull(s.MethodResolution);
            Assert.IsNotNull(s.MethodDeclaration);
            Assert.IsNotNull(s.PropertyDeclaration);
            Assert.IsNotNull(s.ConstSection);
            Assert.IsNotNull(s.TypeSection);
            Assert.IsNotNull(s.FieldDeclaration);
            Assert.AreEqual(0, s.Length);
        }

    }
}
