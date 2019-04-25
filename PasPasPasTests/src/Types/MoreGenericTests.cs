using System;
using PasPasPas.Globals.Types;
using PasPasPas.Options.DataTypes;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {


    public class MoreGenericTests : TypeTest {

        private void AssertDeclTypeDef<T>(string typeDecl, string expression, Func<T, bool> tester) where T : class, ITypeDefinition {
            var file = "SimpleExpr";
            var program = $"program {file}; type {typeDecl} = class procedure x; end; procedure {typeDecl}.X; begin WriteLn({expression}); end; begin end.";
            AssertDeclTypeDef<T>(program, file, NativeIntSize.Undefined, tester);
        }

        [TestMethod]
        public void TestGenericConstraints() {
            Func<IStructuredType, bool> t = (IStructuredType s) => s.GenericParameters[0] == KnownTypeIds.UnconstrainedGenericTypeParameter;
            Func<IStructuredType, bool> t1 = (IStructuredType s) => (s.TypeRegistry.GetTypeByIdOrUndefinedType(s.GenericParameters[0]) as IGenericTypeParameter).Constraints[0] == KnownTypeIds.GenericClassConstraint;
            Func<IStructuredType, bool> t2 = (IStructuredType s) => (s.TypeRegistry.GetTypeByIdOrUndefinedType(s.GenericParameters[0]) as IGenericTypeParameter).Constraints[0] == KnownTypeIds.GenericRecordConstraint;
            Func<IStructuredType, bool> t3 = (IStructuredType s) => (s.TypeRegistry.GetTypeByIdOrUndefinedType(s.GenericParameters[0]) as IGenericTypeParameter).Constraints[0] == KnownTypeIds.GenericConstructorConstraint;
            AssertDeclTypeDef("T<A>", "Self", t);
            AssertDeclTypeDef("T<A : class>", "Self", t1);
            AssertDeclTypeDef("T<A : record>", "Self", t2);
            AssertDeclTypeDef("T<A : constructor>", "Self", t3);
        }

    }
}
