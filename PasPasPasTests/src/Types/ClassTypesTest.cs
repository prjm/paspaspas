using PasPasPas.Globals.Types;
using PasPasPas.Typings.Structured;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test properties of class types
    /// </summary>
    public class ClassTypesTest : TypeTest {

        /// <summary>
        ///     test basics
        /// </summary>
        [TestMethod]
        public void TestBasics() {
            AssertDeclTypeDef("class end", typeKind: BaseType.Structured);
            AssertDeclTypeDef<StructuredTypeDeclaration>("class end", (d) => d.BaseClass == d.DefiningUnit.TypeRegistry.SystemUnit.TObjectType);
        }

        /// <summary>
        ///     test method declarations
        /// </summary>
        [TestMethod]
        public void TestMethodDeclaration() {
            AssertDeclTypeDef<StructuredTypeDeclaration>("class procedure x(); end", (d) => d.Methods[0].Items[0].Parameters == null);
            AssertDeclTypeDef<StructuredTypeDeclaration>("class function x(): String; end", (d) => d.Methods[0].Items[0].ResultType?.TypeDefinition == d.TypeRegistry.SystemUnit.StringType);
            AssertDeclTypeDef<StructuredTypeDeclaration>("class procedure x(a: Integer): String; end", (d) => d.Methods[0].Items[0].Parameters[0]?.TypeDefinition == d.TypeRegistry.SystemUnit.IntegerType);
            AssertDeclTypeDef("class function z(): integer; end", "x.z()", typeKind: BaseType.Integer);
            AssertDeclTypeDef("class function x(): integer; end", "x.x()", typeKind: BaseType.Integer);
        }

        /// <summary>
        ///     test field declarations
        /// </summary>
        [TestMethod]
        public void TestFieldDeclaration() {
            AssertDeclTypeDef("class x: integer; end", "x.x", typeKind: BaseType.Integer);
            AssertDeclTypeDef("class k, l, x: integer; end", "x.x", typeKind: BaseType.Integer);
            AssertDeclTypeDef("class class var x: integer; end", "t.x", typeKind: BaseType.Integer);
        }

        /// <summary>
        ///     test class of operator
        /// </summary>
        [TestMethod]
        public void TestClassOf() {
            AssertDeclTypeDef("class end; y = class of t", "y", typeKind: BaseType.MetaClass);
            AssertDeclTypeDef("class end; y = class of t; z = class of y", "z", typeKind: BaseType.Error);
        }

    }
}
