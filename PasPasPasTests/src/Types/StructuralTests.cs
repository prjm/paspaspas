using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test for structural properties
    /// </summary>
    public class StructuralTests : TypeTest {

        private void AssertExprTypeInProc(string proc, string expression, string typeName = "", string decls = "", int typeId = KnownTypeIds.ErrorType, string proc2 = "") {
            var file = "SimpleExpr";
            var program = $"program {file};{decls} {proc} begin writeln({expression}); end; {proc2} begin end. ";
            AssertExprType(file, program, typeId, false, typeName);
        }

        private enum AccessModifierTestMode {
            InType, InDerivedType, InExternalType
        }

        private void AssertTypeForAccessModifier(string modifier, string decl, string expression, AccessModifierTestMode mode, int typeId, bool classFunction = false) {
            var file = "SimpleExpr";
            var decl0 = string.Empty;
            var decl1 = string.Empty;
            var decl2 = string.Empty;
            var cf = classFunction ? " class " : string.Empty;

            switch (mode) {
                case AccessModifierTestMode.InType: {
                        decl1 = cf + "function x: Integer;";
                        decl2 = cf + "function ta.x: Integer; begin ";
                        break;
                    }

                case AccessModifierTestMode.InDerivedType: {
                        decl1 = cf + "function x: Integer; virtual; end; tb = class(ta) " + cf + " function x: Integer; override;";
                        decl2 = cf + "function ta.x: integer; begin Result := 5; end; " + cf + " function tb.x: Integer; begin ";
                        break;
                    }

                case AccessModifierTestMode.InExternalType: {
                        decl2 = " procedure x; var a: ta; begin ";
                        expression = "a." + expression;
                        break;
                    }

            }

            var decls = $"type ta = class {modifier} {decl} {decl1} end; {decl2} writeln({expression}); end; ";
            var program = $"program {file}; {decls} begin end. ";
            AssertExprType(file, program, typeId, false, string.Empty);
        }


        [TestMethod]
        public void TestResultDef() {
            AssertExprTypeInProc("function a: Integer;", "a", typeId: KnownTypeIds.IntegerType);
            AssertExprTypeInProc("type x = class function a: string; end; function x.a: string;", "Result", typeId: KnownTypeIds.StringType);
            AssertExprTypeInProc("function a: string;", "4", typeId: KnownTypeIds.ShortInt);
            AssertExprTypeInProc("function a: string;", "Result", typeId: KnownTypeIds.StringType);
            AssertExprTypeInProc("function a: string;", "a", typeId: KnownTypeIds.StringType);
            AssertExprTypeInProc("function a: Integer; function b: string; ", "Result", "", "", KnownTypeIds.StringType, " begin end; ");
            AssertExprTypeInProc("function a: Integer; function b: string; ", "b", "", "", KnownTypeIds.StringType, " begin end; ");
            AssertExprTypeInProc("function a: Integer; function b: string; ", "a", "", "", KnownTypeIds.IntegerType, " begin end; ");
        }

        [TestMethod]
        public void TestMethodParams() {
            AssertExprTypeInProc("procedure b(a: Integer);", "a", typeId: KnownTypeIds.IntegerType);
            AssertExprTypeInProc("procedure b(a, c: Integer);", "a", typeId: KnownTypeIds.IntegerType);
            AssertExprTypeInProc("procedure b(a, c: Integer);", "c", typeId: KnownTypeIds.IntegerType);
            AssertExprTypeInProc("function b(a: Integer): string;", "a", typeId: KnownTypeIds.IntegerType);
            AssertExprTypeInProc("type x = class function a(b: Integer): string; end; function x.a(b: Integer): string;", "b", typeId: KnownTypeIds.IntegerType);
            AssertExprTypeInProc("type x = class procedure a(b: Integer); end; procedure x.a;", "b", typeId: KnownTypeIds.IntegerType);
            AssertExprTypeInProc("type x = class function a(b: Integer): string; end; function x.a;", "b", typeId: KnownTypeIds.IntegerType);
        }

        [TestMethod]
        public void TestSelfPointer() {
            AssertExprTypeInProc("type x = class var z: string; procedure b; end; procedure x.b;", "Self", typeId: RegisteredTypes.SmallestUserTypeId);
            AssertExprTypeInProc("type x = class var z: string; procedure b; end; procedure x.b;", "Self.z", typeId: KnownTypeIds.StringType);
            AssertExprTypeInProc("type x = class var z: string; procedure b; end; procedure x.b;", "z", typeId: KnownTypeIds.StringType);
            AssertExprTypeInProc("type x = class class var z: string; procedure b; end; procedure x.b;", "Self.z", typeId: KnownTypeIds.StringType);
            AssertExprTypeInProc("type x = class class var z: string; procedure b; end; procedure x.b;", "z", typeId: KnownTypeIds.StringType);
            AssertExprTypeInProc("type x = class var z: string; class procedure b; end; class procedure x.b;", "Self.z", typeId: KnownTypeIds.ErrorType);
            AssertExprTypeInProc("type x = class var z: string; class procedure b; end; class procedure x.b;", "z", typeId: KnownTypeIds.ErrorType);
            AssertExprTypeInProc("type x = class class var z: string; class procedure b; end; class procedure x.b;", "Self.z", typeId: KnownTypeIds.StringType);
            AssertExprTypeInProc("type x = class class var z: string; class procedure b; end; class procedure x.b;", "z", typeId: KnownTypeIds.StringType);
        }

        [TestMethod]
        public void TestVisibilityPrivate() {
            var i = AccessModifierTestMode.InType;
            var o = AccessModifierTestMode.InDerivedType;
            var u = AccessModifierTestMode.InExternalType;

            AssertTypeForAccessModifier("private", "f: integer;", "f", i, KnownTypeIds.IntegerType);
            AssertTypeForAccessModifier("private", "f: integer;", "f", o, KnownTypeIds.IntegerType);
            AssertTypeForAccessModifier("private", "f: integer;", "f", u, KnownTypeIds.IntegerType);

            AssertTypeForAccessModifier("strict private", "f: integer;", "f", i, KnownTypeIds.IntegerType);
            AssertTypeForAccessModifier("strict private", "f: integer;", "f", o, KnownTypeIds.ErrorType);
            AssertTypeForAccessModifier("strict private", "f: integer;", "f", u, KnownTypeIds.ErrorType);
        }

        [TestMethod]
        public void TestInherited() {
            var o = AccessModifierTestMode.InDerivedType;
            AssertTypeForAccessModifier(string.Empty, "function f: integer;", "inherited", o, KnownTypeIds.IntegerType);
            AssertTypeForAccessModifier(string.Empty, "function f: integer;", "inherited", o, KnownTypeIds.IntegerType, true);
        }

        [TestMethod]
        public void TestVisibilityProtected() {
            var i = AccessModifierTestMode.InType;
            var o = AccessModifierTestMode.InDerivedType;
            AssertTypeForAccessModifier("protected", "f: integer;", "f", o, KnownTypeIds.IntegerType);
            AssertTypeForAccessModifier("protected", "f: integer;", "f", i, KnownTypeIds.IntegerType);
            //AssertTypeForAccessModifier("protected", "f: integer", "f", u, KnownTypeIds.ErrorType);
        }

    }
}
