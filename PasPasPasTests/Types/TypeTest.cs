using System;
using PasPasPas.Api;
using PasPasPas.Global.Runtime;
using PasPasPas.Global.Types;
using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Typings.Common;
using PasPasPasTests.Common;
using PasPasPasTests.Parser;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     base class for type tests
    /// </summary>
    public class TypeTest : CommonTest {

        /// <summary>
        ///     test the type of a given expression
        /// </summary>
        /// <param name="commonType">common type</param>
        /// <param name="expression">expression</param>
        /// <param name="typeId">type id to find</param>
        protected void AssertExprTypeByVar(string commonType, string expression, int typeId, bool resolveSubrange = false, string decls = "") {
            var file = "SimpleExpr";
            var program = $"program {file};{decls} var a,b: {commonType}; begin WriteLn({expression}); end. ";

            SymbolReferencePart searchfunction(object x)
                => (x is SymbolReferencePart srp) && srp.Kind == SymbolReferencePartKind.CallParameters ? x as SymbolReferencePart : null;
            ;

            IExpression firstParam = null;

            firstParam = EvaluateExpressionType(file, program, searchfunction, NativeIntSize.Undefined, out var env) as IExpression;

            Assert.IsNotNull(firstParam);
            Assert.IsNotNull(firstParam.TypeInfo);
            var foundTypeId = default(int);

            if (resolveSubrange && env.TypeRegistry.GetTypeByIdOrUndefinedType(firstParam.TypeInfo.TypeId) is PasPasPas.Typings.Simple.SubrangeType sr) {
                foundTypeId = sr.BaseType.TypeId;
            }
            else {
                foundTypeId = firstParam.TypeInfo.TypeId;
            }

            Assert.AreEqual(typeId, foundTypeId);

        }


        /// <summary>
        ///     test the type of a given expression
        /// </summary>
        /// <param name="expression">expression</param>
        /// <param name="typeId">type id to find</param>
        protected void AssertExprType(string expression, int typeId) {
            var file = "SimpleExpr";
            var program = $"program {file}; begin Writeln({expression}); end. ";
            SymbolReferencePart searchfunction(object x) => x as SymbolReferencePart;
            IExpression firstParam = null;

            firstParam = EvaluateExpressionType(file, program, searchfunction, NativeIntSize.Undefined, out var env) as IExpression;

            Assert.IsNotNull(firstParam);
            Assert.IsNotNull(firstParam.TypeInfo);
            Assert.AreEqual(typeId, firstParam.TypeInfo.TypeId);

        }

        /// <summary>
        ///     test the value of a given expression
        /// </summary>
        /// <param name="expression">expression</param>
        /// <param name="typeId">type id to find</param>
        /// <param name="decls">addition declarations</param>
        protected void AssertExprValue(string expression, ITypeReference value, string decls = "") {
            var file = "SimpleExpr";
            var program = $"program {file};{decls} begin Writeln({expression}); end. ";
            SymbolReferencePart searchfunction(object x) => x as SymbolReferencePart;
            IExpression firstParam = null;

            firstParam = EvaluateExpressionType(file, program, searchfunction, NativeIntSize.Undefined, out var env) as IExpression;

            Assert.IsNotNull(firstParam);
            Assert.IsNotNull(firstParam.TypeInfo);
            Assert.IsTrue(firstParam.TypeInfo.IsConstant);
            Assert.AreEqual(value, firstParam.TypeInfo);

        }


        /// <summary>
        ///     test the type of a declared variable expression
        /// </summary>
        /// <param name="declaration">declaration</param>
        /// <param name="typeId">type id to find</param>
        protected void AssertDeclType(string declaration, int typeId = KnownTypeIds.UnspecifiedType, NativeIntSize intSize = NativeIntSize.Undefined, int typeSize = -1, CommonTypeKind typeKind = CommonTypeKind.UnknownType) {

            void tester(ITypeDefinition def) {

                if (typeId != KnownTypeIds.UnspecifiedType)
                    Assert.AreEqual(typeId, def.TypeId);

                if (typeKind != CommonTypeKind.UnknownType)
                    Assert.AreEqual(typeKind, def.TypeKind);

                if (typeSize > 0) {
                    IFixedSizeType sizedType;
                    if (def is PasPasPas.Typings.Common.TypeAlias alias) {
                        sizedType = alias.BaseType as IFixedSizeType;
                    }
                    else {
                        sizedType = def as IFixedSizeType;
                    }
                    Assert.IsNotNull(sizedType);
                    Assert.AreEqual(typeSize, sizedType.BitSize);
                }

            }

            AssertDeclType(declaration, tester, intSize);
        }
        /// <summary>
        ///     test the type of a declared variable expression
        /// </summary>
        /// <param name="declaration">declaration</param>
        /// <param name="typeId">type id to find</param>
        protected void AssertDeclTypeDef(string declaration, string expression = "x", int typeId = KnownTypeIds.UnspecifiedType, NativeIntSize intSize = NativeIntSize.Undefined, int typeSize = -1, CommonTypeKind typeKind = CommonTypeKind.UnknownType) {

            bool tester(ITypeDefinition def) {

                if (typeId != KnownTypeIds.UnspecifiedType)
                    Assert.AreEqual(typeId, def.TypeId);

                if (typeKind != CommonTypeKind.UnknownType)
                    Assert.AreEqual(typeKind, def.TypeKind);

                if (typeSize > 0) {
                    IFixedSizeType sizedType;
                    if (def is PasPasPas.Typings.Common.TypeAlias alias) {
                        sizedType = alias.BaseType as IFixedSizeType;
                    }
                    else {
                        sizedType = def as IFixedSizeType;
                    }
                    Assert.IsNotNull(sizedType);
                    Assert.AreEqual(typeSize, sizedType.BitSize);
                }

                return true;
            }

            AssertDeclTypeDef<ITypeDefinition>(declaration, tester, intSize, expression);
        }

        /// <summary>
        ///     test the type of a declared variable expression
        /// </summary>
        /// <param name="declaration">declaration</param>
        protected void AssertDeclTypeDef<T>(string declaration, Func<T, bool> test, NativeIntSize intSize = NativeIntSize.Undefined, string expression = "x") where T : class, ITypeDefinition {
            var file = "SimpleExpr";
            var program = $"program {file}; type t = {declaration}; var x : t; begin Writeln({expression}); end. ";
            SymbolReferencePart searchfunction(object x) => x as SymbolReferencePart;
            IExpression firstParam = null;

            firstParam = EvaluateExpressionType(file, program, searchfunction, intSize, out var env) as IExpression;

            var v = firstParam.TypeInfo;
            Assert.IsNotNull(v);
            var t = env.TypeRegistry.GetTypeByIdOrUndefinedType(v.TypeId) as T;
            Assert.IsNotNull(t);
            Assert.IsTrue(test(t));
        }

        protected void AssertAssignmentCompat(string assignTo, string assignFrom, bool compat = true, string predeclaration = "") {
            bool test(ITypeDefinition l, ITypeDefinition r) {
                Assert.IsNotNull(l);
                Assert.IsNotNull(r);
                var canBeAssigned = l.CanBeAssignedFrom(r);
                return (compat && canBeAssigned) || (!compat && !canBeAssigned);
            }

            AssertTestForAssignment(assignTo, assignFrom, test, predeclaration);
        }

        protected void AssertTestForAssignment(string assignTo, string assignFrom, Func<ITypeDefinition, ITypeDefinition, bool> test, string predeclaration = "") {
            var typdef = $"{predeclaration} ta = {assignTo}; tb = {assignFrom}";
            var declaration = "a: ta; b: tb";
            var statement = "a := b";
            AssertTestForGenericStatement(typdef, declaration, statement, test);
        }

        protected void AssertTestForGenericStatement(string typedef, string declaration, string statement, Func<ITypeDefinition, ITypeDefinition, bool> test) {
            var file = "SimpleExpr";
            var program = $"program {file}; type {typedef}; var {declaration}; begin {statement}; end. ";
            StructuredStatement searchfunction(object x) => x as StructuredStatement;
            ISyntaxPart firstParam = null;

            firstParam = EvaluateExpressionType(file, program, searchfunction, NativeIntSize.All32bit, out var env);

            Assert.IsNotNull(firstParam);
            var t = firstParam as StructuredStatement;
            var l = t != null && t.Expressions.Count > 0 ? t.Expressions[0].TypeInfo : null;
            var r = t != null && t.Expressions.Count > 1 ? t.Expressions[1].TypeInfo : null;
            Assert.IsNotNull(t);

            var lt = env.TypeRegistry.GetTypeByIdOrUndefinedType(l.TypeId);
            var rt = env.TypeRegistry.GetTypeByIdOrUndefinedType(r.TypeId);

            Assert.IsTrue(test(lt, rt));
        }

        /// <summary>
        ///     test the type of a declared types
        /// </summary>
        /// <param name="declaration">declaration</param>
        protected void AssertDeclType(string declaration, Action<ITypeDefinition> test, NativeIntSize intSize = NativeIntSize.Undefined) {
            var file = "SimpleExpr";
            var program = $"program {file}; var x : {declaration}; begin Writeln(x); end. ";
            SymbolReferencePart searchfunction(object x) => x is SymbolReferencePart srp && srp.Kind == SymbolReferencePartKind.CallParameters ? srp : null;
            IExpression firstParam = null;

            firstParam = EvaluateExpressionType(file, program, searchfunction, intSize, out var env) as IExpression;

            Assert.IsNotNull(firstParam.TypeInfo);
            var ti = env.TypeRegistry.GetTypeByIdOrUndefinedType(firstParam.TypeInfo.TypeId);
            test(ti);
        }


        private ISyntaxPart EvaluateExpressionType<T>(string file, string program, Func<object, T> searchfunction, NativeIntSize intSize, out ITypedEnvironment env) where T : ISyntaxPart {
            IExpression firstParam;

            env = CreateEnvironment(intSize);
            var api = new ParserApi(env);
            using (var reader = api.CreateParserForString($"{file}.dpr", program)) {

                api.Options.Meta.NativeIntegerSize.Value = intSize;

                var tree = reader.Parse();
                var project = api.CreateAbstractSyntraxTree(tree);
                api.AnnotateWithTypes(project);

                var astVisitor = new AstVisitor<T>() { SearchFunction = searchfunction };
                project.Accept(astVisitor.AsVisitor());

                Assert.IsNotNull(astVisitor.Result);

                if (astVisitor.Result is SymbolReferencePart srp) {
                    Assert.AreEqual(1, srp.Expressions.Count);
                    firstParam = srp.Expressions[0];
                    return firstParam;
                }

                return astVisitor.Result;
            }
        }
    }
}