using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PasPasPasTests.Parser {

    /// <summary>
    ///     test abstract syntax trees
    /// </summary>
    public class AstTest : ParserTestBase {

        [Fact]
        public void TestUnit() {
            Func<object, CompilationUnit> u = t => (t as CompilationUnit);

            RunAstTest("unit z.x; interface implementation end.", t => u(t)?.SymbolName, "z.x");
            RunAstTest("unit z.x; interface implementation end.", t => u(t)?.UnitName.Name, "x");
            RunAstTest("unit z.x; interface implementation end.", t => u(t)?.UnitName.Namespace, "z");
            RunAstTest("unit z.x; interface implementation end.", t => u(t)?.FileType, CompilationUnitType.Unit);
            RunAstTest("unit z.x; deprecated; interface implementation end.", t => u(t)?.Hints?.SymbolIsDeprecated, true);
            RunAstTest("unit z.x; deprecated 'X'; interface implementation end.", t => u(t)?.Hints?.DeprecatedInformation, "X");
            RunAstTest("unit z.x; library; interface implementation end.", t => u(t)?.Hints?.SymbolInLibrary, true);
            RunAstTest("unit z.x; platform; interface implementation end.", t => u(t)?.Hints?.SymbolIsPlatformSpecific, true);
            RunAstTest("unit z.x; experimental; interface implementation end.", t => u(t)?.Hints?.SymbolIsExperimental, true);


            RunAstTest("unit z.x; interface implementation end. § unit z.x; interface implementation end.",
                t => (t as CompilationUnit)?.SymbolName, "z.x", StructuralErrors.DuplicateUnitName);

            RunAstTest("unit z.x.q; interface implementation end.",
                t => (t as CompilationUnit)?.SymbolName, "z.x.q", StructuralErrors.UnitNameDoesNotMatchFileName);
        }

        [Fact]
        public void TestLibrary() {
            Func<object, CompilationUnit> u = t => (t as CompilationUnit);

            RunAstTest("library z.x; begin end.", t => u(t)?.SymbolName, "z.x");
            RunAstTest("library z.x; begin end.", t => u(t)?.UnitName.Name, "x");
            RunAstTest("library z.x; begin end.", t => u(t)?.UnitName.Namespace, "z");
            RunAstTest("library z.x; begin end.", t => u(t)?.FileType, CompilationUnitType.Library);

            RunAstTest("library z.x; deprecated;  begin end.", t => u(t)?.Hints?.SymbolIsDeprecated, true);
            RunAstTest("library z.x; deprecated 'X'; begin end.", t => u(t)?.Hints?.DeprecatedInformation, "X");
            RunAstTest("library z.x; library; begin end.", t => u(t)?.Hints?.SymbolInLibrary, true);
            RunAstTest("library z.x; platform; begin end.", t => u(t)?.Hints?.SymbolIsPlatformSpecific, true);
            RunAstTest("library z.x; experimental; begin end.", t => u(t)?.Hints?.SymbolIsExperimental, true);

            RunAstTest("library z.x; begin end. § unit z.x; interface implementation end.",
                t => (t as CompilationUnit)?.SymbolName, "z.x", StructuralErrors.DuplicateUnitName);

            RunAstTest("library z.x.q;  begin end.",
                t => (t as CompilationUnit)?.SymbolName, "z.x.q", StructuralErrors.UnitNameDoesNotMatchFileName);

        }

        [Fact]
        public void TestProgram() {

            Func<object, CompilationUnit> u = t => (t as CompilationUnit);

            RunAstTest("program z.x; begin end.", t => u(t)?.FileType, CompilationUnitType.Program);
            RunAstTest("program z.x; begin end.", t => u(t)?.SymbolName, "z.x");
            RunAstTest("program z.x; begin end.", t => u(t)?.UnitName.Name, "x");
            RunAstTest("program z.x; begin end.", t => u(t)?.UnitName.Namespace, "z");

            RunAstTest("program z.x; begin end. § unit z.x; interface implementation end.",
                t => (t as CompilationUnit)?.SymbolName, "z.x", StructuralErrors.DuplicateUnitName);

            RunAstTest("program z.x.q; begin end.",
                t => (t as CompilationUnit)?.SymbolName, "z.x.q", StructuralErrors.UnitNameDoesNotMatchFileName);


        }

        [Fact]
        public void TestPackage() {

            Func<object, CompilationUnit> u = t => (t as CompilationUnit);

            RunAstTest("package z.x; requires a, a.a, a.a.a, x; end.", t => u(t)?.FileType, CompilationUnitType.Package);
            RunAstTest("package z.x; requires a, a.a, a.a.a, x; end.", t => u(t)?.SymbolName, "z.x");
            RunAstTest("package z.x; requires a, a.a, a.a.a, x; end.", t => u(t)?.UnitName.Name, "x");
            RunAstTest("package z.x; requires a, a.a, a.a.a, x; end.", t => u(t)?.UnitName.Namespace, "z");


        }

        [Fact]
        public void TestUsesClause() {
            Func<object, RequiredUnitNameList> u = t => (t as CompilationUnit)?.RequiredUnits;
            RunAstTest("unit z.x; interface uses a; implementation end.", t => u(t)?.Contains("a"), true);
            RunAstTest("unit z.x; interface uses a; implementation end.", t => u(t)?["a"].Mode, UnitMode.Interface);

            RunAstTest("unit z.x; interface implementation uses a; end.", t => u(t)?.Contains("a"), true);
            RunAstTest("unit z.x; interface implementation uses a; end.", t => u(t)?["a"].Mode, UnitMode.Implementation);

            RunAstTest("unit z.x; interface uses a, a; implementation end.", t => u(t)?.Contains("a"), true,
                StructuralErrors.RedeclaredUnitNameInUsesList);

            RunAstTest("unit z.x; interface uses a; implementation uses a; end.", t => u(t)?.Contains("a"), true,
                StructuralErrors.RedeclaredUnitNameInUsesList);

            RunAstTest("unit z.x; interface implementation uses a, a; end.", t => u(t)?.Contains("a"), true,
                StructuralErrors.RedeclaredUnitNameInUsesList);

            RunAstTest("package z.x; requires x; contains a; end.", t => u(t)?.Contains("a"), true);
            RunAstTest("package z.x; requires x; contains a; end.", t => u(t)?["a"].Mode, UnitMode.Contains);

            RunAstTest("package z.x; requires x, x; end.", t => u(t)?.Contains("x"), true,
                StructuralErrors.RedeclaredUnitNameInUsesList);
        }

        [Fact]
        public void TestUsesFileClause() {
            Func<object, RequiredUnitNameList> u = t => (t as CompilationUnit)?.RequiredUnits;
            RunAstTest("program z.x; uses a; begin end.", t => u(t)?.Contains("a"), true);
            RunAstTest("program z.x; uses a; begin end.", t => u(t)?["a"].Mode, UnitMode.Program);
            RunAstTest("program z.x; uses a in 'a\\a\\a.pas'; begin end.", t => u(t)?["a"].FileName, "a\\a\\a.pas");

            RunAstTest("program z.x; uses a, a; begin end.", t => u(t)?.Contains("a"), true,
                StructuralErrors.RedeclaredUnitNameInUsesList);

            RunAstTest("library z.x; uses a; begin end.", t => u(t)?.Contains("a"), true);
            RunAstTest("library z.x; uses a; begin end.", t => u(t)?["a"].Mode, UnitMode.Library);
            RunAstTest("library z.x; uses a in 'a\\a\\a.pas'; begin end.", t => u(t)?["a"].FileName, "a\\a\\a.pas");

            RunAstTest("library z.x; uses a, a; begin end.", t => u(t)?.Contains("a"), true,
                StructuralErrors.RedeclaredUnitNameInUsesList);

            RunAstTest("package z.x; requires x; contains a; end.", t => u(t)?.Contains("a"), true);
            RunAstTest("package z.x; requires x; contains a; end.", t => u(t)?["a"].Mode, UnitMode.Contains);
            RunAstTest("package z.x; requires x; contains a in 'a\\a\\a.pas'; end.", t => u(t)?["a"].FileName, "a\\a\\a.pas");

            RunAstTest("package z.x; requires x; contains a, a; end.", t => u(t)?.Contains("a"), true,
                StructuralErrors.RedeclaredUnitNameInUsesList);
        }

        [Fact]
        public void TestConstDeclaration() {
            Func<object, ConstantDeclaration> u = t => ((t as CompilationUnit)?.InterfaceSymbols["x"]) as ConstantDeclaration;
            Func<object, ConstantDeclaration> v = t => ((t as CompilationUnit)?.ImplementationSymbols["x"]) as ConstantDeclaration;

            RunAstTest("unit z.x; interface const x = 5; implementation end.", t => u(t)?.SymbolName, "x");
            RunAstTest("unit z.x; interface const x = 5; implementation end.", t => u(t)?.Mode, DeclarationMode.Const);
            RunAstTest("unit z.x; interface resourcestring x = 'a'; implementation end.", t => u(t)?.Mode, DeclarationMode.ResourceString);

            RunAstTest("unit z.x; interface implementation const x = 5; end.", t => v(t)?.SymbolName, "x");
            RunAstTest("unit z.x; interface implementation const x = 5; end.", t => v(t)?.Mode, DeclarationMode.Const);
            RunAstTest("unit z.x; interface implementation resourcestring x = 'a'; end.", t => v(t)?.Mode, DeclarationMode.ResourceString);

            RunAstTest("unit z.x; interface const x = 5; x = 6; implementation end.", t => u(t)?.Mode, DeclarationMode.Const,
                StructuralErrors.RedeclaredSymbol);
        }

        [Fact]
        public void TestArrayType() {
            Func<object, ArrayTypeDeclaration> u = t => (((t as CompilationUnit)?.InterfaceSymbols["x"]) as ConstantDeclaration)?.TypeValue as ArrayTypeDeclaration;

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
            Func<object, SetTypeDeclaration> u = t => (((t as CompilationUnit)?.InterfaceSymbols["x"]) as ConstantDeclaration)?.TypeValue as SetTypeDeclaration;

            RunAstTest("unit z.x; interface const x : set of array of const = nil; implementation end.", t => ((u(t)?.TypeValue as ArrayTypeDeclaration)?.TypeValue as MetaType)?.Kind, MetaTypeKind.Const);
        }

        [Fact]
        public void TestPointerToType() {
            Func<object, PointerToType> u = t => (((t as CompilationUnit)?.InterfaceSymbols["x"]) as TypeDeclaration)?.TypeValue as PointerToType;
            Func<object, TypeDeclaration> v = t => ((t as CompilationUnit)?.InterfaceSymbols["x"]) as TypeDeclaration;

            RunAstTest("unit z.x; interface type x = ^Pointer; implementation end.", t => (u(t)?.TypeValue as MetaType)?.Kind, MetaTypeKind.Pointer);
            RunAstTest("unit z.x; interface type x = Pointer; implementation end.", t => (v(t)?.TypeValue as MetaType)?.Kind, MetaTypeKind.Pointer);
        }

        [Fact]
        public void TestFileType() {
            Func<object, FileTypeDeclaration> u = t => (((t as CompilationUnit)?.InterfaceSymbols["x"]) as ConstantDeclaration)?.TypeValue as FileTypeDeclaration;

            RunAstTest("unit z.x; interface const x : file of array of const = nil; implementation end.", t => ((u(t)?.TypeValue as ArrayTypeDeclaration)?.TypeValue as MetaType)?.Kind, MetaTypeKind.Const);
        }

        [Fact]
        public void TestClassOfType() {
            Func<object, ClassOfTypeDeclaration> u = t => (((t as CompilationUnit)?.InterfaceSymbols["x"]) as ConstantDeclaration)?.TypeValue as ClassOfTypeDeclaration;

            RunAstTest("unit z.x; interface const x : class of TFuzz = nil; implementation end.", t => (u(t)?.TypeValue as MetaType)?.Kind, MetaTypeKind.NamedType);
            RunAstTest("unit z.x; interface const x : class of TFuzz = nil; implementation end.", t => (u(t)?.TypeValue as MetaType)?.Name?.CompleteName, "TFuzz");
        }

        [Fact]
        public void TestEnumType() {
            Func<object, EnumType> u = t => (((t as CompilationUnit)?.InterfaceSymbols["x"]) as TypeDeclaration)?.TypeValue as EnumType;

            RunAstTest("unit z.x; interface type x = (a, b); implementation end.", t => u(t)?.Contains("a"), true);
            RunAstTest("unit z.x; interface type x = (a, b); implementation end.", t => u(t)?["a"].Name.CompleteName, "a");

            RunAstTest("unit z.x; interface type x = (a, b, a); implementation end.", t => u(t)?["a"].Name.CompleteName, "a",
                StructuralErrors.RedeclaredEnumName);

        }

        [Fact]
        public void TestSubrangeType() {
            Func<object, SubrangeType> u = t => (((t as CompilationUnit)?.InterfaceSymbols["x"]) as TypeDeclaration)?.TypeValue as SubrangeType;

            RunAstTest("unit z.x; interface type x = 3..4; implementation end.", t => u(t) != null, true);

        }

        [Fact]
        public void TestStringType() {
            Func<object, MetaType> u = t => (((t as CompilationUnit)?.InterfaceSymbols["x"]) as TypeDeclaration)?.TypeValue as MetaType;

            RunAstTest("unit z.x; interface type x = string; implementation end.", t => u(t)?.Kind, MetaTypeKind.String);
            RunAstTest("unit z.x; interface type x = ShortString; implementation end.", t => u(t)?.Kind, MetaTypeKind.ShortString);
            RunAstTest("unit z.x; interface type x = Ansistring; implementation end.", t => u(t)?.Kind, MetaTypeKind.AnsiString);
            RunAstTest("unit z.x; interface type x = uniCodeString; implementation end.", t => u(t)?.Kind, MetaTypeKind.UnicodeString);
            RunAstTest("unit z.x; interface type x = wideString; implementation end.", t => u(t)?.Kind, MetaTypeKind.WideString);

            RunAstTest("unit z.x; interface type x = string[nil]; implementation end.", t => (u(t)?.Value as ConstantValue)?.Kind, ConstantValueKind.Nil);
            RunAstTest("unit z.x; interface type x = ansistring(nil); implementation end.", t => (u(t)?.Value as ConstantValue)?.Kind, ConstantValueKind.Nil);
        }

        [Fact]
        public void TestTypeAlias() {
            Func<object, TypeAlias> u = t => (((t as CompilationUnit)?.InterfaceSymbols["x"]) as TypeDeclaration)?.TypeValue as TypeAlias;

            RunAstTest("unit z.x; interface type x = z.q; implementation end.", t => u(t)?.Fragments[0].Name.CompleteName, "z.q");
            RunAstTest("unit z.x; interface type x = z.q; implementation end.", t => u(t)?.IsNewType, false);
            RunAstTest("unit z.x; interface type x = type z.q; implementation end.", t => u(t)?.Fragments[0].Name.CompleteName, "z.q");
            RunAstTest("unit z.x; interface type x = type z.q; implementation end.", t => u(t)?.IsNewType, true);

            RunAstTest("unit z.x; interface type x = type z<r>.q; implementation end.", t => u(t)?.Fragments[0]?.TypeValue is TypeAlias, true);

            RunAstTest("unit z.x; interface type x = type z<r,array of const>.q<t>; implementation end.", t => u(t)?.Fragments[0]?.TypeValue is TypeAlias, false);
            RunAstTest("unit z.x; interface type x = type z<r>.q<t>; implementation end.", t => u(t)?.Fragments[1]?.TypeValue is TypeAlias, true);

            RunAstTest("unit z.x; interface type x = type of z.q; implementation end.", t => u(t)?.IsNewType, true,
                StructuralErrors.UnsupportedTypeOfConstruct);
        }

        [Fact]
        public void TestProceduralType() {
            Func<object, ProceduralType> u = t => (((t as CompilationUnit)?.InterfaceSymbols["x"]) as TypeDeclaration)?.TypeValue as ProceduralType;

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
            Func<object, ParameterDefinitions> u = t => ((((t as CompilationUnit)?.InterfaceSymbols["x"]) as TypeDeclaration)?.TypeValue as ProceduralType)?.Parameters;

            RunAstTest("unit z.x; interface type x = procedure(x: string); implementation end.", t => (u(t)?[0] as ParameterDefinition)?.SymbolName, "x");
            RunAstTest("unit z.x; interface type x = procedure(x: string); implementation end.", t => ((u(t)?[0] as ParameterDefinition)?.TypeValue as MetaType)?.Kind, MetaTypeKind.String);
            RunAstTest("unit z.x; interface type x = procedure([n] x: string); implementation end.", t => (u(t)?[0] as ParameterDefinition)?.Attributes?.FirstOrDefault().SymbolName, "n");
            RunAstTest("unit z.x; interface type x = procedure([n] const [m] x: string); implementation end.", t => (u(t)?[0] as ParameterDefinition)?.Attributes?.Skip(1)?.FirstOrDefault().SymbolName, "m");
            RunAstTest("unit z.x; interface type x = procedure([n] x: string); implementation end.", t => (u(t)?[0] as ParameterDefinition)?.ParameterKind, ParameterReferenceKind.Undefined);
            RunAstTest("unit z.x; interface type x = procedure([n] var x: string); implementation end.", t => (u(t)?[0] as ParameterDefinition)?.ParameterKind, ParameterReferenceKind.Var);
            RunAstTest("unit z.x; interface type x = procedure([n] const x: string); implementation end.", t => (u(t)?[0] as ParameterDefinition)?.ParameterKind, ParameterReferenceKind.Const);
            RunAstTest("unit z.x; interface type x = procedure([n] out x: string); implementation end.", t => (u(t)?[0] as ParameterDefinition)?.ParameterKind, ParameterReferenceKind.Out);
        }
    }
}
