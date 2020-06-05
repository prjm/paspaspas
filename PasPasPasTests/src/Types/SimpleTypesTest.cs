using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Simple;
using PasPasPas.Typings.Structured;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test simple types
    /// </summary>
    public class SimpleTypesTest : TypeTest {

        /// <summary>
        ///     test enumerated types
        /// </summary>
        [TestMethod]
        public void TestEnumTypes() {
            AssertDeclType("(en1, en2)", typeKind: BaseType.Enumeration);
            AssertDeclType("(en1, en2)", (td) => Assert.AreEqual(2, (td as EnumeratedType).Values.Count));
            AssertDeclType("(en1, en2)", (td) => Assert.AreEqual(0L, (((td as EnumeratedType).Values[0].Value as IEnumeratedValue)?.Value as IIntegerValue)?.SignedValue));
            AssertDeclType("(en1, en2)", (td) => Assert.AreEqual(1L, (((td as EnumeratedType).Values[1].Value as IEnumeratedValue)?.Value as IIntegerValue)?.SignedValue));
        }

        /// <summary>
        ///     test subrange types
        /// </summary>
        [TestMethod]
        public void TestSubrangeTypes() {
            AssertDeclType("3..5", typeKind: BaseType.Subrange);
            AssertDeclType("'a'..'z'", typeKind: BaseType.Subrange);
        }

        /// <summary>
        ///     test set types
        /// </summary>
        [TestMethod]
        public void TestSetTypes() {
            AssertDeclType("set of (false, true)", typeKind: BaseType.Set);
            AssertDeclType("set of -3..3", typeKind: BaseType.Set);
            AssertDeclType("set of Boolean", typeKind: BaseType.Set);
            AssertDeclType("set of System.Byte", typeKind: BaseType.Set);
        }

        /// <summary>
        ///     test file types
        /// </summary>
        [TestMethod]
        public void TestFileTypes() {
            AssertDeclType("file", typeKind: BaseType.File);
            AssertDeclType("file of string[20]", typeKind: BaseType.File);
        }

        /// <summary>
        ///     test array types
        /// </summary>
        [TestMethod]
        public void TestArrayTypes() {

            ITypeDefinition GetIndexType(ArrayType array) {
                if (array == null)
                    return default;

                return array.IndexType;
            }

            BaseType GetIndexTypeKind(ArrayType array) {
                var t = GetIndexType(array);
                return t != null ? t.BaseType : BaseType.Unkown;
            };

            AssertDeclType("array [1..4] of Integer", typeKind: BaseType.Array);
            AssertDeclType("array [1..4] of Integer", (td) => Assert.AreEqual(BaseType.Integer, (td as ArrayType)?.BaseType));
            AssertDeclType("array [1..4] of Integer", (td) => Assert.AreEqual(BaseType.Integer, (GetIndexType(td as ArrayType) as SubrangeType)?.BaseType));
            AssertDeclType("array [false..true] of Integer", (td) => Assert.AreEqual(BaseType.Boolean, (GetIndexType(td as ArrayType) as SubrangeType)?.BaseType));
            AssertDeclType("array [Boolean] of Integer", (td) => Assert.AreEqual(BaseType.Boolean, GetIndexTypeKind(td as ArrayType)));
            AssertDeclType("array [System.Boolean] of Integer", (td) => Assert.AreEqual(BaseType.Boolean, GetIndexTypeKind(td as ArrayType)));
        }

        /// <summary>
        ///     test array types
        /// </summary>
        [TestMethod]
        public void TestArrayTypes2() {
            AssertDeclType("array [1..4, 1..4] of Integer", typeKind: BaseType.Array);
            AssertDeclType("array [1..4, 1..4] of Integer", (td) => Assert.AreEqual(BaseType.Array, (td as IArrayType)?.BaseType));
        }

        /// <summary>
        ///     test constant array declarations
        /// </summary>
        [TestMethod]
        public void TestConstantArrays() {
            var r = CreateEnvironment().TypeRegistry.SystemUnit;
            var tc = r.TypeRegistry.CreateTypeFactory(r);
            var at = tc.CreateStaticArrayType(r.CardinalType, string.Empty, r.IntegerType, false);
            AssertExprValue("a", GetArrayValue(at, r.CardinalType, GetIntegerValue(0x7E3), GetIntegerValue(0x81B), GetIntegerValue(0x819), GetIntegerValue(0x81A)), "const a: array [0..3] of Cardinal = ($000007E3, $0000081B, $00000819, $0000081A);");
        }

        /// <summary>
        ///     test declared symbols
        /// </summary>
        [TestMethod]
        public void TestDeclaredSymbols() {
            var kti = CreateEnvironment().TypeRegistry.SystemUnit;
            var p1 = "unit p; interface const A = 5; implementation procedure x; begin writeln(A); end; end;";
            var p2 = "unit p; interface implementation const A = 5; procedure x; begin writeln(A); end; end;";
            var p3 = "unit p; interface implementation procedure x; const A = 5; begin writeln(A); end; end;";
            var p4 = "unit p; interface type TA = class const A = 5; procedure X; end; implementation procedure TA.x; begin writeln(A); end; end;";
            AssertExprValue("", GetIntegerValue((sbyte)5), "", kti.UnspecifiedType, true, p1);
            AssertExprValue("", GetIntegerValue((sbyte)5), "", kti.UnspecifiedType, true, p2);
            AssertExprValue("", GetIntegerValue((sbyte)5), "", kti.UnspecifiedType, true, p3);
            AssertExprValue("", GetIntegerValue((sbyte)5), "", kti.UnspecifiedType, true, p4);
        }

    }
}
