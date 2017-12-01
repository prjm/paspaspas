using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;
using PasPasPas.Typings.Structured;
using Xunit;

namespace PasPasPasTests.Types {

    public class ClassTypesTest : TypeTest {

        [Fact]
        public void TestBasics() {
            AssertDeclTypeDef("class end", typeKind: CommonTypeKind.ClassType);
            AssertDeclTypeDef<StructuredTypeDeclaration>("class end", (d) => d.BaseClass?.TypeId == TypeIds.TObject);
        }

        [Fact]
        public void TestMethodDeclaration() {
            AssertDeclTypeDef<StructuredTypeDeclaration>("class procedure x(); end", (d) => d.Methods[0].Parameters[0].Parameters == null);
            AssertDeclTypeDef<StructuredTypeDeclaration>("class function x(): String; end", (d) => d.Methods[0].Parameters[0].ResultType?.TypeId == TypeIds.StringType);
            AssertDeclTypeDef<StructuredTypeDeclaration>("class procedure x(a: Integer): String; end", (d) => d.Methods[0].Parameters[0].Parameters[0]?.SymbolType.TypeId == TypeIds.IntegerType);
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
