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

            RunAstTest("library z.x; interface implementation end.", t => u(t)?.SymbolName, "z.x");
            RunAstTest("library z.x; interface implementation end.", t => u(t)?.UnitName.Name, "x");
            RunAstTest("library z.x; interface implementation end.", t => u(t)?.UnitName.Namespace, "z");
            RunAstTest("library z.x; interface implementation end.", t => u(t)?.FileType, CompilationUnitType.Library);

            RunAstTest("library z.x; deprecated; interface implementation end.", t => u(t)?.Hints?.SymbolIsDeprecated, true);
            RunAstTest("library z.x; deprecated 'X'; interface implementation end.", t => u(t)?.Hints?.DeprecratedInformation, "X");
            RunAstTest("library z.x; library; interface implementation end.", t => u(t)?.Hints?.SymbolInLibrary, true);
            RunAstTest("library z.x; platform; interface implementation end.", t => u(t)?.Hints?.SymbolIsPlatformSpecific, true);
            RunAstTest("library z.x; experimental; interface implementation end.", t => u(t)?.Hints?.SymbolIsExperimental, true);

            RunAstTest("library z.x; interface implementation end. § unit z.x; interface implementation end.",
                t => (t as CompilationUnit)?.SymbolName, "z.x", StructuralErrors.DuplicateUnitName);

            RunAstTest("library z.x.q; interface implementation end.",
                t => (t as CompilationUnit)?.SymbolName, "z.x.q", StructuralErrors.UnitNameDoesNotMatchFileName);

        }

    }
}
