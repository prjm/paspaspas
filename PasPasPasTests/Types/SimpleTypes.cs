using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Simple;
using Xunit;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test simple types
    /// </summary>
    public class SimpleTypes : TypeTest {

        [Fact]
        public void TestEnumTypes() {
            AssertDeclType("(en1, en2)", typeKind: CommonTypeKind.EnumerationType);
            AssertDeclType("(en1, en2)", (td) => Assert.Equal(2, (td as EnumeratedType).Values.Count));
        }

        [Fact]
        public void TestSubrangeTypes() {
            AssertDeclType("3..5", typeKind: CommonTypeKind.IntegerType);
            AssertDeclType("'a'..'z'", typeKind: CommonTypeKind.WideCharType);
        }

        [Fact]
        public void TestSetTypes() {
            AssertDeclType("set of (false, true)", typeKind: CommonTypeKind.SetType);
            AssertDeclType("set of -3..3", typeKind: CommonTypeKind.SetType);
        }



    }
}
