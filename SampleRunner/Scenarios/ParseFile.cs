using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Api;
using PasPasPas.DesktopPlatform;
using PasPasPas.Infrastructure.Environment;
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
