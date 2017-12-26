using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using System;
using System.Linq;
using Xunit;

namespace PasPasPasTests.Parser {

    /// <summary>
    ///     test abstract syntax trees
    /// </summary>
    public class AstTest : ParserTestBase {

        /*
        [Fact]
        public void TestMisc() {
            Func<object, CompilationUnit> u = t => (t as CompilationUnit);
            RunAstTest("unit z.x; interface type class x implementation end.", t => u(t)?.SymbolName, "z.x",
                ParserBase.UnexpectedToken);
        }
        */

        [Fact]
        public void TestUnit() {
            CompilationUnit u(object t) => (t as CompilationUnit);

            RunAstTest("unit z.x; interface implementation end.", t => u(t)?.SymbolName, "z.x");
            RunAstTest("unit z.x; interface implementation end.", t => u(t)?.UnitName.Name, "x");
            RunAstTest("unit z.x; interface implementation end.", t => u(t)?.UnitName.NamespaceName, "z");
            RunAstTest("unit z.x; interface implementation end.", t => u(t)?.FileType, CompilationUnitType.Unit);
            RunAstTest("unit z.x deprecated; interface implementation end.", t => u(t)?.Hints?.SymbolIsDeprecated, true);
            RunAstTest("unit z.x deprecated 'X'; interface implementation end.", t => u(t)?.Hints?.DeprecatedInformation, "X");
            RunAstTest("unit z.x library; interface implementation end.", t => u(t)?.Hints?.SymbolInLibrary, true);
            RunAstTest("unit z.x platform; interface implementation end.", t => u(t)?.Hints?.SymbolIsPlatformSpecific, true);
            RunAstTest("unit z.x experimental; interface implementation end.", t => u(t)?.Hints?.SymbolIsExperimental, true);

            RunAstTest("unit z.x; interface implementation end. § unit z.x; interface implementation end.",
                t => (t as CompilationUnit)?.SymbolName, "z.x", errorMessages: StructuralErrors.DuplicateUnitName);

            RunAstTest("unit z.x.q; interface implementation end.",
                t => (t as CompilationUnit)?.SymbolName, "z.x.q", errorMessages: StructuralErrors.UnitNameDoesNotMatchFileName);
        }

        [Fact]
        public void TestLibrary() {
            CompilationUnit u(object t) => (t as CompilationUnit);

            RunAstTest("library z.x; begin end.", t => u(t)?.SymbolName, "z.x");
            RunAstTest("library z.x; begin end.", t => u(t)?.UnitName.Name, "x");
            RunAstTest("library z.x; begin end.", t => u(t)?.UnitName.NamespaceName, "z");
            RunAstTest("library z.x; begin end.", t => u(t)?.FileType, CompilationUnitType.Library);

            RunAstTest("library z.x; uses a; begin end.", t => u(t)?.RequiredUnits["a"]?.Name.CompleteName, "a");

            RunAstTest("library z.x deprecated;  begin end.", t => u(t)?.Hints?.SymbolIsDeprecated, true);
            RunAstTest("library z.x deprecated 'X'; begin end.", t => u(t)?.Hints?.DeprecatedInformation, "X");
            RunAstTest("library z.x library; begin end.", t => u(t)?.Hints?.SymbolInLibrary, true);
            RunAstTest("library z.x platform; begin end.", t => u(t)?.Hints?.SymbolIsPlatformSpecific, true);
            RunAstTest("library z.x experimental; begin end.", t => u(t)?.Hints?.SymbolIsExperimental, true);

            RunAstTest("library z.x; begin end. § unit z.x; interface implementation end.",
                t => (t as CompilationUnit)?.SymbolName, "z.x", errorMessages: StructuralErrors.DuplicateUnitName);

            RunAstTest("library z.x.q;  begin end.",
                t => (t as CompilationUnit)?.SymbolName, "z.x.q", errorMessages: StructuralErrors.UnitNameDoesNotMatchFileName);

        }

        [Fact]
        public void TestProgram() {

            CompilationUnit u(object t) => (t as CompilationUnit);

            RunAstTest("program z.x; begin end.", t => u(t)?.FileType, CompilationUnitType.Program);
            RunAstTest("program z.x; begin end.", t => u(t)?.SymbolName, "z.x");
            RunAstTest("program z.x; begin end.", t => u(t)?.UnitName.Name, "x");
            RunAstTest("program z.x; begin end.", t => u(t)?.UnitName.NamespaceName, "z");

            RunAstTest("program z.x(a); begin end.", t => u(t)?.SymbolName, "z.x");
            RunAstTest("program z.x(a,b,c); begin end.", t => u(t)?.SymbolName, "z.x");

            RunAstTest("program z.x; uses a; begin end.", t => u(t)?.RequiredUnits?["a"]?.Name?.CompleteName, "a");
            RunAstTest("program z.x; uses a in 'a.pas'; begin end.", t => u(t)?.RequiredUnits?["a"]?.FileName, "a.pas");

            RunAstTest("program z.x; begin end. § unit z.x; interface implementation end.",
                t => (t as CompilationUnit)?.SymbolName, "z.x", errorMessages: StructuralErrors.DuplicateUnitName);

            RunAstTest("program z.x.q; begin end.",
                t => (t as CompilationUnit)?.SymbolName, "z.x.q", errorMessages: StructuralErrors.UnitNameDoesNotMatchFileName);


        }

        [Fact]
        public void TestPackage() {

            CompilationUnit u(object t) => (t as CompilationUnit);

            RunAstTest("package z.x; requires a, a.a, a.a.a, x; end.", t => u(t)?.FileType, CompilationUnitType.Package);
            RunAstTest("package z.x; requires a, a.a, a.a.a, x; end.", t => u(t)?.SymbolName, "z.x");
            RunAstTest("package z.x; requires a, a.a, a.a.a, x; end.", t => u(t)?.UnitName.Name, "x");
            RunAstTest("package z.x; requires a, a.a, a.a.a, x; end.", t => u(t)?.UnitName.NamespaceName, "z");

            RunAstTest("package z.x; requires a, a.a, a.a.a, x; end.", t => u(t)?.RequiredUnits["a.a"]?.Name?.CompleteName, "a.a");
            RunAstTest("package z.x; requires a, a.a, a.a.a, x; end.", t => u(t)?.RequiredUnits["a.a"]?.Mode, UnitMode.Requires);

            RunAstTest("package z.x; requires a.a, a.a.a; contains b.b in 'b.pas'; end.", t => u(t)?.RequiredUnits["a.a"]?.Name.CompleteName, "a.a");
            RunAstTest("package z.x; requires a, a.a, a.a.a; contains b.b in 'b.pas'; end.", t => u(t)?.RequiredUnits["a.a"]?.Mode, UnitMode.Requires);
            RunAstTest("package z.x; requires a, a.a, a.a.a; contains b.b in 'b.pas'; end.", t => u(t)?.RequiredUnits["b.b"]?.Mode, UnitMode.Contains);


        }

        [Fact]
        public void TestUsesClause() {
            RequiredUnitNameList u(object t) => (t as CompilationUnit)?.RequiredUnits;
            RunAstTest("unit z.x; interface uses a; implementation end.", t => u(t)?.Contains("a"), true);
            RunAstTest("unit z.x; interface uses a; implementation end.", t => u(t)?["a"].Mode, UnitMode.Interface);

            RunAstTest("unit z.x; interface implementation uses a; end.", t => u(t)?.Contains("a"), true);
            RunAstTest("unit z.x; interface implementation uses a; end.", t => u(t)?["a"].Mode, UnitMode.Implementation);

            RunAstTest("unit z.x; interface uses a, a; implementation end.", t => u(t)?.Contains("a"), true,
                errorMessages: StructuralErrors.RedeclaredUnitNameInUsesList);

            RunAstTest("unit z.x; interface uses a; implementation uses a; end.", t => u(t)?.Contains("a"), true,
                errorMessages: StructuralErrors.RedeclaredUnitNameInUsesList);

            RunAstTest("unit z.x; interface implementation uses a, a; end.", t => u(t)?.Contains("a"), true,
                errorMessages: StructuralErrors.RedeclaredUnitNameInUsesList);

            RunAstTest("package z.x; requires x; contains a; end.", t => u(t)?.Contains("a"), true);
            RunAstTest("package z.x; requires x; contains a; end.", t => u(t)?["a"].Mode, UnitMode.Contains);

            RunAstTest("package z.x; requires x, x; end.", t => u(t)?.Contains("x"), true,
                errorMessages: StructuralErrors.RedeclaredUnitNameInUsesList);
        }

        [Fact]
        public void TestUsesFileClause() {
            RequiredUnitNameList u(object t) => (t as CompilationUnit)?.RequiredUnits;
            RunAstTest("program z.x; uses a; begin end.", t => u(t)?.Contains("a"), true);
            RunAstTest("program z.x; uses a; begin end.", t => u(t)?["a"].Mode, UnitMode.Program);
            RunAstTest("program z.x; uses a in 'a\\a\\a.pas'; begin end.", t => u(t)?["a"].FileName, "a\\a\\a.pas");

            RunAstTest("program z.x; uses a, a; begin end.", t => u(t)?.Contains("a"), true,
               errorMessages: StructuralErrors.RedeclaredUnitNameInUsesList);

            RunAstTest("library z.x; uses a; begin end.", t => u(t)?.Contains("a"), true);
            RunAstTest("library z.x; uses a; begin end.", t => u(t)?["a"].Mode, UnitMode.Library);
            RunAstTest("library z.x; uses a in 'a\\a\\a.pas'; begin end.", t => u(t)?["a"].FileName, "a\\a\\a.pas");

            RunAstTest("library z.x; uses a, a; begin end.", t => u(t)?.Contains("a"), true,
                errorMessages: StructuralErrors.RedeclaredUnitNameInUsesList);

            RunAstTest("package z.x; requires x; contains a; end.", t => u(t)?.Contains("a"), true);
            RunAstTest("package z.x; requires x; contains a; end.", t => u(t)?["a"].Mode, UnitMode.Contains);
            RunAstTest("package z.x; requires x; contains a in 'a\\a\\a.pas'; end.", t => u(t)?["a"].FileName, "a\\a\\a.pas");

            RunAstTest("package z.x; requires x; contains a, a; end.", t => u(t)?.Contains("a"), true,
               errorMessages: StructuralErrors.RedeclaredUnitNameInUsesList);
        }

        [Fact]
        public void TestConstDeclaration() {
            ConstantDeclaration u(object t) => ((t as CompilationUnit)?.InterfaceSymbols["x"]) as ConstantDeclaration;
            ConstantDeclaration v(object t) => ((t as CompilationUnit)?.ImplementationSymbols["x"]) as ConstantDeclaration;

            RunAstTest("unit z.x; interface const x = 5; implementation end.", t => u(t)?.SymbolName, "x");
            RunAstTest("unit z.x; interface const x = 5; implementation end.", t => u(t)?.Mode, DeclarationMode.Const);
            RunAstTest("unit z.x; interface resourcestring x = 'a'; implementation end.", t => u(t)?.Mode, DeclarationMode.ResourceString);

            RunAstTest("unit z.x; interface const x : TValue = (); implementation end.", t => u(t)?.SymbolName, "x");
            RunAstTest("unit z.x; interface const x : array[2,2] of integer = ((0,0),(0,0)); implementation end.", t => u(t)?.Value?.GetType(), typeof(ArrayConstant));
            RunAstTest("unit z.x; interface const x : array[2] of integer = (SizeOf(T),SizeOf(T)); implementation end.", t => u(t)?.Value?.GetType(), typeof(ArrayConstant));

            RunAstTest("unit z.x; interface implementation const x = 5; end.", t => v(t)?.SymbolName, "x");
            RunAstTest("unit z.x; interface implementation const x = 5; end.", t => v(t)?.Mode, DeclarationMode.Const);
            RunAstTest("unit z.x; interface implementation resourcestring x = 'a'; end.", t => v(t)?.Mode, DeclarationMode.ResourceString);

            RunAstTest("unit z.x; interface const x = 5; x = 6; implementation end.", t => u(t)?.Mode, DeclarationMode.Const,
                errorMessages: StructuralErrors.RedeclaredSymbol);
        }

        [Fact]
        public void TestArrayType() {
            ArrayTypeDeclaration u(object t) => (((t as CompilationUnit)?.InterfaceSymbols["x"]) as ConstantDeclaration)?.TypeValue as ArrayTypeDeclaration;

            RunAstTest("unit z.x; interface const x : array of const = nil; implementation end.", t => u(t) != null, true);
            RunAstTest("unit z.x; interface const x : array of const = nil; implementation end.", t => (u(t)?.TypeValue as MetaType)?.Kind, MetaTypeKind.Const);
            RunAstTest("unit z.x; interface const x : array of const = nil; implementation end.", t => u(t)?.PackedType, false);
            RunAstTest("unit z.x; interface const x : packed array of const = nil; implementation end.", t => u(t)?.PackedType, true);
            RunAstTest("unit z.x; interface const x : array [5] of const = nil; implementation end.", t => u(t)?.IndexItems.Count, 1);
            RunAstTest("unit z.x; interface const x : array [5, 5] of cost = nil; implementation end.", t => u(t)?.IndexItems.Count, 2);
            RunAstTest("unit z.x; interface const x : array [5..5, 5] of const = nil; implementation end.", t => (u(t)?.IndexItems[0] as BinaryOperator)?.Kind, ExpressionKind.RangeOperator);
        }

        [Fact]
        public void TestSetOfType() {
            SetTypeDeclaration u(object t) => (((t as CompilationUnit)?.InterfaceSymbols["x"]) as ConstantDeclaration)?.TypeValue as SetTypeDeclaration;

            RunAstTest("unit z.x; interface const x : set of array of const = nil; implementation end.", t => ((u(t)?.TypeValue as ArrayTypeDeclaration)?.TypeValue as MetaType)?.Kind, MetaTypeKind.Const);
        }

        [Fact]
        public void TestPointerToType() {
            PointerToType u(object t) => (((t as CompilationUnit)?.InterfaceSymbols["x"]) as TypeDeclaration)?.TypeValue as PointerToType;
            TypeDeclaration v(object t) => ((t as CompilationUnit)?.InterfaceSymbols["x"]) as TypeDeclaration;

            RunAstTest("unit z.x; interface type x = ^Pointer; implementation end.", t => (u(t)?.TypeValue as MetaType)?.Kind, MetaTypeKind.Pointer);
            RunAstTest("unit z.x; interface type x = Pointer; implementation end.", t => (v(t)?.TypeValue as MetaType)?.Kind, MetaTypeKind.Pointer);
        }

        [Fact]
        public void TestFileType() {
            FileTypeDeclaration u(object t) => (((t as CompilationUnit)?.InterfaceSymbols["x"]) as ConstantDeclaration)?.TypeValue as FileTypeDeclaration;

            RunAstTest("unit z.x; interface const x : file of array of const = nil; implementation end.", t => ((u(t)?.TypeValue as ArrayTypeDeclaration)?.TypeValue as MetaType)?.Kind, MetaTypeKind.Const);
        }

        [Fact]
        public void TestClassOfType() {
            ClassOfTypeDeclaration u(object t) => (((t as CompilationUnit)?.InterfaceSymbols["x"]) as ConstantDeclaration)?.TypeValue as ClassOfTypeDeclaration;

            RunAstTest("unit z.x; interface const x : class of TFuzz = nil; implementation end.", t => (u(t)?.TypeValue as MetaType)?.Kind, MetaTypeKind.NamedType);
            RunAstTest("unit z.x; interface const x : class of TFuzz = nil; implementation end.", t => (u(t)?.TypeValue as MetaType)?.Fragments[0]?.Name, "TFuzz");
        }

        [Fact]
        public void TestEnumType() {
            EnumType u(object t) => (((t as CompilationUnit)?.InterfaceSymbols["x"]) as TypeDeclaration)?.TypeValue as EnumType;

            RunAstTest("unit z.x; interface type x = (a, b); implementation end.", t => u(t)?.Contains("a"), true);
            RunAstTest("unit z.x; interface type x = (a, b); implementation end.", t => u(t)?["a"].Name.CompleteName, "a");

            RunAstTest("unit z.x; interface type x = (a, b, a); implementation end.", t => u(t)?["a"].Name.CompleteName, "a",
                errorMessages: StructuralErrors.RedeclaredEnumName);

        }

        [Fact]
        public void TestSubrangeType() {
            SubrangeType u(object t) => (((t as CompilationUnit)?.InterfaceSymbols["x"]) as TypeDeclaration)?.TypeValue as SubrangeType;

            RunAstTest("unit z.x; interface type x = 3..4; implementation end.", t => u(t) != null, true);

        }

        [Fact]
        public void TestStringType() {
            MetaType u(object t) => (((t as CompilationUnit)?.InterfaceSymbols["x"]) as TypeDeclaration)?.TypeValue as MetaType;

            RunAstTest("unit z.x; interface type x = string; implementation end.", t => u(t)?.Kind, MetaTypeKind.String);
            RunAstTest("unit z.x; interface type x = string[232]; implementation end.", t => u(t)?.Kind, MetaTypeKind.ShortString);
            RunAstTest("unit z.x; interface type x = ShortString; implementation end.", t => u(t)?.Kind, MetaTypeKind.ShortStringDefault);
            RunAstTest("unit z.x; interface type x = Ansistring; implementation end.", t => u(t)?.Kind, MetaTypeKind.AnsiString);
            RunAstTest("unit z.x; interface type x = uniCodeString; implementation end.", t => u(t)?.Kind, MetaTypeKind.UnicodeString);
            RunAstTest("unit z.x; interface type x = wideString; implementation end.", t => u(t)?.Kind, MetaTypeKind.WideString);

            RunAstTest("unit z.x; interface type x = string[nil]; implementation end.", t => (u(t)?.Value as ConstantValue)?.Kind, ConstantValueKind.Nil);
            RunAstTest("unit z.x; interface type x = ansistring(nil); implementation end.", t => (u(t)?.Value as ConstantValue)?.Kind, ConstantValueKind.Nil);
        }

        [Fact]
        public void TestTypeAlias() {
            TypeAlias u(object t) => (((t as CompilationUnit)?.InterfaceSymbols["x"]) as TypeDeclaration)?.TypeValue as TypeAlias;

            RunAstTest("unit z.x; interface type x = z.q; implementation end.", t => u(t)?.AsScopedName?.ToString(), "z.q");
            RunAstTest("unit z.x; interface type x = z.q; implementation end.", t => u(t)?.IsNewType, false);
            RunAstTest("unit z.x; interface type x = type z.q; implementation end.", t => u(t)?.AsScopedName.ToString(), "z.q");
            RunAstTest("unit z.x; interface type x = type z.q; implementation end.", t => u(t)?.IsNewType, true);

            RunAstTest("unit z.x; interface type x = type z<r>.q; implementation end.", t => u(t)?.Fragments[0]?.TypeValue is TypeAlias, true);

            RunAstTest("unit z.x; interface type x = type z<r,array of const>.q<t>; implementation end.", t => u(t)?.Fragments[0]?.TypeValue is TypeAlias, false);
            RunAstTest("unit z.x; interface type x = type z<r>.q<t>; implementation end.", t => u(t)?.Fragments[1]?.TypeValue is TypeAlias, true);

            RunAstTest("unit z.x; interface type x = type of z.q; implementation end.", t => u(t)?.IsNewType, true,
                errorMessages: StructuralErrors.UnsupportedTypeOfConstruct);
        }

        [Fact]
        public void TestProceduralType() {
            ProceduralType u(object t) => (((t as CompilationUnit)?.InterfaceSymbols["x"]) as TypeDeclaration)?.TypeValue as ProceduralType;

            RunAstTest("unit z.x; interface type x = procedure(x: string); implementation end.", t => u(t)?.Kind, ProcedureKind.Procedure);
            RunAstTest("unit z.x; interface type x = procedure(x: string); implementation end.", t => u(t)?.MethodDeclaration, false);
            RunAstTest("unit z.x; interface type x = procedure(x: string) of object; implementation end.", t => u(t)?.MethodDeclaration, true);
            RunAstTest("unit z.x; interface type x = procedure(x: string); implementation end.", t => u(t)?.AllowAnonymousMethods, false);
            RunAstTest("unit z.x; interface type x = procedure(x: string) of object; implementation end.", t => u(t)?.AllowAnonymousMethods, false);
            RunAstTest("unit z.x; interface type x = reference to procedure(x: string); implementation end.", t => u(t)?.AllowAnonymousMethods, true);
            RunAstTest("unit z.x; interface type x = function(x: string): string; implementation end.", t => u(t)?.Kind, ProcedureKind.Function);
            RunAstTest("unit z.x; interface type x = function(x: string): string; implementation end.", t => (u(t)?.TypeValue as MetaType)?.Kind, MetaTypeKind.String);
            RunAstTest("unit z.x; interface type x = function(x: string): [x] string; implementation end.", t => (u(t)?.ReturnAttributes.FirstOrDefault())?.SymbolName, "x");

        }

        [Fact]
        public void TestFormalParameters() {
            ParameterTypeDefinition u(object t) => ((((t as CompilationUnit)?.InterfaceSymbols["x"]) as TypeDeclaration)?.TypeValue as ProceduralType)?.Parameters?.Items[0] as ParameterTypeDefinition;

            RunAstTest("unit z.x; interface type x = procedure(x: string); implementation end.", t => u(t)?.Parameters[0].Name.CompleteName, "x");
            RunAstTest("unit z.x; interface type x = procedure(x: string); implementation end.", t => (u(t)?.TypeValue as MetaType)?.Kind, MetaTypeKind.String);
            RunAstTest("unit z.x; interface type x = procedure([n] x: string); implementation end.", t => u(t)?.Parameters[0]?.Attributes?.FirstOrDefault()?.SymbolName, "n");
            RunAstTest("unit z.x; interface type x = procedure([n] const [m] x: string); implementation end.", t => u(t)?.Parameters[0]?.Attributes?.Skip(1)?.FirstOrDefault()?.SymbolName, "m");
            RunAstTest("unit z.x; interface type x = procedure([n] x: string); implementation end.", t => u(t)?.Parameters[0]?.ParameterKind, ParameterReferenceKind.Undefined);
            RunAstTest("unit z.x; interface type x = procedure([n] var x: string); implementation end.", t => u(t)?.Parameters[0]?.ParameterKind, ParameterReferenceKind.Var);
            RunAstTest("unit z.x; interface type x = procedure([n] const x: string); implementation end.", t => u(t)?.Parameters[0]?.ParameterKind, ParameterReferenceKind.Const);
            RunAstTest("unit z.x; interface type x = procedure([n] out x: string); implementation end.", t => u(t)?.Parameters[0]?.ParameterKind, ParameterReferenceKind.Out);
            RunAstTest("unit z.x; interface type x = procedure([n] var x, z: string); implementation end.", t => u(t)?.Parameters[1]?.ParameterKind, ParameterReferenceKind.Var);
            RunAstTest("unit z.x; interface type x = procedure([n] const x, z: string); implementation end.", t => u(t)?.Parameters[1]?.ParameterKind, ParameterReferenceKind.Const);
            RunAstTest("unit z.x; interface type x = procedure([n] out x, z: string); implementation end.", t => u(t)?.Parameters[1]?.ParameterKind, ParameterReferenceKind.Out);

            RunAstTest("unit z.x; interface type x = procedure(x, x: string); implementation end.", t => u(t)?.Parameters[0]?.Name.CompleteName, "x",
                errorMessages: StructuralErrors.DuplicateParameterName);

            RunAstTest("unit z.x; interface type x = procedure(x: string; x: string); implementation end.", t => u(t)?.Parameters[0]?.Name.CompleteName, "x",
                errorMessages: StructuralErrors.DuplicateParameterName);
        }

        [Fact]
        public void TestUnitInitialization() {
            BlockOfStatements u(object t) => (t as CompilationUnit)?.InitializationBlock as BlockOfStatements;

            RunAstTest("unit z.x; interface implementation initialization begin end end.", t => u(t)?.Statements[0]?.GetType(), typeof(BlockOfStatements));
        }

        [Fact]
        public void TestVarDeclaration() {
            VariableDeclaration f(object t) => ((t as CompilationUnit)?.ImplementationSymbols["x"]?.ParentItem as VariableDeclaration);

            RunAstTest("unit z.x; interface implementation var x: string; end.", t => f(t)?.Names[0]?.Name.CompleteName, "x");
            RunAstTest("unit z.x; interface implementation var x: string; end.", t => f(t)?.Mode, DeclarationMode.Var);
            RunAstTest("unit z.x; interface implementation threadvar x: string; end.", t => f(t)?.Mode, DeclarationMode.ThreadVar);
            RunAstTest("unit z.x; interface implementation var x: string experimental; end.", t => f(t)?.Hints?.SymbolIsExperimental, true);
            RunAstTest("unit z.x; interface implementation var [a] x: string; end.", t => f(t)?.Attributes[0]?.SymbolName, "a");
            RunAstTest("unit z.x; interface implementation var x: string; end.", t => f(t)?.TypeValue?.GetType(), typeof(MetaType));
            RunAstTest("unit z.x; interface implementation var x: string = 5; end.", t => f(t)?.ValueKind, VariableValueKind.InitialValue);
            RunAstTest("unit z.x; interface implementation var x: string = nil; end.", t => f(t)?.Value?.GetType(), typeof(ConstantValue));
            RunAstTest("unit z.x; interface implementation var x: string absolute 5; end.", t => f(t)?.ValueKind, VariableValueKind.Absolute);
        }

        [Fact]
        public void TestExportedProcedureHeading() {
            GlobalMethod f(object t) => ((t as CompilationUnit)?.InterfaceSymbols["e"] as GlobalMethod);

            RunAstTest("unit z.x; interface type procedure e(); implementation end.", t => f(t)?.Name?.CompleteName, "e");
            RunAstTest("unit z.x; interface type procedure e(); implementation end.", t => f(t)?.Kind, ProcedureKind.Procedure);
            RunAstTest("unit z.x; interface type function e(): string; implementation end.", t => f(t)?.Name?.CompleteName, "e");
            RunAstTest("unit z.x; interface type function e(): string; implementation end.", t => f(t)?.Kind, ProcedureKind.Function);
            //BOGUS: RunAstTest("unit z.x; interface type [a] procedure e(); implementation end.", t => f(t)?.Attributes[0]?.Name?.CompleteName, "a");

            RunAstTest("unit z.x; interface type function e(): string; implementation end.", t => f(t)?.TypeValue?.GetType(), typeof(MetaType));
            RunAstTest("unit z.x; interface type procedure e(e: string); implementation end.", t => f(t)?.Parameters["e"]?.Name?.CompleteName, "e");
            RunAstTest("unit z.x; interface type procedure e(e: string); implementation end.", t => (f(t)?.Parameters["e"]?.ParentItem as ParameterTypeDefinition)?.TypeValue?.GetType(), typeof(MetaType));

            RunAstTest("unit z.x; interface type procedure e(e: string); overload; implementation end.", t => f(t)?.Directives[0]?.Kind, MethodDirectiveKind.Overload);
            RunAstTest("unit z.x; interface type procedure e(e: string); inline; implementation end.", t => f(t)?.Directives[0]?.Kind, MethodDirectiveKind.Inline);
            RunAstTest("unit z.x; interface type procedure e(e: string); assembler; implementation end.", t => f(t)?.Directives[0]?.Kind, MethodDirectiveKind.Assembler);
            RunAstTest("unit z.x; interface type procedure e(e: string); cdecl; implementation end.", t => f(t)?.Directives[0]?.Kind, MethodDirectiveKind.Cdecl);
            RunAstTest("unit z.x; interface type procedure e(e: string); pascal; implementation end.", t => f(t)?.Directives[0]?.Kind, MethodDirectiveKind.Pascal);
            RunAstTest("unit z.x; interface type procedure e(e: string); register; implementation end.", t => f(t)?.Directives[0]?.Kind, MethodDirectiveKind.Register);
            RunAstTest("unit z.x; interface type procedure e(e: string); safecall; implementation end.", t => f(t)?.Directives[0]?.Kind, MethodDirectiveKind.Safecall);
            RunAstTest("unit z.x; interface type procedure e(e: string); stdcall; implementation end.", t => f(t)?.Directives[0]?.Kind, MethodDirectiveKind.StdCall);
            RunAstTest("unit z.x; interface type procedure e(e: string); export; implementation end.", t => f(t)?.Directives[0]?.Kind, MethodDirectiveKind.Export);
            RunAstTest("unit z.x; interface type procedure e(e: string); far; implementation end.", t => f(t)?.Directives[0]?.Kind, MethodDirectiveKind.Far);
            RunAstTest("unit z.x; interface type procedure e(e: string); near; implementation end.", t => f(t)?.Directives[0]?.Kind, MethodDirectiveKind.Near);
            RunAstTest("unit z.x; interface type procedure e(e: string); local; implementation end.", t => f(t)?.Directives[0]?.Kind, MethodDirectiveKind.Local);
            RunAstTest("unit z.x; interface type procedure e(e: string); deprecated; implementation end.", t => f(t)?.Hints?.SymbolIsDeprecated, true);
            RunAstTest("unit z.x; interface type procedure e(e: string); experimental; implementation end.", t => f(t)?.Hints?.SymbolIsExperimental, true);
            RunAstTest("unit z.x; interface type procedure e(e: string); varargs; implementation end.", t => f(t)?.Directives[0]?.Kind, MethodDirectiveKind.VarArgs);
            RunAstTest("unit z.x; interface type procedure e(e: string); external 'e'; implementation end.", t => f(t)?.Directives[0]?.Kind, MethodDirectiveKind.External);
            RunAstTest("unit z.x; interface type procedure e(e: string); external 'e' name 'e'; implementation end.", t => f(t)?.Directives[0]?.Specifiers[0]?.Kind, MethodDirectiveSpecifierKind.Name);
            RunAstTest("unit z.x; interface type procedure e(e: string); external 'e' index 'e'; implementation end.", t => f(t)?.Directives[0]?.Specifiers[0]?.Kind, MethodDirectiveSpecifierKind.Index);
            RunAstTest("unit z.x; interface type procedure e(e: string); external 'e' delayed; implementation end.", t => f(t)?.Directives[0]?.Specifiers[0]?.Kind, MethodDirectiveSpecifierKind.Delayed);
            RunAstTest("unit z.x; interface type procedure e(e: string); external 'e' dependency 'a', 'b'; implementation end.", t => f(t)?.Directives[0]?.Specifiers[0]?.Kind, MethodDirectiveSpecifierKind.Dependency);
            RunAstTest("unit z.x; interface type procedure e(e: string); forward; implementation end.", t => f(t)?.Directives[0]?.Kind, MethodDirectiveKind.Forward);
            RunAstTest("unit z.x; interface type procedure e(e: string); unsafe; implementation end.", t => f(t)?.Directives[0]?.Kind, MethodDirectiveKind.Unsafe);
        }

        [Fact]
        public void TestExportedMethods() {
            ExportedMethodDeclaration f(object t) => ((t as CompilationUnit)?.InterfaceSymbols["e"] as ExportedMethodDeclaration);

            RunAstTest("unit z.x; interface exports e(); implementation end.", t => f(t)?.Name?.CompleteName, "e");
            RunAstTest("unit z.x; interface exports e() index nil; implementation end.", t => f(t)?.HasIndex, true);
            RunAstTest("unit z.x; interface exports e() name nil; implementation end.", t => f(t)?.HasName, true);
            RunAstTest("unit z.x; interface exports e() index nil name nil; implementation end.", t => f(t)?.HasName, true);
            RunAstTest("unit z.x; interface exports e() index nil name nil; implementation end.", t => f(t)?.HasIndex, true);
            RunAstTest("unit z.x; interface exports e(a: integer); implementation end.", t => f(t)?.Parameters[0]?.Name?.CompleteName, "a");
        }

        [Fact]
        public void TestClassHelperType() {
            StructuredType r(object t) => ((t as CompilationUnit)?.InterfaceSymbols["z"] as TypeDeclaration)?.TypeValue as StructuredType;
            RunAstTest("unit z.x; interface type z = class helper for T function x(): string; end; implementation end.", t => r(t)?.Kind, StructuredTypeKind.ClassHelper);
            RunAstTest("unit z.x; interface type z = class helper for string function x(): string; end; implementation end.", t => r(t)?.BaseTypes[0]?.GetType(), typeof(MetaType));

            // methods
            RunAstTest("unit z.x; interface type z = class helper for T function x: string; end; implementation end.", t => r(t)?.Methods["x"]?.Name?.CompleteName, "x");
            RunAstTest("unit z.x; interface type z = class helper for T private function x: string; end; implementation end.", t => r(t)?.Methods["x"]?.Visibility, MemberVisibility.Private);
            RunAstTest("unit z.x; interface type z = class helper for T private function x: string; experimental; end; implementation end.", t => r(t)?.Methods["x"]?.Visibility, MemberVisibility.Private);

            // properties
            RunAstTest("unit z.x; interface type z = class helper for T property x: string read q; end; implementation end.", t => r(t)?.Properties["x"]?.SymbolName, "x");
            RunAstTest("unit z.x; interface type z = class helper for T property x: string read q; end; implementation end.", t => r(t)?.Properties["x"]?.Accessors[0]?.Name?.CompleteName, "q");
            RunAstTest("unit z.x; interface type z = class helper for T property x: string read q; end; implementation end.", t => r(t)?.Properties["x"]?.Accessors[0]?.Kind, StructurePropertyAccessorKind.Read);

            // symbols
            RunAstTest("unit z.x; interface type z = class helper for T type t = string; end; implementation end.", t => (r(t)?.Symbols["t"] as TypeDeclaration)?.TypeValue?.GetType(), typeof(MetaType));
            RunAstTest("unit z.x; interface type z = class helper for T const c = nil; end; implementation end.", t => r(t)?.Symbols["c"]?.Name?.CompleteName, "c");

        }

        [Fact]
        public void TestRecordHelperType() {
            StructuredType r(object t) => ((t as CompilationUnit)?.InterfaceSymbols["z"] as TypeDeclaration)?.TypeValue as StructuredType;
            RunAstTest("unit z.x; interface type z = record helper for T function x(): string; end; implementation end.", t => r(t)?.Kind, StructuredTypeKind.RecordHelper);
            RunAstTest("unit z.x; interface type z = record helper for string function x(): string; end; implementation end.", t => r(t)?.BaseTypes[0]?.GetType(), typeof(MetaType));

            // methods
            RunAstTest("unit z.x; interface type z = record helper for T function x: string; end; implementation end.", t => r(t)?.Methods["x"]?.Name?.CompleteName, "x");
            RunAstTest("unit z.x; interface type z = record helper for T private function x: string; end; implementation end.", t => r(t)?.Methods["x"]?.Visibility, MemberVisibility.Private);
            RunAstTest("unit z.x; interface type z = record helper for T private function x: string; experimental; end; implementation end.", t => r(t)?.Methods["x"]?.Visibility, MemberVisibility.Private);

            // properties
            RunAstTest("unit z.x; interface type z = record helper for T property x: string read q; end; implementation end.", t => r(t)?.Properties["x"]?.SymbolName, "x");
            RunAstTest("unit z.x; interface type z = record helper for T property x: string read q; end; implementation end.", t => r(t)?.Properties["x"]?.Accessors[0]?.Name?.CompleteName, "q");
            RunAstTest("unit z.x; interface type z = record helper for T property x: string read q; end; implementation end.", t => r(t)?.Properties["x"]?.Accessors[0]?.Kind, StructurePropertyAccessorKind.Read);

            // symbols
            RunAstTest("unit z.x; interface type z = record helper for T const c = nil; end; implementation end.", t => r(t)?.Symbols["c"]?.Name?.CompleteName, "c");
            RunAstTest("unit z.x; interface type z = record helper for T type t = string; end; implementation end.", t => (r(t)?.Symbols["t"] as TypeDeclaration)?.TypeValue?.GetType(), typeof(MetaType));

        }

        [Fact]
        public void TestRecordType() {
            StructuredType r(object t) => ((t as CompilationUnit)?.InterfaceSymbols["z"] as TypeDeclaration)?.TypeValue as StructuredType;
            RunAstTest("unit z.x; interface type z = record x: integer; end; implementation end.", t => r(t)?.Kind, StructuredTypeKind.Record);

            // fields
            RunAstTest("unit z.x; interface type z = record x: integer; end; implementation end.", t => r(t)?.Fields["x"]?.Name?.CompleteName, "x");
            RunAstTest("unit z.x; interface type z = record x: integer; end; implementation end.", t => r(t)?.Fields.Items[0]?.Fields[0]?.Name?.CompleteName, "x");
            RunAstTest("unit z.x; interface type z = record x: integer; end; implementation end.", t => r(t)?.Fields["x"]?.Visibility, MemberVisibility.Public);
            RunAstTest("unit z.x; interface type z = record x: integer experimental; end; implementation end.", t => r(t)?.Fields["x"]?.Hints?.SymbolIsExperimental, true);
            RunAstTest("unit z.x; interface type z = record x: string; end; implementation end.", t => r(t)?.Fields.Items[0]?.TypeValue?.GetType(), typeof(MetaType));

            RunAstTest("unit z.x; interface type z = record private x: integer; end; implementation end.", t => r(t)?.Fields["x"]?.Visibility, MemberVisibility.Private);
            RunAstTest("unit z.x; interface type z = record protected x: integer; end; implementation end.", t => r(t)?.Fields["x"]?.Visibility, MemberVisibility.Protected);
            RunAstTest("unit z.x; interface type z = record public x: integer; end; implementation end.", t => r(t)?.Fields["x"]?.Visibility, MemberVisibility.Public);
            RunAstTest("unit z.x; interface type z = record strict private x: integer; end; implementation end.", t => r(t)?.Fields["x"]?.Visibility, MemberVisibility.StrictPrivate);
            RunAstTest("unit z.x; interface type z = record strict protected x: integer; end; implementation end.", t => r(t)?.Fields["x"]?.Visibility, MemberVisibility.StrictProtected);
            RunAstTest("unit z.x; interface type z = record published x: integer; end; implementation end.", t => r(t)?.Fields["x"]?.Visibility, MemberVisibility.Published);
            RunAstTest("unit z.x; interface type z = record automated x: integer; end; implementation end.", t => r(t)?.Fields["x"]?.Visibility, MemberVisibility.Automated);

            // methods
            RunAstTest("unit z.x; interface type z = record function x: string; end; implementation end.", t => r(t)?.Methods["x"]?.Name?.CompleteName, "x");
            RunAstTest("unit z.x; interface type z = record private function x: string; end; implementation end.", t => r(t)?.Methods["x"]?.Visibility, MemberVisibility.Private);
            RunAstTest("unit z.x; interface type z = record private function x: string; experimental; end; implementation end.", t => r(t)?.Methods["x"]?.Visibility, MemberVisibility.Private);

            // properties
            RunAstTest("unit z.x; interface type z = record property x: string read q; end; implementation end.", t => r(t)?.Properties["x"]?.SymbolName, "x");
            RunAstTest("unit z.x; interface type z = record property x: string read q; end; implementation end.", t => r(t)?.Properties["x"]?.Accessors[0]?.Name?.CompleteName, "q");
            RunAstTest("unit z.x; interface type z = record property x: string read q; end; implementation end.", t => r(t)?.Properties["x"]?.Accessors[0]?.Kind, StructurePropertyAccessorKind.Read);

            // symbols
            RunAstTest("unit z.x; interface type z = record const c = nil; end; implementation end.", t => r(t)?.Symbols["c"]?.Name?.CompleteName, "c");
            RunAstTest("unit z.x; interface type z = record type t = string; end; implementation end.", t => (r(t)?.Symbols["t"] as TypeDeclaration)?.TypeValue?.GetType(), typeof(MetaType));

            // variants
            RunAstTest("unit z.x; interface type z = record case z : integer of 1, 2: (a: string); 3: (q: string); end; implementation end.", t => r(t)?.Variants["a"]?.TypeValue?.GetType(), typeof(MetaType));
            RunAstTest("unit z.x; interface type z = record case z : integer of 1, 2: (a: string); 3: (q: string); end; implementation end.", t => r(t)?.Variants?.Items[0]?.Items[0]?.Fields[0]?.TypeValue?.GetType(), typeof(MetaType));
            RunAstTest("unit z.x; interface type z = record case z : integer of 1, 2: (a: string); 3: (q: string); end; implementation end.", t => r(t)?.Variants?.Items[0]?.Name?.CompleteName, "z");
            RunAstTest("unit z.x; interface type z = record case z : integer of 1, 2: (a: string); 3: (q: string); end; implementation end.", t => r(t)?.Variants?.Items[0]?.Items[0]?.Expressions?.Count, 2);
        }

        [Fact]
        public void TestClassType() {
            StructuredType u(object t) => ((t as CompilationUnit)?.InterfaceSymbols["x"] as TypeDeclaration)?.TypeValue as StructuredType;
            StructureField f(object t) => (t as StructuredType)?.Fields["n"];
            StructureProperty p(object t) => (t as StructuredType)?.Properties["n"];
            StructureMethod m(object t) => (t as StructuredType)?.Methods["m"];
            StructureMethodResolution r(object t) => (t as StructuredType)?.MethodResolutions.Resolutions[0];
            DeclaredSymbol c(object t) => (t as StructuredType)?.Symbols["c"];

            RunAstTest("unit z.x; interface type x = class class var c: integer; var n: integer; end; implementation end.", t => f(t)?.ClassItem, false);

            RunAstTest("unit z.x; interface type x = class end; implementation end.", t => u(t)?.Kind, StructuredTypeKind.Class);
            RunAstTest("unit z.x; interface type x = class(TObject) end; implementation end.", t => (u(t)?.BaseTypes[0] as MetaType)?.AsScopedName?.ToString(), "TObject");
            RunAstTest("unit z.x; interface type x = class sealed end; implementation end.", t => u(t)?.SealedClass, true);
            RunAstTest("unit z.x; interface type x = class abstract end; implementation end.", t => u(t)?.AbstractClass, true);
            RunAstTest("unit z.x; interface type x = class end; implementation end.", t => u(t)?.ForwardDeclaration, false);
            RunAstTest("unit z.x; interface type x = class; implementation end.", t => u(t)?.ForwardDeclaration, true);

            RunAstTest("unit z.x; interface type x = class n: integer; end; implementation end.", t => (u(t)?.Fields["n"]?.TypeValue as TypeAlias)?.AsScopedName?.ToString(), "integer");

            // fields

            RunAstTest("unit z.x; interface type x = class n: integer; end; implementation end.", t => f(t)?.Visibility, MemberVisibility.Public);
            RunAstTest("unit z.x; interface type x = class class var n: integer; end; implementation end.", t => f(t)?.ClassItem, true);
            RunAstTest("unit z.x; interface type x = class strict private n: integer; end; implementation end.", t => f(t)?.Visibility, MemberVisibility.StrictPrivate);
            RunAstTest("unit z.x; interface type x = class strict protected n: integer; end; implementation end.", t => f(t)?.Visibility, MemberVisibility.StrictProtected);
            RunAstTest("unit z.x; interface type x = class private n: integer; end; implementation end.", t => f(t)?.Visibility, MemberVisibility.Private);
            RunAstTest("unit z.x; interface type x = class protected n: integer; end; implementation end.", t => f(t)?.Visibility, MemberVisibility.Protected);
            RunAstTest("unit z.x; interface type x = class public n: integer; end; implementation end.", t => f(t)?.Visibility, MemberVisibility.Public);
            RunAstTest("unit z.x; interface type x = class published n: integer; end; implementation end.", t => f(t)?.Visibility, MemberVisibility.Published);
            RunAstTest("unit z.x; interface type x = class automated n: integer; end; implementation end.", t => f(t)?.Visibility, MemberVisibility.Automated);
            RunAstTest("unit z.x; interface type x = class [a] n: integer; end; implementation end.", t => f(t)?.Attributes[0]?.Name?.CompleteName, "a");
            RunAstTest("unit z.x; interface type x = class [a] x, [b] n: integer; end; implementation end.", t => f(t)?.Attributes[0]?.Name?.CompleteName, "b");

            RunAstTest("unit z.x; interface type x = class n: integer; end; implementation end.", t => f(t)?.ClassItem, false);

            RunAstTest("unit z.x; interface type x = class class var c: integer; protected n: integer; end; implementation end.", t => f(t)?.ClassItem, false);
            RunAstTest("unit z.x; interface type x = class class var n: integer; end; implementation end.", t => f(t)?.ClassItem, true);
            RunAstTest("unit z.x; interface type x = class class var q: integer; n: integer; end; implementation end.", t => f(t)?.ClassItem, true);

            RunAstTest("unit z.x; interface type x = class n: integer deprecated; end; implementation end.", t => u(t)?.Fields["n"].Hints.SymbolIsDeprecated, true);

            RunAstTest("unit z.x; interface type x = class n, n: integer; end; implementation end.", t => f(t)?.Name?.CompleteName, "n",
                errorMessages: StructuralErrors.DuplicateFieldName);

            RunAstTest("unit z.x; interface type x = class n: integer; n: integer; end; implementation end.", t => f(t)?.Name?.CompleteName, "n",
                errorMessages: StructuralErrors.DuplicateFieldName);

            // properties

            RunAstTest("unit z.x; interface type x = class property n: integer; end; implementation end.", t => p(t)?.Visibility, MemberVisibility.Public);
            RunAstTest("unit z.x; interface type x = class property n: integer; end; implementation end.", t => p(t)?.Name.CompleteName, "n");
            RunAstTest("unit z.x; interface type x = class [a] property n: integer; end; implementation end.", t => p(t)?.Attributes[0]?.Name.CompleteName, "a");
            RunAstTest("unit z.x; interface type x = class strict private property n: integer; end; implementation end.", t => p(t)?.Visibility, MemberVisibility.StrictPrivate);
            RunAstTest("unit z.x; interface type x = class strict protected property n: integer; end; implementation end.", t => p(t)?.Visibility, MemberVisibility.StrictProtected);
            RunAstTest("unit z.x; interface type x = class private property n: integer; end; implementation end.", t => p(t)?.Visibility, MemberVisibility.Private);
            RunAstTest("unit z.x; interface type x = class protected property n: integer; end; implementation end.", t => p(t)?.Visibility, MemberVisibility.Protected);
            RunAstTest("unit z.x; interface type x = class public property n: integer; end; implementation end.", t => p(t)?.Visibility, MemberVisibility.Public);
            RunAstTest("unit z.x; interface type x = class published property n: integer; end; implementation end.", t => p(t)?.Visibility, MemberVisibility.Published);
            RunAstTest("unit z.x; interface type x = class automated property n: integer; end; implementation end.", t => p(t)?.Visibility, MemberVisibility.Automated);
            RunAstTest("unit z.x; interface type x = class property n: integer read q; end; implementation end.", t => p(t)?.Accessors[0]?.Kind, StructurePropertyAccessorKind.Read);
            RunAstTest("unit z.x; interface type x = class property n: integer read q; end; implementation end.", t => p(t)?.Accessors[0]?.Name.CompleteName, "q");
            RunAstTest("unit z.x; interface type x = class property n: integer write q; end; implementation end.", t => p(t)?.Accessors[0]?.Kind, StructurePropertyAccessorKind.Write);
            RunAstTest("unit z.x; interface type x = class property n: integer write q; end; implementation end.", t => p(t)?.Accessors[0]?.Name.CompleteName, "q");
            RunAstTest("unit z.x; interface type x = class property n: integer add q; end; implementation end.", t => p(t)?.Accessors[0]?.Kind, StructurePropertyAccessorKind.Add);
            RunAstTest("unit z.x; interface type x = class property n: integer add q; end; implementation end.", t => p(t)?.Accessors[0]?.Name.CompleteName, "q");
            RunAstTest("unit z.x; interface type x = class property n: integer remove q; end; implementation end.", t => p(t)?.Accessors[0]?.Kind, StructurePropertyAccessorKind.Remove);
            RunAstTest("unit z.x; interface type x = class property n: integer remove q; end; implementation end.", t => p(t)?.Accessors[0]?.Name.CompleteName, "q");
            RunAstTest("unit z.x; interface type x = class property n: integer readonly; end; implementation end.", t => p(t)?.Accessors[0]?.Kind, StructurePropertyAccessorKind.ReadOnly);
            RunAstTest("unit z.x; interface type x = class property n: integer writeonly; end; implementation end.", t => p(t)?.Accessors[0]?.Kind, StructurePropertyAccessorKind.WriteOnly);
            RunAstTest("unit z.x; interface type x = class property n: integer dispid 27; end; implementation end.", t => p(t)?.Accessors[0]?.Kind, StructurePropertyAccessorKind.DispId);
            RunAstTest("unit z.x; interface type x = class property n: integer stored 27; end; implementation end.", t => p(t)?.Accessors[0]?.Kind, StructurePropertyAccessorKind.Stored);
            RunAstTest("unit z.x; interface type x = class property n: integer default 27; end; implementation end.", t => p(t)?.Accessors[0]?.Kind, StructurePropertyAccessorKind.Default);
            RunAstTest("unit z.x; interface type x = class property n: integer nodefault; end; implementation end.", t => p(t)?.Accessors[0]?.Kind, StructurePropertyAccessorKind.NoDefault);
            RunAstTest("unit z.x; interface type x = class property n: integer implements x; end; implementation end.", t => p(t)?.Accessors[0]?.Kind, StructurePropertyAccessorKind.Implements);
            RunAstTest("unit z.x; interface type x = class property n: integer implements x; end; implementation end.", t => p(t)?.Accessors[0]?.Name?.CompleteName, "x");
            RunAstTest("unit z.x; interface type x = class property n[q: integer]: integer; end; implementation end.", t => p(t)?.Parameters.Items[0]?.Parameters[0]?.Name?.CompleteName, "q");

            // methods
            RunAstTest("unit z.x; interface type x = class procedure m(q: integer); end; implementation end.", t => m(t)?.ClassItem, false);
            RunAstTest("unit z.x; interface type x = class class procedure m(q: integer); end; implementation end.", t => m(t)?.ClassItem, true);
            RunAstTest("unit z.x; interface type x = class function m(q: integer): string; end; implementation end.", t => m(t)?.ClassItem, false);
            RunAstTest("unit z.x; interface type x = class class function m(q: integer): string; end; implementation end.", t => m(t)?.ClassItem, true);

            RunAstTest("unit z.x; interface type x = class procedure m(q: integer); end; implementation end.", t => m(t)?.Name?.CompleteName, "m");
            RunAstTest("unit z.x; interface type x = class procedure m(q: integer); end; implementation end.", t => m(t)?.Parameters["q"]?.Name?.CompleteName, "q");
            RunAstTest("unit z.x; interface type x = class procedure m(q: string); end; implementation end.", t => m(t)?.Parameters?.Items[0]?.TypeValue?.GetType(), typeof(MetaType));
            RunAstTest("unit z.x; interface type x = class function m(q: integer): integer; end; implementation end.", t => m(t)?.Name?.CompleteName, "m");
            RunAstTest("unit z.x; interface type x = class function m(q: integer): integer; end; implementation end.", t => m(t)?.Parameters["q"]?.Name?.CompleteName, "q");
            RunAstTest("unit z.x; interface type x = class function m(q: string): integer; end; implementation end.", t => m(t)?.Parameters?.Items[0]?.TypeValue?.GetType(), typeof(MetaType));
            RunAstTest("unit z.x; interface type x = class function m(q: integer): string; end; implementation end.", t => m(t)?.TypeValue?.GetType(), typeof(MetaType));

            RunAstTest("unit z.x; interface type x = class constructor m(q: string); end; implementation end.", t => m(t)?.Kind, ProcedureKind.Constructor);
            RunAstTest("unit z.x; interface type x = class destructor m(q: string); end; implementation end.", t => m(t)?.Kind, ProcedureKind.Destructor);
            RunAstTest("unit z.x; interface type x = class procedure m(q: string); end; implementation end.", t => m(t)?.Kind, ProcedureKind.Procedure);
            RunAstTest("unit z.x; interface type x = class function m(q: string): integer; end; implementation end.", t => m(t)?.Kind, ProcedureKind.Function);

            RunAstTest("unit z.x; interface type x = class procedure m(q: integer); reintroduce; end; implementation end.", t => m(t)?.Directives[0].Kind, MethodDirectiveKind.Reintroduce);
            RunAstTest("unit z.x; interface type x = class procedure m(q: integer); overload; end; implementation end.", t => m(t)?.Directives[0].Kind, MethodDirectiveKind.Overload);
            RunAstTest("unit z.x; interface type x = class procedure m(q: integer); inline; end; implementation end.", t => m(t)?.Directives[0].Kind, MethodDirectiveKind.Inline);
            RunAstTest("unit z.x; interface type x = class procedure m(q: integer); assembler; end; implementation end.", t => m(t)?.Directives[0].Kind, MethodDirectiveKind.Assembler);
            RunAstTest("unit z.x; interface type x = class procedure m(q: integer); message 5; end; implementation end.", t => m(t)?.Directives[0].Kind, MethodDirectiveKind.Message);
            RunAstTest("unit z.x; interface type x = class procedure m(q: integer); static; end; implementation end.", t => m(t)?.Directives[0].Kind, MethodDirectiveKind.Static);
            RunAstTest("unit z.x; interface type x = class procedure m(q: integer); dynamic; end; implementation end.", t => m(t)?.Directives[0].Kind, MethodDirectiveKind.Dynamic);
            RunAstTest("unit z.x; interface type x = class procedure m(q: integer); override; end; implementation end.", t => m(t)?.Directives[0].Kind, MethodDirectiveKind.Override);
            RunAstTest("unit z.x; interface type x = class procedure m(q: integer); virtual; end; implementation end.", t => m(t)?.Directives[0].Kind, MethodDirectiveKind.Virtual);
            RunAstTest("unit z.x; interface type x = class procedure m(q: integer); abstract; end; implementation end.", t => m(t)?.Directives[0].Kind, MethodDirectiveKind.Abstract);
            RunAstTest("unit z.x; interface type x = class procedure m(q: integer); final; end; implementation end.", t => m(t)?.Directives[0].Kind, MethodDirectiveKind.Final);
            RunAstTest("unit z.x; interface type x = class procedure m(q: integer); cdecl; end; implementation end.", t => m(t)?.Directives[0].Kind, MethodDirectiveKind.Cdecl);
            RunAstTest("unit z.x; interface type x = class procedure m(q: integer); pascal; end; implementation end.", t => m(t)?.Directives[0].Kind, MethodDirectiveKind.Pascal);
            RunAstTest("unit z.x; interface type x = class procedure m(q: integer); register; end; implementation end.", t => m(t)?.Directives[0].Kind, MethodDirectiveKind.Register);
            RunAstTest("unit z.x; interface type x = class procedure m(q: integer); safecall; end; implementation end.", t => m(t)?.Directives[0].Kind, MethodDirectiveKind.Safecall);
            RunAstTest("unit z.x; interface type x = class procedure m(q: integer); stdcall; end; implementation end.", t => m(t)?.Directives[0].Kind, MethodDirectiveKind.StdCall);
            RunAstTest("unit z.x; interface type x = class procedure m(q: integer); export; end; implementation end.", t => m(t)?.Directives[0].Kind, MethodDirectiveKind.Export);
            RunAstTest("unit z.x; interface type x = class procedure m(q: integer); dispid 27; end; implementation end.", t => m(t)?.Directives[0].Kind, MethodDirectiveKind.DispId);
            RunAstTest("unit z.x; interface type x = class procedure m(q: integer); deprecated; end; implementation end.", t => m(t)?.Hints?.SymbolIsDeprecated, true);
            RunAstTest("unit z.x; interface type x = class procedure m(q: integer); library; end; implementation end.", t => m(t)?.Hints?.SymbolInLibrary, true);
            RunAstTest("unit z.x; interface type x = class procedure m(q: integer); experimental; end; implementation end.", t => m(t)?.Hints?.SymbolIsExperimental, true);
            RunAstTest("unit z.x; interface type x = class procedure m(q: integer); platform; end; implementation end.", t => m(t)?.Hints?.SymbolIsPlatformSpecific, true);

            // method overloading
            RunAstTest("unit z.x; interface type x = class procedure m(q: integer); overload; procedure m(q: string); overload; end; implementation end.", t => m(t)?.Overloads[0]?.Name.CompleteName, "m");

            // method resolutions

            RunAstTest("unit z.x; interface type x = class procedure m.x = t; end; implementation end.", t => r(t)?.Kind, StructureMethodResolutionKind.Procedure);
            RunAstTest("unit z.x; interface type x = class procedure m.x = t; end; implementation end.", t => r(t)?.Target?.CompleteName, "t");
            RunAstTest("unit z.x; interface type x = class procedure m.x = t; end; implementation end.", t => r(t)?.TypeValue?.GetType(), typeof(MetaType));
            RunAstTest("unit z.x; interface type x = class function m.x = t; end; implementation end.", t => r(t)?.Kind, StructureMethodResolutionKind.Function);
            RunAstTest("unit z.x; interface type x = class function m.x = t; end; implementation end.", t => r(t)?.Target?.CompleteName, "t");
            RunAstTest("unit z.x; interface type x = class function m.x = t; end; implementation end.", t => r(t)?.TypeValue?.GetType(), typeof(MetaType));

            // symbols

            RunAstTest("unit z.x; interface type x = class const c = 5; end; implementation end.", t => (c(t) as ConstantDeclaration)?.Mode, DeclarationMode.Const);
            RunAstTest("unit z.x; interface type x = class type c = string; end; implementation end.", t => (c(t) as TypeDeclaration)?.TypeValue?.GetType(), typeof(MetaType));


            // attributes

            RunAstTest("unit z.x; interface type x = class [a] procedure m(q: integer); platform; end; implementation end.", t => m(t)?.Attributes?[0]?.SymbolName, "a");

        }

        [Fact]
        public void TestUnitFinalization() {
            BlockOfStatements u(object t) => (t as CompilationUnit)?.FinalizationBlock as BlockOfStatements;

            RunAstTest("unit z.x; interface implementation initialization finalization begin end end.", t => u(t)?.Statements[0]?.GetType(), typeof(BlockOfStatements));
        }

        [Fact]
        public void TestCompoundStatement() {
            BlockOfStatements u(object t) => (t as CompilationUnit)?.FinalizationBlock as BlockOfStatements;

            RunAstTest("unit z.x; interface implementation initialization finalization begin begin end; end; end.", t => (u(t)?.Statements[0] as BlockOfStatements)?.Statements[0]?.GetType(), typeof(BlockOfStatements));
            RunAstTest("unit z.x; interface implementation initialization finalization begin asm end; end; end.", t => (u(t)?.Statements[0] as BlockOfStatements)?.Statements[0]?.GetType(), typeof(BlockOfAssemblerStatements));

            RunAstTest("unit z.x; interface implementation initialization finalization begin a : begin end; end; end.", t => (u(t)?.Statements[0] as BlockOfStatements)?.LabelName?.CompleteName, "a");
            RunAstTest("unit z.x; interface implementation initialization finalization begin 10 : begin end; end; end.", t => (u(t)?.Statements[0] as BlockOfStatements)?.LabelName?.CompleteName, "10");
            RunAstTest("unit z.x; interface implementation initialization finalization begin $FF : begin end; end; end.", t => (u(t)?.Statements[0] as BlockOfStatements)?.LabelName?.CompleteName, "255");
        }

        [Fact]
        public void TestInterfaceType() {
            StructuredType r(object t) => ((t as CompilationUnit)?.InterfaceSymbols["z"] as TypeDeclaration)?.TypeValue as StructuredType;
            StructureMethod m(object t) => r(t)?.Methods?["m"];
            StructureProperty p(object t) => r(t)?.Properties?["n"];
            RunAstTest("unit z.x; interface type z = interface end; implementation end.", t => r(t)?.Kind, StructuredTypeKind.Interface);
            RunAstTest("unit z.x; interface type z = interface(q) end; implementation end.", t => (r(t)?.BaseTypes[0] as MetaType)?.AsScopedName?.ToString(), "q");
            RunAstTest("unit z.x; interface type z = interface(q) [s] end; implementation end.", t => r(t)?.GuidName?.CompleteName, "s");
            RunAstTest("unit z.x; interface type z = interface(q) ['s'] end; implementation end.", t => r(t)?.GuidId, 's');

            RunAstTest("unit z.x; interface type z = interface procedure m(q: integer); end; implementation end.", t => m(t)?.Kind, ProcedureKind.Procedure);
            RunAstTest("unit z.x; interface type z = interface function m(q: integer): string; end; implementation end.", t => m(t)?.Kind, ProcedureKind.Function);

            RunAstTest("unit z.x; interface type z = interface property n: integer; end; implementation end.", t => p(t)?.Visibility, MemberVisibility.Public);
            RunAstTest("unit z.x; interface type z = interface property n: integer; end; implementation end.", t => p(t)?.Name.CompleteName, "n");
            RunAstTest("unit z.x; interface type z = interface property n: integer read q; end; implementation end.", t => p(t)?.Accessors[0]?.Kind, StructurePropertyAccessorKind.Read);
            RunAstTest("unit z.x; interface type z = interface property n: integer read q; end; implementation end.", t => p(t)?.Accessors[0]?.Name.CompleteName, "q");
            RunAstTest("unit z.x; interface type z = interface property n: integer write q; end; implementation end.", t => p(t)?.Accessors[0]?.Kind, StructurePropertyAccessorKind.Write);
            RunAstTest("unit z.x; interface type z = interface property n: integer write q; end; implementation end.", t => p(t)?.Accessors[0]?.Name.CompleteName, "q");
            RunAstTest("unit z.x; interface type z = interface property n: integer add q; end; implementation end.", t => p(t)?.Accessors[0]?.Kind, StructurePropertyAccessorKind.Add);
            RunAstTest("unit z.x; interface type z = interface property n: integer add q; end; implementation end.", t => p(t)?.Accessors[0]?.Name.CompleteName, "q");
            RunAstTest("unit z.x; interface type z = interface property n: integer remove q; end; implementation end.", t => p(t)?.Accessors[0]?.Kind, StructurePropertyAccessorKind.Remove);
            RunAstTest("unit z.x; interface type z = interface property n: integer remove q; end; implementation end.", t => p(t)?.Accessors[0]?.Name.CompleteName, "q");
            RunAstTest("unit z.x; interface type z = interface property n: integer readonly; end; implementation end.", t => p(t)?.Accessors[0]?.Kind, StructurePropertyAccessorKind.ReadOnly);
            RunAstTest("unit z.x; interface type z = interface property n: integer writeonly; end; implementation end.", t => p(t)?.Accessors[0]?.Kind, StructurePropertyAccessorKind.WriteOnly);
            RunAstTest("unit z.x; interface type z = interface property n: integer dispid 27; end; implementation end.", t => p(t)?.Accessors[0]?.Kind, StructurePropertyAccessorKind.DispId);
            RunAstTest("unit z.x; interface type z = interface property n: integer stored 27; end; implementation end.", t => p(t)?.Accessors[0]?.Kind, StructurePropertyAccessorKind.Stored);
            RunAstTest("unit z.x; interface type z = interface property n: integer default 27; end; implementation end.", t => p(t)?.Accessors[0]?.Kind, StructurePropertyAccessorKind.Default);
            RunAstTest("unit z.x; interface type z = interface property n: integer nodefault; end; implementation end.", t => p(t)?.Accessors[0]?.Kind, StructurePropertyAccessorKind.NoDefault);
            RunAstTest("unit z.x; interface type z = interface property n: integer implements x; end; implementation end.", t => p(t)?.Accessors[0]?.Kind, StructurePropertyAccessorKind.Implements);
            RunAstTest("unit z.x; interface type z = interface property n: integer implements x; end; implementation end.", t => p(t)?.Accessors[0]?.Name?.CompleteName, "x");
            RunAstTest("unit z.x; interface type z = interface property n[q: integer]: integer; end; implementation end.", t => p(t)?.Parameters.Items[0]?.Parameters[0]?.Name?.CompleteName, "q");


        }

        [Fact]
        public void TestObjectType() {
            StructuredType r(object t) => ((t as CompilationUnit)?.InterfaceSymbols["z"] as TypeDeclaration)?.TypeValue as StructuredType;
            RunAstTest("unit z.x; interface type z = object x: integer; end; implementation end.", t => r(t)?.Kind, StructuredTypeKind.Object);

            // fields
            RunAstTest("unit z.x; interface type z = object x: integer; end; implementation end.", t => r(t)?.Fields["x"]?.Name?.CompleteName, "x");
            RunAstTest("unit z.x; interface type z = object x: integer; end; implementation end.", t => r(t)?.Fields.Items[0]?.Fields[0]?.Name?.CompleteName, "x");
            RunAstTest("unit z.x; interface type z = object x: integer; end; implementation end.", t => r(t)?.Fields["x"]?.Visibility, MemberVisibility.Public);
            RunAstTest("unit z.x; interface type z = object x: integer experimental; end; implementation end.", t => r(t)?.Fields["x"]?.Hints?.SymbolIsExperimental, true);
            RunAstTest("unit z.x; interface type z = object x: string; end; implementation end.", t => r(t)?.Fields.Items[0]?.TypeValue?.GetType(), typeof(MetaType));

            RunAstTest("unit z.x; interface type z = object private x: integer; end; implementation end.", t => r(t)?.Fields["x"]?.Visibility, MemberVisibility.Private);
            RunAstTest("unit z.x; interface type z = object protected x: integer; end; implementation end.", t => r(t)?.Fields["x"]?.Visibility, MemberVisibility.Protected);
            RunAstTest("unit z.x; interface type z = object public x: integer; end; implementation end.", t => r(t)?.Fields["x"]?.Visibility, MemberVisibility.Public);
            RunAstTest("unit z.x; interface type z = object strict private x: integer; end; implementation end.", t => r(t)?.Fields["x"]?.Visibility, MemberVisibility.StrictPrivate);
            RunAstTest("unit z.x; interface type z = object strict protected x: integer; end; implementation end.", t => r(t)?.Fields["x"]?.Visibility, MemberVisibility.StrictProtected);
            RunAstTest("unit z.x; interface type z = object published x: integer; end; implementation end.", t => r(t)?.Fields["x"]?.Visibility, MemberVisibility.Published);
            RunAstTest("unit z.x; interface type z = object automated x: integer; end; implementation end.", t => r(t)?.Fields["x"]?.Visibility, MemberVisibility.Automated);

            // methods
            RunAstTest("unit z.x; interface type z = object function x: string; end; implementation end.", t => r(t)?.Methods["x"]?.Name?.CompleteName, "x");
            RunAstTest("unit z.x; interface type z = object private function x: string; end; implementation end.", t => r(t)?.Methods["x"]?.Visibility, MemberVisibility.Private);
            RunAstTest("unit z.x; interface type z = object private function x: string; experimental; end; implementation end.", t => r(t)?.Methods["x"]?.Visibility, MemberVisibility.Private);

            // properties
            RunAstTest("unit z.x; interface type z = object property x: string read q; end; implementation end.", t => r(t)?.Properties["x"]?.SymbolName, "x");
            RunAstTest("unit z.x; interface type z = object property x: string read q; end; implementation end.", t => r(t)?.Properties["x"]?.Accessors[0]?.Name?.CompleteName, "q");
            RunAstTest("unit z.x; interface type z = object property x: string read q; end; implementation end.", t => r(t)?.Properties["x"]?.Accessors[0]?.Kind, StructurePropertyAccessorKind.Read);

            // symbols
            RunAstTest("unit z.x; interface type z = object const c = nil; end; implementation end.", t => r(t)?.Symbols["c"]?.Name?.CompleteName, "c");
            RunAstTest("unit z.x; interface type z = object type t = string; end; implementation end.", t => (r(t)?.Symbols["t"] as TypeDeclaration)?.TypeValue?.GetType(), typeof(MetaType));

        }

        [Fact]
        public void TestBlockDeclaredSymbols() {
            CompilationUnit u(object t) => t as CompilationUnit;
            StructuredType r(object t) => ((t as CompilationUnit)?.InterfaceSymbols["Tx"] as TypeDeclaration)?.TypeValue as StructuredType;
            string i(object t) => r(t)?.Methods["m"]?.Implementation?.Symbols["x"]?.Name?.CompleteName;
            MethodImplementation p(object t) => ((t as CompilationUnit)?.ImplementationSymbols["m"] as MethodImplementation);
            MethodImplementation v(object t) => ((((t as CompilationUnit)?.ImplementationSymbols["m"] as MethodImplementation)?.Block as BlockOfStatements)?.Statements?[0] as StructuredStatement)?.Value as MethodImplementation;

            RunAstTest("library z.x; const x = nil; begin end.", t => u(t)?.Symbols?["x"]?.Name?.CompleteName, "x");
            RunAstTest("program z.x; const x = nil; begin end.", t => u(t)?.Symbols?["x"]?.Name?.CompleteName, "x");
            RunAstTest("unit z.x; interface type Tx = class procedure m(); end; implementation procedure Tx.m; const x = nil; begin end; end.", t => i(t), "x");
            RunAstTest("unit z.x; interface implementation procedure m(); const x = nil; begin end; end.", t => p(t)?.Symbols?["x"]?.Name?.CompleteName, "x");
            RunAstTest("unit z.x; interface implementation procedure m(); var q: TAction; begin q := procedure const x = nil; begin end; end; end.", t => v(t)?.Symbols?["x"]?.Name?.CompleteName, "x");
        }

        [Fact]
        public void TestAsmBlock() {
            CompilationUnit u(object t) => t as CompilationUnit;
            StructuredType r(object t) => ((t as CompilationUnit)?.InterfaceSymbols["Tx"] as TypeDeclaration)?.TypeValue as StructuredType;
            MethodImplementation i(object t) => r(t)?.Methods["m"]?.Implementation;
            MethodImplementation p(object t) => ((t as CompilationUnit)?.ImplementationSymbols["m"] as MethodImplementation);
            MethodImplementation v(object t) => ((((t as CompilationUnit)?.ImplementationSymbols["m"] as MethodImplementation)?.Block as BlockOfStatements)?.Statements?[0] as StructuredStatement)?.Value as MethodImplementation;

            RunAstTest("library z.x; asm end.", t => u(t)?.Block.GetType(), typeof(BlockOfAssemblerStatements));
            RunAstTest("program z.x; asm end.", t => u(t)?.Block.GetType(), typeof(BlockOfAssemblerStatements));
            RunAstTest("unit z.x; interface type Tx = class procedure m(); end; implementation procedure Tx.m; asm end; end.", t => i(t)?.Block.GetType(), typeof(BlockOfAssemblerStatements));
            RunAstTest("unit z.x; interface implementation procedure m(); asm end; end.", t => p(t)?.Block.GetType(), typeof(BlockOfAssemblerStatements));
            RunAstTest("unit z.x; interface implementation procedure m(); var q: TAction; begin q := procedure asm end; end; end.", t => v(t)?.Block.GetType(), typeof(BlockOfAssemblerStatements));
        }

        [Fact]
        public void TestAnonymousFunction() {
            MethodImplementation p(object t) => ((((t as CompilationUnit)?.ImplementationSymbols["m"] as MethodImplementation)?.Block as BlockOfStatements)?.Statements?[0] as StructuredStatement)?.Value as MethodImplementation;
            RunAstTest("unit z.x; interface implementation procedure m(); var q: TAction; begin q := procedure begin end; end; end.", t => p(t)?.Name?.CompleteName, "$1");
        }

        [Fact]
        public void TestProcedure() {
            var five = GetIntegerValue(5);
            MethodImplementation r(object t) => ((t as CompilationUnit)?.ImplementationSymbols["m"] as MethodImplementation);
            RunAstTest("unit z.x; interface implementation procedure m(); begin end; end.", t => r(t)?.Name.CompleteName, "m");
            RunAstTest("unit z.x; interface implementation procedure m(var x: Integer = 5); begin end; end.", t => (r(t)?.Parameters?.Items[0]?.Value as ConstantValue)?.LiteralValue, five);
        }

        [Fact]
        public void TestMethodImplementation() {
            StructuredType r(object t) => ((t as CompilationUnit)?.InterfaceSymbols["Tx"] as TypeDeclaration)?.TypeValue as StructuredType;
            StructuredType n(object t) => (r(t)?.Symbols["Tz"] as TypeDeclaration)?.TypeValue as StructuredType;
            MethodImplementation i(object t) => r(t)?.Methods["m"]?.Implementation;
            MethodImplementation j(object t) => n(t)?.Methods["m"]?.Implementation;

            StructuredType r1(object t) => ((t as CompilationUnit)?.ImplementationSymbols["Tx"] as TypeDeclaration)?.TypeValue as StructuredType;
            StructuredType n1(object t) => (r1(t)?.Symbols["Tz"] as TypeDeclaration)?.TypeValue as StructuredType;
            MethodImplementation i1(object t) => r1(t)?.Methods["m"]?.Implementation;
            MethodImplementation j1(object t) => n1(t)?.Methods["m"]?.Implementation;

            RunAstTest("unit z.x; interface type Tx = class procedure m(); end; implementation procedure Tx.m; begin end; end.", t => i(t)?.Name?.Name, "m");
            RunAstTest("unit z.x; interface type Tx = class procedure m(); end; implementation procedure Tx.m; begin end; end.", t => i(t)?.Name?.NamespaceName, "Tx");

            RunAstTest("unit z.x; interface type Tx = class procedure m(); end; implementation procedure Tx.m; begin end; end.", t => i(t)?.Kind, ProcedureKind.Procedure);
            RunAstTest("unit z.x; interface type Tx = class type Tz = class procedure m(); end; end; implementation procedure Tx.Tz.m; begin end; end.", t => j(t)?.Kind, ProcedureKind.Procedure);
            RunAstTest("unit z.x; interface implementation type Tx = class procedure m(); end; procedure Tx.m; begin end; end.", t => i1(t)?.Kind, ProcedureKind.Procedure);
            RunAstTest("unit z.x; interface implementation type Tx = class type Tz = class procedure m(); end; end; procedure Tx.Tz.m; begin end; end.", t => j1(t)?.Kind, ProcedureKind.Procedure);

            RunAstTest("unit z.x; interface type Tx = class procedure m(); end; implementation procedure Tx.m; reintroduce; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Reintroduce);
            RunAstTest("unit z.x; interface type Tx = class procedure m(); end; implementation procedure Tx.m; overload; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Overload);
            RunAstTest("unit z.x; interface type Tx = class procedure m(); end; implementation procedure Tx.m; inline; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Inline);
            RunAstTest("unit z.x; interface type Tx = class procedure m(); end; implementation procedure Tx.m; assembler; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Assembler);
            RunAstTest("unit z.x; interface type Tx = class procedure m(); end; implementation procedure Tx.m; message 0; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Message);
            RunAstTest("unit z.x; interface type Tx = class procedure m(); end; implementation procedure Tx.m; static; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Static);
            RunAstTest("unit z.x; interface type Tx = class procedure m(); end; implementation procedure Tx.m; dynamic; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Dynamic);
            RunAstTest("unit z.x; interface type Tx = class procedure m(); end; implementation procedure Tx.m; override; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Override);
            RunAstTest("unit z.x; interface type Tx = class procedure m(); end; implementation procedure Tx.m; virtual; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Virtual);
            RunAstTest("unit z.x; interface type Tx = class procedure m(); end; implementation procedure Tx.m; abstract; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Abstract);
            RunAstTest("unit z.x; interface type Tx = class procedure m(); end; implementation procedure Tx.m; final; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Final);
            RunAstTest("unit z.x; interface type Tx = class procedure m(); end; implementation procedure Tx.m; cdecl; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Cdecl);
            RunAstTest("unit z.x; interface type Tx = class procedure m(); end; implementation procedure Tx.m; pascal; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Pascal);
            RunAstTest("unit z.x; interface type Tx = class procedure m(); end; implementation procedure Tx.m; register; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Register);
            RunAstTest("unit z.x; interface type Tx = class procedure m(); end; implementation procedure Tx.m; stdcall; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.StdCall);
            RunAstTest("unit z.x; interface type Tx = class procedure m(); end; implementation procedure Tx.m; safecall; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Safecall);
            RunAstTest("unit z.x; interface type Tx = class procedure m(); end; implementation procedure Tx.m; register; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Register);
            RunAstTest("unit z.x; interface type Tx = class procedure m(); end; implementation procedure Tx.m; experimental; begin end; end.", t => i(t)?.Hints?.SymbolIsExperimental, true);

            RunAstTest("unit z.x; interface type Tx = class procedure m<T>(); end; implementation procedure Tx.m<T>; begin end; end.", t => (i(t)?.Name as GenericSymbolName)?.NamePart?.Parameters[0], "T");
            RunAstTest("unit z.x; interface type Tx<Q> = class procedure m<T>(); end; implementation procedure Tx<Q>.m<T>; begin end; end.", t => (i(t)?.Name as GenericSymbolName)?.NamespaceParts.FirstOrDefault()?.Parameters[0], "Q");

            //RunAstTest("unit z.x; interface type Tx = class procedure m(const a: string); end; implementation procedure Tx.m; begin end; end.", t => i(t)?., "Tx");
        }

        [Fact]
        public void TestProcedureDeclaration() {
            MethodImplementation r(object t) => ((t as CompilationUnit)?.ImplementationSymbols["p"] as MethodImplementation);
            MethodImplementation i(object t) => r(t);

            RunAstTest("unit z.x; interface implementation procedure p; begin end; end.", t => r(t)?.Kind, ProcedureKind.Procedure);
            RunAstTest("unit z.x; interface implementation procedure p(x: string); begin end; end.", t => r(t)?.Parameters[0]?.SymbolName, "x");
            RunAstTest("unit z.x; interface implementation function p: string; begin end; end.", t => r(t)?.Kind, ProcedureKind.Function);
            RunAstTest("unit z.x; interface implementation function p: string; begin end; end.", t => r(t)?.TypeValue?.GetType(), typeof(MetaType));

            RunAstTest("unit z.x; interface implementation procedure p; overload; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Overload);
            RunAstTest("unit z.x; interface implementation procedure p; inline; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Inline);
            RunAstTest("unit z.x; interface implementation procedure p; assembler; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Assembler);
            RunAstTest("unit z.x; interface implementation procedure p; cdecl; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Cdecl);
            RunAstTest("unit z.x; interface implementation procedure p; pascal; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Pascal);
            RunAstTest("unit z.x; interface implementation procedure p; register; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Register);
            RunAstTest("unit z.x; interface implementation procedure p; stdcall; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.StdCall);
            RunAstTest("unit z.x; interface implementation procedure p; safecall; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Safecall);
            RunAstTest("unit z.x; interface implementation procedure p; register; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Register);
            RunAstTest("unit z.x; interface implementation procedure p; near; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Near);
            RunAstTest("unit z.x; interface implementation procedure p; local; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Local);
            RunAstTest("unit z.x; interface implementation procedure p; far; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Far);
            RunAstTest("unit z.x; interface implementation procedure p; unsafe; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Unsafe);
            RunAstTest("unit z.x; interface implementation procedure p; forward; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.Forward);
            RunAstTest("unit z.x; interface implementation procedure p; varargs; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.VarArgs);
            RunAstTest("unit z.x; interface implementation procedure p; external; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.External);
            RunAstTest("unit z.x; interface implementation procedure p; experimental; begin end; end.", t => i(t)?.Hints?.SymbolIsExperimental, true);

            RunAstTest("unit z.x; interface implementation procedure p; external 'e'; begin end; end.", t => i(t)?.Directives[0]?.Kind, MethodDirectiveKind.External);
            RunAstTest("unit z.x; interface implementation procedure p; external 'e' name 'e'; begin end; end.", t => i(t)?.Directives[0]?.Specifiers[0]?.Kind, MethodDirectiveSpecifierKind.Name);
            RunAstTest("unit z.x; interface implementation procedure p; external 'e' index 'e'; begin end; end.", t => i(t)?.Directives[0]?.Specifiers[0]?.Kind, MethodDirectiveSpecifierKind.Index);
            RunAstTest("unit z.x; interface implementation procedure p; external 'e' delayed; begin end; end.", t => i(t)?.Directives[0]?.Specifiers[0]?.Kind, MethodDirectiveSpecifierKind.Delayed);
            RunAstTest("unit z.x; interface implementation procedure p; external 'e' dependency 'a', 'b'; begin end; end.", t => i(t)?.Directives[0]?.Specifiers[0]?.Kind, MethodDirectiveSpecifierKind.Dependency);
        }

        [Fact]
        public void TestAsmStatement() {
            var one = GetIntegerValue(1);
            AssemblerStatement r(object t) => (((t as CompilationUnit)?.ImplementationSymbols["p"] as MethodImplementation)?.Block as BlockOfAssemblerStatements)?.Statements[0];

            RunAstTest("unit z.x; interface implementation procedure p; asm .noframe end; end.", t => r(t)?.Kind, AssemblerStatementKind.NoFrameOperation);
            RunAstTest("unit z.x; interface implementation procedure p; asm .pushenv a end; end.", t => r(t)?.Kind, AssemblerStatementKind.PushEnvOperation);
            RunAstTest("unit z.x; interface implementation procedure p; asm .pushenv a end; end.", t => (r(t)?.Operands[0] as SymbolReference)?.Name?.CompleteName, "a");
            RunAstTest("unit z.x; interface implementation procedure p; asm .savenv a end; end.", t => r(t)?.Kind, AssemblerStatementKind.SaveEnvOperation);
            RunAstTest("unit z.x; interface implementation procedure p; asm .savenv a end; end.", t => (r(t)?.Operands[0] as SymbolReference)?.Name?.CompleteName, "a");
            RunAstTest("unit z.x; interface implementation procedure p; asm .params 1 end; end.", t => r(t)?.Kind, AssemblerStatementKind.ParamsOperation);
            RunAstTest("unit z.x; interface implementation procedure p; asm .params 1 end; end.", t => (r(t)?.Operands[0] as ConstantValue)?.LiteralValue, one);

            RunAstTest("unit z.x; interface implementation procedure p; asm mov ax, 1 end; end.", t => r(t)?.OpCode?.CompleteName, "mov");
            RunAstTest("unit z.x; interface implementation procedure p; asm @1: mov ax, 1 end; end.", t => r(t)?.LabelName?.CompleteName, "@1");
            RunAstTest("unit z.x; interface implementation procedure p; asm @@TestOne: mov ax, 1 end; end.", t => r(t)?.LabelName?.CompleteName, "@@TestOne");
            RunAstTest("unit z.x; interface implementation procedure p; asm @_@_@_: mov ax, 1 end; end.", t => r(t)?.LabelName?.CompleteName, "@_@_@_");
            RunAstTest("unit z.x; interface implementation procedure p; asm 1: mov ax, 1 end; end.", t => r(t)?.LabelName?.CompleteName, "1");
            RunAstTest("unit z.x; interface implementation procedure p; asm x: mov ax, 1 end; end.", t => r(t)?.LabelName?.CompleteName, "x");

            RunAstTest("unit z.x; interface implementation procedure p; asm lock mov ax, 1 end; end.", t => r(t)?.LockPrefix?.CompleteName, "lock");
            RunAstTest("unit z.x; interface implementation procedure p; asm repne mov ax, 1 end; end.", t => r(t)?.LockPrefix?.CompleteName, "repne");
            RunAstTest("unit z.x; interface implementation procedure p; asm repnz mov ax, 1 end; end.", t => r(t)?.LockPrefix?.CompleteName, "repnz");
            RunAstTest("unit z.x; interface implementation procedure p; asm cs mov ax, 1 end; end.", t => r(t)?.SegmentPrefix?.CompleteName, "cs");
            RunAstTest("unit z.x; interface implementation procedure p; asm ds mov ax, 1 end; end.", t => r(t)?.SegmentPrefix?.CompleteName, "ds");
            RunAstTest("unit z.x; interface implementation procedure p; asm es mov ax, 1 end; end.", t => r(t)?.SegmentPrefix?.CompleteName, "es");
            RunAstTest("unit z.x; interface implementation procedure p; asm cs lock mov ax, 1 end; end.", t => r(t)?.LockPrefix?.CompleteName, "lock");
            RunAstTest("unit z.x; interface implementation procedure p; asm ds repne mov ax, 1 end; end.", t => r(t)?.LockPrefix?.CompleteName, "repne");
            RunAstTest("unit z.x; interface implementation procedure p; asm es repnz mov ax, 1 end; end.", t => r(t)?.LockPrefix?.CompleteName, "repnz");
            RunAstTest("unit z.x; interface implementation procedure p; asm lock cs mov ax, 1 end; end.", t => r(t)?.LockPrefix?.CompleteName, "lock");
            RunAstTest("unit z.x; interface implementation procedure p; asm repne ds mov ax, 1 end; end.", t => r(t)?.LockPrefix?.CompleteName, "repne");
            RunAstTest("unit z.x; interface implementation procedure p; asm repnz es mov ax, 1 end; end.", t => r(t)?.LockPrefix?.CompleteName, "repnz");

            RunAstTest("unit z.x; interface implementation procedure p; asm mov 1 and 1, 1 end; end.", t => (r(t)?.Operands[0] as BinaryOperator)?.Kind, ExpressionKind.And);
            RunAstTest("unit z.x; interface implementation procedure p; asm mov 1 or 1, 1 end; end.", t => (r(t)?.Operands[0] as BinaryOperator)?.Kind, ExpressionKind.Or);
            RunAstTest("unit z.x; interface implementation procedure p; asm mov 1 xor 1, 1 end; end.", t => (r(t)?.Operands[0] as BinaryOperator)?.Kind, ExpressionKind.Xor);
            RunAstTest("unit z.x; interface implementation procedure p; asm mov  not 1, 1 end; end.", t => (r(t)?.Operands[0] as UnaryOperator)?.Kind, ExpressionKind.Not);
            RunAstTest("unit z.x; interface implementation procedure p; asm mov  type 1, 1 end; end.", t => (r(t)?.Operands[0] as UnaryOperator)?.Kind, ExpressionKind.AsmType);
            RunAstTest("unit z.x; interface implementation procedure p; asm mov  [1], 1 end; end.", t => (r(t)?.Operands[0] as UnaryOperator)?.Kind, ExpressionKind.AsmMemorySubexpression);

            RunAstTest("unit z.x; interface implementation procedure p; asm mov  1 + 2, 1 end; end.", t => (r(t)?.Operands[0] as BinaryOperator)?.Kind, ExpressionKind.Plus);
            RunAstTest("unit z.x; interface implementation procedure p; asm mov  1 - 2, 1 end; end.", t => (r(t)?.Operands[0] as BinaryOperator)?.Kind, ExpressionKind.Minus);
            RunAstTest("unit z.x; interface implementation procedure p; asm mov  1 * 2, 1 end; end.", t => (r(t)?.Operands[0] as BinaryOperator)?.Kind, ExpressionKind.Times);
            RunAstTest("unit z.x; interface implementation procedure p; asm mov  1 / 2, 1 end; end.", t => (r(t)?.Operands[0] as BinaryOperator)?.Kind, ExpressionKind.Slash);
            RunAstTest("unit z.x; interface implementation procedure p; asm mov  1 mod 2, 1 end; end.", t => (r(t)?.Operands[0] as BinaryOperator)?.Kind, ExpressionKind.Mod);
            RunAstTest("unit z.x; interface implementation procedure p; asm mov  1 shr 2, 1 end; end.", t => (r(t)?.Operands[0] as BinaryOperator)?.Kind, ExpressionKind.Shr);
            RunAstTest("unit z.x; interface implementation procedure p; asm mov  1 shl 2, 1 end; end.", t => (r(t)?.Operands[0] as BinaryOperator)?.Kind, ExpressionKind.Shl);
            RunAstTest("unit z.x; interface implementation procedure p; asm mov  1.OFFSET 5, 1 end; end.", t => (r(t)?.Operands[0] as BinaryOperator)?.Kind, ExpressionKind.Dot);
            RunAstTest("unit z.x; interface implementation procedure p; asm mov  CS:5, 1 end; end.", t => (r(t)?.Operands[0] as BinaryOperator)?.Kind, ExpressionKind.AsmSegmentPrefix);

            RunAstTest("unit z.x; interface implementation procedure p; asm mov  1, 1 end; end.", t => (r(t)?.Operands[0] as ConstantValue)?.Kind, ConstantValueKind.Integer);
            RunAstTest("unit z.x; interface implementation procedure p; asm mov  1.1, 1 end; end.", t => (r(t)?.Operands[0] as ConstantValue)?.Kind, ConstantValueKind.RealNumber);
            RunAstTest("unit z.x; interface implementation procedure p; asm mov  $FF, 1 end; end.", t => (r(t)?.Operands[0] as ConstantValue)?.Kind, ConstantValueKind.HexNumber);
            RunAstTest("unit z.x; interface implementation procedure p; asm mov  'a', 1 end; end.", t => (r(t)?.Operands[0] as ConstantValue)?.Kind, ConstantValueKind.QuotedString);
            RunAstTest("unit z.x; interface implementation procedure p; asm mov  \"a\", 1 end; end.", t => (r(t)?.Operands[0] as ConstantValue)?.Kind, ConstantValueKind.QuotedString);
            RunAstTest("unit z.x; interface implementation procedure p; asm mov  a, 1 end; end.", t => (r(t)?.Operands[0] as SymbolReference)?.Name?.CompleteName, "a");
            RunAstTest("unit z.x; interface implementation procedure p; asm mov  @a, 1 end; end.", t => (r(t)?.Operands[0] as SymbolReference)?.LabelName?.CompleteName, "@a");

            RunAstTest("unit z.x; interface implementation procedure p; asm mov  offset 1, 1 end; end.", t => (r(t)?.Operands[0] as UnaryOperator)?.Kind, ExpressionKind.AsmOffset);
            RunAstTest("unit z.x; interface implementation procedure p; asm mov  byte 1, 1 end; end.", t => (r(t)?.Operands[0] as UnaryOperator)?.Kind, ExpressionKind.AsmBytePointerByte);
            RunAstTest("unit z.x; interface implementation procedure p; asm mov  word 1, 1 end; end.", t => (r(t)?.Operands[0] as UnaryOperator)?.Kind, ExpressionKind.AsmBytePointerWord);
            RunAstTest("unit z.x; interface implementation procedure p; asm mov  dword 1, 1 end; end.", t => (r(t)?.Operands[0] as UnaryOperator)?.Kind, ExpressionKind.AsmBytePointerDWord);
            RunAstTest("unit z.x; interface implementation procedure p; asm mov  qword 1, 1 end; end.", t => (r(t)?.Operands[0] as UnaryOperator)?.Kind, ExpressionKind.AsmBytePointerQWord);
            RunAstTest("unit z.x; interface implementation procedure p; asm mov  tbyte 1, 1 end; end.", t => (r(t)?.Operands[0] as UnaryOperator)?.Kind, ExpressionKind.AsmBytePointerTByte);
        }

        [Fact]
        public void TestLabel() {
            MethodImplementation r(object t) => ((t as CompilationUnit)?.ImplementationSymbols["p"] as MethodImplementation);

            RunAstTest("unit z.x; interface implementation procedure p; label l; begin l: s; end; end.", t => r(t)?.Symbols[0]?.Name?.CompleteName, "l");
            RunAstTest("unit z.x; interface implementation procedure p; label 1; begin 1: s; end; end.", t => r(t)?.Symbols[0]?.Name?.CompleteName, "1");
        }

        [Fact]
        public void TestRecordConstant() {
            RecordConstant r(object t) => (((t as CompilationUnit)?.ImplementationSymbols["p"] as MethodImplementation)?.Symbols["n"] as ConstantDeclaration)?.Value as RecordConstant;

            RunAstTest("unit z.x; interface implementation procedure p; const n = (a: 1); begin l: s; end; end.", t => r(t)?.GetType(), typeof(RecordConstant));
            RunAstTest("unit z.x; interface implementation procedure p; const n = (a: 1); begin l: s; end; end.", t => (r(t)?.Items[0] as RecordConstantItem)?.Name?.CompleteName, "a");
            RunAstTest("unit z.x; interface implementation procedure p; const n = (a: 1); begin l: s; end; end.", t => r(t)?.Items?.Count, 1);
        }

        [Fact]
        public void TestSetConstant() {
            ArrayConstant r(object t) => (((t as CompilationUnit)?.ImplementationSymbols["p"] as MethodImplementation)?.Symbols["n"] as ConstantDeclaration)?.Value as ArrayConstant;

            RunAstTest("unit z.x; interface implementation procedure p; const n = (2, 1); begin l: s; end; end.", t => r(t)?.GetType(), typeof(ArrayConstant));
            RunAstTest("unit z.x; interface implementation procedure p; const n = (nil, nil); begin l: s; end; end.", t => r(t)?.Items[0]?.GetType(), typeof(ConstantValue));
        }

        [Fact]
        public void TestUnaryOperators() {
            UnaryOperator r(object t) => (((t as CompilationUnit)?.ImplementationSymbols["p"] as MethodImplementation)?.Symbols["n"] as ConstantDeclaration)?.Value as UnaryOperator;
            SymbolReferencePart q0(object t) => ((((t as CompilationUnit)?.ImplementationSymbols["p"] as MethodImplementation)?.Symbols["n"] as ConstantDeclaration)?.Value as SymbolReference)?.SymbolParts[0];
            SymbolReferencePart q1(object t) => ((((t as CompilationUnit)?.ImplementationSymbols["p"] as MethodImplementation)?.Symbols["n"] as ConstantDeclaration)?.Value as SymbolReference)?.SymbolParts[1];
            SymbolReferencePart q2(object t) => ((((t as CompilationUnit)?.ImplementationSymbols["p"] as MethodImplementation)?.Symbols["n"] as ConstantDeclaration)?.Value as SymbolReference)?.SymbolParts[2];

            RunAstTest("unit z.x; interface implementation procedure p; const n = @1; begin l: s; end; end.", t => r(t)?.Kind, ExpressionKind.AddressOf);
            RunAstTest("unit z.x; interface implementation procedure p; const n = not 1; begin l: s; end; end.", t => r(t)?.Kind, ExpressionKind.Not);
            RunAstTest("unit z.x; interface implementation procedure p; const n = +1; begin l: s; end; end.", t => r(t)?.Kind, ExpressionKind.UnaryPlus);
            RunAstTest("unit z.x; interface implementation procedure p; const n = -1; begin l: s; end; end.", t => r(t)?.Kind, ExpressionKind.UnaryMinus);
            RunAstTest("unit z.x; interface implementation procedure p; const n = l^; begin l: s; end; end.", t => q0(t)?.Kind, SymbolReferencePartKind.SubItem);
            RunAstTest("unit z.x; interface implementation procedure p; const n = l^; begin l: s; end; end.", t => q0(t)?.Name.CompleteName, "l");
            RunAstTest("unit z.x; interface implementation procedure p; const n = l^; begin l: s; end; end.", t => q1(t)?.Kind, SymbolReferencePartKind.Dereference);
            RunAstTest("unit z.x; interface implementation procedure p; const n = l^.q; begin l: s; end; end.", t => q2(t)?.Kind, SymbolReferencePartKind.SubItem);
            RunAstTest("unit z.x; interface implementation procedure p; const n = l^.q; begin l: s; end; end.", t => q2(t)?.Name?.CompleteName, "q");
            RunAstTest("unit z.x; interface implementation procedure p; const n = l^.q<z>; begin l: s; end; end.", t => q2(t)?.GenericType?.TypeReference, true);
            RunAstTest("unit z.x; interface implementation procedure p; const n = l^.q<z>; begin l: s; end; end.", t => (q2(t)?.GenericType?.TypeValue as TypeAlias)?.AsScopedName?.ToString(), "z");
            RunAstTest("unit z.x; interface implementation procedure p; const n = l^[1]; begin l: s; end; end.", t => q2(t)?.Kind, SymbolReferencePartKind.ArrayIndex);
            RunAstTest("unit z.x; interface implementation procedure p; const n = l^[1]; begin l: s; end; end.", t => q2(t)?.Expressions[0]?.GetType(), typeof(ConstantValue));
            RunAstTest("unit z.x; interface implementation procedure p; const n = l^(1); begin l: s; end; end.", t => q2(t)?.Kind, SymbolReferencePartKind.CallParameters);
            RunAstTest("unit z.x; interface implementation procedure p; const n = l^(1); begin l: s; end; end.", t => q2(t)?.Expressions[0]?.GetType(), typeof(ConstantValue));
            RunAstTest("unit z.x; interface implementation procedure p; const n = l^(x := 1); begin l: s; end; end.", t => (q2(t)?.Expressions[0] as SymbolReference)?.NamedParameter, true);
            RunAstTest("unit z.x; interface implementation procedure p; const n = l^(x := 1); begin l: s; end; end.", t => (q2(t)?.Expressions[0] as SymbolReference)?.Name?.CompleteName, "x");
            RunAstTest("unit z.x; interface implementation procedure p; const n = l^(x := 1); begin l: s; end; end.", t => (q2(t)?.Expressions[0] as SymbolReference)?.Value?.GetType(), typeof(ConstantValue));
            RunAstTest("unit z.x; interface implementation procedure p; const n = l^(1:1:1); begin l: s; end; end.", t => q2(t)?.Kind, SymbolReferencePartKind.CallParameters);
            RunAstTest("unit z.x; interface implementation procedure p; const n = l^(1:1:1); begin l: s; end; end.", t => q2(t)?.Expressions[0]?.GetType(), typeof(FormattedExpression));
        }

        [Fact]
        public void TestFactors() {
            IExpression r(object t) => (((t as CompilationUnit)?.ImplementationSymbols["p"] as MethodImplementation)?.Symbols["n"] as ConstantDeclaration)?.Value;

            RunAstTest("unit z.x; interface implementation procedure p; const n = ^Baz; begin l: s; end; end.", t => (r(t) as SymbolReference)?.PointerTo, true);
            RunAstTest("unit z.x; interface implementation procedure p; const n = nil; begin l: s; end; end.", t => (r(t) as ConstantValue)?.Kind, ConstantValueKind.Nil);
            RunAstTest("unit z.x; interface implementation procedure p; const n = false; begin l: s; end; end.", t => (r(t) as ConstantValue)?.Kind, ConstantValueKind.False);
            RunAstTest("unit z.x; interface implementation procedure p; const n = true; begin l: s; end; end.", t => (r(t) as ConstantValue)?.Kind, ConstantValueKind.True);
            RunAstTest("unit z.x; interface implementation procedure p; const n = 5; begin l: s; end; end.", t => (r(t) as ConstantValue)?.Kind, ConstantValueKind.Integer);
            RunAstTest("unit z.x; interface implementation procedure p; const n = 5.55; begin l: s; end; end.", t => (r(t) as ConstantValue)?.Kind, ConstantValueKind.RealNumber);
            RunAstTest("unit z.x; interface implementation procedure p; const n = 'a'; begin l: s; end; end.", t => (r(t) as ConstantValue)?.Kind, ConstantValueKind.QuotedString);
            RunAstTest("unit z.x; interface implementation procedure p; const n = $FFFF; begin l: s; end; end.", t => (r(t) as ConstantValue)?.Kind, ConstantValueKind.HexNumber);

            RunAstTest("unit z.x; interface implementation procedure p; const n = r = (a).q; begin l: s; end; end.", t => ((r(t) as BinaryOperator)?.RightOperand as SymbolReference)?.Inherited, false);
        }

        [Fact]
        public void TestDesignator() {
            SymbolReference r(object t) => (((t as CompilationUnit)?.ImplementationSymbols["p"] as MethodImplementation)?.Symbols["n"] as ConstantDeclaration)?.Value as SymbolReference;
            SymbolReferencePart p(object t) => ((((t as CompilationUnit)?.ImplementationSymbols["p"] as MethodImplementation)?.Symbols["n"] as ConstantDeclaration)?.Value as SymbolReference)?.SymbolParts[0] as SymbolReferencePart;

            RunAstTest("unit z.x; interface implementation procedure p; const n = inherited; begin l: s; end; end.", t => r(t)?.Inherited, true);
            RunAstTest("unit z.x; interface implementation procedure p; const n = q; begin l: s; end; end.", t => r(t)?.Inherited, false);
            RunAstTest("unit z.x; interface implementation procedure p; const n = q; begin l: s; end; end.", t => p(t)?.Name?.CompleteName, "q");
            RunAstTest("unit z.x; interface implementation procedure p; const n = (q).a; begin l: s; end; end.", t => (r(t)?.Inherited), false);

            RunAstTest("unit z.x; interface implementation procedure p; const n = q.a(); begin l: s; end; end.", t => (r(t)?.TypeValue as MetaType)?.AsScopedName?.ToString(), "q");
            RunAstTest("unit z.x; interface implementation procedure p; const n = q.q.a(); begin l: s; end; end.", t => (r(t)?.TypeValue as MetaType)?.AsScopedName?.ToString(), "q.q");
            RunAstTest("unit z.x; interface implementation procedure p; const n = q.a[9]; begin l: s; end; end.", t => (r(t)?.TypeValue as MetaType)?.AsScopedName?.ToString(), "q");
            RunAstTest("unit z.x; interface implementation procedure p; const n = q.q.a[9]; begin l: s; end; end.", t => (r(t)?.TypeValue as MetaType)?.AsScopedName?.ToString(), "q.q");
        }

        [Fact]
        public void TestSetExpressions() {
            SetExpression r(object t) => (((t as CompilationUnit)?.ImplementationSymbols["p"] as MethodImplementation)?.Symbols["n"] as ConstantDeclaration)?.Value as SetExpression;

            RunAstTest("unit z.x; interface implementation procedure p; const n = [1,2,3]; begin l: s; end; end.", t => r(t)?.Expressions[0]?.GetType(), typeof(ConstantValue));
            RunAstTest("unit z.x; interface implementation procedure p; const n = [1..2,2,3]; begin l: s; end; end.", t => r(t)?.Expressions[0]?.GetType(), typeof(BinaryOperator));
            RunAstTest("unit z.x; interface implementation procedure p; const n = [1..2,2,3]; begin l: s; end; end.", t => (r(t)?.Expressions[0] as BinaryOperator)?.LeftOperand?.GetType(), typeof(ConstantValue));
        }

        [Fact]
        public void TestExceptionStatements() {
            StructuredStatement r(object t) => (((t as CompilationUnit)?.ImplementationSymbols["p"] as MethodImplementation)?.Block as BlockOfStatements)?.Statements[0] as StructuredStatement;

            RunAstTest("unit z.x; interface implementation procedure p; begin raise x; end; end.", t => r(t)?.Kind, StructuredStatementKind.Raise);
            RunAstTest("unit z.x; interface implementation procedure p; begin raise x at z; end; end.", t => r(t)?.Kind, StructuredStatementKind.RaiseAt);
            RunAstTest("unit z.x; interface implementation procedure p; begin raise at z; end; end.", t => r(t)?.Kind, StructuredStatementKind.RaiseAtOnly);

            RunAstTest("unit z.x; interface implementation procedure p; begin try raise at z; finally raise at z; end; end; end.", t => r(t)?.Kind, StructuredStatementKind.TryFinally);
            RunAstTest("unit z.x; interface implementation procedure p; begin try raise at z; finally raise at z; end; end; end.", t => r(t)?.Statements?.Statements[0]?.GetType(), typeof(BlockOfStatements));
            RunAstTest("unit z.x; interface implementation procedure p; begin try raise at z; finally raise at z; end; end; end.", t => r(t)?.Statements?.Statements[1]?.GetType(), typeof(BlockOfStatements));

            RunAstTest("unit z.x; interface implementation procedure p; begin try raise at z; except on E: Exception do begin raise at z; end; end; end; end.", t => r(t)?.Kind, StructuredStatementKind.TryExcept);
            RunAstTest("unit z.x; interface implementation procedure p; begin try raise at z; except on E: Exception do begin raise at z; end; else begin end; end; end; end.", t => r(t)?.Statements?.Statements[0]?.GetType(), typeof(BlockOfStatements));
            RunAstTest("unit z.x; interface implementation procedure p; begin try raise at z; except on E: Exception do begin raise at z; end; else begin end; end; end; end.", t => r(t)?.Statements?.Statements[1]?.GetType(), typeof(StructuredStatement));
            RunAstTest("unit z.x; interface implementation procedure p; begin try raise at z; except on E: Exception do begin raise at z; end; else begin end; end; end; end.", t => (r(t)?.Statements?.Statements[1] as StructuredStatement)?.Kind, StructuredStatementKind.ExceptElse);
            RunAstTest("unit z.x; interface implementation procedure p; begin try raise at z; except on E: Exception do begin raise at z; end; else begin end; end; end; end.", t => (r(t)?.Statements?.Statements[1] as StructuredStatement)?.Statements.Statements[0]?.GetType(), typeof(StructuredStatement));
            RunAstTest("unit z.x; interface implementation procedure p; begin try raise at z; except on E: Exception do begin raise at z; end; else begin end; end; end; end.", t => ((r(t)?.Statements?.Statements[1] as StructuredStatement)?.Statements.Statements[0] as StructuredStatement)?.Kind, StructuredStatementKind.ExceptOn);
        }

        [Fact]
        public void TestWithStatement() {
            StructuredStatement r(object t) => (((t as CompilationUnit)?.ImplementationSymbols["p"] as MethodImplementation)?.Block as BlockOfStatements)?.Statements[0] as StructuredStatement;

            RunAstTest("unit z.x; interface implementation procedure p; begin with 1 do begin end; end; end.", t => r(t)?.Kind, StructuredStatementKind.With);
        }

        [Fact]
        public void TestRepeatStatement() {
            StructuredStatement r(object t) => (((t as CompilationUnit)?.ImplementationSymbols["p"] as MethodImplementation)?.Block as BlockOfStatements)?.Statements[0] as StructuredStatement;

            RunAstTest("unit z.x; interface implementation procedure p; begin repeat x until true; end; end.", t => r(t)?.Kind, StructuredStatementKind.Repeat);
        }

        [Fact]
        public void TestCaseStatement() {
            StructuredStatement r(object t) => (((t as CompilationUnit)?.ImplementationSymbols["p"] as MethodImplementation)?.Block as BlockOfStatements)?.Statements[0] as StructuredStatement;

            RunAstTest("unit z.x; interface implementation procedure p; begin case x of 1: l; 2..3,4: m; else z; end; end; end.", t => r(t)?.Kind, StructuredStatementKind.Case);
            RunAstTest("unit z.x; interface implementation procedure p; begin case x of 1: l; 2..3,4: m; else z; end; end; end.", t => (r(t)?.Statements?.Statements?.LastOrDefault() as StructuredStatement)?.Kind, StructuredStatementKind.CaseElse);
            RunAstTest("unit z.x; interface implementation procedure p; begin case x of 1: l; 2..3,4: m; else z; end; end; end.", t => (r(t)?.Statements?.Statements[1] as StructuredStatement)?.Kind, StructuredStatementKind.CaseItem);
            RunAstTest("unit z.x; interface implementation procedure p; begin case x of 1: l; 2..3,4: m; else z; end; end; end.", t => (r(t)?.Statements?.Statements[1] as StructuredStatement)?.Expressions[0]?.GetType(), typeof(BinaryOperator));
        }

        [Fact]
        public void TestWhileStatement() {
            StructuredStatement r(object t) => (((t as CompilationUnit)?.ImplementationSymbols["p"] as MethodImplementation)?.Block as BlockOfStatements)?.Statements[0] as StructuredStatement;

            RunAstTest("unit z.x; interface implementation procedure p; begin while true do x; end; end.", t => r(t)?.Kind, StructuredStatementKind.While);
            RunAstTest("unit z.x; interface implementation procedure p; begin while true do x; end; end.", t => r(t)?.Expressions?.FirstOrDefault()?.GetType(), typeof(ConstantValue));
        }

        [Fact]
        public void TestForStatement() {
            StructuredStatement r(object t) => (((t as CompilationUnit)?.ImplementationSymbols["p"] as MethodImplementation)?.Block as BlockOfStatements)?.Statements[0] as StructuredStatement;

            RunAstTest("unit z.x; interface implementation procedure p; begin for I:= 0 to 1 do x; end; end.", t => r(t)?.Kind, StructuredStatementKind.ForTo);
            RunAstTest("unit z.x; interface implementation procedure p; begin for I:= 0 to 1 do x; end; end.", t => r(t)?.Name?.CompleteName, "I");
            RunAstTest("unit z.x; interface implementation procedure p; begin for I:= 0 to 1 do x; end; end.", t => r(t)?.Expressions?.FirstOrDefault()?.GetType(), typeof(ConstantValue));
            //RunAstTest("unit z.x; interface implementation procedure p; begin for I:= 0 to 1 do x; end; end.", t => r(t)?.Expressions?.LastOrDefault()?.GetType(), typeof(ConstantValue));

            RunAstTest("unit z.x; interface implementation procedure p; begin for I:= 0 downto 1 do x; end; end.", t => r(t)?.Kind, StructuredStatementKind.ForDownTo);
            RunAstTest("unit z.x; interface implementation procedure p; begin for I:= 0 downto 1 do x; end; end.", t => r(t)?.Name?.CompleteName, "I");
            RunAstTest("unit z.x; interface implementation procedure p; begin for I:= 0 downto 1 do x; end; end.", t => r(t)?.Expressions?.FirstOrDefault()?.GetType(), typeof(ConstantValue));
            //RunAstTest("unit z.x; interface implementation procedure p; begin for I:= 0 downto 1 do x; end; end.", t => r(t)?.Expressions?.LastOrDefault()?.GetType(), typeof(ConstantValue));

            RunAstTest("unit z.x; interface implementation procedure p; begin for I in Q do x; end; end.", t => r(t)?.Kind, StructuredStatementKind.ForIn);
            RunAstTest("unit z.x; interface implementation procedure p; begin for I in Q do x; end; end.", t => r(t)?.Name?.CompleteName, "I");
        }

        [Fact]
        public void TestClosureExpression() {
            MethodImplementation r(object t) => (((t as CompilationUnit)?.ImplementationSymbols["p"] as MethodImplementation)?.Symbols["n"] as ConstantDeclaration)?.Value as MethodImplementation;

            RunAstTest("unit z.x; interface implementation procedure p; const n = procedure(const a: string) begin end; begin end; end.", t => r(t)?.Name.CompleteName, "$1");
            RunAstTest("unit z.x; interface implementation procedure p; const n = procedure(const a: string) begin end; begin end; end.", t => r(t)?.Parameters[0]?.ParameterKind, ParameterReferenceKind.Const);
            RunAstTest("unit z.x; interface implementation procedure p; const n = procedure(const a: string) begin end; begin end; end.", t => r(t)?.Parameters[0]?.Name?.CompleteName, "a");
            RunAstTest("unit z.x; interface implementation procedure p; const n = function(const a: string): Integer begin end; begin end; end.", t => r(t)?.Name.CompleteName, "$1");
            RunAstTest("unit z.x; interface implementation procedure p; const n = function(const a: string): String begin end; begin end; end.", t => (r(t)?.TypeValue as MetaType)?.Kind, MetaTypeKind.String);
        }

        [Fact]
        public void TestIfStatement() {
            StructuredStatement r(object t) => (((t as CompilationUnit)?.ImplementationSymbols["p"] as MethodImplementation)?.Block as BlockOfStatements)?.Statements[0] as StructuredStatement;

            RunAstTest("unit z.x; interface implementation procedure p; begin if a then b else c; end; end.", t => r(t)?.Kind, StructuredStatementKind.IfThen);
            RunAstTest("unit z.x; interface implementation procedure p; begin if a then b else c; end; end.", t => (r(t)?.Statements.Statements.LastOrDefault() as StructuredStatement)?.Kind, StructuredStatementKind.IfElse);
        }

        [Fact]
        public void TestGotoStatement() {
            StructuredStatement r(object t) => (((t as CompilationUnit)?.ImplementationSymbols["p"] as MethodImplementation)?.Block as BlockOfStatements)?.Statements[0] as StructuredStatement;

            RunAstTest("unit z.x; interface implementation procedure p; begin break; end; end.", t => r(t)?.Kind, StructuredStatementKind.Break);
            RunAstTest("unit z.x; interface implementation procedure p; begin continue; end; end.", t => r(t)?.Kind, StructuredStatementKind.Continue);
            RunAstTest("unit z.x; interface implementation procedure p; begin goto x; end; end.", t => r(t)?.Kind, StructuredStatementKind.GoToLabel);
            RunAstTest("unit z.x; interface implementation procedure p; begin exit; end; end.", t => r(t)?.Kind, StructuredStatementKind.Exit);
        }

        [Fact]
        public void TestAssignmentAndSimpleStatements() {
            StructuredStatement r(object t) => (((t as CompilationUnit)?.ImplementationSymbols["p"] as MethodImplementation)?.Block as BlockOfStatements)?.Statements[0] as StructuredStatement;

            RunAstTest("unit z.x; interface implementation procedure p; begin a := x; end; end.", t => r(t)?.Kind, StructuredStatementKind.Assignment);
            RunAstTest("unit z.x; interface implementation procedure p; begin x(); end; end.", t => r(t)?.Kind, StructuredStatementKind.ExpressionStatement);
        }

        [Fact]
        public void TestBinaryOperators() {
            BinaryOperator r(object t) => (((t as CompilationUnit)?.ImplementationSymbols["p"] as MethodImplementation)?.Symbols["n"] as ConstantDeclaration)?.Value as BinaryOperator;

            RunAstTest("unit z.x; interface implementation procedure p; const n = 1 < 2; begin l: s; end; end.", t => r(t)?.Kind, ExpressionKind.LessThen);
            RunAstTest("unit z.x; interface implementation procedure p; const n = 1 <= 2; begin l: s; end; end.", t => r(t)?.Kind, ExpressionKind.LessThenEquals);
            RunAstTest("unit z.x; interface implementation procedure p; const n = 1 > 2; begin l: s; end; end.", t => r(t)?.Kind, ExpressionKind.GreaterThen);
            RunAstTest("unit z.x; interface implementation procedure p; const n = 1 >= 2; begin l: s; end; end.", t => r(t)?.Kind, ExpressionKind.GreaterThenEquals);
            RunAstTest("unit z.x; interface implementation procedure p; const n = 1 <> 2; begin l: s; end; end.", t => r(t)?.Kind, ExpressionKind.NotEquals);
            RunAstTest("unit z.x; interface implementation procedure p; const n = 1 = 2; begin l: s; end; end.", t => r(t)?.Kind, ExpressionKind.EqualsSign);
            RunAstTest("unit z.x; interface implementation procedure p; const n = a is TObject; begin l: s; end; end.", t => r(t)?.Kind, ExpressionKind.Is);
            RunAstTest("unit z.x; interface implementation procedure p; const n = 1 in [1,2]; begin l: s; end; end.", t => r(t)?.Kind, ExpressionKind.In);
            RunAstTest("unit z.x; interface implementation procedure p; const n = 1 in [1,2]; begin l: s; end; end.", t => r(t)?.RightOperand?.GetType(), typeof(SetExpression));

            RunAstTest("unit z.x; interface implementation procedure p; const n = 1 + 2; begin l: s; end; end.", t => r(t)?.Kind, ExpressionKind.Plus);
            RunAstTest("unit z.x; interface implementation procedure p; const n = 1 - 2; begin l: s; end; end.", t => r(t)?.Kind, ExpressionKind.Minus);
            RunAstTest("unit z.x; interface implementation procedure p; const n = 1 or 2; begin l: s; end; end.", t => r(t)?.Kind, ExpressionKind.Or);
            RunAstTest("unit z.x; interface implementation procedure p; const n = 1 xor 2; begin l: s; end; end.", t => r(t)?.Kind, ExpressionKind.Xor);

            RunAstTest("unit z.x; interface implementation procedure p; const n = 1 * 2; begin l: s; end; end.", t => r(t)?.Kind, ExpressionKind.Times);
            RunAstTest("unit z.x; interface implementation procedure p; const n = 1 / 2; begin l: s; end; end.", t => r(t)?.Kind, ExpressionKind.Slash);
            RunAstTest("unit z.x; interface implementation procedure p; const n = 1 div 2; begin l: s; end; end.", t => r(t)?.Kind, ExpressionKind.Div);
            RunAstTest("unit z.x; interface implementation procedure p; const n = 1 mod 2; begin l: s; end; end.", t => r(t)?.Kind, ExpressionKind.Mod);
            RunAstTest("unit z.x; interface implementation procedure p; const n = 1 and 2; begin l: s; end; end.", t => r(t)?.Kind, ExpressionKind.And);
            RunAstTest("unit z.x; interface implementation procedure p; const n = 1 shl 2; begin l: s; end; end.", t => r(t)?.Kind, ExpressionKind.Shl);
            RunAstTest("unit z.x; interface implementation procedure p; const n = 1 shr 2; begin l: s; end; end.", t => r(t)?.Kind, ExpressionKind.Shr);
            RunAstTest("unit z.x; interface implementation procedure p; const n = a as TObject; begin l: s; end; end.", t => r(t)?.Kind, ExpressionKind.As);
        }

    }
}
