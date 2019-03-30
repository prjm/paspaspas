using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Structured;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    public class ClassTypesTest : TypeTest {

        [TestMethod]
        public void TestBasics() {
            AssertDeclTypeDef("class end", typeKind: CommonTypeKind.ClassType);
            AssertDeclTypeDef<StructuredTypeDeclaration>("class end", (d) => d.BaseClass?.TypeId == KnownTypeIds.TClass);
        }

        [TestMethod]
        public void TestMethodDeclaration() {
            AssertDeclTypeDef<StructuredTypeDeclaration>("class procedure x(); end", (d) => d.Methods[0].Parameters[0].Parameters == null);
            AssertDeclTypeDef<StructuredTypeDeclaration>("class function x(): String; end", (d) => d.Methods[0].Parameters[0].ResultType?.TypeId == KnownTypeIds.StringType);
            AssertDeclTypeDef<StructuredTypeDeclaration>("class procedure x(a: Integer): String; end", (d) => d.Methods[0].Parameters[0].Parameters[0]?.SymbolType?.TypeId == KnownTypeIds.IntegerType);
            AssertDeclTypeDef("class function z(): integer; end", "x.z()", typeKind: CommonTypeKind.IntegerType);
            AssertDeclTypeDef("class function x(): integer; end", "x.x()", typeKind: CommonTypeKind.IntegerType);
        }

        [TestMethod]
        public void TestFieldDeclaration() {
            AssertDeclTypeDef("class x: integer; end", "x.x", typeKind: CommonTypeKind.IntegerType);
            AssertDeclTypeDef("class k, l, x: integer; end", "x.x", typeKind: CommonTypeKind.IntegerType);
            AssertDeclTypeDef("class class var x: integer; end", "t.x", typeKind: CommonTypeKind.IntegerType);
        }

        [TestMethod]
        public void TestClassOf() {
            AssertDeclTypeDef("class end; y = class of t", "y", typeKind: CommonTypeKind.MetaClassType);
            AssertDeclTypeDef("class end; y = class of t; z = class of y", "z", KnownTypeIds.ErrorType);
        }

    }
}
