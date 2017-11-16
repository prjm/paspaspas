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
        public void SimpleTests() {
            AssertDeclTypeDef("class end", typeKind: CommonTypeKind.ClassType);
            AssertDeclTypeDef<StructuredTypeDeclaration>("class end", (d) => d.BaseClass?.TypeId == TypeIds.TObject);
        }

    }
}
