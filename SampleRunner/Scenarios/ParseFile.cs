﻿using System.Text;
using PasPasPas.Api;
using PasPasPas.Parsing;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace SampleRunner.Scenarios {
    public class ParseFile {


        public static void Run(StringBuilder b, IParserEnvironment environment, string testPath, int reapeat) {
            for (var i = 0; i < reapeat; i++) {
                var parserApi = new ParserApi(environment);
                using (var parser = parserApi.CreateParserForPath(testPath)) {
                    var result = parser.Parse();
                    var visitor = new TerminalVisitor();
                    result.Accept(new Visitor(visitor));
                    b.Append(visitor.ResultBuilder.ToString());
                }
            }
        }
    }
}