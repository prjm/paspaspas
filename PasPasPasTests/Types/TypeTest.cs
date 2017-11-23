using System;
using PasPasPas.Api;
using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Types;
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
        ///     test the type of a given expressiom
        /// </summary>
        /// <param name="expression">expression</param>
        /// <param name="typeId">type id to find</param>
        protected void AssertExprType(string expression, int typeId) {
            var file = "SimpleExpr";
            var program = $"program {file}; begin Writeln({expression}); end. ";
            Func<object, SymbolReferencePart> searchfunction = x => x as SymbolReferencePart;
            IExpression firstParam = null;

            firstParam = EvaluateExpressionType(file, program, searchfunction, NativeIntSize.Undefined);

            Assert.IsNotNull(firstParam.TypeInfo);
            Assert.AreEqual(typeId, firstParam.TypeInfo.TypeId);

        }

        /// <summary>
        ///     test the type of a declared variable expressiom
        /// </summary>
        /// <param name="declaration">declareation</param>
        /// <param name="typeId">type id to find</param>
        protected void AssertDeclType(string declaration, int typeId = TypeIds.UnspecifiedType, NativeIntSize intSize = NativeIntSize.Undefined, int typeSize = -1, CommonTypeKind typeKind = CommonTypeKind.UnknownType) {

            Action<ITypeDefinition> tester = (ITypeDefinition def) => {

                if (typeId != TypeIds.UnspecifiedType)
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

            };

            AssertDeclType(declaration, tester, intSize);
        }
        /// <summary>
        ///     test the type of a declared variable expressiom
        /// </summary>
        /// <param name="declaration">declareation</param>
        /// <param name="typeId">type id to find</param>
        protected void AssertDeclTypeDef(string declaration, string expression = "x", int typeId = TypeIds.UnspecifiedType, NativeIntSize intSize = NativeIntSize.Undefined, int typeSize = -1, CommonTypeKind typeKind = CommonTypeKind.UnknownType) {

            Func<ITypeDefinition, Boolean> tester = (ITypeDefinition def) => {

                if (typeId != TypeIds.UnspecifiedType)
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
            };

            AssertDeclTypeDef(declaration, tester, intSize, expression);
        }

        /// <summary>
        ///     test the type of a declared variable expressiom
        /// </summary>
        /// <param name="declaration">declareation</param>
        /// <param name="typeId">type id to find</param>
        protected void AssertDeclTypeDef<T>(string declaration, Func<T, Boolean> test, NativeIntSize intSize = NativeIntSize.Undefined, string expression = "x") where T : class, ITypeDefinition {
            var file = "SimpleExpr";
            var program = $"program {file}; type t = {declaration}; var x : t; begin Writeln({expression}); end. ";
            Func<object, SymbolReferencePart> searchfunction = x => x as SymbolReferencePart;
            IExpression firstParam = null;

            firstParam = EvaluateExpressionType(file, program, searchfunction, intSize);

            var t = firstParam.TypeInfo as T;
            Assert.IsNotNull(t);
            Assert.IsTrue(test(t));
        }

        /// <summary>
        ///     test the type of a declared types
        /// </summary>
        /// <param name="declaration">declareation</param>
        /// <param name="typeId">type id to find</param>
        protected void AssertDeclType(string declaration, Action<ITypeDefinition> test, NativeIntSize intSize = NativeIntSize.Undefined) {
            var file = "SimpleExpr";
            var program = $"program {file}; var x : {declaration}; begin Writeln(x); end. ";
            Func<object, SymbolReferencePart> searchfunction = x => x as SymbolReferencePart;
            IExpression firstParam = null;

            firstParam = EvaluateExpressionType(file, program, searchfunction, intSize);

            Assert.IsNotNull(firstParam.TypeInfo);
            test(firstParam.TypeInfo);
        }


        private IExpression EvaluateExpressionType(string file, string program, Func<object, SymbolReferencePart> searchfunction, NativeIntSize intSize) {
            IExpression firstParam;
            var env = CreateEnvironment(intSize);
            var api = new ParserApi(env);
            using (var reader = api.CreateParserForString($"{file}.dpr", program)) {

                api.Options.Meta.NativeIntegerSize.Value = intSize;

                var tree = reader.Parse();
                var project = api.CreateAbstractSyntraxTree(tree);
                api.AnnotateWithTypes(project);

                var astVisitor = new AstVisitor<SymbolReferencePart>() { SearchFunction = searchfunction };
                project.Accept(astVisitor.AsVisitor());

                Assert.IsNotNull(astVisitor.Result);
                Assert.AreEqual(1, astVisitor.Result.Expressions.Count);

                firstParam = astVisitor.Result.Expressions[0];
            }

            return firstParam;
        }
    }
}