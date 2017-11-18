using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;
using Xunit;

namespace PasPasPasTests.Types {
    public class ClassTypesTest : TypeTest {

        [Fact]
        public void TestBasics() {
            AssertDeclTypeDef("class end", typeKind: CommonTypeKind.ClassType);
            AssertDeclTypeDef<StructuredTypeDeclaration>("class end", (d) => d.BaseClass?.TypeId == TypeIds.TObject);
        }

        [Fact]
        public void TestMethodDeclaraion() {
            AssertDeclTypeDef<StructuredTypeDeclaration>("class procedure x(); end", (d) => d.Methods[0].Parameters[0].Parameters.Count == 0);
            AssertDeclTypeDef<StructuredTypeDeclaration>("class procedure x(); end", (d) => d.Methods[0].Parameters[0].ResultType == null);
        }

    }
}
