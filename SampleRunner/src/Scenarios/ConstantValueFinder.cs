﻿#nullable disable
using System.Collections.Generic;
using System.IO;
using PasPasPas.Api;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Standard;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace SampleRunner.Scenarios {

    /// <summary>
    ///     helper visitor to find constants
    /// </summary>
    internal class ConstantVisitor : IStartVisitor<ConstDeclarationSymbol> {

        private readonly IStartEndVisitor visitor;

        internal ConstantVisitor()
            => visitor = new Visitor(this);

        /// <summary>
        ///     get non-generic visitor
        /// </summary>
        /// <returns></returns>
        public IStartEndVisitor AsVisitor()
            => visitor;

        public void StartVisit(ConstDeclarationSymbol element) {
            var terminalVisitor = new TerminalVisitor();
            element.Accept(terminalVisitor.AsVisitor());
            var line = terminalVisitor.ResultBuilder.ToString();
            line = line.Replace('\n', ' ');
            line = line.Replace('\r', ' ');
            line = line.Replace('\t', ' ');
            line = line.Trim();

            if (!string.IsNullOrWhiteSpace(line))
                Items.Add(line);
        }

        /// <summary>
        ///     items
        /// </summary>
        public List<string> Items { get; } = new List<string>();
    }

    internal class ConstantDeclarationVisitor : IStartVisitor<ConstantDeclaration> {

        internal ConstantDeclarationVisitor() => visitor = new Visitor(this);

        private readonly IStartEndVisitor visitor;

        internal string Value { get; private set; }

        public IStartEndVisitor AsVisitor()
            => visitor;

        public void StartVisit(ConstantDeclaration element) {
            if (Value == default)
                if (element.TypeInfo?.SymbolKind == SymbolTypeKind.Constant)
                    Value = element.TypeInfo?.ToString();
        }
    }

    /// <summary>
    ///     find constant values
    /// </summary>
    public static class ConstantValueFinder {

        /// <summary>
        ///     run scenarios
        /// </summary>
        /// <param name="b"></param>
        /// <param name="environment"></param>
        /// <param name="testPath"></param>
        /// <param name="reapeat"></param>
        public static void Run(TextWriter b, ITypedEnvironment environment, string testPath, int reapeat) {
            var hasName = false;

            for (var i = 0; i < reapeat; i++) {
                var resolver = CommonApi.CreateAnyFileResolver();
                var options = Factory.CreateOptions(resolver, environment);
                var parserApi = Factory.CreateParserApi(options);
                var file = environment.CreateFileReference(testPath);

                using (var parser = parserApi.CreateParser(file)) {
                    var result = parser.Parse();
                    var visitor = new ConstantVisitor();
                    result.Accept(visitor.AsVisitor());

                    foreach (var item in visitor.Items) {
                        var filePath = Path.GetFullPath(testPath);
                        var path = Path.Combine(Path.GetDirectoryName(filePath), "dummy.dpr");
                        var dummyProgram = $"program dummy; const {item} begin end.";
                        var file2 = environment.CreateFileReference(path);
                        var resolver2 = CommonApi.CreateResolverForSingleString(file2, dummyProgram);
                        var options2 = Factory.CreateOptions(resolver2, environment);
                        var parserApi2 = Factory.CreateParserApi(options2);

                        using (var parser2 = parserApi2.CreateParser(file2)) {

                            var result2 = parser2.Parse();
                            var project = parserApi2.CreateAbstractSyntraxTree(result2);
                            parserApi2.AnnotateWithTypes(project);
                            var visitor2 = new ConstantDeclarationVisitor();
                            project.Accept(visitor2.AsVisitor());

                            if (string.IsNullOrWhiteSpace(visitor2.Value))
                                continue;

                            if (!hasName) {
                                b.WriteLine(Path.GetFileName(testPath));
                                hasName = true;
                            }

                            b.WriteLine(item);
                            b.Write("\t\t = ");
                            b.WriteLine(visitor2.Value);
                        }
                    }
                }
            }

        }

    }
}
