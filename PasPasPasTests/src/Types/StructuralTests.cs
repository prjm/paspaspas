﻿#nullable disable
using System;
using System.Linq;
using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Globals.Types;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test for structural properties
    /// </summary>
    public class StructuralTests : TypeTest {

        private void AssertExprTypeInProc(string proc, string expression, string typeName = "", string decls = "", ITypeDefinition typeId = default, string proc2 = "") {
            var file = "SimpleExpr";
            var program = $"program {file};{decls} {proc} begin writeln({expression}); end; {proc2} begin end. ";
            AssertExprType(file, program, typeId, false, typeName);
        }

        private enum AccessModifierTestMode {
            InType, InDerivedType, InExternalType
        }

        private ISystemUnit KnownTypeIds
            => CreateEnvironment().TypeRegistry.SystemUnit;

        private void AssertTypeForAccessModifier(string modifier, string decl, string expression, AccessModifierTestMode mode, ITypeDefinition typeId, bool classFunction = false) {
            var file = "SimpleExpr";
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

        /// <summary>
        ///     test result definitions
        /// </summary>
        [TestMethod]
        public void TestResultDef() {
            AssertExprTypeInProc("function a: Integer;", "a", typeId: KnownTypeIds.IntegerType);
            AssertExprTypeInProc("type x = class function a: string; end; function x.a: string;", "Result", typeId: KnownTypeIds.StringType);
            AssertExprTypeInProc("function a: string;", "4", typeId: KnownTypeIds.ShortIntType);
            AssertExprTypeInProc("function a: string;", "Result", typeId: KnownTypeIds.StringType);
            AssertExprTypeInProc("function a: string;", "a", typeId: KnownTypeIds.StringType);
            AssertExprTypeInProc("function a: Integer; function b: string; ", "Result", "", "", KnownTypeIds.StringType, " begin end; ");
            AssertExprTypeInProc("function a: Integer; function b: string; ", "b", "", "", KnownTypeIds.StringType, " begin end; ");
            AssertExprTypeInProc("function a: Integer; function b: string; ", "a", "", "", KnownTypeIds.IntegerType, " begin end; ");
        }

        /// <summary>
        ///     test method parameters
        /// </summary>
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

        /// <summary>
        ///     test self pointer
        /// </summary>
        [TestMethod]
        public void TestSelfPointer() {
            var e = CreateEnvironment();
            var ct = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var c = ct.CreateStructuredType("x", StructuredTypeKind.Class);
            AssertExprTypeInProc("type x = class var z: string; procedure b; end; procedure x.b;", "Self", typeId: c);
            AssertExprTypeInProc("type x = class var z: string; procedure b; end; procedure x.b;", "Self.z", typeId: KnownTypeIds.StringType);
            AssertExprTypeInProc("type x = class var z: string; procedure b; end; procedure x.b;", "z", typeId: KnownTypeIds.StringType);
            AssertExprTypeInProc("type x = class class var z: string; procedure b; end; procedure x.b;", "Self.z", typeId: KnownTypeIds.StringType);
            AssertExprTypeInProc("type x = class class var z: string; procedure b; end; procedure x.b;", "z", typeId: KnownTypeIds.StringType);
            AssertExprTypeInProc("type x = class var z: string; class procedure b; end; class procedure x.b;", "Self.z", typeId: KnownTypeIds.ErrorType);
            AssertExprTypeInProc("type x = class var z: string; class procedure b; end; class procedure x.b;", "z", typeId: KnownTypeIds.ErrorType);
            AssertExprTypeInProc("type x = class class var z: string; class procedure b; end; class procedure x.b;", "Self.z", typeId: KnownTypeIds.StringType);
            AssertExprTypeInProc("type x = class class var z: string; class procedure b; end; class procedure x.b;", "z", typeId: KnownTypeIds.StringType);
        }

        /// <summary>
        ///     test visibility for private members
        /// </summary>
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

        /// <summary>
        ///     test calls to inherited methods
        /// </summary>
        [TestMethod]
        public void TestInherited() {
            var o = AccessModifierTestMode.InDerivedType;
            AssertTypeForAccessModifier(string.Empty, "function f: integer;", "inherited", o, KnownTypeIds.IntegerType);
            AssertTypeForAccessModifier(string.Empty, "function f: integer;", "inherited", o, KnownTypeIds.IntegerType, true);
        }

        /// <summary>
        ///     test protected visibility of members
        /// </summary>
        [TestMethod]
        public void TestVisibilityProtected() {
            var i = AccessModifierTestMode.InType;
            var o = AccessModifierTestMode.InDerivedType;
            var u = AccessModifierTestMode.InExternalType;

            AssertTypeForAccessModifier("protected", "f: integer;", "f", o, KnownTypeIds.IntegerType);
            AssertTypeForAccessModifier("protected", "f: integer;", "f", i, KnownTypeIds.IntegerType);
            AssertTypeForAccessModifier("protected", "f: integer", "f", u, KnownTypeIds.IntegerType);

            AssertTypeForAccessModifier("strict protected", "f: integer;", "f", o, KnownTypeIds.IntegerType);
            AssertTypeForAccessModifier("strict protected", "f: integer;", "f", i, KnownTypeIds.IntegerType);
            AssertTypeForAccessModifier("strict protected", "f: integer", "f", u, KnownTypeIds.ErrorType);
        }

        /*
        [TestMethod]
        public void TestForwardDeclarations() {
            AssertExprTypeInProc("function a: Integer; begin Result := 5; end; procedure b; ", "a", typeId: KnownTypeIds.IntegerType);
            AssertExprTypeInProc("function a: integer; forward; function a: Integer; begin Result := 5; end; procedure b; ", "a", typeId: KnownTypeIds.IntegerType);
            AssertExprTypeInProc("function a: integer; forward; function a: integer; forward; function a: Integer; begin Result := 5; end; procedure b; ", "a", typeId: KnownTypeIds.IntegerType);
        }
        */

        /// <summary>
        ///     test global methods
        /// </summary>
        [TestMethod]
        public void TestGlobalMethod() {
            var f = "SimpleExpr";
            var p = $"unit {f}; interface procedure a; implementation procedure a; begin WriteLn(x); end; end.";
            bool v(IErrorType e) => (e.DefiningUnit.TypeRegistry.Units.Where(t => string.Equals(f, t.Name, StringComparison.OrdinalIgnoreCase)) as IUnitType)?.Symbols.Any(t => t.Name == "a") ?? false;

            AssertDeclTypeDef<IErrorType>(program: p, f, NativeIntSize.Undefined, v);
        }

        /// <summary>
        ///     test global method invocation
        /// </summary>
        [TestMethod]
        public void TestGlobalMethodInvocation() {
            var p = "WriteLn('a')";
            AssertStatementType(p, default, kind: SymbolTypeKind.IntrinsicRoutineResult);
        }

    }
}
