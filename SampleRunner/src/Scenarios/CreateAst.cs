using System.IO;
using PasPasPas.Api;
using PasPasPas.Globals.Environment;
using PasPasPas.Options.Bundles;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace SampleRunner.Scenarios {

    public static class CreateAst {

        public static void Run(TextWriter b, ITypedEnvironment environment, string testPath, int reapeat) {
            for (var i = 0; i < reapeat; i++) {
                var options = new OptionSet(environment);
                var parserApi = Factory.CreateParserApi(environment, options);
                var data = parserApi.Tokenizer.Readers.CreateInputForPath(testPath);
                using (var parser = parserApi.CreateParser(data)) {
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