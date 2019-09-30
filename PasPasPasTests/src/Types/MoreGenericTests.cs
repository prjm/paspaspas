using System;
using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Globals.Types;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     generic tests
    /// </summary>
    public class MoreGenericTests : TypeTest {

        private void AssertDeclTypeDef<T>(string typeDecl, string expression, Func<T, bool> tester, string otherDecl = "") where T : class, ITypeDefinition {
            var file = "SimpleExpr";
            var program = $"program {file}; type {otherDecl} {typeDecl} = class procedure x; end; procedure {typeDecl}.X; begin WriteLn({expression}); end; begin end.";
            AssertDeclTypeDef<T>(program, file, NativeIntSize.Undefined, tester);
        }

        /*
        /// <summary>
        ///     test generic constraints
        /// </summary>
        [TestMethod]
        public void TestGenericConstraints() {
            bool t(IStructuredType s) => s.GenericParameters[0] == KnownTypeIds.UnconstrainedGenericTypeParameter;
            bool t1(IStructuredType s) => (s.TypeRegistry.GetTypeByIdOrUndefinedType(s.GenericParameters[0]) as IGenericTypeParameter).Constraints[0] == KnownTypeIds.GenericClassConstraint;
            bool t2(IStructuredType s) => (s.TypeRegistry.GetTypeByIdOrUndefinedType(s.GenericParameters[0]) as IGenericTypeParameter).Constraints[0] == KnownTypeIds.GenericRecordConstraint;
            bool t3(IStructuredType s) => (s.TypeRegistry.GetTypeByIdOrUndefinedType(s.GenericParameters[0]) as IGenericTypeParameter).Constraints[0] == KnownTypeIds.GenericConstructorConstraint;
            bool t4(IStructuredType s) {
                var c = (s.TypeRegistry.GetTypeByIdOrUndefinedType(s.GenericParameters[0]) as IGenericTypeParameter).Constraints[0];
                var c1 = s.TypeRegistry.GetTypeByIdOrUndefinedType(c);
                return c1.TypeKind == CommonTypeKind.MetaClassType;
            }

            AssertDeclTypeDef("T<A>", "Self", (Func<IStructuredType, bool>)t);
            AssertDeclTypeDef("T<A : class>", "Self", (Func<IStructuredType, bool>)t1);
            AssertDeclTypeDef("T<A : record>", "Self", (Func<IStructuredType, bool>)t2);
            AssertDeclTypeDef("T<A : constructor>", "Self", (Func<IStructuredType, bool>)t3);
            AssertDeclTypeDef("T<A : TA>", "Self", (Func<IStructuredType, bool>)t4, "TA = class end; ");
        }
        */
        /*
        /// <summary>
        ///     test generic constraints
        /// </summary>
        [TestMethod]
        public void TestGenericConstraints2() {
            T r<T>(IStructuredType s, int? i) where T : class
                => s.TypeRegistry.GetTypeByIdOrUndefinedType(i ?? KnownTypeIds.ErrorType) as T;

            bool t(IStructuredType s)
                => r<IExtensibleGenericType>(s, r<StructuredTypeDeclaration>(s, s.BaseClassId)?.Methods[0]?.TypeId) != default;

            AssertDeclTypeDef("TB", "TA<TB>.A()", (Func<IStructuredType, bool>)t, "TA = class procedure A<T>; end; ");
        }

        /// <summary>
        ///     test generic parameter
        /// </summary>
        [TestMethod]
        public void TestUnconstrainedGenericParameter() {
            bool t(IStructuredType s) => s.GenericParameters[0] == s.TypeRegistry.GetTypeByIdOrUndefinedType(KnownTypeIds.UnconstrainedGenericTypeParameter).TypeId;
            AssertDeclTypeDef("T<A>", "Self", (Func<IStructuredType, bool>)t);
        }
        */
    }
}
