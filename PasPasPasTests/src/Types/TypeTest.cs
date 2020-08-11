using System;
using System.Diagnostics.CodeAnalysis;
using PasPasPas.Api;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Abstract;
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
        /// <param name="resolveSubrange"></param>
        /// <param name="decls">declarations</param>
        protected void AssertExprTypeByVar(string commonType, string expression, ITypeDefinition typeId, bool resolveSubrange = false, string decls = "") {
            var file = "SimpleExpr";
            var program = $"program {file};{decls} var a,b: {commonType}; begin WriteLn({expression}); end. ";
            AssertExprType(file, program, typeId, resolveSubrange, null);
        }

        /// <summary>
        ///     assert constant expression type
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="typeId"></param>
        /// <param name="resolveSubrange"></param>
        /// <param name="typeName"></param>
        /// <param name="decls"></param>
        protected void AssertExprTypeByConst(string expression, ITypeDefinition typeId, bool resolveSubrange = false, string typeName = "", string decls = "") {
            var file = "SimpleExpr";
            var program = $"program {file};{decls} const a = {expression}; begin WriteLn(a); end. ";
            AssertExprType(file, program, typeId, resolveSubrange, typeName);
        }

        [return: MaybeNull]
        static StructuredStatement SearchForStructuredStatement(object x) {
            if (x is StructuredStatement)
                return x as StructuredStatement;
            return default;
        }

        /// <summary>
        ///     assert statement type
        /// </summary>
        /// <param name="statement"></param>
        /// <param name="decls"></param>
        /// <param name="tester"></param>
        protected void AssertStatementType(string statement, string decls, Action<StructuredStatement?> tester) {
            var file = "SimpleExpr";
            var program = $"program {file};{decls} begin {statement}; end. ";

            var stm = EvaluateExpressionType(file, program, SearchForStructuredStatement, NativeIntSize.Undefined, out var env) as StructuredStatement;
            tester(stm);
        }

        [return: MaybeNull]
        static SymbolReferencePart SearchForSymbolReferencePart(object x)
                => x is SymbolReferencePart srp && srp.Kind == SymbolReferencePartKind.CallParameters ? x as SymbolReferencePart : null;

        [return: MaybeNull]
        static SymbolReferencePart SearchForWriteLn(object x) {
            if (string.Equals((x as SymbolReferencePart)?.Name, "writeln", StringComparison.OrdinalIgnoreCase))
                return x as SymbolReferencePart;
            return default;
        }

        /// <summary>
        ///     assert expression type
        /// </summary>
        /// <param name="file"></param>
        /// <param name="program"></param>
        /// <param name="typeId"></param>
        /// <param name="resolveSubrange"></param>
        /// <param name="typeName"></param>
        protected void AssertExprType(string file, string program, ITypeDefinition typeId, bool resolveSubrange, string? typeName) {

            IExpression? firstParam = null;

            firstParam = EvaluateExpressionType(file, program, SearchForSymbolReferencePart, NativeIntSize.Undefined, out var env) as IExpression;

            Assert.IsNotNull(firstParam);
            Assert.IsNotNull(firstParam.TypeInfo);

            var foundTypeId = default(ITypeDefinition);
            var foundTypeName = "";

            if (resolveSubrange && firstParam.TypeInfo.TypeDefinition is ISubrangeType sr) {
                foundTypeId = sr.SubrangeOfType;
                var type = foundTypeId;
                foundTypeName = type.ToString();
            }
            else {
                foundTypeId = firstParam.TypeInfo.TypeDefinition;
                var type = foundTypeId;
                foundTypeName = type.ToString();
            }

            if (!string.IsNullOrEmpty(typeName))
                Assert.AreEqual(typeName, foundTypeName ?? string.Empty, StringComparer.OrdinalIgnoreCase);
            else
                Assert.AreEqual(typeId, foundTypeId);
        }



        /// <summary>
        ///     test the type of a given expression
        /// </summary>
        /// <param name="expression">expression</param>
        /// <param name="typeId">type id to find</param>
        /// <param name="decls">declarations</param>
        protected void AssertExprType(string expression, ITypeDefinition typeId, string decls = "") {
            void tester(IExpression? firstParam) {
                Assert.IsNotNull(firstParam);
                Assert.IsNotNull(firstParam.TypeInfo);
                Assert.AreEqual(typeId, firstParam.TypeInfo.TypeDefinition);
            }

            AssertExprType(expression, decls, tester);
        }

        /// <summary>
        ///     test the type of a given expression
        /// </summary>
        /// <param name="expression">expression</param>
        /// <param name="tester">test function</param>
        /// <param name="decls">declarations</param>
        protected void AssertExprType(string expression, string decls, Action<IExpression?> tester) {
            var file = "SimpleExpr";
            var program = $"program {file};{decls} begin Writeln({expression}); end. ";

            IExpression? firstParam = null;

            firstParam = EvaluateExpressionType(file, program, SearchForWriteLn, NativeIntSize.Undefined, out var env) as IExpression;
            tester(firstParam);
        }

        [return: MaybeNull]
        BlockOfStatements SearchBlock(object x) {
            if (x is BlockOfStatements b)
                return b;
            return default;
        }

        /// <summary>
        ///     assert statement type
        /// </summary>
        /// <param name="statemnt"></param>
        /// <param name="value"></param>
        /// <param name="decls"></param>
        /// <param name="typDefinition"></param>
        /// <param name="completeSource"></param>
        /// <param name="kind"></param>
        protected void AssertStatementType(string statemnt, IValue value, string decls = "", ITypeDefinition? typDefinition = default, string? completeSource = default, SymbolTypeKind kind = SymbolTypeKind.Undefined) {
            var file = "SimpleExpr";
            var program = completeSource ?? $"program {file};{decls} begin {statemnt}; end. ";

            var block = EvaluateExpressionType(file, program, SearchBlock, NativeIntSize.Undefined, out var env) as BlockOfStatements;
            Assert.IsNotNull(block);

            var statement = block.Statements[0] as StructuredStatement;
            Assert.IsNotNull(statement);

            var e = statement.Expressions[0] as SymbolReference;
            Assert.IsNotNull(e);

            var p = e.SymbolParts[0] as SymbolReferencePart;
            Assert.IsNotNull(p);
            Assert.IsNotNull(p.Value);

            if (value != default)
                Assert.AreEqual(value, e.TypeInfo);

            if (kind != SymbolTypeKind.Undefined)
                Assert.AreEqual(kind, e.TypeInfo.SymbolKind);

            if (typDefinition != default)
                Assert.AreEqual(typDefinition, e.TypeInfo.TypeDefinition);
        }

        /// <summary>
        ///     test the value of a given expression
        /// </summary>
        /// <param name="expression">expression</param>
        /// <param name="value"></param>
        /// <param name="typedef">type id to find</param>
        /// <param name="isConstant"></param>
        /// <param name="completeSource"></param>
        /// <param name="decls">addition declarations</param>
        protected void AssertExprValue(string expression, ITypeSymbol value, string decls = "", ITypeDefinition? typedef = default, bool isConstant = true, string? completeSource = null) {
            var file = "SimpleExpr";
            var program = completeSource ?? $"program {file};{decls} begin Writeln({expression}); end. ";

            var firstParam = EvaluateExpressionType(file, program, SearchForWriteLn, NativeIntSize.Undefined, out var env) as IExpression;

            Assert.IsNotNull(firstParam);
            Assert.IsNotNull(firstParam.TypeInfo);

            if (isConstant)
                Assert.IsTrue(firstParam.TypeInfo.IsConstant());

            if (value.SymbolKind == SymbolTypeKind.InvocationResult) {
                var r = firstParam.TypeInfo;
                Assert.AreEqual(value.GetBaseType(), r.GetBaseType());
                Assert.AreEqual(value.SymbolKind, r.SymbolKind);
                Assert.AreEqual(value.TypeDefinition, r.TypeDefinition);
            }
            else if (value.SymbolKind == SymbolTypeKind.IntrinsicRoutineResult) {
                var r = firstParam.TypeInfo;
                Assert.AreEqual(value.GetBaseType(), r.GetBaseType());
                Assert.AreEqual(value.SymbolKind, r.SymbolKind);
                Assert.AreEqual(value.TypeDefinition, r.TypeDefinition);
            }
            else {
                Assert.AreEqual(value.TypeDefinition, firstParam.TypeInfo.TypeDefinition);
                Assert.AreEqual(value, firstParam.TypeInfo);
            }

            if (typedef != default)
                Assert.AreEqual(typedef, firstParam.TypeInfo.TypeDefinition);
        }


        /// <summary>
        ///     test the type of a declared variable expression
        /// </summary>
        /// <param name="declaration">declaration</param>
        /// <param name="typeDef">type id to find</param>
        /// <param name="intSize"></param>
        /// <param name="typeSize"></param>
        /// <param name="typeKind"></param>
        protected void AssertDeclType(string declaration, ITypeDefinition? typeDef = default, NativeIntSize intSize = NativeIntSize.Undefined, int typeSize = -1, BaseType typeKind = BaseType.Unkown) {

            void tester(ITypeDefinition def) {

                if (typeDef != default)
                    Assert.AreEqual(typeDef, def);

                if (typeKind != BaseType.Unkown)
                    Assert.AreEqual(typeKind, def.BaseType);

                if (typeSize > 0) {
                    IFixedSizeType? sizedType;
                    if (def is IAliasedType alias) {
                        sizedType = alias.BaseTypeDefinition as IFixedSizeType;
                    }
                    else {
                        sizedType = def as IFixedSizeType;
                    }
                    Assert.IsNotNull(sizedType);
                    Assert.AreEqual(typeSize, sizedType.TypeSizeInBytes);
                }

            }

            AssertDeclType(declaration, tester, intSize);
        }

        /// <summary>
        ///     test the type of a declared variable expression
        /// </summary>
        /// <param name="declaration">declaration</param>
        /// <param name="expression"></param>
        /// <param name="typeDef">type id to find</param>
        /// <param name="intSize"></param>
        /// <param name="typeSize"></param>
        /// <param name="typeKind"></param>
        protected void AssertDeclTypeDef(string declaration, string expression = "x", ITypeDefinition? typeDef = default, NativeIntSize intSize = NativeIntSize.Undefined, int typeSize = -1, BaseType typeKind = BaseType.Unkown) {

            bool tester(ITypeDefinition def) {

                if (typeDef != default)
                    Assert.AreEqual(typeDef, def);

                if (typeKind != BaseType.Unkown)
                    Assert.AreEqual(typeKind, def.BaseType);

                if (typeSize > 0) {
                    IFixedSizeType? sizedType;
                    if (def is IAliasedType alias) {
                        sizedType = alias.BaseTypeDefinition as IFixedSizeType;
                    }
                    else {
                        sizedType = def as IFixedSizeType;
                    }
                    Assert.IsNotNull(sizedType);
                    Assert.AreEqual(typeSize, sizedType.TypeSizeInBytes);
                }

                return true;
            }

            AssertDeclTypeDef<ITypeDefinition>(declaration, tester, intSize, expression);
        }

        /// <summary>
        ///     test the type of a declared variable expression
        /// </summary>
        /// <param name="declaration">declaration</param>
        /// <param name="test"></param>
        /// <param name="intSize"></param>
        /// <param name="expression"></param>
        /// <param name="typeName"></param>
        protected void AssertDeclTypeDef<T>(string declaration, Func<T, bool> test, NativeIntSize intSize = NativeIntSize.Undefined, string expression = "x", string typeName = "t") where T : class, ITypeDefinition {
            var file = "SimpleExpr";
            var program = $"program {file}; type {typeName} = {declaration}; var x : {typeName}; begin Writeln({expression}); end. ";
            AssertDeclTypeDef<T>(program, file, intSize, test);
        }

        [return: MaybeNull]
        SymbolReferencePart SearchForSymbolRefPart(object x)
            => x as SymbolReferencePart;


        /// <summary>
        ///     assert declaration type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="program"></param>
        /// <param name="file"></param>
        /// <param name="intSize"></param>
        /// <param name="test"></param>
        protected void AssertDeclTypeDef<T>(string program, string file, NativeIntSize intSize, Func<T, bool> test) where T : class, ITypeDefinition {
            IExpression? firstParam = null;

            firstParam = EvaluateExpressionType(file, program, SearchForSymbolRefPart, intSize, out var env) as IExpression;

            var v = firstParam?.TypeInfo;
            Assert.IsNotNull(v);
            var t = v.TypeDefinition as T;
            Assert.IsNotNull(t);
            Assert.IsTrue(test(t));
        }

        /// <summary>
        ///     assert assignment compatibility
        /// </summary>
        /// <param name="assignTo"></param>
        /// <param name="assignFrom"></param>
        /// <param name="compat"></param>
        /// <param name="predeclaration"></param>
        protected void AssertAssignmentCompat(string assignTo, string assignFrom, bool compat = true, string predeclaration = "") {
            bool test(ITypeDefinition l, ITypeDefinition r) {
                Assert.IsNotNull(l);
                Assert.IsNotNull(r);
                var canBeAssigned = l.CanBeAssignedFromType(r);
                return compat && canBeAssigned || !compat && !canBeAssigned;
            }

            AssertTestForAssignment(assignTo, assignFrom, test, predeclaration);
        }

        /// <summary>
        ///     assert test
        /// </summary>
        /// <param name="assignTo"></param>
        /// <param name="assignFrom"></param>
        /// <param name="test"></param>
        /// <param name="predeclaration"></param>
        protected void AssertTestForAssignment(string assignTo, string assignFrom, Func<ITypeDefinition, ITypeDefinition, bool> test, string predeclaration = "") {
            var typdef = $"{predeclaration} ta = {assignTo}; tb = {assignFrom}";
            var declaration = "a: ta; b: tb";
            var statement = "a := b";
            AssertTestForGenericStatement(typdef, declaration, statement, test);
        }

        [return: MaybeNull]
        StructuredStatement SearchForStatement(object x)
            => x as StructuredStatement;

        /// <summary>
        ///     assert generic type
        /// </summary>
        /// <param name="typedef"></param>
        /// <param name="declaration"></param>
        /// <param name="statement"></param>
        /// <param name="test"></param>
        protected void AssertTestForGenericStatement(string typedef, string declaration, string statement, Func<ITypeDefinition, ITypeDefinition, bool> test) {
            var file = "SimpleExpr";
            var program = $"program {file}; type {typedef}; var {declaration}; begin {statement}; end. ";
            ISyntaxPart? firstParam = null;

            firstParam = EvaluateExpressionType(file, program, SearchForStatement, NativeIntSize.All32bit, out var env);

            Assert.IsNotNull(firstParam);
            var t = firstParam as StructuredStatement;
            var l = t != null && t.Expressions.Count > 0 ? t.Expressions[0].TypeInfo : null;
            var r = t != null && t.Expressions.Count > 1 ? t.Expressions[1].TypeInfo : null;
            Assert.IsNotNull(t);

            var lt = l?.TypeDefinition;
            var rt = r?.TypeDefinition;
            Assert.IsNotNull(lt);
            Assert.IsNotNull(rt);
            Assert.IsTrue(test(lt, rt));
        }

        [return: MaybeNull]
        SymbolReferencePart SearchForParams(object x)
            => x is SymbolReferencePart srp && srp.Kind == SymbolReferencePartKind.CallParameters ? srp : null;


        /// <summary>
        ///     test the type of a declared types
        /// </summary>
        /// <param name="declaration">declaration</param>
        /// <param name="test"></param>
        /// <param name="intSize">integer size</param>
        protected void AssertDeclType(string declaration, Action<ITypeDefinition> test, NativeIntSize intSize = NativeIntSize.Undefined) {
            var file = "SimpleExpr";
            var program = $"program {file}; var x : {declaration}; begin Writeln(x); end. ";
            IExpression? firstParam = null;

            firstParam = EvaluateExpressionType(file, program, SearchForParams, intSize, out var env) as IExpression;

            Assert.IsNotNull(firstParam?.TypeInfo);
            var ti = firstParam.TypeInfo.TypeDefinition;
            test(ti);
        }



        /// <summary>
        ///     evaluate an expression type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <param name="program"></param>
        /// <param name="searchfunction"></param>
        /// <param name="intSize"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        protected ISyntaxPart EvaluateExpressionType<T>(string file, string program, TestFunction<T> searchfunction, NativeIntSize intSize, out ITypedEnvironment env) where T : ISyntaxPart {
            IExpression firstParam;

            env = CreateEnvironment(intSize);
            var path = env.CreateFileReference($"{file}.dpr");
            var data = CommonApi.CreateResolverForSingleString(path, program);
            var options = Factory.CreateOptions(data, env);
            var api = Factory.CreateParserApi(options);

            using (var reader = api.CreateParser(path)) {

                api.Options.Meta.NativeIntegerSize.Value = intSize;

                var tree = reader.Parse();
                var project = api.CreateAbstractSyntraxTree(tree);
                api.AnnotateWithTypes(project);

                var astVisitor = new AstVisitor<T>(searchfunction);
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