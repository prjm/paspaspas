using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test statements and types
    /// </summary>
    public class StatementTypeTest : TypeTest {

        [Fact]
        public void TestTypeIdentity() {
            AssertAssignmentCompat("Integer", "System.Integer");
            AssertAssignmentCompat("System.Integer", "Integer");
            AssertAssignmentCompat("Real", "Double");
            AssertAssignmentCompat("TObject", "TObject");
            AssertAssignmentCompat("type TObject", "type TObject", false);
            AssertAssignmentCompat("type Integer", "type Int64");
            AssertAssignmentCompat("type Int64", "type Integer");
            AssertAssignmentCompat("type Single", "type Double");
            AssertAssignmentCompat("type Real", "type Double");
            AssertAssignmentCompat("type Real", "type Extended");
            AssertAssignmentCompat("type AnsiChar", "AnsiChar");
            AssertAssignmentCompat("WideChar", "type WideChar");
            AssertAssignmentCompat("type Integer", "-3..3");
            AssertAssignmentCompat("type Char", "'b'..'r'");
            AssertAssignmentCompat("type Boolean", "false..true");
            AssertAssignmentCompat("(e1, e2, e3)", "e1..e3");
            ;
        }

    }
}
