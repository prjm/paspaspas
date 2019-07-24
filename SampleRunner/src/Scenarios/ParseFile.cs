using System.IO;
using PasPasPas.Api;
using PasPasPas.Globals.Environment;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace SampleRunner.Scenarios {
    public static class ParseFile {

        public static void Run(TextWriter b, ITypedEnvironment environment, string testPath, int reapeat) {
            for (var i = 0; i < reapeat; i++) {
                var parserApi = new ParserApi(environment);
                var data = parserApi.Tokenizer.Readers.CreateInputForPath(testPath);
                using (var parser = parserApi.CreateParser(data)) {
                    var result = parser.Parse();
                    var visitor = new TerminalWriterVisitor(b);
                    result.Accept(new Visitor(visitor));
                }
            }
        }
    }
}
