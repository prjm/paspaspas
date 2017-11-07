using System;
using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree.Abstract;
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

            firstParam = EvaluateExpressionType(file, program, searchfunction);

            Assert.IsNotNull(firstParam.TypeInfo);
            Assert.AreEqual(typeId, firstParam.TypeInfo.TypeId);

        }

        /// <summary>
        ///     test the type of a declared variable expressiom
        /// </summary>
        /// <param name="declaration">declareation</param>
        /// <param name="typeId">type id to find</param>
        protected void AssertDeclType(string declaration, int typeId) {
            var file = "SimpleExpr";
            var program = $"program {file}; var x : {declaration}; begin Writeln(x); end. ";
            Func<object, SymbolReferencePart> searchfunction = x => x as SymbolReferencePart;
            IExpression firstParam = null;

            firstParam = EvaluateExpressionType(file, program, searchfunction);

            Assert.IsNotNull(firstParam.TypeInfo);
            Assert.AreEqual(typeId, firstParam.TypeInfo.TypeId);

        }


        private IExpression EvaluateExpressionType(string file, string program, Func<object, SymbolReferencePart> searchfunction) {
            IExpression firstParam;
            var env = CreateEnvironment();
            var api = new ParserApi(env);
            using (var reader = api.CreateParserForString($"{file}.dpr", program)) {
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