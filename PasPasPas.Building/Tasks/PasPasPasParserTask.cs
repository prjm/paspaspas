using System;
using System.Collections.Generic;
using PasPasPas.Building.Definition;
using PasPasPas.Building.Engine;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.Parser;
using PasPasPas.Options.Bundles;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Parsing.Tokenizer;
using System.Text;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Building.Tasks {

    /// <summary>
    ///     dummy parent item
    /// </summary>
    internal class StubParent : SyntaxPartBase {

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(this, startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }

    }

    /// <summary>
    ///     a simple parser runner
    /// </summary>
    public class PasPasPasParserTask : TaskBase {

        /// <summary>
        ///     path settings
        /// </summary>
        public SettingGroup Path { get; }
            = new SettingGroup();

        /// <summary>
        ///     clear settings
        /// </summary>
        /// <param name="settings"></param>
        public override void ClearVariables(BuildSettings settings) {
            base.ClearVariables(settings);
            Path.Clear();
        }

        /// <summary>
        ///     initialize variables
        /// </summary>
        /// <param name="settings">settings</param>
        /// <param name="variables">variables</param>
        public override void InitializeVariables(BuildSettings settings, Dictionary<string, Setting> variables) {
            base.InitializeVariables(settings, variables);
            Path.Resolve(variables);
        }

        /// <summary>
        ///     run task
        /// </summary>
        /// <param name="settings">settings</param>
        public override object Run(BuildSettings settings) {
            var result = new StringBuilder();
            var count = 0;

            foreach (FileReference file in Path.AsFileList()) {
                count++;
                var logManager = new LogManager();
                var environment = new ParserServices(logManager);
                var log = new LogTarget();
                environment.Options = new OptionSet(settings.FileSystemAccess);

                IExtendableSyntaxPart resultTree;

                var parser = new StandardParser(environment);
                using (IParserInput inputFile = settings.FileSystemAccess.OpenFileForReading(file))
                using (var reader = new StackedFileReader()) {
                    result.AppendLine("-----------------------<< " + file.Path + " (" + count + ")");
                    reader.AddFile(inputFile);
                    parser.BaseTokenizer = new StandardTokenizer(environment, reader);
                    var hasError = false;

                    log.ProcessMessage += (x, y) => {
                        result.Append(y.Message.MessageID.ToString() + Environment.NewLine);
                        hasError = hasError ||
                        y.Message.Severity == MessageSeverity.Error ||
                        y.Message.Severity == MessageSeverity.FatalError;
                    };

                    resultTree = new StubParent();
                    try {
                        parser.ParseFile(resultTree);

                    }
                    catch (Exception exception) {
                        result.AppendLine("<<XXXX>> Exception!");
                        result.Append(exception.ToString());
                    }
                }

#if DEBUG

                var result1 = new StringBuilder();

                bool dummy = false;
                using (IParserInput inputFile1 = settings.FileSystemAccess.OpenFileForReading(file))
                using (var reader1 = new StackedFileReader()) {
                    reader1.AddFile(inputFile1);

                    while (!reader1.AtEof)
                        result1.Append(reader1.FetchChar(out dummy));
                }

#endif

                log.ClearEventHandlers();

#if DEBUG

                var visitor = new TerminalVisitor();
                var options = new TerminalVisitorOptions();
                VisitorHelper.AcceptVisitor(resultTree, visitor, options);
                if (!string.Equals(result1.ToString(), options.ResultBuilder.ToString(), StringComparison.Ordinal)) {
                    result.AppendLine("<<XXXX>> Different!");
                    result.AppendLine(result1.ToString());
                    result.AppendLine("<<XXXX>> Different!");
                    result.AppendLine(options.ResultBuilder.ToString());

                    var visitor1 = new StructureVisitor();
                    var options1 = new StructureVisitorOptions();

                    result.AppendLine("<<XXXX>> Tree");
                    VisitorHelper.AcceptVisitor(resultTree, visitor1, options1);
                    result.AppendLine(options1.ResultBuilder.ToString());


                    return result;
                }



                //return resultTree;
                var transformVisitor = new TreeTransformer();
                var transformVisitorOptions = new TreeTransformerOptions();
                VisitorHelper.AcceptVisitor(resultTree, transformVisitor, transformVisitorOptions);
#endif

            }



            return result;
        }
    }
}
