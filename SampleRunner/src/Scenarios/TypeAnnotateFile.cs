﻿#nullable disable
using System.IO;
using PasPasPas.Api;
using PasPasPas.Globals.Environment;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace SampleRunner.Scenarios {

    /// <summary>
    ///     test type annotations
    /// </summary>
    public static class TypeAnnotateFile {

        /// <summary>
        ///     run example
        /// </summary>
        /// <param name="b"></param>
        /// <param name="environment"></param>
        /// <param name="testPath"></param>
        /// <param name="reapeat"></param>
        public static void Run(TextWriter b, ITypedEnvironment environment, string testPath, int reapeat) {
            for (var i = 0; i < reapeat; i++) {
                var resolver = CommonApi.CreateAnyFileResolver();
                var options = Factory.CreateOptions(resolver, environment);
                var parserApi = Factory.CreateParserApi(options);
                var file = environment.CreateFileReference(testPath);

                using (var parser = parserApi.CreateParser(file)) {
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
