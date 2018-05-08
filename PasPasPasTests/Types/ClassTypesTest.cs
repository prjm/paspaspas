using PasPasPas.Global.Runtime;
using PasPasPas.Global.Types;
using PasPasPas.Typings.Structured;
using Xunit;

namespace PasPasPasTests.Types {

    public class ClassTypesTest : TypeTest {

        [Fact]
        public void TestBasics() {
            AssertDeclTypeDef("class end", typeKind: CommonTypeKind.ClassType);
            AssertDeclTypeDef<StructuredTypeDeclaration>("class end", (d) => d.BaseClass?.TypeId == KnownTypeIds.TObject);
        }

        [Fact]
        public void TestMethodDeclaration() {
            AssertDeclTypeDef<StructuredTypeDeclaration>("class procedure x(); end", (d) => d.Methods[0].Parameters[0].Parameters == null);
            AssertDeclTypeDef<StructuredTypeDeclaration>("class function x(): String; end", (d) => d.Methods[0].Parameters[0].ResultType == KnownTypeIds.StringType);
            AssertDeclTypeDef<StructuredTypeDeclaration>("class procedure x(a: Integer): String; end", (d) => d.Methods[0].Parameters[0].Parameters[0]?.SymbolType == KnownTypeIds.IntegerType);
            AssertDeclTypeDef("class function z(): integer; end", "x.z()", typeKind: CommonTypeKind.IntegerType);
            AssertDeclTypeDef("class function x(): integer; end", "x.x()", typeKind: CommonTypeKind.IntegerType);
        }

        [Fact]
        public void TestFieldDeclaration() {
            AssertDeclTypeDef("class x: integer; end", "x.x", typeKind: CommonTypeKind.IntegerType);
            AssertDeclTypeDef("class k, l, x: integer; end", "x.x", typeKind: CommonTypeKind.IntegerType);
            AssertDeclTypeDef("class class var x: integer; end", "t.x", typeKind: CommonTypeKind.IntegerType);
        }

    }
}
