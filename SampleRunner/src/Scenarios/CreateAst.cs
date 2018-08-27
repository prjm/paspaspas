using System;
using System.Text;
using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using PasPasPas.Typings.Common;

namespace SampleRunner.Scenarios {

    public static class CreateAst {

        public static void Run(StringBuilder b, ITypedEnvironment environment, string testPath, int reapeat) {
            for (var i = 0; i < reapeat; i++) {
                var parserApi = new ParserApi(environment);
                using (var parser = parserApi.CreateParserForPath(testPath)) {
                    var result = parser.Parse();
                    //Console.ReadLine();
                    var project = parserApi.CreateAbstractSyntraxTree(result);
                    //Console.ReadLine();
                    var visitor = new TerminalVisitor();
                    result.Accept(new Visitor(visitor));
                    b.Append(visitor.ResultBuilder.ToString());
                }
            }
        }

    }
}