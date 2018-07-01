using PasPasPas.Infrastructure.Files;
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
            Assert.AreEqual(14, s.Length);

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
            Assert.AreEqual(0, s.Length);

            s = RunEmptyCstTest(p => p.ParseBlock(), "const x = 5; begin end");
            Assert.IsNotNull(s.DeclarationSections);
            Assert.IsNotNull(s.Body);
            Assert.AreEqual(0, s.Length);
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
            var s = RunEmptyCstTest(p => p.ParseCaseItem(), "5: x;");
            Assert.IsNotNull(s.ColonSymbol);
            Assert.IsNotNull(s.CaseStatement);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(5, s.Length);
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
            var s = RunEmptyCstTest(p => p.ParseBlockBody() as BlockBodySymbol, "asm end");
            Assert.IsNotNull(s.AssemblerBlock);
            Assert.AreEqual(7, s.Length);

            s = RunEmptyCstTest(p => p.ParseBlockBody() as BlockBodySymbol, "begin end");
            Assert.IsNotNull(s.Body);
            Assert.AreEqual(0, s.Length);
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
        public void TestAssemblyAttribute() {
            var s = RunEmptyCstTest(p => p.ParseAssemblyAttribute(), "[assembly:Foo()]");
            Assert.IsNotNull(s.OpenBraces);
            Assert.IsNotNull(s.AssemblySymbol);
            Assert.IsNotNull(s.ColonSymbol);
            Assert.IsNotNull(s.Attribute);
            Assert.IsNotNull(s.CloseBraces);
            Assert.AreEqual(11, s.Length);
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

        [TestCase]
        public void TestClassField() {
            var s = RunEmptyCstTest(p => p.ParseClassFieldDeclararation(), "");
            Assert.IsNotNull(s.Names);
            Assert.IsNotNull(s.ColonSymbol);
            Assert.IsNotNull(s.TypeDecl);
            Assert.IsNotNull(s.Hint);
            Assert.IsNotNull(s.Semicolon);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestClassHelperDef() {
            var s = RunEmptyCstTest(p => p.ParseClassHelper());
            Assert.IsNotNull(s.ClassSymbol);
            Assert.IsNotNull(s.HelperSymbol);
            Assert.IsNotNull(s.ClassParent);
            Assert.IsNotNull(s.ForSymbol);
            Assert.IsNotNull(s.HelperName);
            Assert.IsNotNull(s.HelperItems);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(0, s.Length);
        }


        [TestCase]
        public void TestClassHelperItem() {
            var mode = ClassDeclarationMode.Other;
            var s = RunEmptyCstTest(p => p.ParseClassHelperItem(ref mode));
            Assert.IsNotNull(s.Attributes1);
            Assert.IsNotNull(s.ClassSymbol);
            Assert.IsNotNull(s.Attributes2);
            Assert.IsNotNull(s.VarSymbol);
            Assert.IsNotNull(s.StrictSymbol);
            Assert.IsNotNull(s.MethodDeclaration);
            Assert.IsNotNull(s.PropertyDeclaration);
            Assert.IsNotNull(s.ConstDeclaration);
            Assert.IsNotNull(s.TypeSection);
            Assert.IsNotNull(s.FieldDeclaration);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestClassHelperItems() {
            var s = RunEmptyCstTest(p => p.ParseClassHelperItems(), "");
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestClassMethod() {
            var s = RunEmptyCstTest(p => p.ParseMethodDeclaration(), "");
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
            Assert.AreEqual(0, s.Length);
        }


        [TestCase]
        public void TestClassOfDeclaration() {
            var s = RunEmptyCstTest(p => p.ParseClassOfDeclaration(), "");
            Assert.IsNotNull(s.ClassSymbol);
            Assert.IsNotNull(s.OfSymbol);
            Assert.IsNotNull(s.TypeRef);
            Assert.AreEqual(0, s.Length);
        }


        [TestCase]
        public void TestClassProperty() {
            var s = RunEmptyCstTest(p => p.ParsePropertyDeclaration(), "");
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
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestClassPropertySpecifier() {
            var s = RunEmptyCstTest(p => p.ParseClassPropertyAccessSpecifier(), "");
            Assert.IsNotNull(s.PropertyReadWrite);
            Assert.IsNotNull(s.PropertyDispInterface);
            Assert.IsNotNull(s.StoredSymbol);
            Assert.IsNotNull(s.StoredProperty);
            Assert.IsNotNull(s.DefaultSymbol);
            Assert.IsNotNull(s.DefaultProperty);
            Assert.IsNotNull(s.NoDefaultSymbol);
            Assert.IsNotNull(s.ImplentsSymbol);
            Assert.IsNotNull(s.ImplementsTypeId);
            Assert.AreEqual(0, s.Length);
        }


        [TestCase]
        public void TestClassPropertyReadWrite() {
            var s = RunEmptyCstTest(p => p.ParseClassPropertyReadWrite(), "");
            Assert.IsNotNull(s.Modifier);
            Assert.IsNotNull(s.Member);
            Assert.AreEqual(0, s.Length);
        }


        [TestCase]
        public void TestClassPropertyDispIntf() {
            var s = RunEmptyCstTest(p => p.ParseClassPropertyDispInterface(), "");
            Assert.IsNotNull(s.Modifier);
            Assert.IsNotNull(s.DispId);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestClosureExpression() {
            var s = RunEmptyCstTest(p => p.ParseClosureExpression(), "");
            Assert.IsNotNull(s.ProcSymbol);
            Assert.IsNotNull(s.Parameters);
            Assert.IsNotNull(s.ColonSymbol);
            Assert.IsNotNull(s.ReturnType);
            Assert.IsNotNull(s.Block);
            Assert.AreEqual(0, s.Length);
        }

        [TestCase]
        public void TestCompoundStatement() {
            var s = RunEmptyCstTest(p => p.ParseCompoundStatement(), "");
            Assert.IsNotNull(s.AssemblerBlock);
            Assert.IsNotNull(s.BeginSymbol);
            Assert.IsNotNull(s.Statements);
            Assert.IsNotNull(s.EndSymbol);
            Assert.AreEqual(0, s.Length);
        }


        [TestCase]
        public void TestConstantExpression() {
            var s = RunEmptyCstTest(p => p.ParseConstantExpression(), "");
            Assert.IsNotNull(s.OpenParen);
            Assert.IsNotNull(s.CloseParen);
            Assert.IsNotNull(s.Value);
            Assert.AreEqual(0, s.Length);
        }

    }
}
