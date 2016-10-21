using PasPasPas.Parsing.SyntaxTree.Abstract;
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

            RunAstTest("unit a; interface implementation end.", t => (t as CompilationUnit)?.SymbolName, "a");

        }

    }
}
