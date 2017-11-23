using PasPasPas.Typings.Structured;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Simple;
using Xunit;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test simple types
    /// </summary>
    public class SimpleTypesTest : TypeTest {

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
            AssertDeclType("set of Boolean", typeKind: CommonTypeKind.SetType);
            AssertDeclType("set of System.Byte", typeKind: CommonTypeKind.SetType);
        }

        [Fact]
        public void TestArrayTypes() {
            AssertDeclType("array [1..4] of Integer", typeKind: CommonTypeKind.ArrayType);
            AssertDeclType("array [1..4] of Integer", (td) => Assert.Equal(CommonTypeKind.IntegerType, (td as ArrayType)?.BaseType?.TypeKind));
            AssertDeclType("array [1..4] of Integer", (td) => Assert.Equal(CommonTypeKind.IntegerType, (td as ArrayType)?.IndexTypes[0]?.TypeKind));
            AssertDeclType("array [false..true] of Integer", (td) => Assert.Equal(CommonTypeKind.BooleanType, (td as ArrayType)?.IndexTypes[0]?.TypeKind));
            AssertDeclType("array [Boolean] of Integer", (td) => Assert.Equal(CommonTypeKind.BooleanType, (td as ArrayType)?.IndexTypes[0]?.TypeKind));
            AssertDeclType("array [System.Boolean] of Integer", (td) => Assert.Equal(CommonTypeKind.BooleanType, (td as ArrayType)?.IndexTypes[0]?.TypeKind));
        }


    }
}
