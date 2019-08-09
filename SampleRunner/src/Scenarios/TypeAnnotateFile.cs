using System.IO;
using PasPasPas.Api;
using PasPasPas.Globals.Environment;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace SampleRunner.Scenarios {

    public static class TypeAnnotateFile {

        public static void Run(TextWriter b, ITypedEnvironment environment, string testPath, int reapeat) {
            for (var i = 0; i < reapeat; i++) {
                var options = Factory.CreateOptions(environment, default);
                var parserApi = Factory.CreateParserApi(environment, options);
                var file = parserApi.Tokenizer.Readers.CreateFileRef(testPath);
                var resolver = CommonApi.CreateAnyFileResolver(parserApi.Tokenizer.Readers);

                using (var parser = parserApi.CreateParser(resolver, file)) {
                    var result = parser.Parse();
                    var project = parserApi.CreateAbstractSyntraxTree(result);
                    parserApi.AnnotateWithTypes(project);
                    var visitor = new TerminalVisitor();
                    result.Accept(new Visitor(visitor));
                    b.Write(visitor.ResultBuilder.ToString());
                }
            }
        }
    }
}
