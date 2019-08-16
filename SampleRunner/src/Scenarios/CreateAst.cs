using System.IO;
using PasPasPas.Api;
using PasPasPas.Globals.Environment;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace SampleRunner.Scenarios {

    public static class CreateAst {

        public static void Run(TextWriter b, ITypedEnvironment environment, string testPath, int reapeat) {
            for (var i = 0; i < reapeat; i++) {
                var resolver = CommonApi.CreateAnyFileResolver();
                var options = Factory.CreateOptions(resolver, environment);
                var parserApi = Factory.CreateParserApi(options);
                var file = parserApi.Tokenizer.Readers.CreateFileRef(testPath);

                using (var parser = parserApi.CreateParser(file)) {
                    var result = parser.Parse();
                    var project = parserApi.CreateAbstractSyntraxTree(result);
                    var visitor = new TerminalVisitor();

                    result.Accept(new Visitor(visitor));
                    b.Write(visitor.ResultBuilder.ToString());
                }
            }
        }

    }
}