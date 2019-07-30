using PasPasPas.Globals.Files;
using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.SyntaxTree.Standard;
using PasPasPasTests.Common;

namespace PasPasPasTests.Parser {

    public class CstTest : ParserTestBase {

        [TestMethod]
        public void TestAbstract() {
            var s = RunCstTest(p => p.ParseAbstractDirective(), "abstract ;");
            Assert.IsNotNull(s.Directive);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(10, s.Length);

            s = RunCstTest(p => p.ParseAbstractDirective(), "final ;");
            Assert.IsNotNull(s.Directive);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(7, s.Length);
        }

        [TestMethod]
        public void TestReintroduce() {
            var s = RunCstTest(p => p.ParseReintroduceDirective(), "reintroduce;");
            Assert.IsNotNull(s.Directive);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(12, s.Length);
        }

        [TestMethod]
        public void TestOverload() {
            var s = RunCstTest(p => p.ParseOverloadDirective(), "overload;");
            Assert.IsNotNull(s.Directive);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(9, s.Length);
        }

        [TestMethod]
        public void TestInline() {
            var s = RunCstTest(p => p.ParseInlineDirective(), "inline ;");
            Assert.IsNotNull(s.Directive);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(8, s.Length);

            s = RunCstTest(p => p.ParseInlineDirective(), "assembler ;");
            Assert.IsNotNull(s.Directive);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(11, s.Length);
        }

        [TestMethod]
        public void TestBinding() {
            var s = RunCstTest(p => p.ParseBindingDirective(), "message WM_TEXT;");
            Assert.IsNotNull(s.Directive);
            Assert.IsNotNull(s.MessageExpression);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(16, s.Length);
        }

        [TestMethod]
        public void TestCallConvention() {
            var s = RunCstTest(p => p.ParseCallConvention(), "cdecl;");
            Assert.IsNotNull(s.Directive);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(6, s.Length);
        }

        [TestMethod]
        public void TestHintDirective() {
            var s = RunCstTest(p => p.ParseHint(false), "library");
            Assert.IsNotNull(s.Symbol);
            Assert.AreEqual(7, s.Length);

            s = RunCstTest(p => p.ParseHint(false), "deprecated 'a'");
            Assert.IsNotNull(s.Symbol);
            Assert.IsNotNull(s.DeprecatedComment);
            Assert.AreEqual(14, s.Length);
        }

        [TestMethod]
        public void TestHintList() {
            var s = RunCstTest(p => p.ParseHints(true), "library; deprecated;");
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[0].Semicolon);
            Assert.IsNotNull(s.Items[1]);
            Assert.IsNotNull(s.Items[1].Semicolon);
            Assert.AreEqual(20, s.Length);

        }

        [TestMethod]
        public void TestProgram() {
            var s = RunCstTest(p => p.ParseProgram(new FileReference(CstPath)), "program z.x; uses a in 'a'; begin x; end.");
            Assert.IsNotNull(s.ProgramHead);
            Assert.IsNotNull(s.Uses);
            Assert.IsNotNull(s.MainBlock);
            Assert.IsNotNull(s.Dot);
            Assert.AreEqual(41, s.Length);
        }

        [TestMethod]
        public void TestLibrary() {
            var s = RunCstTest(p => p.ParseLibrary(new FileReference(CstPath)), "library a; uses x in 'x'; begin end.");
            Assert.IsNotNull(s.LibraryHead);
            Assert.IsNotNull(s.Uses);
            Assert.IsNotNull(s.MainBlock);
            Assert.IsNotNull(s.Dot);
            Assert.AreEqual(36, s.Length);
        }

        [TestMethod]
        public void TestLibraryHead() {
            var s = RunCstTest(p => p.ParseLibraryHead(), "library a.b deprecated;");
            Assert.IsNotNull(s.LibrarySymbol);
            Assert.IsNotNull(s.LibraryName);
            Assert.IsNotNull(s.Hints);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(23, s.Length);
        }

        [TestMethod]
        public void TestProgramHeader() {
            var s = RunCstTest(p => p.ParseProgramHead(), "program a.b(x,y);");
            Assert.IsNotNull(s.ProgramSymbol);
            Assert.IsNotNull(s.Name);
            Assert.IsNotNull(s.Parameters);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(17, s.Length);
        }

        [TestMethod]
        public void TestArrayIndex() {
            var s = RunCstTest(p => p.ParseArrayIndex(), "1");
            Assert.IsNotNull(s.StartIndex);
            Assert.AreEqual(1, s.Length);

            s = RunCstTest(p => p.ParseArrayIndex(), "1..2");
            Assert.IsNotNull(s.StartIndex);
            Assert.IsNotNull(s.DotDot);
            Assert.IsNotNull(s.EndIndex);
            Assert.AreEqual(4, s.Length);

            s = RunCstTest(p => p.ParseArrayIndex(), "1..2,");
            Assert.IsNotNull(s.StartIndex);
            Assert.IsNotNull(s.DotDot);
            Assert.IsNotNull(s.EndIndex);
            Assert.IsNotNull(s.Comma);
            Assert.AreEqual(5, s.Length);
        }

        [TestMethod]
        public void TestArrayType() {
            var s = RunCstTest(p => p.ParseArrayType(), "array of const");
            Assert.IsNotNull(s.Array);
            Assert.IsNotNull(s.OfSymbol);
            Assert.IsNotNull(s.ConstSymbol);
            Assert.AreEqual(14, s.Length);

            s = RunCstTest(p => p.ParseArrayType(), "array [1] of const");
            Assert.IsNotNull(s.Array);
            Assert.IsNotNull(s.OpenBraces);
            Assert.IsTrue(s.Items.Length == 1);
            Assert.IsNotNull(s.CloseBraces);
            Assert.IsNotNull(s.OfSymbol);
            Assert.IsNotNull(s.ConstSymbol);
            Assert.AreEqual(18, s.Length);

            s = RunCstTest(p => p.ParseArrayType(), "array [1,1] of Integer");
            Assert.IsNotNull(s.Array);
            Assert.IsNotNull(s.OpenBraces);
            Assert.IsTrue(s.Items.Length == 2);
            Assert.IsNotNull(s.CloseBraces);
            Assert.IsNotNull(s.OfSymbol);
            Assert.IsNotNull(s.TypeSpecification);
            Assert.AreEqual(22, s.Length);
        }


        [TestMethod]
        public void TestInterfaceSection() {
            var s = RunCstTest(p => p.ParseUnitInterface(), "interface uses a; type x = class;");
            Assert.IsNotNull(s.InterfaceSymbol);
            Assert.IsNotNull(s.UsesClause);
            Assert.IsNotNull(s.InterfaceDeclaration);
            Assert.AreEqual(33, s.Length);
        }

        [TestMethod]
        public void TestInterfaceDeclaration() {
            var s = RunCstTest(p => p.ParseInterfaceDeclaration(), "const a = 5;");
            Assert.IsNotNull(s.Items[0]);
            Assert.AreEqual(12, s.Length);
        }

        [TestMethod]
        public void TestInterfaceDefinition() {
            var s = RunCstTest(p => p.ParseInterfaceDef(), "interface(a) ['a'] function a:x; end");
            Assert.IsNotNull(s.InterfaceSymbol);
            Assert.IsNotNull(s.ParentInterface);
            Assert.IsNotNull(s.GuidSymbol);
            Assert.IsNotNull(s.Items);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(36, s.Length);
        }

        [TestMethod]
        public void TestAsmBlock() {
            var s = RunCstTest(p => p.ParseAsmBlock(), "asm end");
            Assert.IsNotNull(s.AsmSymbol);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(7, s.Length);

            s = RunCstTest(p => p.ParseAsmBlock(), "asm int 3 end");
            Assert.IsNotNull(s.AsmSymbol);
            Assert.IsNotNull(s.EndSymbol);
            Assert.IsTrue(s.Items.Length == 1);
            Assert.AreEqual(13, s.Length);

            s = RunCstTest(p => p.ParseAsmBlock(), "asm .noframe end");
            Assert.IsNotNull(s.AsmSymbol);
            Assert.IsNotNull(s.EndSymbol);
            Assert.IsTrue(s.Items.Length == 1);
            Assert.AreEqual(16, s.Length);
        }

        [TestMethod]
        public void TestBlock() {
            var s = RunCstTest(p => p.ParseBlock(), "const x = 5;");
            Assert.IsNotNull(s.DeclarationSections);
            Assert.AreEqual(12, s.Length);

            s = RunCstTest(p => p.ParseBlock(), "const x = 5; begin end");
            Assert.IsNotNull(s.DeclarationSections);
            Assert.IsNotNull(s.Body);
            Assert.AreEqual(22, s.Length);
        }

        [TestMethod]
        public void TestAsmFactor() {
            var s = RunCstTest(p => p.ParseAssemblyFactor(), "cs:3");
            Assert.IsNotNull(s.SegmentPrefix);
            Assert.IsNotNull(s.ColonSymbol);
            Assert.IsNotNull(s.SegmentExpression);
            Assert.AreEqual(4, s.Length);

            s = RunCstTest(p => p.ParseAssemblyFactor(), "(3)");
            Assert.IsNotNull(s.OpenParen);
            Assert.IsNotNull(s.Subexpression);
            Assert.IsNotNull(s.CloseParen);
            Assert.AreEqual(3, s.Length);

            s = RunCstTest(p => p.ParseAssemblyFactor(), "[3]");
            Assert.IsNotNull(s.OpenBraces);
            Assert.IsNotNull(s.MemorySubexpression);
            Assert.IsNotNull(s.CloseBraces);
            Assert.AreEqual(3, s.Length);

            s = RunCstTest(p => p.ParseAssemblyFactor(), "@x");
            Assert.IsNotNull(s.Label);
            Assert.AreEqual(2, s.Length);

            s = RunCstTest(p => p.ParseAssemblyFactor(), "'a'");
            Assert.IsNotNull(s.QuotedString);
            Assert.AreEqual(3, s.Length);

            s = RunCstTest(p => p.ParseAssemblyFactor(), "\"a\"");
            Assert.IsNotNull(s.QuotedString);
            Assert.AreEqual(3, s.Length);

            s = RunCstTest(p => p.ParseAssemblyFactor(), "$3");
            Assert.IsNotNull(s.HexNumber);
            Assert.AreEqual(2, s.Length);

            s = RunCstTest(p => p.ParseAssemblyFactor(), "3");
            Assert.IsNotNull(s.Number);
            Assert.AreEqual(1, s.Length);

            s = RunCstTest(p => p.ParseAssemblyFactor(), "3.1");
            Assert.IsNotNull(s.RealNumber);
            Assert.AreEqual(3, s.Length);
        }

        [TestMethod]
        public void TestAsmLabel() {
            var s = RunCstTest(p => p.ParseAssemblyLabel(), "a");
            Assert.IsNotNull(s.Label);
            Assert.AreEqual(1, s.Length);

            s = RunCstTest(p => p.ParseAssemblyLabel(), "@a");
            Assert.IsNotNull(s.LocalLabel);
            Assert.AreEqual(2, s.Length);
        }

        [TestMethod]
        public void TestRealNumberSymobl() {
            var s = RunCstTest(p => p.RequireRealValue(), "2.5");
            Assert.IsNotNull(s.Symbol);
            Assert.AreEqual(3, s.Length);
        }

        [TestMethod]
        public void TestAsmOpCodeSymbol() {
            var s = RunCstTest(p => p.ParseAssemblyOpcode() as AsmOpCodeSymbol, "int");
            Assert.IsNotNull(s.OpCode);
            Assert.AreEqual(3, s.Length);
        }

        [TestMethod]
        public void TestAsmPrefixSymbol() {
            var s = RunCstTest(p => p.ParseAssemblyPrefix() as AsmPrefixSymbol, "lock cs");
            Assert.IsNotNull(s.LockPrefix);
            Assert.IsNotNull(s.SegmentPrefix);
            Assert.AreEqual(7, s.Length);

            s = RunCstTest(p => p.ParseAssemblyPrefix() as AsmPrefixSymbol, "cs lock");
            Assert.IsNotNull(s.LockPrefix);
            Assert.IsNotNull(s.SegmentPrefix);
            Assert.AreEqual(7, s.Length);
        }

        [TestMethod]
        public void TestCaseStatement() {
            var s = RunCstTest(p => p.ParseCaseStatement(), "case a of 1: x; end");
            Assert.IsNotNull(s.CaseSymbol);
            Assert.IsNotNull(s.CaseExpression);
            Assert.IsNotNull(s.OfSymbol);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(19, s.Length);

            s = RunCstTest(p => p.ParseCaseStatement(), "case a of 1: x; else y; end");
            Assert.IsNotNull(s.CaseSymbol);
            Assert.IsNotNull(s.CaseExpression);
            Assert.IsNotNull(s.OfSymbol);
            Assert.IsNotNull(s.ElseSymbol);
            Assert.IsNotNull(s.Else);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(27, s.Length);
        }

        [TestMethod]
        public void TestCaseItem() {
            var s = RunCstTest(p => p.ParseCaseItem(), "5: x;");
            Assert.IsNotNull(s.ColonSymbol);
            Assert.IsNotNull(s.CaseStatement);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(5, s.Length);
        }

        [TestMethod]
        public void TestClassDeclaration() {
            var s = RunCstTest(p => p.ParseClassDeclaration(), "class of TAbc");
            Assert.IsNotNull(s.ClassOf);
            Assert.AreEqual(13, s.Length);

            s = RunCstTest(p => p.ParseClassDeclaration(), "class (TObject) end");
            Assert.IsNotNull(s.ClassDef);
            Assert.AreEqual(19, s.Length);

            s = RunCstTest(p => p.ParseClassDeclaration(), "class helper for TObject end");
            Assert.IsNotNull(s.ClassHelper);
            Assert.AreEqual(28, s.Length);

            s = RunCstTest(p => p.ParseClassDeclaration(), "interface end");
            Assert.IsNotNull(s.InterfaceDef);
            Assert.AreEqual(13, s.Length);

            s = RunCstTest(p => p.ParseClassDeclaration(), "object end");
            Assert.IsNotNull(s.ObjectDecl);
            Assert.AreEqual(10, s.Length);

            s = RunCstTest(p => p.ParseClassDeclaration(), "record a,b,c: Integer; end");
            Assert.IsNotNull(s.RecordDecl);
            Assert.AreEqual(26, s.Length);

            s = RunCstTest(p => p.ParseClassDeclaration(), "record helper for Tabc end");
            Assert.IsNotNull(s.RecordHelper);
            Assert.AreEqual(26, s.Length);
        }

        [TestMethod]
        public void TestCaseLabel() {
            var s = RunCstTest(p => p.ParseCaseLabel(), "5");
            Assert.IsNotNull(s.StartExpression);
            Assert.AreEqual(1, s.Length);

            s = RunCstTest(p => p.ParseCaseLabel(), "5..7");
            Assert.IsNotNull(s.StartExpression);
            Assert.IsNotNull(s.Dots);
            Assert.IsNotNull(s.EndExpression);
            Assert.AreEqual(4, s.Length);

            s = RunCstTest(p => p.ParseCaseLabel(), "5..7,");
            Assert.IsNotNull(s.StartExpression);
            Assert.IsNotNull(s.Dots);
            Assert.IsNotNull(s.EndExpression);
            Assert.AreEqual(5, s.Length);

        }

        [TestMethod]
        public void TestBlockBody() {
            var s = RunCstTest(p => p.ParseBlockBody() as BlockBodySymbol, "asm end");
            Assert.IsNotNull(s.AssemblerBlock);
            Assert.AreEqual(7, s.Length);

            s = RunCstTest(p => p.ParseBlockBody() as BlockBodySymbol, "begin end");
            Assert.IsNotNull(s.Body);
            Assert.AreEqual(9, s.Length);
        }

        [TestMethod]
        public void TestAsmPseudoOpSymbol() {
            var s = RunCstTest(p => p.ParseAsmPseudoOp(), ".PARAMS 3");
            Assert.IsNotNull(s.DotSymbol);
            Assert.IsTrue(s.Mode == AsmPrefixSymbolKind.ParamsOperation);
            Assert.IsNotNull(s.NumberOfParams);
            Assert.AreEqual(9, s.Length);

            s = RunCstTest(p => p.ParseAsmPseudoOp(), ".PUSHENV EAX");
            Assert.IsNotNull(s.DotSymbol);
            Assert.IsTrue(s.Mode == AsmPrefixSymbolKind.PushEnvOperation);
            Assert.IsNotNull(s.Kind);
            Assert.IsNotNull(s.Register);
            Assert.AreEqual(12, s.Length);

            s = RunCstTest(p => p.ParseAsmPseudoOp(), ".NOFRAME");
            Assert.IsNotNull(s.DotSymbol);
            Assert.IsTrue(s.Mode == AsmPrefixSymbolKind.NoFrame);
            Assert.IsNotNull(s.Kind);
            Assert.AreEqual(8, s.Length);
        }

        [TestMethod]
        public void TestAsmStatement() {
            var s = RunCstTest(p => p.ParseAsmStatement(), "x: lock int 3");
            Assert.IsNotNull(s.Label);
            Assert.IsNotNull(s.ColonSymbol);
            Assert.IsNotNull(s.OpCode);
            Assert.IsNotNull(s.Prefix);
            Assert.AreEqual(13, s.Length);
        }

        [TestMethod]
        public void TestAsmTerm() {
            var s = RunCstTest(p => p.ParseAssemblyTerm(), "3 / 3");
            Assert.IsNotNull(s.LeftOperand);
            Assert.IsNotNull(s.Operator);
            Assert.IsNotNull(s.RightOperand);
            Assert.AreEqual(5, s.Length);

            s = RunCstTest(p => p.ParseAssemblyTerm(), "EAX.4");
            Assert.IsNotNull(s.DotSymbol);
            Assert.IsNotNull(s.Subtype);
            Assert.AreEqual(5, s.Length);
        }

        [TestMethod]
        public void TestAsmOperand() {
            var s = RunCstTest(p => p.ParseAssemblyOperand(), "not x");
            Assert.IsNotNull(s.NotSymbol);
            Assert.IsNotNull(s.NotExpression);
            Assert.AreEqual(5, s.Length);

            s = RunCstTest(p => p.ParseAssemblyOperand(true), "a and b, ");
            Assert.IsNotNull(s.Operand);
            Assert.IsNotNull(s.LeftTerm);
            Assert.IsNotNull(s.RightTerm);
            Assert.IsNotNull(s.Comma);
            Assert.AreEqual(9, s.Length);
        }

        [TestMethod]
        public void TestAsmExpression() {
            var s = RunCstTest(p => p.ParseAssemblyExpression(), "offset 3");
            Assert.IsNotNull(s.OffsetSymbol);
            Assert.IsNotNull(s.Offset);
            Assert.AreEqual(8, s.Length);

            s = RunCstTest(p => p.ParseAssemblyExpression(), "byte 3");
            Assert.IsNotNull(s.BytePtrKind);
            Assert.IsNotNull(s.BytePtr);
            Assert.AreEqual(6, s.Length);

            s = RunCstTest(p => p.ParseAssemblyExpression(), "type 3");
            Assert.IsNotNull(s.TypeSymbol);
            Assert.IsNotNull(s.TypeExpression);
            Assert.AreEqual(6, s.Length);

            s = RunCstTest(p => p.ParseAssemblyExpression(), "3");
            Assert.IsNotNull(s.LeftOperand);
            Assert.AreEqual(1, s.Length);

            s = RunCstTest(p => p.ParseAssemblyExpression(), "3 + 4");
            Assert.IsNotNull(s.LeftOperand);
            Assert.IsNotNull(s.Operator);
            Assert.IsNotNull(s.RightOperand);
            Assert.AreEqual(5, s.Length);
        }

        [TestMethod]
        public void TestUnit() {
            var s = RunCstTest(p => p.ParseUnit(new FileReference(CstPath)), "unit z.x; interface implementation initialization finalization end.");
            Assert.IsNotNull(s.UnitHead);
            Assert.IsNotNull(s.UnitInterface);
            Assert.IsNotNull(s.UnitImplementation);
            Assert.IsNotNull(s.UnitBlock);
            Assert.IsNotNull(s.DotSymbol);
            Assert.AreEqual(67, s.Length);
        }


        [TestMethod]
        public void TestClassDeclarationItems() {
            var s = RunCstTest(p => p.ParseClassDeclartionItems(), "var a,b: integer;");
            Assert.AreEqual(17, s.Length);
        }

        [TestMethod]
        public void TestClassDeclarationItem() {
            var mode = ClassDeclarationMode.Other;

            var s = RunCstTest(p => p.ParseClassDeclarationItem(ref mode), "var");
            Assert.AreEqual(mode, ClassDeclarationMode.Fields);
            Assert.IsNotNull(s.VarSymbol);
            Assert.AreEqual(3, s.Length);

            s = RunCstTest(p => p.ParseClassDeclarationItem(ref mode), "class var");
            Assert.AreEqual(mode, ClassDeclarationMode.ClassFields);
            Assert.IsNotNull(s.VarSymbol);
            Assert.IsNotNull(s.ClassSymbol);
            Assert.AreEqual(9, s.Length);

            s = RunCstTest(p => p.ParseClassDeclarationItem(ref mode), "protected");
            Assert.AreEqual(mode, ClassDeclarationMode.Fields);
            Assert.AreEqual(s.Visibility, TokenKind.Protected);
            Assert.IsNotNull(s.VisibilitySymbol);
            Assert.AreEqual(9, s.Length);

            s = RunCstTest(p => p.ParseClassDeclarationItem(ref mode), "strict private");
            Assert.AreEqual(mode, ClassDeclarationMode.Fields);
            Assert.AreEqual(s.Visibility, TokenKind.Private);
            Assert.IsNotNull(s.StrictSymbol);
            Assert.IsNotNull(s.VisibilitySymbol);
            Assert.AreEqual(14, s.Length);

            s = RunCstTest(p => p.ParseClassDeclarationItem(ref mode), "function x: integer;");
            Assert.AreEqual(mode, ClassDeclarationMode.Other);
            Assert.IsNotNull(s.MethodDeclaration);
            Assert.AreEqual(20, s.Length);

            s = RunCstTest(p => p.ParseClassDeclarationItem(ref mode), "class function x: integer;");
            Assert.AreEqual(mode, ClassDeclarationMode.Other);
            Assert.IsNotNull(s.MethodDeclaration);
            Assert.IsNotNull(s.ClassSymbol);
            Assert.AreEqual(26, s.Length);

            s = RunCstTest(p => p.ParseClassDeclarationItem(ref mode), "class property x: integer read p write p;");
            Assert.AreEqual(mode, ClassDeclarationMode.Other);
            Assert.IsNotNull(s.PropertyDeclaration);
            Assert.IsNotNull(s.ClassSymbol);
            Assert.AreEqual(41, s.Length);

            s = RunCstTest(p => p.ParseClassDeclarationItem(ref mode), "const x = 4;");
            Assert.AreEqual(mode, ClassDeclarationMode.Other);
            Assert.IsNotNull(s.ConstSection);
            Assert.AreEqual(12, s.Length);

            s = RunCstTest(p => p.ParseClassDeclarationItem(ref mode), "type x = string;");
            Assert.AreEqual(mode, ClassDeclarationMode.Other);
            Assert.IsNotNull(s.TypeSection);
            Assert.AreEqual(16, s.Length);

            mode = ClassDeclarationMode.Fields;
            s = RunCstTest(p => p.ParseClassDeclarationItem(ref mode), "x: string;");
            Assert.AreEqual(mode, ClassDeclarationMode.Fields);
            Assert.IsNotNull(s.FieldDeclaration);
            Assert.AreEqual(10, s.Length);
        }

        [TestMethod]
        public void TestClassDeclarationSymbol() {
            var s = RunCstTest(p => p.ParseClassDefinition(), "class");
            Assert.IsTrue(s.ForwardDeclaration);
            Assert.IsNotNull(s.ClassSymbol);
            Assert.AreEqual(5, s.Length);

            s = RunCstTest(p => p.ParseClassDefinition(), "class sealed end");
            Assert.IsFalse(s.ForwardDeclaration);
            Assert.IsNotNull(s.ClassSymbol);
            Assert.IsNotNull(s.SealedSymbol);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(16, s.Length);

            s = RunCstTest(p => p.ParseClassDefinition(), "class abstract end");
            Assert.IsFalse(s.ForwardDeclaration);
            Assert.IsNotNull(s.ClassSymbol);
            Assert.IsNotNull(s.AbstractSymbol);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(18, s.Length);

            s = RunCstTest(p => p.ParseClassDefinition(), "class (TDummy) end");
            Assert.IsFalse(s.ForwardDeclaration);
            Assert.IsNotNull(s.ClassSymbol);
            Assert.IsNotNull(s.ClassParent);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(18, s.Length);

            s = RunCstTest(p => p.ParseClassDefinition(), "class x: Integer; end");
            Assert.IsFalse(s.ForwardDeclaration);
            Assert.IsNotNull(s.ClassSymbol);
            Assert.IsNotNull(s.ClassItems);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(21, s.Length);
        }

        [TestMethod]
        public void TestClassField() {
            var s = RunCstTest(p => p.ParseClassFieldDeclararation(), "s: string deprecated;");
            Assert.IsNotNull(s.Names);
            Assert.IsNotNull(s.ColonSymbol);
            Assert.IsNotNull(s.TypeDecl);
            Assert.IsNotNull(s.Hint);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(21, s.Length);
        }

        [TestMethod]
        public void TestClassHelperDef() {
            var s = RunCstTest(p => p.ParseClassHelper(), "class helper (TQ) for TObject procedure x; end");
            Assert.IsNotNull(s.ClassSymbol);
            Assert.IsNotNull(s.HelperSymbol);
            Assert.IsNotNull(s.ClassParent);
            Assert.IsNotNull(s.ForSymbol);
            Assert.IsNotNull(s.HelperName);
            Assert.IsNotNull(s.HelperItems);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(46, s.Length);
        }


        [TestMethod]
        public void TestClassHelperItem() {
            var mode = ClassDeclarationMode.Other;

            var s = RunCstTest(p => p.ParseClassHelperItem(ref mode), "var");
            Assert.AreEqual(mode, ClassDeclarationMode.Fields);
            Assert.IsNotNull(s.VarSymbol);
            Assert.AreEqual(3, s.Length);

            s = RunCstTest(p => p.ParseClassHelperItem(ref mode), "class var");
            Assert.AreEqual(mode, ClassDeclarationMode.ClassFields);
            Assert.IsNotNull(s.VarSymbol);
            Assert.IsNotNull(s.ClassSymbol);
            Assert.AreEqual(9, s.Length);

            s = RunCstTest(p => p.ParseClassHelperItem(ref mode), "protected");
            Assert.AreEqual(mode, ClassDeclarationMode.Fields);
            Assert.AreEqual(s.Visibility, TokenKind.Protected);
            Assert.IsNotNull(s.Visibility);
            Assert.AreEqual(9, s.Length);

            s = RunCstTest(p => p.ParseClassHelperItem(ref mode), "strict private");
            Assert.AreEqual(mode, ClassDeclarationMode.Fields);
            Assert.AreEqual(s.Visibility, TokenKind.Private);
            Assert.IsNotNull(s.StrictSymbol);
            Assert.IsNotNull(s.Visibility);
            Assert.AreEqual(14, s.Length);

            s = RunCstTest(p => p.ParseClassHelperItem(ref mode), "function x: integer;");
            Assert.AreEqual(mode, ClassDeclarationMode.Other);
            Assert.IsNotNull(s.MethodDeclaration);
            Assert.AreEqual(20, s.Length);

            s = RunCstTest(p => p.ParseClassHelperItem(ref mode), "class function x: integer;");
            Assert.AreEqual(mode, ClassDeclarationMode.Other);
            Assert.IsNotNull(s.MethodDeclaration);
            Assert.IsNotNull(s.ClassSymbol);
            Assert.AreEqual(26, s.Length);

            s = RunCstTest(p => p.ParseClassHelperItem(ref mode), "class property x: integer read p write p;");
            Assert.AreEqual(mode, ClassDeclarationMode.Other);
            Assert.IsNotNull(s.PropertyDeclaration);
            Assert.IsNotNull(s.ClassSymbol);
            Assert.AreEqual(41, s.Length);

            s = RunCstTest(p => p.ParseClassHelperItem(ref mode), "const x = 4;");
            Assert.AreEqual(mode, ClassDeclarationMode.Other);
            Assert.IsNotNull(s.ConstDeclaration);
            Assert.AreEqual(12, s.Length);

            s = RunCstTest(p => p.ParseClassHelperItem(ref mode), "type x = string;");
            Assert.AreEqual(mode, ClassDeclarationMode.Other);
            Assert.IsNotNull(s.TypeSection);
            Assert.AreEqual(16, s.Length);

            mode = ClassDeclarationMode.Fields;
            s = RunCstTest(p => p.ParseClassHelperItem(ref mode), "x: string;");
            Assert.AreEqual(mode, ClassDeclarationMode.Fields);
            Assert.IsNotNull(s.FieldDeclaration);
            Assert.AreEqual(10, s.Length);
        }

        [TestMethod]
        public void TestClassHelperItems() {
            var s = RunCstTest(p => p.ParseClassHelperItems(), "const x = 5;");
            Assert.AreEqual(1, s.Items.Length);
            Assert.AreEqual(12, s.Length);
        }

        [TestMethod]
        public void TestClassMethod() {
            var s = RunCstTest(p => p.ParseMethodDeclaration(), "procedure x;");
            Assert.IsNotNull(s.MethodSymbol);
            Assert.IsNotNull(s.Identifier);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(12, s.Length);

            s = RunCstTest(p => p.ParseMethodDeclaration(), "procedure x<T>;");
            Assert.IsNotNull(s.MethodSymbol);
            Assert.IsNotNull(s.Identifier);
            Assert.IsNotNull(s.GenericDefinition);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(15, s.Length);

            s = RunCstTest(p => p.ParseMethodDeclaration(), "function x<T>(): [x] T; overload;");
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

        [TestMethod]
        public void TestClassOfDeclaration() {
            var s = RunCstTest(p => p.ParseClassOfDeclaration(), "class of tzing");
            Assert.IsNotNull(s.ClassSymbol);
            Assert.IsNotNull(s.OfSymbol);
            Assert.IsNotNull(s.TypeRef);
            Assert.AreEqual(14, s.Length);
        }


        [TestMethod]
        public void TestClassProperty() {
            var s = RunCstTest(p => p.ParsePropertyDeclaration(), "property x [a: string]: string index 4 read q; default;");
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

        [TestMethod]
        public void TestClassPropertySpecifier() {
            var s = RunCstTest(p => p.ParseClassPropertyAccessSpecifier(), "read x");
            Assert.IsNotNull(s.PropertyReadWrite);
            Assert.AreEqual(6, s.Length);

            s = RunCstTest(p => p.ParseClassPropertyAccessSpecifier(), "dispid 5");
            Assert.IsNotNull(s.PropertyDispInterface);
            Assert.AreEqual(8, s.Length);

            s = RunCstTest(p => p.ParseClassPropertyAccessSpecifier(), "stored true");
            Assert.IsNotNull(s.StoredSymbol);
            Assert.IsNotNull(s.StoredProperty);
            Assert.AreEqual(11, s.Length);

            s = RunCstTest(p => p.ParseClassPropertyAccessSpecifier(), "default 4");
            Assert.IsNotNull(s.DefaultSymbol);
            Assert.IsNotNull(s.DefaultProperty);
            Assert.AreEqual(9, s.Length);

            s = RunCstTest(p => p.ParseClassPropertyAccessSpecifier(), "nodefault");
            Assert.IsNotNull(s.NoDefaultSymbol);
            Assert.AreEqual(9, s.Length);

            s = RunCstTest(p => p.ParseClassPropertyAccessSpecifier(), "implements a");
            Assert.IsNotNull(s.ImplentsSymbol);
            Assert.IsNotNull(s.ImplementsTypeId);
            Assert.AreEqual(12, s.Length);
        }


        [TestMethod]
        public void TestClassPropertyReadWrite() {
            var s = RunCstTest(p => p.ParseClassPropertyReadWrite(), "read x");
            Assert.IsNotNull(s.Modifier);
            Assert.IsNotNull(s.Member);
            Assert.AreEqual(6, s.Length);

            s = RunCstTest(p => p.ParseClassPropertyReadWrite(), "write x");
            Assert.IsNotNull(s.Modifier);
            Assert.IsNotNull(s.Member);
            Assert.AreEqual(7, s.Length);

            s = RunCstTest(p => p.ParseClassPropertyReadWrite(), "add x");
            Assert.IsNotNull(s.Modifier);
            Assert.IsNotNull(s.Member);
            Assert.AreEqual(5, s.Length);

            s = RunCstTest(p => p.ParseClassPropertyReadWrite(), "remove x");
            Assert.IsNotNull(s.Modifier);
            Assert.IsNotNull(s.Member);
            Assert.AreEqual(8, s.Length);
        }


        [TestMethod]
        public void TestClassPropertyDispIntf() {
            var s = RunCstTest(p => p.ParseClassPropertyDispInterface(), "readonly");
            Assert.IsNotNull(s.Modifier);
            Assert.AreEqual(8, s.Length);

            s = RunCstTest(p => p.ParseClassPropertyDispInterface(), "writeonly");
            Assert.IsNotNull(s.Modifier);
            Assert.AreEqual(9, s.Length);

            s = RunCstTest(p => p.ParseClassPropertyDispInterface(), "dispid 5");
            Assert.IsNotNull(s.DispId);
            Assert.AreEqual(8, s.Length);
        }

        [TestMethod]
        public void TestClosureExpression() {
            var s = RunCstTest(p => p.ParseClosureExpression(), "function (p: Integer): Boolean begin end");
            Assert.IsNotNull(s.ProcSymbol);
            Assert.IsNotNull(s.Parameters);
            Assert.IsNotNull(s.ColonSymbol);
            Assert.IsNotNull(s.ReturnType);
            Assert.IsNotNull(s.Block);
            Assert.AreEqual(40, s.Length);
        }

        [TestMethod]
        public void TestCompoundStatement() {
            var s = RunCstTest(p => p.ParseCompoundStatement(), "asm end");
            Assert.IsNotNull(s.AssemblerBlock);
            Assert.AreEqual(7, s.Length);

            s = RunCstTest(p => p.ParseCompoundStatement(), "begin x; end");
            Assert.IsNotNull(s.BeginSymbol);
            Assert.IsNotNull(s.Statements);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(12, s.Length);
        }


        [TestMethod]
        public void TestConstantExpression() {
            var s = RunCstTest(p => p.ParseConstantExpression(), "5");
            Assert.IsNotNull(s.Value);
            Assert.AreEqual(1, s.Length);

            s = RunCstTest(p => p.ParseConstantExpression(), "(1,3,5)");
            Assert.IsNotNull(s.OpenParen);
            Assert.IsNotNull(s.Items);
            Assert.IsNotNull(s.CloseParen);
            Assert.IsTrue(s.IsArrayConstant);
            Assert.AreEqual(7, s.Length);

            s = RunCstTest(p => p.ParseConstantExpression(), "(a: 5)");
            Assert.IsNotNull(s.OpenParen);
            Assert.IsNotNull(s.Items);
            Assert.IsNotNull(s.CloseParen);
            Assert.IsTrue(s.IsRecordConstant);
            Assert.AreEqual(6, s.Length);
        }

        [TestMethod]
        public void TestConstDeclaration() {
            var s = RunCstTest(p => p.ParseConstDeclaration(), "[a,b] a: TA = 5 library;");
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

        [TestMethod]
        public void TestConstSection() {
            var s = RunCstTest(p => p.ParseConstSection(true), "const a = 5;");
            Assert.IsNotNull(s.ConstSymbol);
            Assert.IsNotNull(s.Items);
            Assert.AreEqual(12, s.Length);
        }

        [TestMethod]
        public void TestConstrainedGeneric() {
            var s = RunCstTest(p => p.ParseGenericConstraint(true), "constructor, ");
            Assert.IsNotNull(s.ConstraintSymbol);
            Assert.IsNotNull(s.Comma);
            Assert.AreEqual(13, s.Length);

            s = RunCstTest(p => p.ParseGenericConstraint(true), "fonstructor, ");
            Assert.IsNotNull(s.ConstraintIdentifier);
            Assert.IsNotNull(s.Comma);
            Assert.AreEqual(13, s.Length);
        }

        [TestMethod]
        public void TestDeclarations() {
            var s = RunCstTest(p => p.ParseDeclarationSections(), "label x;");
            Assert.IsNotNull(s.Items[0]);
            Assert.AreEqual(8, s.Length);

            s = RunCstTest(p => p.ParseDeclarationSections(), "const x = 5;");
            Assert.IsNotNull(s.Items[0]);
            Assert.AreEqual(12, s.Length);

            s = RunCstTest(p => p.ParseDeclarationSections(), "type TA = string;");
            Assert.IsNotNull(s.Items[0]);
            Assert.AreEqual(17, s.Length);

            s = RunCstTest(p => p.ParseDeclarationSections(), "var a: Integer = 5;");
            Assert.IsNotNull(s.Items[0]);
            Assert.AreEqual(19, s.Length);

            s = RunCstTest(p => p.ParseDeclarationSections(), "exports a(x: string);");
            Assert.IsNotNull(s.Items[0]);
            Assert.AreEqual(21, s.Length);

            s = RunCstTest(p => p.ParseDeclarationSections(), "procedure TA<B>.C(const A: STRING): Integer; begin end;");
            Assert.IsNotNull(s.Items[0]);
            Assert.AreEqual(55, s.Length);

            s = RunCstTest(p => p.ParseDeclarationSections(), "function a(const A: TA<A>): Integer; begin end;");
            Assert.IsNotNull(s.Items[0]);
            Assert.AreEqual(47, s.Length);
        }

        [TestMethod]
        public void TestDesignatorItem() {
            var s = RunCstTest(p => p.ParseDesignatorItem(false, default), "^");
            Assert.AreEqual(1, s.Length);

            s = RunCstTest(p => p.ParseDesignatorItem(false, default), "a[x]");
            Assert.AreEqual(4, s.Length);

            s = RunCstTest(p => p.ParseDesignatorItem(false, default), "a<b>[x]");
            Assert.AreEqual(7, s.Length);
        }

        [TestMethod]
        public void TestDesignatorStatement() {
            var s = RunCstTest(p => p.ParseDesignator(), "inherited");
            Assert.IsNotNull(s.Inherited);
            Assert.AreEqual(9, s.Length);

            s = RunCstTest(p => p.ParseDesignator(), "inherited a.a");
            Assert.IsNotNull(s.Inherited);
            Assert.IsNotNull(s.Items[0].Subitem);
            Assert.IsNotNull(s.Items[1].DotSymbol);
            Assert.IsNotNull(s.Items[1].Subitem);
            Assert.AreEqual(13, s.Length);

            s = RunCstTest(p => p.ParseDesignator(), "inherited a<x>.a");
            Assert.IsNotNull(s.Inherited);
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[1]);
            Assert.AreEqual(16, s.Length);
        }

        [TestMethod]
        public void TestDispId() {
            var s = RunCstTest(p => p.ParseDispIdDirective(true), "dispid 5;");
            Assert.IsNotNull(s.DispId);
            Assert.IsNotNull(s.DispExpression);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(9, s.Length);

            s = RunCstTest(p => p.ParseDispIdDirective(false), "dispid 5");
            Assert.IsNotNull(s.DispId);
            Assert.IsNotNull(s.DispExpression);
            Assert.AreEqual(8, s.Length);
        }

        [TestMethod]
        public void TestEnumValue() {
            var s = RunCstTest(p => p.ParseEnumTypeValue(), "a");
            Assert.IsNotNull(s.EnumName);
            Assert.AreEqual(1, s.Length);

            s = RunCstTest(p => p.ParseEnumTypeValue(), "a = 5");
            Assert.IsNotNull(s.EnumName);
            Assert.IsNotNull(s.EqualsSymbol);
            Assert.IsNotNull(s.Value);
            Assert.AreEqual(5, s.Length);

            s = RunCstTest(p => p.ParseEnumTypeValue(), "a = 5,");
            Assert.IsNotNull(s.EnumName);
            Assert.IsNotNull(s.EqualsSymbol);
            Assert.IsNotNull(s.Value);
            Assert.IsNotNull(s.Comma);
            Assert.AreEqual(6, s.Length);
        }

        [TestMethod]
        public void TestEnumTypeDef() {
            var s = RunCstTest(p => p.ParseEnumType(), "(a,b,c)");
            Assert.IsNotNull(s.OpenParen);
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[1]);
            Assert.IsNotNull(s.Items[2]);
            Assert.IsNotNull(s.CloseParen);
            Assert.AreEqual(7, s.Length);
        }

        [TestMethod]
        public void TestExceptHandler() {
            var s = RunCstTest(p => p.ParseExceptHandler(), "on e: Exception do begin end;");
            Assert.IsNotNull(s.On);
            Assert.IsNotNull(s.Name);
            Assert.IsNotNull(s.Colon);
            Assert.IsNotNull(s.HandlerType);
            Assert.IsNotNull(s.DoSymbol);
            Assert.IsNotNull(s.Statement);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(29, s.Length);
        }

        [TestMethod]
        public void TestExportedProcHeading() {
            var s = RunCstTest(p => p.ParseExportedProcedureHeading(), "function a(var x: string): tobject; inline;");
            Assert.IsNotNull(s.ProcSymbol);
            Assert.IsNotNull(s.Name);
            Assert.IsNotNull(s.Parameters);
            Assert.IsNotNull(s.ColonSymbol);
            Assert.IsNotNull(s.ResultType);
            Assert.IsNotNull(s.Semicolon);
            Assert.IsNotNull(s.Directives);
            Assert.AreEqual(43, s.Length);
        }

        [TestMethod]
        public void TestExportItem() {
            var s = RunCstTest(p => p.ParseExportItem(false), "a(var x: string) index 3 name '3'");
            Assert.IsNotNull(s.ExportName);
            Assert.IsNotNull(s.Parameters);
            Assert.IsNotNull(s.IndexSymbol);
            Assert.IsNotNull(s.IndexParameter);
            Assert.IsNotNull(s.NameSymbol);
            Assert.IsNotNull(s.NameParameter);
            Assert.AreEqual(33, s.Length);
        }

        [TestMethod]
        public void TestExportsSection() {
            var s = RunCstTest(p => p.ParseExportsSection(), "exports a(x: string) name '3', b() name '4' ;");
            Assert.IsNotNull(s.Exports);
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[0].Comma);
            Assert.IsNotNull(s.Items[1]);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(45, s.Length);
        }

        [TestMethod]
        public void TestExpression() {
            var s = RunCstTest(p => p.ParseExpression(), "procedure () begin end");
            Assert.IsNotNull(s.ClosureExpression);
            Assert.AreEqual(22, s.Length);

            s = RunCstTest(p => p.ParseExpression(), "5");
            Assert.IsNotNull(s.LeftOperand);
            Assert.AreEqual(1, s.Length);

            s = RunCstTest(p => p.ParseExpression(), "5 < 6");
            Assert.IsNotNull(s.LeftOperand);
            Assert.IsNotNull(s.Operator);
            Assert.IsNotNull(s.RightOperand);
            Assert.AreEqual(5, s.Length);

            s = RunCstTest(p => p.ParseExpression(), "5 <= 6");
            Assert.IsNotNull(s.LeftOperand);
            Assert.IsNotNull(s.Operator);
            Assert.IsNotNull(s.RightOperand);
            Assert.AreEqual(6, s.Length);

            s = RunCstTest(p => p.ParseExpression(), "5 > 6");
            Assert.IsNotNull(s.LeftOperand);
            Assert.IsNotNull(s.Operator);
            Assert.IsNotNull(s.RightOperand);
            Assert.AreEqual(5, s.Length);

            s = RunCstTest(p => p.ParseExpression(), "5 >= 6");
            Assert.IsNotNull(s.LeftOperand);
            Assert.IsNotNull(s.Operator);
            Assert.IsNotNull(s.RightOperand);
            Assert.AreEqual(6, s.Length);

            s = RunCstTest(p => p.ParseExpression(), "5 = 6");
            Assert.IsNotNull(s.LeftOperand);
            Assert.IsNotNull(s.Operator);
            Assert.IsNotNull(s.RightOperand);
            Assert.AreEqual(5, s.Length);

            s = RunCstTest(p => p.ParseExpression(), "5 <> 6");
            Assert.IsNotNull(s.LeftOperand);
            Assert.IsNotNull(s.Operator);
            Assert.IsNotNull(s.RightOperand);
            Assert.AreEqual(6, s.Length);

            s = RunCstTest(p => p.ParseExpression(), "5 in a");
            Assert.IsNotNull(s.LeftOperand);
            Assert.IsNotNull(s.Operator);
            Assert.IsNotNull(s.RightOperand);
            Assert.AreEqual(6, s.Length);

            s = RunCstTest(p => p.ParseExpression(), "5 is a");
            Assert.IsNotNull(s.LeftOperand);
            Assert.IsNotNull(s.Operator);
            Assert.IsNotNull(s.RightOperand);
            Assert.AreEqual(6, s.Length);
        }

        [TestMethod]
        public void TestExternalDirective() {
            var s = RunCstTest(p => p.ParseExternalDirective(), "varargs;");
            Assert.IsNotNull(s.Directive);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(8, s.Length);

            s = RunCstTest(p => p.ParseExternalDirective(), "external '5';");
            Assert.IsNotNull(s.Directive);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(13, s.Length);

            s = RunCstTest(p => p.ParseExternalDirective(), "external '5' name 'a';");
            Assert.IsNotNull(s.Directive);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(22, s.Length);
        }

        [TestMethod]
        public void TestExternalDirectiveSpecifier() {
            var s = RunCstTest(p => p.ParseExternalSpecifier(), "name 5");
            Assert.IsNotNull(s.Specifier);
            Assert.IsNotNull(s.Items[0]);
            Assert.AreEqual(6, s.Length);

            s = RunCstTest(p => p.ParseExternalSpecifier(), "index 5");
            Assert.IsNotNull(s.Specifier);
            Assert.IsNotNull(s.Items[0]);
            Assert.AreEqual(7, s.Length);

            s = RunCstTest(p => p.ParseExternalSpecifier(), "dependency 5,2");
            Assert.IsNotNull(s.Specifier);
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[0].Comma);
            Assert.IsNotNull(s.Items[1]);
            Assert.AreEqual(14, s.Length);

            s = RunCstTest(p => p.ParseExternalSpecifier(), "delayed");
            Assert.IsNotNull(s.Specifier);
            Assert.AreEqual(7, s.Length);
        }

        [TestMethod]
        public void TestFactor() {
            var s = RunCstTest(p => p.ParseFactor(), "@x");
            Assert.IsNotNull(s.UnaryOperator);
            Assert.IsNotNull(s.UnaryOperand);
            Assert.AreEqual(2, s.Length);

            s = RunCstTest(p => p.ParseFactor(), "not x");
            Assert.IsNotNull(s.UnaryOperator);
            Assert.IsNotNull(s.UnaryOperand);
            Assert.AreEqual(5, s.Length);

            s = RunCstTest(p => p.ParseFactor(), "+x");
            Assert.IsNotNull(s.UnaryOperator);
            Assert.IsNotNull(s.UnaryOperand);
            Assert.AreEqual(2, s.Length);

            s = RunCstTest(p => p.ParseFactor(), "-x");
            Assert.IsNotNull(s.UnaryOperator);
            Assert.IsNotNull(s.UnaryOperand);
            Assert.AreEqual(2, s.Length);

            s = RunCstTest(p => p.ParseFactor(), "^x");
            Assert.IsNotNull(s.UnaryOperator);
            Assert.IsNotNull(s.PointerTo);
            Assert.AreEqual(2, s.Length);

            s = RunCstTest(p => p.ParseFactor(), "5");
            Assert.IsNotNull(s.IntValue);
            Assert.AreEqual(1, s.Length);

            s = RunCstTest(p => p.ParseFactor(), "5.a");
            Assert.IsNotNull(s.IntValue);
            Assert.IsNotNull(s.Designator);
            Assert.AreEqual(3, s.Length);

            s = RunCstTest(p => p.ParseFactor(), "$5");
            Assert.IsNotNull(s.HexValue);
            Assert.AreEqual(2, s.Length);

            s = RunCstTest(p => p.ParseFactor(), "$5.a");
            Assert.IsNotNull(s.HexValue);
            Assert.IsNotNull(s.Designator);
            Assert.AreEqual(4, s.Length);

            s = RunCstTest(p => p.ParseFactor(), "5.4");
            Assert.IsNotNull(s.RealValue);
            Assert.AreEqual(3, s.Length);

            s = RunCstTest(p => p.ParseFactor(), "3.4.a");
            Assert.IsNotNull(s.RealValue);
            Assert.IsNotNull(s.Designator);
            Assert.AreEqual(5, s.Length);

            s = RunCstTest(p => p.ParseFactor(), "'a'");
            Assert.IsNotNull(s.StringValue);
            Assert.AreEqual(3, s.Length);

            s = RunCstTest(p => p.ParseFactor(), "'a'.a");
            Assert.IsNotNull(s.StringValue);
            Assert.IsNotNull(s.Designator);
            Assert.AreEqual(5, s.Length);

            s = RunCstTest(p => p.ParseFactor(), "true");
            Assert.IsNotNull(s.UnaryOperator);
            Assert.AreEqual(4, s.Length);

            s = RunCstTest(p => p.ParseFactor(), "true.a");
            Assert.IsNotNull(s.UnaryOperator);
            Assert.IsNotNull(s.Designator);
            Assert.AreEqual(6, s.Length);

            s = RunCstTest(p => p.ParseFactor(), "false");
            Assert.IsNotNull(s.UnaryOperator);
            Assert.AreEqual(5, s.Length);

            s = RunCstTest(p => p.ParseFactor(), "false.a");
            Assert.IsNotNull(s.UnaryOperator);
            Assert.IsNotNull(s.Designator);
            Assert.AreEqual(7, s.Length);

            s = RunCstTest(p => p.ParseFactor(), "nil");
            Assert.IsNotNull(s.UnaryOperator);
            Assert.AreEqual(3, s.Length);

            s = RunCstTest(p => p.ParseFactor(), "[a]");
            Assert.IsNotNull(s.SetSection);
            Assert.AreEqual(3, s.Length);

            s = RunCstTest(p => p.ParseFactor(), "ShortString");
            Assert.IsNotNull(s.Designator);
            Assert.AreEqual(11, s.Length);

            s = RunCstTest(p => p.ParseFactor(), "string");
            Assert.IsNotNull(s.Designator);
            Assert.AreEqual(6, s.Length);

            s = RunCstTest(p => p.ParseFactor(), "WideString");
            Assert.IsNotNull(s.Designator);
            Assert.AreEqual(10, s.Length);

            s = RunCstTest(p => p.ParseFactor(), "UnicodeString");
            Assert.IsNotNull(s.Designator);
            Assert.AreEqual(13, s.Length);

            s = RunCstTest(p => p.ParseFactor(), "AnsiString");
            Assert.IsNotNull(s.Designator);
            Assert.AreEqual(10, s.Length);

            s = RunCstTest(p => p.ParseFactor(), ".a");
            Assert.IsNotNull(s.Designator);
            Assert.AreEqual(2, s.Length);

            s = RunCstTest(p => p.ParseFactor(), "inherited a");
            Assert.IsNotNull(s.Designator);
            Assert.AreEqual(11, s.Length);

            s = RunCstTest(p => p.ParseFactor(), "(a)");
            Assert.IsNotNull(s.OpenParen);
            Assert.IsNotNull(s.ParenExpression);
            Assert.IsNotNull(s.CloseParen);
            Assert.AreEqual(3, s.Length);
        }

        [TestMethod]
        public void TestFileType() {
            var s = RunCstTest(p => p.ParseFileType(), "file");
            Assert.IsNotNull(s.FileSymbol);
            Assert.AreEqual(4, s.Length);

            s = RunCstTest(p => p.ParseFileType(), "file of ta");
            Assert.IsNotNull(s.FileSymbol);
            Assert.IsNotNull(s.OfSymbol);
            Assert.IsNotNull(s.TypeDefinition);
            Assert.AreEqual(10, s.Length);
        }

        [TestMethod]
        public void TestFormalParameter() {
            var dummy = TokenKind.Undefined - 1;
            var s = RunCstTest(p => p.ParseFormalParameter(true, ref dummy), "[a] const [b] a,");
            Assert.IsNotNull(s.Attributes1);
            Assert.IsNotNull(s.ParameterKind);
            Assert.IsNotNull(s.Attributes2);
            Assert.IsNotNull(s.ParameterName);
            Assert.IsNotNull(s.Comma);
            Assert.AreEqual(16, s.Length);
        }

        [TestMethod]
        public void TestFormalParameterDefinition() {
            var s = RunCstTest(p => p.ParseFormalParameterDefinition(true), "a: integer = 5;");
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.ColonSymbol);
            Assert.IsNotNull(s.TypeDeclaration);
            Assert.IsNotNull(s.EqualsSign);
            Assert.IsNotNull(s.DefaultValue);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(15, s.Length);
        }

        [TestMethod]
        public void TestFormalParameters() {
            var s = RunCstTest(p => p.ParseFormalParameters(), "a: string; b: string; d: integer");
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[1]);
            Assert.IsNotNull(s.Items[2]);
            Assert.AreEqual(32, s.Length);
        }

        [TestMethod]
        public void TestFormattedExpression() {
            var s = RunCstTest(p => p.ParseFormattedExpression(), "a:2:1");
            Assert.IsNotNull(s.Expression);
            Assert.IsNotNull(s.Colon1);
            Assert.IsNotNull(s.Width);
            Assert.IsNotNull(s.Colon2);
            Assert.IsNotNull(s.Decimals);
            Assert.AreEqual(5, s.Length);
        }

        [TestMethod]
        public void TestForStatement() {
            var s = RunCstTest(p => p.ParseForStatement(), "for I := 0 to 9 do begin a; end");
            Assert.IsNotNull(s.ForKeyword);
            Assert.IsNotNull(s.Variable);
            Assert.IsNotNull(s.Assignment);
            Assert.IsNotNull(s.StartExpression);
            Assert.IsNotNull(s.LoopOperator);
            Assert.IsNotNull(s.EndExpression);
            Assert.IsNotNull(s.DoKeyword);
            Assert.IsNotNull(s.Statement);
            Assert.AreEqual(31, s.Length);

            s = RunCstTest(p => p.ParseForStatement(), "for I := 9 downto 0 do begin a; end");
            Assert.IsNotNull(s.ForKeyword);
            Assert.IsNotNull(s.Variable);
            Assert.IsNotNull(s.Assignment);
            Assert.IsNotNull(s.StartExpression);
            Assert.IsNotNull(s.LoopOperator);
            Assert.IsNotNull(s.EndExpression);
            Assert.IsNotNull(s.DoKeyword);
            Assert.IsNotNull(s.Statement);
            Assert.AreEqual(35, s.Length);

            s = RunCstTest(p => p.ParseForStatement(), "for a in b do begin a; end");
            Assert.IsNotNull(s.ForKeyword);
            Assert.IsNotNull(s.Variable);
            Assert.IsNotNull(s.EndExpression);
            Assert.IsNotNull(s.LoopOperator);
            Assert.IsNotNull(s.DoKeyword);
            Assert.IsNotNull(s.Statement);
            Assert.AreEqual(26, s.Length);
        }

        [TestMethod]
        public void TestForwardDirective() {
            var s = RunCstTest(p => p.ParseForwardDirective(), "forward;");
            Assert.IsNotNull(s.Directive);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(8, s.Length);
        }

        [TestMethod]
        public void TestParseFunctionDirectives() {
            var s = RunCstTest(p => p.ParseFunctionDirectives(), "overload; inline;");
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[1]);
            Assert.AreEqual(17, s.Length);
        }

        [TestMethod]
        public void TestParseGenericDefinition() {
            var s = RunCstTest(p => p.ParseGenericDefinition(), "<a>");
            Assert.IsNotNull(s.OpenBrackets);
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.CloseBrackets);
            Assert.AreEqual(3, s.Length);

            s = RunCstTest(p => p.ParseGenericDefinition(), "<a, b>");
            Assert.IsNotNull(s.OpenBrackets);
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[1]);
            Assert.IsNotNull(s.CloseBrackets);
            Assert.AreEqual(6, s.Length);

            s = RunCstTest(p => p.ParseGenericDefinition(), "<a: a, b; b: c>");
            Assert.IsNotNull(s.OpenBrackets);
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[1]);
            Assert.IsNotNull(s.CloseBrackets);
            Assert.AreEqual(15, s.Length);
        }

        [TestMethod]
        public void TestGenericDefinitionPart() {
            var s = RunCstTest(p => p.ParseGenericDefinitionPart(), "a: b, c");
            Assert.IsNotNull(s.Identifier);
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[1]);
            Assert.AreEqual(7, s.Length);
        }

        [TestMethod]
        public void TestGenericNamespaceName() {
            var s = RunCstTest(p => p.ParseGenericNamespaceName(), "a<b>.c");
            Assert.IsNotNull(s.Name);
            Assert.IsNotNull(s.GenericPart);
            Assert.AreEqual(4, s.Length);

            s = RunCstTest(p => p.ParseGenericNamespaceName(), "a");
            Assert.IsNotNull(s.Name);
            Assert.AreEqual(1, s.Length);
        }

        [TestMethod]
        public void TestGenericTypeIdent() {
            var s = RunCstTest(p => p.ParseGenericTypeIdent(), "a<b>");
            Assert.IsNotNull(s.Identifier);
            Assert.IsNotNull(s.GenericDefinition);
            Assert.AreEqual(4, s.Length);

            s = RunCstTest(p => p.ParseGenericTypeIdent(), "a");
            Assert.IsNotNull(s.Identifier);
            Assert.AreEqual(1, s.Length);
        }

        [TestMethod]
        public void TestGenericSuffix() {
            var s = RunCstTest(p => p.ParseGenericSuffix(), "<a>");
            Assert.IsNotNull(s.OpenBracket);
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.CloseBracket);
            Assert.AreEqual(3, s.Length);

            s = RunCstTest(p => p.ParseGenericSuffix(), "<a,b>");
            Assert.IsNotNull(s.OpenBracket);
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[0].Comma);
            Assert.IsNotNull(s.Items[1]);
            Assert.IsNotNull(s.CloseBracket);
            Assert.AreEqual(5, s.Length);
        }

        [TestMethod]
        public void TestGotoStatement() {
            var s = RunCstTest(p => p.ParseGoToStatement(), "goto a");
            Assert.IsNotNull(s.GotoSymbol);
            Assert.IsNotNull(s.GoToLabel);
            Assert.AreEqual(6, s.Length);

            s = RunCstTest(p => p.ParseGoToStatement(), "break");
            Assert.IsNotNull(s.GotoSymbol);
            Assert.AreEqual(5, s.Length);

            s = RunCstTest(p => p.ParseGoToStatement(), "continue");
            Assert.IsNotNull(s.GotoSymbol);
            Assert.AreEqual(8, s.Length);

            s = RunCstTest(p => p.ParseGoToStatement(), "exit");
            Assert.IsNotNull(s.GotoSymbol);
            Assert.AreEqual(4, s.Length);

            s = RunCstTest(p => p.ParseGoToStatement(), "exit(5)");
            Assert.IsNotNull(s.GotoSymbol);
            Assert.IsNotNull(s.OpenParen);
            Assert.IsNotNull(s.ExitExpression);
            Assert.IsNotNull(s.CloseParen);
            Assert.AreEqual(7, s.Length);
        }

        [TestMethod]
        public void TestHexNumber() {
            var s = RunCstTest(p => p.RequireHexValue(), "$33");
            Assert.IsNotNull(s.Symbol);
            Assert.AreEqual(3, s.Length);
        }

        [TestMethod]
        public void TestIdentifier() {
            var s = RunCstTest(p => p.RequireIdentifier(), "rr");
            Assert.IsNotNull(s.Symbol);
            Assert.AreEqual(2, s.Length);

            s = RunCstTest(p => p.RequireIdentifier(true), "if");
            Assert.IsNotNull(s.Symbol);
            Assert.AreEqual(2, s.Length);
        }

        [TestMethod]
        public void TestIdentList() {
            var s = RunCstTest(p => p.ParseIdentList(true), "a,[a] b");
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[1]);
            Assert.AreEqual(7, s.Length);
        }

        [TestMethod]
        public void TestParseIfStatement() {
            var s = RunCstTest(p => p.ParseIfStatement(), "if a then b");
            Assert.IsNotNull(s.IfSymbol);
            Assert.IsNotNull(s.Condition);
            Assert.IsNotNull(s.ThenPart);
            Assert.IsNotNull(s.ThenSymbol);
            Assert.AreEqual(11, s.Length);

            s = RunCstTest(p => p.ParseIfStatement(), "if a then b else c");
            Assert.IsNotNull(s.IfSymbol);
            Assert.IsNotNull(s.Condition);
            Assert.IsNotNull(s.ThenPart);
            Assert.IsNotNull(s.ThenSymbol);
            Assert.IsNotNull(s.ElseSymbol);
            Assert.IsNotNull(s.ElsePart);
            Assert.AreEqual(18, s.Length);
        }

        [TestMethod]
        public void TestInterfaceGuid() {
            var s = RunCstTest(p => p.ParseInterfaceGuid(), "['a']");
            Assert.IsNotNull(s.OpenBraces);
            Assert.IsNotNull(s.Id);
            Assert.IsNotNull(s.CloseBraces);
            Assert.AreEqual(5, s.Length);

            s = RunCstTest(p => p.ParseInterfaceGuid(), "[a]");
            Assert.IsNotNull(s.OpenBraces);
            Assert.IsNotNull(s.IdIdentifier);
            Assert.IsNotNull(s.CloseBraces);
            Assert.AreEqual(3, s.Length);
        }

        [TestMethod]
        public void TestInterfaceItem() {
            var s = RunCstTest(p => p.ParseInterfaceItem(out var x), "function x: string;");
            Assert.IsNotNull(s.Method);
            Assert.AreEqual(19, s.Length);

            s = RunCstTest(p => p.ParseInterfaceItem(out var x), "property x: string read GetX;");
            Assert.IsNotNull(s.Property);
            Assert.AreEqual(29, s.Length);
        }

        [TestMethod]
        public void TestLabel() {
            var s = RunCstTest(p => p.ParseLabel(), "a");
            Assert.IsNotNull(s.LabelName);
            Assert.AreEqual(1, s.Length);

            s = RunCstTest(p => p.ParseLabel(), "$F");
            Assert.IsNotNull(s.LabelName);
            Assert.AreEqual(2, s.Length);

            s = RunCstTest(p => p.ParseLabel(), "3");
            Assert.IsNotNull(s.LabelName);
            Assert.AreEqual(1, s.Length);
        }

        [TestMethod]
        public void TestLocalAsmLabel() {
            var s = RunCstTest(p => p.ParseLocalAsmLabel(), "@a");
            Assert.IsNotNull(s.AtSymbol);
            Assert.IsNotNull(s.Items[0]);
            Assert.AreEqual(2, s.Length);

            s = RunCstTest(p => p.ParseLocalAsmLabel(), "@3");
            Assert.IsNotNull(s.AtSymbol);
            Assert.IsNotNull(s.Items[0]);
            Assert.AreEqual(2, s.Length);

            s = RunCstTest(p => p.ParseLocalAsmLabel(), "@$F");
            Assert.IsNotNull(s.AtSymbol);
            Assert.IsNotNull(s.Items[0]);
            Assert.AreEqual(3, s.Length);

            s = RunCstTest(p => p.ParseLocalAsmLabel(), "@@4");
            Assert.IsNotNull(s.AtSymbol);
            Assert.IsNotNull(s.Items[0]);
            Assert.AreEqual(3, s.Length);

        }

        [TestMethod]
        public void TestMethodDecl() {
            var s = RunCstTest(p => p.ParseMethodDecl(default, default), "function a.b.c(a: string): string; inline; begin end;");
            Assert.IsNotNull(s.Heading);
            Assert.IsNotNull(s.Semicolon);
            Assert.IsNotNull(s.Directives);
            Assert.IsNotNull(s.MethodBody);
            Assert.IsNotNull(s.Semicolon2);
            Assert.AreEqual(53, s.Length);
        }

        [TestMethod]
        public void TestMethodDeclarationName() {
            var s = RunCstTest(p => p.ParseMethodDeclarationName(false), "a.b.c<d>.");
            Assert.IsNotNull(s.Name);
            Assert.IsNotNull(s.GenericDefinition);
            Assert.IsNotNull(s.Dot);
            Assert.AreEqual(9, s.Length);
        }

        [TestMethod]
        public void TestMethodDeclarationHeading() {
            var s = RunCstTest(p => p.ParseMethodDeclHeading(), "function x(a: string): [a] string");
            Assert.IsNotNull(s.KindSymbol);
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Parameters);
            Assert.IsNotNull(s.ResultTypeAttributes);
            Assert.IsNotNull(s.ResultType);
            Assert.AreEqual(33, s.Length);
        }

        [TestMethod]
        public void TestMethodDirectives() {
            var s = RunCstTest(p => p.ParseMethodDirectives(), "overload; reintroduce;");
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[1]);
            Assert.AreEqual(22, s.Length);
        }

        [TestMethod]
        public void TestMethodResolution() {
            var s = RunCstTest(p => p.ParseMethodResolution(), "function a.c = d;");
            Assert.IsNotNull(s.KindSymbol);
            Assert.IsNotNull(s.Name);
            Assert.IsNotNull(s.EqualsSign);
            Assert.IsNotNull(s.ResolveIdentifier);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(17, s.Length);
        }

        [TestMethod]
        public void TestNamespaceFileName() {
            var s = RunCstTest(p => p.ParseNamespaceFileName(true), "a.b.c");
            Assert.IsNotNull(s.NamespaceName);
            Assert.AreEqual(5, s.Length);

            s = RunCstTest(p => p.ParseNamespaceFileName(true), "a.b.c in 'a'");
            Assert.IsNotNull(s.NamespaceName);
            Assert.IsNotNull(s.InSymbol);
            Assert.IsNotNull(s.QuotedFileName);
            Assert.AreEqual(12, s.Length);

            s = RunCstTest(p => p.ParseNamespaceFileName(true), "a.b.c in 'a',");
            Assert.IsNotNull(s.NamespaceName);
            Assert.IsNotNull(s.InSymbol);
            Assert.IsNotNull(s.QuotedFileName);
            Assert.IsNotNull(s.Comma);
            Assert.AreEqual(13, s.Length);
        }

        [TestMethod]
        public void TestNamespaceFileNameList() {
            var s = RunCstTest(p => p.ParseNamespaceFileNameList(), "a,b,c");
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[1]);
            Assert.IsNotNull(s.Items[2]);
            Assert.AreEqual(5, s.Length);

            s = RunCstTest(p => p.ParseNamespaceFileNameList(), "a in 'a',b in 'b',c in 'c'");
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[1]);
            Assert.IsNotNull(s.Items[2]);
            Assert.AreEqual(26, s.Length);
        }

        [TestMethod]
        public void TestNamespaceName() {
            var s = RunCstTest(p => p.ParseNamespaceName(), "a.b.c");
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[1]);
            Assert.IsNotNull(s.Items[2]);
            Assert.AreEqual(5, s.Length);
        }

        [TestMethod]
        public void TestNamespaceNameList() {
            var s = RunCstTest(p => p.ParseNamespaceNameList(), "a.b.c, d");
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[0].Comma);
            Assert.IsNotNull(s.Items[1]);
            Assert.AreEqual(8, s.Length);
        }


        [TestMethod]
        public void TestObjectDeclaration() {
            var s = RunCstTest(p => p.ParseObjectDecl(), "object(a) procedure x; end");
            Assert.IsNotNull(s.ObjectSymbol);
            Assert.IsNotNull(s.ClassParent);
            Assert.IsNotNull(s.Items);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(26, s.Length);
        }

        [TestMethod]
        public void TestObjectItem() {
            var mode = ClassDeclarationMode.Other;

            var s = RunCstTest(p => p.ParseObjectItem(ref mode), "var");
            Assert.AreEqual(mode, ClassDeclarationMode.Fields);
            Assert.IsNotNull(s.VarSymbol);
            Assert.AreEqual(3, s.Length);

            s = RunCstTest(p => p.ParseObjectItem(ref mode), "protected");
            Assert.AreEqual(mode, ClassDeclarationMode.Fields);
            Assert.AreEqual(s.Visibility.GetSymbolKind(), TokenKind.Protected);
            Assert.AreEqual(9, s.Length);

            s = RunCstTest(p => p.ParseObjectItem(ref mode), "strict private");
            Assert.AreEqual(mode, ClassDeclarationMode.Fields);
            Assert.AreEqual(s.Visibility.GetSymbolKind(), TokenKind.Private);
            Assert.IsNotNull(s.Strict);
            Assert.AreEqual(14, s.Length);

            s = RunCstTest(p => p.ParseObjectItem(ref mode), "function x: integer;");
            Assert.AreEqual(mode, ClassDeclarationMode.Other);
            Assert.IsNotNull(s.MethodDeclaration);
            Assert.AreEqual(20, s.Length);

            s = RunCstTest(p => p.ParseObjectItem(ref mode), "property x: integer read p write p;");
            Assert.AreEqual(mode, ClassDeclarationMode.Other);
            Assert.IsNotNull(s.Property);
            Assert.AreEqual(35, s.Length);

            s = RunCstTest(p => p.ParseObjectItem(ref mode), "const x = 4;");
            Assert.AreEqual(mode, ClassDeclarationMode.Other);
            Assert.IsNotNull(s.ConstSection);
            Assert.AreEqual(12, s.Length);

            s = RunCstTest(p => p.ParseObjectItem(ref mode), "type x = string;");
            Assert.AreEqual(mode, ClassDeclarationMode.Other);
            Assert.IsNotNull(s.TypeSection);
            Assert.AreEqual(16, s.Length);

            mode = ClassDeclarationMode.Fields;
            s = RunCstTest(p => p.ParseObjectItem(ref mode), "x: string;");
            Assert.AreEqual(mode, ClassDeclarationMode.Fields);
            Assert.IsNotNull(s.FieldDeclaration);
            Assert.AreEqual(10, s.Length);
        }

        [TestMethod]
        public void TestObjectItems() {
            var s = RunCstTest(p => p.ParseObjectItems(), "procedure x;");
            Assert.IsNotNull(s.Items[0]);
            Assert.AreEqual(12, s.Length);
        }

        [TestMethod]
        public void TestOldCallConvention() {
            var s = RunCstTest(p => p.ParseOldCallConvention(), "far ;");
            Assert.IsNotNull(s.Directive);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(5, s.Length);

            s = RunCstTest(p => p.ParseOldCallConvention(), "near ;");
            Assert.IsNotNull(s.Directive);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(6, s.Length);

            s = RunCstTest(p => p.ParseOldCallConvention(), "local ;");
            Assert.IsNotNull(s.Directive);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(7, s.Length);
        }

        [TestMethod]
        public void TestPackageContains() {
            var s = RunCstTest(p => p.ParseContainsClause(), "contains a in 'a';");
            Assert.IsNotNull(s.ContainsSymbol);
            Assert.IsNotNull(s.ContainsList);
            Assert.IsNotNull(s.ContainsList.Items[0].InSymbol);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(18, s.Length);
        }

        [TestMethod]
        public void TestPackageHead() {
            var s = RunCstTest(p => p.ParsePackageHead(), "package a.b.c.d;");
            Assert.IsNotNull(s.PackageSymbol);
            Assert.IsNotNull(s.PackageName);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(16, s.Length);
        }

        [TestMethod]
        public void TestPackageRequires() {
            var s = RunCstTest(p => p.ParseRequiresClause(), "requires a;");
            Assert.IsNotNull(s.RequiresSymbol);
            Assert.IsNotNull(s.RequiresList);
            Assert.IsNotNull(s.RequiresList.Items[0]);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(11, s.Length);
        }

        [TestMethod]
        public void TestPackage() {
            var s = RunCstTest(p => p.ParsePackage(new FileReference(CstPath)), "package z.x; requires a; contains b in 'b'; end. ");
            Assert.IsNotNull(s.PackageHead);
            Assert.IsNotNull(s.RequiresClause);
            Assert.IsNotNull(s.ContainsClause);
            Assert.IsNotNull(s.EndSymbol);
            Assert.IsNotNull(s.DotSymbol);
            Assert.AreEqual(49, s.Length);
        }

        [TestMethod]
        public void TestParameter() {
            var s = RunCstTest(p => p.ParseParameter(), "5");
            Assert.IsNotNull(s.Expression);
            Assert.AreEqual(1, s.Length);

            s = RunCstTest(p => p.ParseParameter(), "a := 5");
            Assert.IsNotNull(s.Expression);
            Assert.IsNotNull(s.ParameterName);
            Assert.IsNotNull(s.AssignmentSymbol);
            Assert.AreEqual(6, s.Length);

            s = RunCstTest(p => p.ParseParameter(), ",");
            Assert.IsNotNull(s.Comma);
            Assert.AreEqual(1, s.Length);
        }

        [TestMethod]
        public void TestParentClass() {
            var s = RunCstTest(p => p.ParseClassParent(), "(a,b,c)");
            Assert.IsNotNull(s.OpenParen);
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[0].Comma);
            Assert.IsNotNull(s.Items[1]);
            Assert.IsNotNull(s.Items[1].Comma);
            Assert.IsNotNull(s.Items[2]);
            Assert.IsNotNull(s.CloseParen);
            Assert.AreEqual(7, s.Length);
        }

        [TestMethod]
        public void TestPointerType() {
            var s = RunCstTest(p => p.ParsePointerType(), "Pointer");
            Assert.IsNotNull(s.PointerSymbol);
            Assert.AreEqual(7, s.Length);

            s = RunCstTest(p => p.ParsePointerType(), "^aaaa");
            Assert.IsNotNull(s.PointerSymbol);
            Assert.IsNotNull(s.TypeSpecification);
            Assert.AreEqual(5, s.Length);
        }

        [TestMethod]
        public void TestProcedureDeclarationHeading() {
            var s = RunCstTest(p => p.ParseProcedureDeclarationHeading(), "function a(const a: string): [a] string");
            Assert.IsNotNull(s.KindSymbol);
            Assert.IsNotNull(s.Name);
            Assert.IsNotNull(s.Parameters);
            Assert.IsNotNull(s.ColonSymbol);
            Assert.IsNotNull(s.ResultTypeAttributes);
            Assert.IsNotNull(s.ResultType);
            Assert.AreEqual(39, s.Length);
        }

        [TestMethod]
        public void TestProcedureDeclaration() {
            var s = RunCstTest(p => p.ParseProcedureDeclaration(null), "function a(const a: string): [a] string; inline; begin end;");
            Assert.IsNotNull(s.Heading);
            Assert.IsNotNull(s.Semicolon);
            Assert.IsNotNull(s.Directives);
            Assert.IsNotNull(s.Body);
            Assert.IsNotNull(s.Semicolon2);
            Assert.AreEqual(59, s.Length);
        }

        [TestMethod]
        public void TestProcedureReference() {
            var s = RunCstTest(p => p.ParseProcedureReference(), "reference to procedure (a: integer)");
            Assert.IsNotNull(s.Reference);
            Assert.IsNotNull(s.ToSymbol);
            Assert.IsNotNull(s.ProcedureType);
            Assert.AreEqual(35, s.Length);
        }

        [TestMethod]
        public void TestProcedureType() {
            var s = RunCstTest(p => p.ParseProcedureRefType(), "function (a: integer): [a] string of object");
            Assert.IsNotNull(s.KindSymbol);
            Assert.IsNotNull(s.Parameters);
            Assert.IsNotNull(s.ReturnTypeAttributes);
            Assert.IsNotNull(s.ReturnType);
            Assert.IsNotNull(s.Parameters);
            Assert.IsNotNull(s.OfSymbol);
            Assert.IsNotNull(s.ObjectSymbol);
            Assert.AreEqual(43, s.Length);
        }

        [TestMethod]
        public void TestProcedureTypeDefinition() {
            var s = RunCstTest(p => p.ParseProcedureRefType(false), "function (a: integer): [a] string");
            Assert.IsNotNull(s.KindSymbol);
            Assert.IsNotNull(s.Parameters);
            Assert.IsNotNull(s.ColonSymbol);
            Assert.IsNotNull(s.ReturnTypeAttributes);
            Assert.IsNotNull(s.ReturnType);
            Assert.AreEqual(33, s.Length);
        }


        [TestMethod]
        public void TestProgramParameters() {
            var s = RunCstTest(p => p.ParseProgramParams(), "(a,b,z)");
            Assert.IsNotNull(s.OpenParen);
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[0].Comma);
            Assert.IsNotNull(s.Items[1]);
            Assert.IsNotNull(s.Items[1].Comma);
            Assert.IsNotNull(s.Items[2]);
            Assert.AreEqual(7, s.Length);
        }

        [TestMethod]
        public void TestQuotedString() {
            var s = RunCstTest(p => p.RequireString(), "'aa'");
            Assert.IsNotNull(s.Symbol);
            Assert.AreEqual(4, s.Length);
        }

        [TestMethod]
        public void TestRaiseStatement() {
            var s = RunCstTest(p => p.ParseRaiseStatement(), "raise a");
            Assert.IsNotNull(s.RaiseSymbol);
            Assert.IsNotNull(s.RaiseExpression);
            Assert.AreEqual(7, s.Length);

            s = RunCstTest(p => p.ParseRaiseStatement(), "raise a at 4");
            Assert.IsNotNull(s.RaiseSymbol);
            Assert.IsNotNull(s.RaiseExpression);
            Assert.IsNotNull(s.AtSymbol);
            Assert.IsNotNull(s.AtExpression);
            Assert.AreEqual(12, s.Length);
        }

        [TestMethod]
        public void TestRealNumber() {
            var s = RunCstTest(p => p.RequireRealValue(), "3.4443");
            Assert.IsNotNull(s.Symbol);
            Assert.AreEqual(6, s.Length);
        }

        [TestMethod]
        public void TestRecordConstantExpression() {
            var s = RunCstTest(p => p.ParseRecordConstant(true), "a: 5;");
            Assert.IsNotNull(s.Name);
            Assert.IsNotNull(s.ColonSymbol);
            Assert.IsNotNull(s.Value);
            Assert.IsNotNull(s.Separator);
            Assert.AreEqual(5, s.Length);
        }

        [TestMethod]
        public void TestRecordDeclaration() {
            var s = RunCstTest(p => p.ParseRecordDecl(), "record a: string end");
            Assert.IsNotNull(s.RecordSymbol);
            Assert.IsNotNull(s.FieldList);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(20, s.Length);

            s = RunCstTest(p => p.ParseRecordDecl(), "record a: string; case byte of 1: (x: integer); 2: (z: integer); end");
            Assert.IsNotNull(s.RecordSymbol);
            Assert.IsNotNull(s.FieldList);
            Assert.IsNotNull(s.VariantSection);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(68, s.Length);

            s = RunCstTest(p => p.ParseRecordDecl(), "record var a: string; end");
            Assert.IsNotNull(s.Items);
            Assert.AreEqual(25, s.Length);
        }

        [TestMethod]
        public void TestRecordField() {
            var s = RunCstTest(p => p.ParseRecordField(true), "a, b, c: integer library;");
            Assert.IsNotNull(s.Names);
            Assert.IsNotNull(s.ColonSymbol);
            Assert.IsNotNull(s.FieldType);
            Assert.IsNotNull(s.Hint);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(25, s.Length);
        }

        [TestMethod]
        public void TestRecordFieldList() {
            var s = RunCstTest(p => p.ParseRecordFieldList(true), "a: integer; b: cardinal;");
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[1]);
            Assert.AreEqual(24, s.Length);
        }

        [TestMethod]
        public void TestRecordHelper() {
            var s = RunCstTest(p => p.ParseRecordHelper(), "record helper for TA public function x(): integer; end");
            Assert.IsNotNull(s.RecordSymbol);
            Assert.IsNotNull(s.HelperSymbol);
            Assert.IsNotNull(s.ForSymbol);
            Assert.IsNotNull(s.Name);
            Assert.IsNotNull(s.Items);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(54, s.Length);
        }

        [TestMethod]
        public void TestRecordItem() {
            var m = RecordDeclarationMode.Fields;
            var s = RunCstTest(p => p.ParseRecordItem(ref m), "a: integer;");
            Assert.IsNotNull(s.Fields);
            Assert.AreEqual(11, s.Length);

            m = RecordDeclarationMode.Other;
            s = RunCstTest(p => p.ParseRecordItem(ref m), "var");
            Assert.IsNotNull(s.VarSymbol);
            Assert.AreEqual(3, s.Length);

            m = RecordDeclarationMode.Other;
            s = RunCstTest(p => p.ParseRecordItem(ref m), "class var");
            Assert.IsNotNull(s.VarSymbol);
            Assert.IsNotNull(s.ClassSymbol);
            Assert.AreEqual(9, s.Length);

            m = RecordDeclarationMode.Other;
            s = RunCstTest(p => p.ParseRecordItem(ref m), "strict protected");
            Assert.IsNotNull(s.VarSymbol);
            Assert.IsNotNull(s.ClassSymbol);
            Assert.AreEqual(16, s.Length);

            m = RecordDeclarationMode.Other;
            s = RunCstTest(p => p.ParseRecordItem(ref m), "property x: string read z write z;");
            Assert.IsNotNull(s.PropertyDeclaration);
            Assert.AreEqual(34, s.Length);

            m = RecordDeclarationMode.Other;
            s = RunCstTest(p => p.ParseRecordItem(ref m), "function ip(): integer;");
            Assert.IsNotNull(s.MethodDeclaration);
            Assert.AreEqual(23, s.Length);

            m = RecordDeclarationMode.Other;
            s = RunCstTest(p => p.ParseRecordItem(ref m), "const x = 5;");
            Assert.IsNotNull(s.ConstSection);
            Assert.AreEqual(12, s.Length);

            m = RecordDeclarationMode.Other;
            s = RunCstTest(p => p.ParseRecordItem(ref m), "type a = class end;");
            Assert.IsNotNull(s.TypeSection);
            Assert.AreEqual(19, s.Length);

            m = RecordDeclarationMode.Other;
            s = RunCstTest(p => p.ParseRecordItem(ref m), "case byte of 1: (a: string);");
            Assert.IsNotNull(s.VariantSection);
            Assert.AreEqual(28, s.Length);
        }

        [TestMethod]
        public void TestRecordItems() {
            var s = RunCstTest(p => p.ParseRecordItems(), "procedure a; procedure b;");
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[1]);
            Assert.AreEqual(25, s.Length);
        }

        [TestMethod]
        public void TestRecordHelperItems() {
            var s = RunCstTest(p => p.ParseRecordHelperItems(), "procedure a; procedure b;");
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[1]);
            Assert.AreEqual(25, s.Length);
        }

        [TestMethod]
        public void TestRecordHelperItem() {
            var m = RecordDeclarationMode.Fields;
            var s = RunCstTest(p => p.ParseRecordHelperItem(ref m), "a: integer;");
            Assert.IsNotNull(s.FieldDeclaration);
            Assert.AreEqual(11, s.Length);

            m = RecordDeclarationMode.Other;
            s = RunCstTest(p => p.ParseRecordHelperItem(ref m), "var");
            Assert.IsNotNull(s.VarSymbol);
            Assert.AreEqual(3, s.Length);

            m = RecordDeclarationMode.Other;
            s = RunCstTest(p => p.ParseRecordHelperItem(ref m), "class var");
            Assert.IsNotNull(s.VarSymbol);
            Assert.IsNotNull(s.ClassSymbol);
            Assert.AreEqual(9, s.Length);

            m = RecordDeclarationMode.Other;
            s = RunCstTest(p => p.ParseRecordHelperItem(ref m), "strict protected");
            Assert.IsNotNull(s.VarSymbol);
            Assert.IsNotNull(s.ClassSymbol);
            Assert.AreEqual(16, s.Length);

            m = RecordDeclarationMode.Other;
            s = RunCstTest(p => p.ParseRecordHelperItem(ref m), "property x: string read z write z;");
            Assert.IsNotNull(s.PropertyDeclaration);
            Assert.AreEqual(34, s.Length);

            m = RecordDeclarationMode.Other;
            s = RunCstTest(p => p.ParseRecordHelperItem(ref m), "function ip(): integer;");
            Assert.IsNotNull(s.MethodDeclaration);
            Assert.AreEqual(23, s.Length);

            m = RecordDeclarationMode.Other;
            s = RunCstTest(p => p.ParseRecordHelperItem(ref m), "const x = 5;");
            Assert.IsNotNull(s.ConstDeclaration);
            Assert.AreEqual(12, s.Length);

            m = RecordDeclarationMode.Other;
            s = RunCstTest(p => p.ParseRecordHelperItem(ref m), "type a = class end;");
            Assert.IsNotNull(s.TypeSection);
            Assert.AreEqual(19, s.Length);
        }

        [TestMethod]
        public void TestRecordVariant() {
            var s = RunCstTest(p => p.ParseRecordVariant(), "1,7,99: (a: byte; b:byte);");
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[1]);
            Assert.IsNotNull(s.Items[2]);
            Assert.IsNotNull(s.ColonSymbol);
            Assert.IsNotNull(s.OpenParen);
            Assert.IsNotNull(s.FieldList);
            Assert.IsNotNull(s.CloseParen);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(26, s.Length);
        }

        [TestMethod]
        public void TestRecordVariantSection() {
            var s = RunCstTest(p => p.ParseRecordVariantSection(), "case byte of 1: (a: byte); 2: (a: byte)");
            Assert.IsNotNull(s.CaseSymbol);
            Assert.IsNotNull(s.TypeDeclaration);
            Assert.IsNotNull(s.OfSymbol);
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[1]);
            Assert.AreEqual(39, s.Length);
        }

        [TestMethod]
        public void TestRepeat() {
            var s = RunCstTest(p => p.ParseRepeatStatement(), "repeat a; b; until c > 5");
            Assert.IsNotNull(s.Repeat);
            Assert.IsNotNull(s.Statements);
            Assert.IsNotNull(s.Until);
            Assert.IsNotNull(s.Condition); ;
            Assert.AreEqual(24, s.Length);
        }

        [TestMethod]
        public void TestSetDefinition() {
            var s = RunCstTest(p => p.ParseSetDefinition(), "set of tenum");
            Assert.IsNotNull(s.SetSymbol);
            Assert.IsNotNull(s.OfSymbol);
            Assert.IsNotNull(s.TypeDefinition);
            Assert.AreEqual(12, s.Length);
        }

        [TestMethod]
        public void TestSetSection() {
            var s = RunCstTest(p => p.ParseSetSection(), "[a,b]");
            Assert.IsNotNull(s.OpenBraces);
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[0].Continuation);
            Assert.IsNotNull(s.Items[1]);
            Assert.IsNotNull(s.CloseBraces);
            Assert.AreEqual(5, s.Length);
        }

        [TestMethod]
        public void TestSetSectionPart() {
            var s = RunCstTest(p => p.ParseSetSectionPart(), "a,");
            Assert.IsNotNull(s.SetExpression);
            Assert.IsNotNull(s.Continuation);
            Assert.AreEqual(2, s.Length);

            s = RunCstTest(p => p.ParseSetSectionPart(), "a..");
            Assert.IsNotNull(s.SetExpression);
            Assert.IsNotNull(s.Continuation);
            Assert.AreEqual(3, s.Length);
        }

        [TestMethod]
        public void TestSimpleType() {
            var s = RunCstTest(p => p.ParseSimpleType(), "(a,b)");
            Assert.IsNotNull(s.EnumType);
            Assert.AreEqual(5, s.Length);

            s = RunCstTest(p => p.ParseSimpleType(), "type a");
            Assert.IsNotNull(s.NewType);
            Assert.AreEqual(6, s.Length);

            s = RunCstTest(p => p.ParseSimpleType(), "type of a");
            Assert.IsNotNull(s.NewType);
            Assert.IsNotNull(s.TypeOf);
            Assert.AreEqual(9, s.Length);

            s = RunCstTest(p => p.ParseSimpleType(), "1..3");
            Assert.IsNotNull(s.SubrangeStart);
            Assert.IsNotNull(s.DotDot);
            Assert.IsNotNull(s.SubrangeEnd);
            Assert.AreEqual(4, s.Length);
        }

        [TestMethod]
        public void TestSimpleExpression() {
            var s = RunCstTest(p => p.ParseSimpleExpression() as SimpleExpression, "a+b");
            Assert.IsNotNull(s.LeftOperand);
            Assert.IsNotNull(s.Operator);
            Assert.IsNotNull(s.RightOperand);
            Assert.AreEqual(3, s.Length);
        }

        [TestMethod]
        public void TestInteger() {
            var s = RunCstTest(p => p.RequireInteger(), "3364");
            Assert.IsNotNull(s.Value);
            Assert.AreEqual(4, s.Length);
        }

        [TestMethod]
        public void TestStatement() {
            var s = RunCstTest(p => p.ParseStatement(true), "a: x;");
            Assert.IsNotNull(s.Label);
            Assert.IsNotNull(s.ColonSymbol);
            Assert.IsNotNull(s.Part);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(5, s.Length);
        }

        [TestMethod]
        public void TestStringType() {
            var s = RunCstTest(p => p.ParseStringType(), "string[23]");
            Assert.IsNotNull(s.StringSymbol);
            Assert.IsNotNull(s.OpenParen);
            Assert.IsNotNull(s.StringLength);
            Assert.IsNotNull(s.CloseParen);
            Assert.AreEqual(10, s.Length);
        }

        [TestMethod]
        public void TestStructType() {
            var s = RunCstTest(p => p.ParseStructType(), "packed record end");
            Assert.IsNotNull(s.PackedSymbol);
            Assert.IsNotNull(s.Part);
            Assert.AreEqual(17, s.Length);
        }

        [TestMethod]
        public void TestTerm() {
            var s = RunCstTest(p => p.ParseTerm() as TermSymbol, "5*4");
            Assert.IsNotNull(s.LeftOperand);
            Assert.IsNotNull(s.Operator);
            Assert.IsNotNull(s.RightOperand);
            Assert.AreEqual(3, s.Length);
        }

        [TestMethod]
        public void TestTry() {
            var s = RunCstTest(p => p.ParseTryStatement(), "try a; except b; end");
            Assert.IsNotNull(s.TrySymbol);
            Assert.IsNotNull(s.Try);
            Assert.IsNotNull(s.ExceptSymbol);
            Assert.IsNotNull(s.Handlers.Statements);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(20, s.Length);

            s = RunCstTest(p => p.ParseTryStatement(), "try a; finally b; end");
            Assert.IsNotNull(s.TrySymbol);
            Assert.IsNotNull(s.Try);
            Assert.IsNotNull(s.FinallySymbol);
            Assert.IsNotNull(s.Finally);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(21, s.Length);
        }

        [TestMethod]
        public void TestExceptHandlers() {
            var s = RunCstTest(p => p.ParseExceptHandlers(), "a;");
            Assert.IsNotNull(s.Statements);
            Assert.AreEqual(2, s.Length);

            s = RunCstTest(p => p.ParseExceptHandlers(), "on a do x; on b do x; else x;");
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[1]);
            Assert.IsNotNull(s.ElseSymbol);
            Assert.IsNotNull(s.Statements);
            Assert.AreEqual(29, s.Length);

            s = RunCstTest(p => p.ParseExceptHandlers(), "on a:a do x; on b:b do x; else x;");
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[1]);
            Assert.IsNotNull(s.ElseSymbol);
            Assert.IsNotNull(s.Statements);
            Assert.AreEqual(33, s.Length);

        }

        [TestMethod]
        public void TestTypeDeclaration() {
            var s = RunCstTest(p => p.ParseTypeDeclaration(), "[a] ta = record end;");
            Assert.IsNotNull(s.Attributes);
            Assert.IsNotNull(s.TypeId);
            Assert.IsNotNull(s.EqualsSign);
            Assert.IsNotNull(s.TypeSpecification);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(20, s.Length);
        }

        [TestMethod]
        public void TestTypeName() {
            var s = RunCstTest(p => p.ParseTypeName(), "a.b.c");
            Assert.IsNotNull(s.Items[0]);
            Assert.AreEqual(5, s.Length);

            s = RunCstTest(p => p.ParseTypeName(), "string");
            Assert.IsNotNull(s.StringTypeSymbol);
            Assert.AreEqual(6, s.Length);
        }

        [TestMethod]
        public void TestTypeSection() {
            var s = RunCstTest(p => p.ParseTypeSection(false), "type a = class end;");
            Assert.IsNotNull(s.TypeKeyword);
            Assert.IsNotNull(s.Items[0]);
            Assert.AreEqual(19, s.Length);
        }

        [TestMethod]
        public void TestTypeSpecification() {
            var s = RunCstTest(p => p.ParseTypeSpecification(), "array of string");
            Assert.IsNotNull(s.StructuredType);
            Assert.AreEqual(15, s.Length);

            s = RunCstTest(p => p.ParseTypeSpecification(), "pointer");
            Assert.IsNotNull(s.PointerType);
            Assert.AreEqual(7, s.Length);

            s = RunCstTest(p => p.ParseTypeSpecification(), "string");
            Assert.IsNotNull(s.StringType);
            Assert.AreEqual(6, s.Length);

            s = RunCstTest(p => p.ParseTypeSpecification(), "procedure of object");
            Assert.IsNotNull(s.ProcedureType);
            Assert.AreEqual(19, s.Length);

            s = RunCstTest(p => p.ParseTypeSpecification(), "procedure");
            Assert.IsNotNull(s.ProcedureType);
            Assert.AreEqual(9, s.Length);

            s = RunCstTest(p => p.ParseTypeSpecification(), "reference to procedure");
            Assert.IsNotNull(s.RefProcedureType);
            Assert.AreEqual(22, s.Length);

            s = RunCstTest(p => p.ParseTypeSpecification(), "(a,b)");
            Assert.IsNotNull(s.SimpleType);
            Assert.AreEqual(5, s.Length);
        }

        [TestMethod]
        public void TestUnitBlock() {
            var s = RunCstTest(p => p.ParseUnitBlock(), "end");
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(3, s.Length);

            s = RunCstTest(p => p.ParseUnitBlock(), "begin a; end");
            Assert.IsNotNull(s.MainBlock);
            Assert.AreEqual(12, s.Length);

            s = RunCstTest(p => p.ParseUnitBlock(), "initialization end");
            Assert.IsNotNull(s.Initialization);
            Assert.AreEqual(18, s.Length);
        }

        [TestMethod]
        public void TestUnitFinalization() {
            var s = RunCstTest(p => p.ParseUnitFinalization(), "finalization a;");
            Assert.IsNotNull(s.FinalizationSymbol);
            Assert.IsNotNull(s.Statements);
            Assert.IsNotNull(s.Statements.Items[0]);
            Assert.AreEqual(15, s.Length);
        }

        [TestMethod]
        public void TestUnitHead() {
            var s = RunCstTest(p => p.ParseUnitHead(), "unit a.b.c library;");
            Assert.IsNotNull(s.Unit);
            Assert.IsNotNull(s.UnitName);
            Assert.IsNotNull(s.Hint);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(19, s.Length);
        }

        [TestMethod]
        public void TestUnitImplementation() {
            var s = RunCstTest(p => p.ParseUnitImplementation(), "implementation uses a; type t = class end;");
            Assert.IsNotNull(s.Implementation);
            Assert.IsNotNull(s.UsesClause);
            Assert.IsNotNull(s.DeclarationSections);
            Assert.AreEqual(42, s.Length);
        }

        [TestMethod]
        public void TestUnitInitialization() {
            var s = RunCstTest(p => p.ParseUnitInitialization(), "initialization a; finalization b;");
            Assert.IsNotNull(s.InitSymbol);
            Assert.IsNotNull(s.Statements);
            Assert.IsNotNull(s.Finalization);
            Assert.AreEqual(33, s.Length);
        }

        [TestMethod]
        public void TestUnitInterface() {
            var s = RunCstTest(p => p.ParseUnitInterface(), "interface uses a; type t = class end;");
            Assert.IsNotNull(s.InterfaceSymbol);
            Assert.IsNotNull(s.UsesClause);
            Assert.IsNotNull(s.InterfaceDeclaration);
            Assert.AreEqual(37, s.Length);
        }

        [TestMethod]
        public void TestUnsafeDirective() {
            var s = RunCstTest(p => p.ParseUnsafeDirective(), "unsafe;");
            Assert.IsNotNull(s.Directive);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(7, s.Length);
        }

        [TestMethod]
        public void TestUserAttribute() {
            var s = RunCstTest(p => p.ParseAttribute(true), "a:b(1,2),");
            Assert.IsNotNull(s.Prefix);
            Assert.IsNotNull(s.Colon);
            Assert.IsNotNull(s.Name);
            Assert.IsNotNull(s.Expressions);
            Assert.IsNotNull(s.Expressions.Items[0]);
            Assert.IsNotNull(s.CloseParen);
            Assert.IsNotNull(s.Comma);
            Assert.AreEqual(9, s.Length);
        }

        [TestMethod]
        public void TestUserAttributes() {
            var s = RunCstTest(p => p.ParseAttributes(), "[a][b]");
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[1]);
            Assert.AreEqual(6, s.Length);
        }


        [TestMethod]
        public void TestUserAttributeSet() {
            var s = RunCstTest(p => p.ParseAttributeSet(), "[a,b]");
            Assert.IsNotNull(s.OpenBraces);
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[1]);
            Assert.IsNotNull(s.CloseBraces);
            Assert.AreEqual(5, s.Length);
        }


        [TestMethod]
        public void TestUseClauseSymbol() {
            var s = RunCstTest(p => p.ParseUsesClause(), "uses a,b;");
            Assert.IsNotNull(s.UsesSymbol);
            Assert.IsNotNull(s.UsesList.Items[0]);
            Assert.IsNotNull(s.UsesList.Items[1]);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(9, s.Length);
        }

        [TestMethod]
        public void TestUseFileClauseSymbol() {
            var s = RunCstTest(p => p.ParseUsesFileClause(), "uses a in 'a', b in 'b';");
            Assert.IsNotNull(s.UsesSymbol);
            Assert.IsNotNull(s.Files.Items[0]);
            Assert.IsNotNull(s.Files.Items[1]);
            Assert.AreEqual(24, s.Length);
        }

        [TestMethod]
        public void TestWith() {
            var s = RunCstTest(p => p.ParseWithStatement(), "with a,b do c");
            Assert.IsNotNull(s.Items[0]);
            Assert.IsNotNull(s.Items[1]);
            Assert.AreEqual(13, s.Length);
        }

        [TestMethod]
        public void TestWhile() {
            var s = RunCstTest(p => p.ParseWhileStatement(), "while a < b do c");
            Assert.IsNotNull(s.WhileSymbol);
            Assert.IsNotNull(s.Condition);
            Assert.IsNotNull(s.DoSymbol);
            Assert.IsNotNull(s.Statement);
            Assert.AreEqual(16, s.Length);
        }
    }
}