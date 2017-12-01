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
        }

    }
}
