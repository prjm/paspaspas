using System.IO;
using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace SampleRunner.Scenarios {
    public static class ParseFile {

        public static void Run(TextWriter b, string testPath, int reapeat) {
            for (var i = 0; i < reapeat; i++) {
                using (var parser = CommonApi.CreateParserForFiles(testPath)) {
                    var result = parser.Parse();
                    var visitor = new TerminalWriterVisitor(b);
                    result.Accept(new Visitor(visitor));
                }
            }
        }
    }
}
