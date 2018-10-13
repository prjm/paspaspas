using System.IO;
using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using PasPasPas.Typings.Common;

namespace SampleRunner.Scenarios {
    public static class ParseFile {

        public static void Run(TextWriter b, ITypedEnvironment environment, string testPath, int reapeat) {
            for (var i = 0; i < reapeat; i++) {
                var parserApi = new ParserApi(environment);
                using (var parser = parserApi.CreateParserForPath(testPath)) {
                    var result = parser.Parse();
                    var visitor = new TerminalWriterVisitor(b);
                    result.Accept(new Visitor(visitor));
                }
            }
        }
    }
}
