
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test statements and types
    /// </summary>
    public class StatementTypeTest : TypeTest {

        [TestMethod]
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
            AssertAssignmentCompat("e1..e2", "e1..e3", predeclaration: "tq=(e1, e2, e3);");
            AssertAssignmentCompat("-3..3", "type Integer");
            AssertAssignmentCompat("'b'..'r'", "type Char");
            AssertAssignmentCompat("false..true", "type Boolean");
            AssertAssignmentCompat("-4..4", "-3..3");
            AssertAssignmentCompat("'a'..'n'", "'b'..'r'");
            AssertAssignmentCompat("false..true", "false..true");
            AssertAssignmentCompat("set of -4..4", "set of -3..3");
            AssertAssignmentCompat("packed array[0..3] of char", "packed array[0..3] of char");
            AssertAssignmentCompat("string[5]", "string[1]");
            AssertAssignmentCompat("real", "integer");
            AssertAssignmentCompat("extended", "byte");
            AssertAssignmentCompat("integer", "real", false);
            AssertAssignmentCompat("string", "AnsiChar");
            AssertAssignmentCompat("string", "WideChar");
            AssertAssignmentCompat("string", "string");
            AssertAssignmentCompat("string", "WideString");
            AssertAssignmentCompat("string", "UnicodeString");
            AssertAssignmentCompat("string", "ShortString");
            AssertAssignmentCompat("string", "RawByteString");
            AssertAssignmentCompat("string", "string[23]");
        }

    }
}
