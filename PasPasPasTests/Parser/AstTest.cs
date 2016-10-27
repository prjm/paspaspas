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
            RunAstTest("unit z.x; deprecated 'X'; interface implementation end.", t => u(t)?.Hints?.DeprecratedInformation, "X");
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
            RunAstTest("library z.x; deprecated 'X'; begin end.", t => u(t)?.Hints?.DeprecratedInformation, "X");
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
            Func<object, UnitNameList> u = t => (t as CompilationUnit)?.RequiredUnits;
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
        }

    }
}
