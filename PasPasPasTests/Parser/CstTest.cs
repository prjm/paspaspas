using PasPasPas.Infrastructure.Files;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.SyntaxTree.Standard;
using PasPasPasTests.Common;

namespace PasPasPasTests.Parser {

    public class CstTest : ParserTestBase {

        [TestCase]
        public void TestAbstract() {
            var s = RunEmptyCstTest(p => p.ParseAbstractDirective(), "abstract ;");
            Assert.IsNotNull(s.Directive);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(10, s.Length);

            s = RunEmptyCstTest(p => p.ParseAbstractDirective(), "final ;");
            Assert.IsNotNull(s.Directive);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(7, s.Length);
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
            var s = RunEmptyCstTest(p => p.ParseBindingDirective(), "message WM_TEXT;");
            Assert.IsNotNull(s.Directive);
            Assert.IsNotNull(s.MessageExpression);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(16, s.Length);
        }

        [TestCase]
        public void TestCallConvention() {
            var s = RunEmptyCstTest(p => p.ParseCallConvention(), "cdecl;");
            Assert.IsNotNull(s.Directive);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(6, s.Length);
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
            //Assert.IsNotNull(s.Hints);
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
            var s = RunEmptyCstTest(p => p.ParseArrayIndex(), "1");
            Assert.IsNotNull(s.StartIndex);
            Assert.AreEqual(1, s.Length);

            s = RunEmptyCstTest(p => p.ParseArrayIndex(), "1..2");
            Assert.IsNotNull(s.StartIndex);
            Assert.IsNotNull(s.DotDot);
            Assert.IsNotNull(s.EndIndex);
            Assert.AreEqual(4, s.Length);

            s = RunEmptyCstTest(p => p.ParseArrayIndex(), "1..2,");
            Assert.IsNotNull(s.StartIndex);
            Assert.IsNotNull(s.DotDot);
            Assert.IsNotNull(s.EndIndex);
            Assert.IsNotNull(s.Comma);
            Assert.AreEqual(5, s.Length);
        }

        [TestCase]
        public void TestArrayType() {
            var s = RunEmptyCstTest(p => p.ParseArrayType(), "array of const");
            Assert.IsNotNull(s.Array);
            Assert.IsNotNull(s.OfSymbol);
            Assert.IsNotNull(s.ConstSymbol);
            Assert.AreEqual(14, s.Length);

            s = RunEmptyCstTest(p => p.ParseArrayType(), "array [1] of const");
            Assert.IsNotNull(s.Array);
            Assert.IsNotNull(s.OpenBraces);
            Assert.IsTrue(s.Items.Length == 1);
            Assert.IsNotNull(s.CloseBraces);
            Assert.IsNotNull(s.OfSymbol);
            Assert.IsNotNull(s.ConstSymbol);
            Assert.AreEqual(18, s.Length);

            s = RunEmptyCstTest(p => p.ParseArrayType(), "array [1,1] of Integer");
            Assert.IsNotNull(s.Array);
            Assert.IsNotNull(s.OpenBraces);
            Assert.IsTrue(s.Items.Length == 2);
            Assert.IsNotNull(s.CloseBraces);
            Assert.IsNotNull(s.OfSymbol);
            Assert.IsNotNull(s.TypeSpecification);
            Assert.AreEqual(22, s.Length);
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
            var s = RunEmptyCstTest(p => p.ParseAsmBlock(), "asm end");
            Assert.IsNotNull(s.AsmSymbol);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(7, s.Length);

            s = RunEmptyCstTest(p => p.ParseAsmBlock(), "asm int 3 end");
            Assert.IsNotNull(s.AsmSymbol);
            Assert.IsNotNull(s.EndSymbol);
            Assert.IsTrue(s.Items.Length == 1);
            Assert.AreEqual(13, s.Length);

            s = RunEmptyCstTest(p => p.ParseAsmBlock(), "asm .noframe end");
            Assert.IsNotNull(s.AsmSymbol);
            Assert.IsNotNull(s.EndSymbol);
            Assert.IsTrue(s.Items.Length == 1);
            Assert.AreEqual(16, s.Length);
        }

        [TestCase]
        public void TestBlock() {
            var s = RunEmptyCstTest(p => p.ParseBlock(), "const x = 5");
            Assert.IsNotNull(s.DeclarationSections);
            Assert.AreEqual(11, s.Length);

            s = RunEmptyCstTest(p => p.ParseBlock(), "const x = 5; begin end");
            Assert.IsNotNull(s.DeclarationSections);
            Assert.IsNotNull(s.Body);
            Assert.AreEqual(22, s.Length);
        }

        [TestCase]
        public void TestAsmFactor() {
            var s = RunEmptyCstTest(p => p.ParseAssemblyFactor(), "cs:3");
            Assert.IsNotNull(s.SegmentPrefix);
            Assert.IsNotNull(s.ColonSymbol);
            Assert.IsNotNull(s.SegmentExpression);
            Assert.AreEqual(4, s.Length);

            s = RunEmptyCstTest(p => p.ParseAssemblyFactor(), "(3)");
            Assert.IsNotNull(s.OpenParen);
            Assert.IsNotNull(s.Subexpression);
            Assert.IsNotNull(s.CloseParen);
            Assert.AreEqual(3, s.Length);

            s = RunEmptyCstTest(p => p.ParseAssemblyFactor(), "[3]");
            Assert.IsNotNull(s.OpenBraces);
            Assert.IsNotNull(s.MemorySubexpression);
            Assert.IsNotNull(s.CloseBraces);
            Assert.AreEqual(3, s.Length);

            s = RunEmptyCstTest(p => p.ParseAssemblyFactor(), "@x");
            Assert.IsNotNull(s.Label);
            Assert.AreEqual(2, s.Length);

            s = RunEmptyCstTest(p => p.ParseAssemblyFactor(), "'a'");
            Assert.IsNotNull(s.QuotedString);
            Assert.AreEqual(3, s.Length);

            s = RunEmptyCstTest(p => p.ParseAssemblyFactor(), "\"a\"");
            Assert.IsNotNull(s.QuotedString);
            Assert.AreEqual(3, s.Length);

            s = RunEmptyCstTest(p => p.ParseAssemblyFactor(), "$3");
            Assert.IsNotNull(s.HexNumber);
            Assert.AreEqual(2, s.Length);

            s = RunEmptyCstTest(p => p.ParseAssemblyFactor(), "3");
            Assert.IsNotNull(s.Number);
            Assert.AreEqual(1, s.Length);

            s = RunEmptyCstTest(p => p.ParseAssemblyFactor(), "3.1");
            Assert.IsNotNull(s.RealNumber);
            Assert.AreEqual(3, s.Length);
        }

        [TestCase]
        public void TestAsmLabel() {
            var s = RunEmptyCstTest(p => p.ParseAssemblyLabel(), "a");
            Assert.IsNotNull(s.Label);
            Assert.AreEqual(1, s.Length);

            s = RunEmptyCstTest(p => p.ParseAssemblyLabel(), "@a");
            Assert.IsNotNull(s.LocalLabel);
            Assert.AreEqual(2, s.Length);
        }

        [TestCase]
        public void TestRealNumberSymobl() {
            var s = RunEmptyCstTest(p => p.RequireRealValue(), "2.5");
            Assert.IsNotNull(s.Symbol);
            Assert.AreEqual(3, s.Length);
        }

        [TestCase]
        public void TestAsmOpCodeSymbol() {
            var s = RunEmptyCstTest(p => p.ParseAssemblyOpcode() as AsmOpCodeSymbol, "int");
            Assert.IsNotNull(s.OpCode);
            Assert.AreEqual(3, s.Length);
        }

        [TestCase]
        public void TestAsmPrefixSymbol() {
            var s = RunEmptyCstTest(p => p.ParseAssemblyPrefix() as AsmPrefixSymbol, "lock cs");
            Assert.IsNotNull(s.LockPrefix);
            Assert.IsNotNull(s.SegmentPrefix);
            Assert.AreEqual(7, s.Length);

            s = RunEmptyCstTest(p => p.ParseAssemblyPrefix() as AsmPrefixSymbol, "cs lock");
            Assert.IsNotNull(s.LockPrefix);
            Assert.IsNotNull(s.SegmentPrefix);
            Assert.AreEqual(7, s.Length);
        }

        [TestCase]
        public void TestCaseStatement() {
            var s = RunEmptyCstTest(p => p.ParseCaseStatement(), "case a of 1: x; end");
            Assert.IsNotNull(s.CaseSymbol);
            Assert.IsNotNull(s.CaseExpression);
            Assert.IsNotNull(s.OfSymbol);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(19, s.Length);

            s = RunEmptyCstTest(p => p.ParseCaseStatement(), "case a of 1: x; else y; end");
            Assert.IsNotNull(s.CaseSymbol);
            Assert.IsNotNull(s.CaseExpression);
            Assert.IsNotNull(s.OfSymbol);
            Assert.IsNotNull(s.ElseSymbol);
            Assert.IsNotNull(s.Else);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(27, s.Length);
        }

        [TestCase]
        public void TestCaseItem() {
            var s = RunEmptyCstTest(p => p.ParseCaseItem(), "5: x;");
            Assert.IsNotNull(s.ColonSymbol);
            Assert.IsNotNull(s.CaseStatement);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(5, s.Length);
        }

        [TestCase]
        public void TestClassDeclaration() {
            var s = RunEmptyCstTest(p => p.ParseClassDeclaration(), "class of TAbc");
            Assert.IsNotNull(s.ClassOf);
            Assert.AreEqual(13, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassDeclaration(), "class (TObject) end");
            Assert.IsNotNull(s.ClassDef);
            Assert.AreEqual(19, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassDeclaration(), "class helper for TObject end");
            Assert.IsNotNull(s.ClassHelper);
            Assert.AreEqual(28, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassDeclaration(), "interface end");
            Assert.IsNotNull(s.InterfaceDef);
            Assert.AreEqual(13, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassDeclaration(), "object end");
            Assert.IsNotNull(s.ObjectDecl);
            Assert.AreEqual(10, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassDeclaration(), "record a,b,c: Integer; end");
            Assert.IsNotNull(s.RecordDecl);
            Assert.AreEqual(26, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassDeclaration(), "record helper for Tabc end");
            Assert.IsNotNull(s.RecordHelper);
            Assert.AreEqual(26, s.Length);
        }

        [TestCase]
        public void TestCaseLabel() {
            var s = RunEmptyCstTest(p => p.ParseCaseLabel(), "5");
            Assert.IsNotNull(s.StartExpression);
            Assert.AreEqual(1, s.Length);

            s = RunEmptyCstTest(p => p.ParseCaseLabel(), "5..7");
            Assert.IsNotNull(s.StartExpression);
            Assert.IsNotNull(s.Dots);
            Assert.IsNotNull(s.EndExpression);
            Assert.AreEqual(4, s.Length);

            s = RunEmptyCstTest(p => p.ParseCaseLabel(), "5..7,");
            Assert.IsNotNull(s.StartExpression);
            Assert.IsNotNull(s.Dots);
            Assert.IsNotNull(s.EndExpression);
            Assert.AreEqual(5, s.Length);

        }

        [TestCase]
        public void TestBlockBody() {
            var s = RunEmptyCstTest(p => p.ParseBlockBody() as BlockBodySymbol, "asm end");
            Assert.IsNotNull(s.AssemblerBlock);
            Assert.AreEqual(7, s.Length);

            s = RunEmptyCstTest(p => p.ParseBlockBody() as BlockBodySymbol, "begin end");
            Assert.IsNotNull(s.Body);
            Assert.AreEqual(9, s.Length);
        }

        [TestCase]
        public void TestAsmPseudoOpSymbol() {
            var s = RunEmptyCstTest(p => p.ParseAsmPseudoOp(), ".PARAMS 3");
            Assert.IsNotNull(s.DotSymbol);
            Assert.IsTrue(s.Mode == AsmPrefixSymbolKind.ParamsOperation);
            Assert.IsNotNull(s.NumberOfParams);
            Assert.AreEqual(9, s.Length);

            s = RunEmptyCstTest(p => p.ParseAsmPseudoOp(), ".PUSHENV EAX");
            Assert.IsNotNull(s.DotSymbol);
            Assert.IsTrue(s.Mode == AsmPrefixSymbolKind.PushEnvOperation);
            Assert.IsNotNull(s.Kind);
            Assert.IsNotNull(s.Register);
            Assert.AreEqual(12, s.Length);

            s = RunEmptyCstTest(p => p.ParseAsmPseudoOp(), ".NOFRAME");
            Assert.IsNotNull(s.DotSymbol);
            Assert.IsTrue(s.Mode == AsmPrefixSymbolKind.NoFrame);
            Assert.IsNotNull(s.Kind);
            Assert.AreEqual(8, s.Length);
        }

        [TestCase]
        public void TestAsmStatement() {
            var s = RunEmptyCstTest(p => p.ParseAsmStatement(), "x: lock int 3");
            Assert.IsNotNull(s.Label);
            Assert.IsNotNull(s.ColonSymbol);
            Assert.IsNotNull(s.OpCode);
            Assert.IsNotNull(s.Prefix);
            Assert.AreEqual(13, s.Length);
        }

        [TestCase]
        public void TestAsmTerm() {
            var s = RunEmptyCstTest(p => p.ParseAssemblyTerm(), "3 / 3");
            Assert.IsNotNull(s.LeftOperand);
            Assert.IsNotNull(s.Operator);
            Assert.IsNotNull(s.RightOperand);
            Assert.AreEqual(5, s.Length);

            s = RunEmptyCstTest(p => p.ParseAssemblyTerm(), "EAX.4");
            Assert.IsNotNull(s.DotSymbol);
            Assert.IsNotNull(s.Subtype);
            Assert.AreEqual(5, s.Length);
        }

        [TestCase]
        public void TestAsmOperand() {
            var s = RunEmptyCstTest(p => p.ParseAssemblyOperand(), "not x");
            Assert.IsNotNull(s.NotSymbol);
            Assert.IsNotNull(s.NotExpression);
            Assert.AreEqual(5, s.Length);

            s = RunEmptyCstTest(p => p.ParseAssemblyOperand(true), "a and b, ");
            Assert.IsNotNull(s.Operand);
            Assert.IsNotNull(s.LeftTerm);
            Assert.IsNotNull(s.RightTerm);
            Assert.IsNotNull(s.Comma);
            Assert.AreEqual(9, s.Length);
        }

        [TestCase]
        public void TestAsmExpression() {
            var s = RunEmptyCstTest(p => p.ParseAssemblyExpression(), "offset 3");
            Assert.IsNotNull(s.OffsetSymbol);
            Assert.IsNotNull(s.Offset);
            Assert.AreEqual(8, s.Length);

            s = RunEmptyCstTest(p => p.ParseAssemblyExpression(), "byte 3");
            Assert.IsNotNull(s.BytePtrKind);
            Assert.IsNotNull(s.BytePtr);
            Assert.AreEqual(6, s.Length);

            s = RunEmptyCstTest(p => p.ParseAssemblyExpression(), "type 3");
            Assert.IsNotNull(s.TypeSymbol);
            Assert.IsNotNull(s.TypeExpression);
            Assert.AreEqual(6, s.Length);

            s = RunEmptyCstTest(p => p.ParseAssemblyExpression(), "3");
            Assert.IsNotNull(s.LeftOperand);
            Assert.AreEqual(1, s.Length);

            s = RunEmptyCstTest(p => p.ParseAssemblyExpression(), "3 + 4");
            Assert.IsNotNull(s.LeftOperand);
            Assert.IsNotNull(s.Operator);
            Assert.IsNotNull(s.RightOperand);
            Assert.AreEqual(5, s.Length);
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
        public void TestClassDeclarationItems() {
            var s = RunEmptyCstTest(p => p.ParseClassDeclartionItems(), "var a,b: integer;");
            Assert.AreEqual(17, s.Length);
        }

        [TestCase]
        public void TestClassDeclarationItem() {
            var mode = ClassDeclarationMode.Other;

            var s = RunEmptyCstTest(p => p.ParseClassDeclarationItem(ref mode), "var");
            Assert.AreEqual(mode, ClassDeclarationMode.Fields);
            Assert.IsNotNull(s.VarSymbol);
            Assert.AreEqual(3, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassDeclarationItem(ref mode), "class var");
            Assert.AreEqual(mode, ClassDeclarationMode.ClassFields);
            Assert.IsNotNull(s.VarSymbol);
            Assert.IsNotNull(s.ClassSymbol);
            Assert.AreEqual(9, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassDeclarationItem(ref mode), "protected");
            Assert.AreEqual(mode, ClassDeclarationMode.Fields);
            Assert.AreEqual(s.Visibility, TokenKind.Protected);
            Assert.IsNotNull(s.VisibilitySymbol);
            Assert.AreEqual(9, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassDeclarationItem(ref mode), "strict private");
            Assert.AreEqual(mode, ClassDeclarationMode.Fields);
            Assert.AreEqual(s.Visibility, TokenKind.Private);
            Assert.IsNotNull(s.StrictSymbol);
            Assert.IsNotNull(s.VisibilitySymbol);
            Assert.AreEqual(14, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassDeclarationItem(ref mode), "function x: integer;");
            Assert.AreEqual(mode, ClassDeclarationMode.Other);
            Assert.IsNotNull(s.MethodDeclaration);
            Assert.AreEqual(20, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassDeclarationItem(ref mode), "class function x: integer;");
            Assert.AreEqual(mode, ClassDeclarationMode.Other);
            Assert.IsNotNull(s.MethodDeclaration);
            Assert.IsNotNull(s.ClassSymbol);
            Assert.AreEqual(26, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassDeclarationItem(ref mode), "class property x: integer read p write p;");
            Assert.AreEqual(mode, ClassDeclarationMode.Other);
            Assert.IsNotNull(s.PropertyDeclaration);
            Assert.IsNotNull(s.ClassSymbol);
            Assert.AreEqual(41, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassDeclarationItem(ref mode), "const x = 4;");
            Assert.AreEqual(mode, ClassDeclarationMode.Other);
            Assert.IsNotNull(s.ConstSection);
            Assert.AreEqual(12, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassDeclarationItem(ref mode), "type x = string;");
            Assert.AreEqual(mode, ClassDeclarationMode.Other);
            Assert.IsNotNull(s.TypeSection);
            Assert.AreEqual(16, s.Length);

            mode = ClassDeclarationMode.Fields;
            s = RunEmptyCstTest(p => p.ParseClassDeclarationItem(ref mode), "x: string;");
            Assert.AreEqual(mode, ClassDeclarationMode.Fields);
            Assert.IsNotNull(s.FieldDeclaration);
            Assert.AreEqual(10, s.Length);
        }

        [TestCase]
        public void TestClassDeclarationSymbol() {
            var s = RunEmptyCstTest(p => p.ParseClassDefinition(), "class");
            Assert.IsTrue(s.ForwardDeclaration);
            Assert.IsNotNull(s.ClassSymbol);
            Assert.AreEqual(5, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassDefinition(), "class sealed end");
            Assert.IsFalse(s.ForwardDeclaration);
            Assert.IsNotNull(s.ClassSymbol);
            Assert.IsNotNull(s.SealedSymbol);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(16, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassDefinition(), "class abstract end");
            Assert.IsFalse(s.ForwardDeclaration);
            Assert.IsNotNull(s.ClassSymbol);
            Assert.IsNotNull(s.AbstractSymbol);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(18, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassDefinition(), "class (TDummy) end");
            Assert.IsFalse(s.ForwardDeclaration);
            Assert.IsNotNull(s.ClassSymbol);
            Assert.IsNotNull(s.ClassParent);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(18, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassDefinition(), "class x: Integer; end");
            Assert.IsFalse(s.ForwardDeclaration);
            Assert.IsNotNull(s.ClassSymbol);
            Assert.IsNotNull(s.ClassItems);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(21, s.Length);
        }

        [TestCase]
        public void TestClassField() {
            var s = RunEmptyCstTest(p => p.ParseClassFieldDeclararation(), "s: string deprecated;");
            Assert.IsNotNull(s.Names);
            Assert.IsNotNull(s.ColonSymbol);
            Assert.IsNotNull(s.TypeDecl);
            Assert.IsNotNull(s.Hint);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(21, s.Length);
        }

        [TestCase]
        public void TestClassHelperDef() {
            var s = RunEmptyCstTest(p => p.ParseClassHelper(), "class helper (TQ) for TObject procedure x; end");
            Assert.IsNotNull(s.ClassSymbol);
            Assert.IsNotNull(s.HelperSymbol);
            Assert.IsNotNull(s.ClassParent);
            Assert.IsNotNull(s.ForSymbol);
            Assert.IsNotNull(s.HelperName);
            Assert.IsNotNull(s.HelperItems);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(46, s.Length);
        }


        [TestCase]
        public void TestClassHelperItem() {
            var mode = ClassDeclarationMode.Other;

            var s = RunEmptyCstTest(p => p.ParseClassHelperItem(ref mode), "var");
            Assert.AreEqual(mode, ClassDeclarationMode.Fields);
            Assert.IsNotNull(s.VarSymbol);
            Assert.AreEqual(3, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassHelperItem(ref mode), "class var");
            Assert.AreEqual(mode, ClassDeclarationMode.ClassFields);
            Assert.IsNotNull(s.VarSymbol);
            Assert.IsNotNull(s.ClassSymbol);
            Assert.AreEqual(9, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassHelperItem(ref mode), "protected");
            Assert.AreEqual(mode, ClassDeclarationMode.Fields);
            Assert.AreEqual(s.Visibility, TokenKind.Protected);
            Assert.IsNotNull(s.Visibility);
            Assert.AreEqual(9, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassHelperItem(ref mode), "strict private");
            Assert.AreEqual(mode, ClassDeclarationMode.Fields);
            Assert.AreEqual(s.Visibility, TokenKind.Private);
            Assert.IsNotNull(s.StrictSymbol);
            Assert.IsNotNull(s.Visibility);
            Assert.AreEqual(14, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassHelperItem(ref mode), "function x: integer;");
            Assert.AreEqual(mode, ClassDeclarationMode.Other);
            Assert.IsNotNull(s.MethodDeclaration);
            Assert.AreEqual(20, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassHelperItem(ref mode), "class function x: integer;");
            Assert.AreEqual(mode, ClassDeclarationMode.Other);
            Assert.IsNotNull(s.MethodDeclaration);
            Assert.IsNotNull(s.ClassSymbol);
            Assert.AreEqual(26, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassHelperItem(ref mode), "class property x: integer read p write p;");
            Assert.AreEqual(mode, ClassDeclarationMode.Other);
            Assert.IsNotNull(s.PropertyDeclaration);
            Assert.IsNotNull(s.ClassSymbol);
            Assert.AreEqual(41, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassHelperItem(ref mode), "const x = 4;");
            Assert.AreEqual(mode, ClassDeclarationMode.Other);
            Assert.IsNotNull(s.ConstDeclaration);
            Assert.AreEqual(12, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassHelperItem(ref mode), "type x = string;");
            Assert.AreEqual(mode, ClassDeclarationMode.Other);
            Assert.IsNotNull(s.TypeSection);
            Assert.AreEqual(16, s.Length);

            mode = ClassDeclarationMode.Fields;
            s = RunEmptyCstTest(p => p.ParseClassHelperItem(ref mode), "x: string;");
            Assert.AreEqual(mode, ClassDeclarationMode.Fields);
            Assert.IsNotNull(s.FieldDeclaration);
            Assert.AreEqual(10, s.Length);
        }

        [TestCase]
        public void TestClassHelperItems() {
            var s = RunEmptyCstTest(p => p.ParseClassHelperItems(), "const x = 5");
            Assert.AreEqual(1, s.Items.Length);
            Assert.AreEqual(11, s.Length);
        }

        [TestCase]
        public void TestClassMethod() {
            var s = RunEmptyCstTest(p => p.ParseMethodDeclaration(), "procedure x;");
            Assert.IsNotNull(s.MethodSymbol);
            Assert.IsNotNull(s.Identifier);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(12, s.Length);

            s = RunEmptyCstTest(p => p.ParseMethodDeclaration(), "procedure x<T>;");
            Assert.IsNotNull(s.MethodSymbol);
            Assert.IsNotNull(s.Identifier);
            Assert.IsNotNull(s.GenericDefinition);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(15, s.Length);

            s = RunEmptyCstTest(p => p.ParseMethodDeclaration(), "function x<T>(): [x] T; overload;");
            Assert.IsNotNull(s.MethodSymbol);
            Assert.IsNotNull(s.Identifier);
            Assert.IsNotNull(s.GenericDefinition);
            Assert.IsNotNull(s.OpenParen);
            Assert.IsNotNull(s.Parameters);
            Assert.IsNotNull(s.CloseParen);
            Assert.IsNotNull(s.ColonSymbol);
            Assert.IsNotNull(s.ResultAttributes);
            Assert.IsNotNull(s.ResultType);
            Assert.IsNotNull(s.Semicolon);
            Assert.IsNotNull(s.Directives);
            Assert.AreEqual(33, s.Length);
        }

        [TestCase]
        public void TestClassOfDeclaration() {
            var s = RunEmptyCstTest(p => p.ParseClassOfDeclaration(), "class of tzing");
            Assert.IsNotNull(s.ClassSymbol);
            Assert.IsNotNull(s.OfSymbol);
            Assert.IsNotNull(s.TypeRef);
            Assert.AreEqual(14, s.Length);
        }


        [TestCase]
        public void TestClassProperty() {
            var s = RunEmptyCstTest(p => p.ParsePropertyDeclaration(), "property x [a: string]: string index 4 read q; default;");
            Assert.IsNotNull(s.PropertySymbol);
            Assert.IsNotNull(s.PropertyName);
            Assert.IsNotNull(s.OpenBraces);
            Assert.IsNotNull(s.ArrayIndex);
            Assert.IsNotNull(s.CloseBraces);
            Assert.IsNotNull(s.ColonSymbol);
            Assert.IsNotNull(s.TypeName);
            Assert.IsNotNull(s.IndexSymbol);
            Assert.IsNotNull(s.PropertyIndex);
            Assert.IsNotNull(s.Semicolon);
            Assert.IsNotNull(s.DefaultSymbol);
            Assert.IsNotNull(s.Semicolon2);
            Assert.AreEqual(55, s.Length);
        }

        [TestCase]
        public void TestClassPropertySpecifier() {
            var s = RunEmptyCstTest(p => p.ParseClassPropertyAccessSpecifier(), "read x");
            Assert.IsNotNull(s.PropertyReadWrite);
            Assert.AreEqual(6, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassPropertyAccessSpecifier(), "dispid 5");
            Assert.IsNotNull(s.PropertyDispInterface);
            Assert.AreEqual(8, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassPropertyAccessSpecifier(), "stored true");
            Assert.IsNotNull(s.StoredSymbol);
            Assert.IsNotNull(s.StoredProperty);
            Assert.AreEqual(11, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassPropertyAccessSpecifier(), "default 4");
            Assert.IsNotNull(s.DefaultSymbol);
            Assert.IsNotNull(s.DefaultProperty);
            Assert.AreEqual(9, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassPropertyAccessSpecifier(), "nodefault");
            Assert.IsNotNull(s.NoDefaultSymbol);
            Assert.AreEqual(9, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassPropertyAccessSpecifier(), "implements a");
            Assert.IsNotNull(s.ImplentsSymbol);
            Assert.IsNotNull(s.ImplementsTypeId);
            Assert.AreEqual(12, s.Length);
        }


        [TestCase]
        public void TestClassPropertyReadWrite() {
            var s = RunEmptyCstTest(p => p.ParseClassPropertyReadWrite(), "read x");
            Assert.IsNotNull(s.Modifier);
            Assert.IsNotNull(s.Member);
            Assert.AreEqual(6, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassPropertyReadWrite(), "write x");
            Assert.IsNotNull(s.Modifier);
            Assert.IsNotNull(s.Member);
            Assert.AreEqual(7, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassPropertyReadWrite(), "add x");
            Assert.IsNotNull(s.Modifier);
            Assert.IsNotNull(s.Member);
            Assert.AreEqual(5, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassPropertyReadWrite(), "remove x");
            Assert.IsNotNull(s.Modifier);
            Assert.IsNotNull(s.Member);
            Assert.AreEqual(8, s.Length);
        }


        [TestCase]
        public void TestClassPropertyDispIntf() {
            var s = RunEmptyCstTest(p => p.ParseClassPropertyDispInterface(), "readonly");
            Assert.IsNotNull(s.Modifier);
            Assert.AreEqual(8, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassPropertyDispInterface(), "writeonly");
            Assert.IsNotNull(s.Modifier);
            Assert.AreEqual(9, s.Length);

            s = RunEmptyCstTest(p => p.ParseClassPropertyDispInterface(), "dispid 5");
            Assert.IsNotNull(s.DispId);
            Assert.AreEqual(8, s.Length);
        }

        [TestCase]
        public void TestClosureExpression() {
            var s = RunEmptyCstTest(p => p.ParseClosureExpression(), "function (p: Integer): Boolean begin end");
            Assert.IsNotNull(s.ProcSymbol);
            Assert.IsNotNull(s.Parameters);
            Assert.IsNotNull(s.ColonSymbol);
            Assert.IsNotNull(s.ReturnType);
            Assert.IsNotNull(s.Block);
            Assert.AreEqual(40, s.Length);
        }

        [TestCase]
        public void TestCompoundStatement() {
            var s = RunEmptyCstTest(p => p.ParseCompoundStatement(), "asm end");
            Assert.IsNotNull(s.AssemblerBlock);
            Assert.AreEqual(7, s.Length);

            s = RunEmptyCstTest(p => p.ParseCompoundStatement(), "begin x; end");
            Assert.IsNotNull(s.BeginSymbol);
            Assert.IsNotNull(s.Statements);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(12, s.Length);
        }


        [TestCase]
        public void TestConstantExpression() {
            var s = RunEmptyCstTest(p => p.ParseConstantExpression(), "5");
            Assert.IsNotNull(s.Value);
            Assert.AreEqual(1, s.Length);

            s = RunEmptyCstTest(p => p.ParseConstantExpression(), "(1,3,5)");
            Assert.IsNotNull(s.OpenParen);
            Assert.IsNotNull(s.Items);
            Assert.IsNotNull(s.CloseParen);
            Assert.IsTrue(s.IsArrayConstant);
            Assert.AreEqual(7, s.Length);

            s = RunEmptyCstTest(p => p.ParseConstantExpression(), "(a: 5)");
            Assert.IsNotNull(s.OpenParen);
            Assert.IsNotNull(s.Items);
            Assert.IsNotNull(s.CloseParen);
            Assert.IsTrue(s.IsRecordConstant);
            Assert.AreEqual(6, s.Length);
        }

        [TestCase]
        public void TestConstDeclaration() {
            var s = RunEmptyCstTest(p => p.ParseConstDeclaration(), "[a,b] a: TA = 5 library;");
            Assert.IsNotNull(s.Attributes);
            Assert.IsNotNull(s.Identifier);
            Assert.IsNotNull(s.Colon);
            Assert.IsNotNull(s.TypeSpecification);
            Assert.IsNotNull(s.EqualsSign);
            Assert.IsNotNull(s.Value);
            Assert.IsNotNull(s.Hint);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(24, s.Length);
        }

        [TestCase]
        public void TestConstSection() {
            var s = RunEmptyCstTest(p => p.ParseConstSection(true), "const a = 5;");
            Assert.IsNotNull(s.ConstSymbol);
            Assert.IsNotNull(s.Items);
            Assert.AreEqual(12, s.Length);
        }

        [TestCase]
        public void TestConstrainedGeneric() {
            var s = RunEmptyCstTest(p => p.ParseGenericConstraint(true), "constructor, ");
            Assert.IsNotNull(s.ConstraintSymbol);
            Assert.IsNotNull(s.Comma);
            Assert.AreEqual(13, s.Length);

            s = RunEmptyCstTest(p => p.ParseGenericConstraint(true), "fonstructor, ");
            Assert.IsNotNull(s.ConstraintIdentifier);
            Assert.IsNotNull(s.Comma);
            Assert.AreEqual(13, s.Length);
        }

        [TestCase]
        public void TestDeclarations() {
            var s = RunEmptyCstTest(p => p.ParseDeclarationSections(), "label x;");
            Assert.IsNotNull(s.Items[0]);
            Assert.AreEqual(8, s.Length);

            s = RunEmptyCstTest(p => p.ParseDeclarationSections(), "const x = 5;");
            Assert.IsNotNull(s.Items[0]);
            Assert.AreEqual(12, s.Length);

            s = RunEmptyCstTest(p => p.ParseDeclarationSections(), "type TA = string;");
            Assert.IsNotNull(s.Items[0]);
            Assert.AreEqual(17, s.Length);

            s = RunEmptyCstTest(p => p.ParseDeclarationSections(), "var a: Integer = 5;");
            Assert.IsNotNull(s.Items[0]);
            Assert.AreEqual(19, s.Length);

            s = RunEmptyCstTest(p => p.ParseDeclarationSections(), "exports a(x: string);");
            Assert.IsNotNull(s.Items[0]);
            Assert.AreEqual(21, s.Length);

            s = RunEmptyCstTest(p => p.ParseDeclarationSections(), "procedure TA<B>.C(const A: STRING): Integer; begin end;");
            Assert.IsNotNull(s.Items[0]);
            Assert.AreEqual(55, s.Length);

            s = RunEmptyCstTest(p => p.ParseDeclarationSections(), "function a(const A: TA<A>): Integer; begin end;");
            Assert.IsNotNull(s.Items[0]);
            Assert.AreEqual(47, s.Length);
        }

        [TestCase]
        public void TestDesignatorItem() {
            var s = RunEmptyCstTest(p => p.ParseDesignatorItem(true), "^");
            Assert.AreEqual(1, s.Length);

            s = RunEmptyCstTest(p => p.ParseDesignatorItem(true), ".a[x]");
            Assert.AreEqual(5, s.Length);

            s = RunEmptyCstTest(p => p.ParseDesignatorItem(true), ".a<b>[x]");
            Assert.AreEqual(8, s.Length);
        }

    }
}