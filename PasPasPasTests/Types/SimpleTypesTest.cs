using PasPasPas.Typings.Structured;
using PasPasPas.Typings.Simple;
using Xunit;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

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
            AssertDeclType("3..5", typeKind: CommonTypeKind.SubrangeType);
            AssertDeclType("'a'..'z'", typeKind: CommonTypeKind.SubrangeType);
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

            ITypeDefinition GetIndexType(ArrayType array) {
                if (array == null)
                    return default;

                var registry = array.TypeRegistry;
                return registry.GetTypeByIdOrUndefinedType(array.IndexTypes[0].TypeId);
            }

            CommonTypeKind GetIndexTypeKind(ArrayType array) {
                var t = GetIndexType(array);
                return t != null ? t.TypeKind : CommonTypeKind.UnknownType;
            };

            AssertDeclType("array [1..4] of Integer", typeKind: CommonTypeKind.ArrayType);
            AssertDeclType("array [1..4] of Integer", (td) => Assert.Equal(CommonTypeKind.IntegerType, (td as ArrayType)?.BaseType?.TypeKind));
            AssertDeclType("array [1..4] of Integer", (td) => Assert.Equal(CommonTypeKind.IntegerType, (GetIndexType(td as ArrayType) as SubrangeType)?.BaseType?.TypeKind));
            AssertDeclType("array [false..true] of Integer", (td) => Assert.Equal(CommonTypeKind.BooleanType, (GetIndexType(td as ArrayType) as SubrangeType)?.BaseType.TypeKind));
            AssertDeclType("array [Boolean] of Integer", (td) => Assert.Equal(CommonTypeKind.BooleanType, GetIndexTypeKind(td as ArrayType)));
            AssertDeclType("array [System.Boolean] of Integer", (td) => Assert.Equal(CommonTypeKind.BooleanType, GetIndexTypeKind(td as ArrayType)));
        }


    }
}
