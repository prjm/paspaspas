using System;
using PasPasPas.Api;
using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;
using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;
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
        /// <param name="expression">expression</param>
        /// <param name="typeId">type id to find</param>
        protected void AssertExprType(string expression, int typeId) {
            var file = "SimpleExpr";
            var program = $"program {file}; begin Writeln({expression}); end. ";
            SymbolReferencePart searchfunction(object x) => x as SymbolReferencePart;
            IExpression firstParam = null;

            firstParam = EvaluateExpressionType(file, program, searchfunction, NativeIntSize.Undefined) as IExpression;

            Assert.IsNotNull(firstParam);
            Assert.IsNotNull(firstParam.TypeInfo);
            Assert.AreEqual(typeId, firstParam.TypeInfo.TypeId);

        }

        /// <summary>
        ///     test the value of a given expressiom
        /// </summary>
        /// <param name="expression">expression</param>
        /// <param name="typeId">type id to find</param>
        protected void AssertExprValue(string expression, IValue value) {
            var file = "SimpleExpr";
            var program = $"program {file}; begin Writeln({expression}); end. ";
            SymbolReferencePart searchfunction(object x) => x as SymbolReferencePart;
            IExpression firstParam = null;

            firstParam = EvaluateExpressionType(file, program, searchfunction, NativeIntSize.Undefined) as IExpression;

            Assert.IsNotNull(firstParam);
            Assert.IsNotNull(firstParam.LiteralValue);
            Assert.IsNotNull(firstParam.TypeInfo);
            Assert.IsTrue(firstParam.IsConstant);
            Assert.AreEqual(value, firstParam.LiteralValue);

        }


        /// <summary>
        ///     test the type of a declared variable expressiom
        /// </summary>
        /// <param name="declaration">declareation</param>
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
        ///     test the type of a declared variable expressiom
        /// </summary>
        /// <param name="declaration">declareation</param>
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
        ///     test the type of a declared variable expressiom
        /// </summary>
        /// <param name="declaration">declareation</param>
        protected void AssertDeclTypeDef<T>(string declaration, Func<T, bool> test, NativeIntSize intSize = NativeIntSize.Undefined, string expression = "x") where T : class, ITypeDefinition {
            var file = "SimpleExpr";
            var program = $"program {file}; type t = {declaration}; var x : t; begin Writeln({expression}); end. ";
            SymbolReferencePart searchfunction(object x) => x as SymbolReferencePart;
            IExpression firstParam = null;

            firstParam = EvaluateExpressionType<SymbolReferencePart>(file, program, searchfunction, intSize) as IExpression;

            var t = firstParam.TypeInfo as T;
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

            firstParam = EvaluateExpressionType(file, program, searchfunction, NativeIntSize.All32bit);

            Assert.IsNotNull(firstParam);
            var t = firstParam as StructuredStatement;
            var l = t != null && t.Expressions.Count > 0 ? t.Expressions[0].TypeInfo : null;
            var r = t != null && t.Expressions.Count > 1 ? t.Expressions[1].TypeInfo : null;
            Assert.IsNotNull(t);
            Assert.IsTrue(test(l, r));

        }

        /// <summary>
        ///     test the type of a declared types
        /// </summary>
        /// <param name="declaration">declareation</param>
        protected void AssertDeclType(string declaration, Action<ITypeDefinition> test, NativeIntSize intSize = NativeIntSize.Undefined) {
            var file = "SimpleExpr";
            var program = $"program {file}; var x : {declaration}; begin Writeln(x); end. ";
            SymbolReferencePart searchfunction(object x) => x is SymbolReferencePart srp && srp.Kind == SymbolReferencePartKind.CallParameters ? srp : null;
            IExpression firstParam = null;

            firstParam = EvaluateExpressionType(file, program, searchfunction, intSize) as IExpression;

            Assert.IsNotNull(firstParam.TypeInfo);
            test(firstParam.TypeInfo);
        }


        private ISyntaxPart EvaluateExpressionType<T>(string file, string program, Func<object, T> searchfunction, NativeIntSize intSize) where T : ISyntaxPart {
            IExpression firstParam;
            var env = CreateEnvironment(intSize);
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