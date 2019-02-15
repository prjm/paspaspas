using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;
using PasPasPas.Typings.Simple;
using PasPasPas.Typings.Structured;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test simple types
    /// </summary>
    public class SimpleTypesTest : TypeTest {

        [TestMethod]
        public void TestEnumTypes() {
            AssertDeclType("(en1, en2)", typeKind: CommonTypeKind.EnumerationType);
            AssertDeclType("(en1, en2)", (td) => Assert.AreEqual(2, (td as EnumeratedType).Values.Count));
            AssertDeclType("(en1, en2)", (td) => Assert.AreEqual(0L, (((td as EnumeratedType).Values[0].Value as IEnumeratedValue)?.Value as IIntegerValue)?.SignedValue));
            AssertDeclType("(en1, en2)", (td) => Assert.AreEqual(1L, (((td as EnumeratedType).Values[1].Value as IEnumeratedValue)?.Value as IIntegerValue)?.SignedValue));
        }

        [TestMethod]
        public void TestSubrangeTypes() {
            AssertDeclType("3..5", typeKind: CommonTypeKind.SubrangeType);
            AssertDeclType("'a'..'z'", typeKind: CommonTypeKind.SubrangeType);
        }

        [TestMethod]
        public void TestSetTypes() {
            AssertDeclType("set of (false, true)", typeKind: CommonTypeKind.SetType);
            AssertDeclType("set of -3..3", typeKind: CommonTypeKind.SetType);
            AssertDeclType("set of Boolean", typeKind: CommonTypeKind.SetType);
            AssertDeclType("set of System.Byte", typeKind: CommonTypeKind.SetType);
        }

        [TestMethod]
        public void TestArrayTypes() {

            ITypeDefinition GetIndexType(ArrayType array) {
                if (array == null)
                    return default;

                var registry = array.TypeRegistry;
                return registry.GetTypeByIdOrUndefinedType(array.IndexTypes[0]);
            }

            CommonTypeKind GetIndexTypeKind(ArrayType array) {
                var t = GetIndexType(array);
                return t != null ? t.TypeKind : CommonTypeKind.UnknownType;
            };

            AssertDeclType("array [1..4] of Integer", typeKind: CommonTypeKind.ArrayType);
            AssertDeclType("array [1..4] of Integer", (td) => Assert.AreEqual(CommonTypeKind.IntegerType, (td as ArrayType)?.BaseType?.TypeKind));
            AssertDeclType("array [1..4] of Integer", (td) => Assert.AreEqual(CommonTypeKind.IntegerType, (GetIndexType(td as ArrayType) as SubrangeType)?.BaseType?.TypeKind));
            AssertDeclType("array [false..true] of Integer", (td) => Assert.AreEqual(CommonTypeKind.BooleanType, (GetIndexType(td as ArrayType) as SubrangeType)?.BaseType.TypeKind));
            AssertDeclType("array [Boolean] of Integer", (td) => Assert.AreEqual(CommonTypeKind.BooleanType, GetIndexTypeKind(td as ArrayType)));
            AssertDeclType("array [System.Boolean] of Integer", (td) => Assert.AreEqual(CommonTypeKind.BooleanType, GetIndexTypeKind(td as ArrayType)));
        }

        [TestMethod]
        public void TestConstantArrays()
            => AssertExprValue("a", GetArrayValue(RegisteredTypes.SmallestUserTypeId, KnownTypeIds.CardinalType, GetIntegerValue(0x7E3), GetIntegerValue(0x81B), GetIntegerValue(0x819), GetIntegerValue(0x81A)), "const a: array [0..3] of Cardinal = ($000007E3, $0000081B, $00000819, $0000081A);");


    }
}
