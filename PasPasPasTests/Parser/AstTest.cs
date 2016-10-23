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

            RunAstTest("unit z.x; interface implementation end.", t => (t as CompilationUnit)?.SymbolName, "z.x");
            RunAstTest("unit z.x; interface implementation end.", t => (t as CompilationUnit)?.UnitName.Name, "x");
            RunAstTest("unit z.x; interface implementation end.", t => (t as CompilationUnit)?.UnitName.Namespace, "z");

            RunAstTest("unit z.x; interface implementation end. § unit z.x; interface implementation end.",
                t => (t as CompilationUnit)?.SymbolName, "z.x", StructuralErrors.DuplicateUnitName);

            RunAstTest("unit z.x.q; interface implementation end.",
                t => (t as CompilationUnit)?.SymbolName, "z.x.q", StructuralErrors.UnitNameDoesNotMatchFilename);
        }

    }
}
