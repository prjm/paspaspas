#nullable disable
using System.IO;
using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace SampleRunner.Scenarios {

    /// <summary>
    ///     parse a file
    /// </summary>
    public static class ParseFile {

        /// <summary>
        ///     run the test scenario
        /// </summary>
        /// <param name="b"></param>
        /// <param name="testPath"></param>
        /// <param name="reapeat"></param>
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
