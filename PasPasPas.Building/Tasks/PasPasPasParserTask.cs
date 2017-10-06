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
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Infrastructure.Files;

namespace PasPasPas.Building.Tasks {

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

            foreach (var file in Path.AsFileList(settings.FileSystemAccess)) {
                count++;
                var logManager = new LogManager();
                var log = new LogTarget();
                var options = new OptionSet(settings.FileSystemAccess);

                ISyntaxPart resultTree = null;

                var buffer = new FileBuffer();
                var reader = new StackedFileReader(buffer);
                var parser = new StandardParser(logManager, options, reader);

                buffer.Add(file, settings.FileSystemAccess.OpenFileForReading(file));
                result.AppendLine("-----------------------<< " + file.Path + " (" + count + ")");
                reader.AddFileToRead(file);
                var hasError = false;

                log.ProcessMessage += (x, y) => {
                    result.Append(y.Message.MessageID.ToString() + Environment.NewLine);
                    hasError = hasError ||
                    y.Message.Severity == MessageSeverity.Error ||
                    y.Message.Severity == MessageSeverity.FatalError;
                };

                try {
                    resultTree = parser.ParseFile();
                }
                catch (Exception exception) {
                    result.AppendLine("<<XXXX>> Exception!");
                    result.Append(exception.ToString());
                    return result;
                }

#if DEBUG

                var result1 = new StringBuilder();
                /*
   var dummy = false;
   using (IParserInput inputFile1 = settings.FileSystemAccess.OpenFileForReading(file))
   using (var reader1 = new OldStackedFileReader()) {
       reader1.AddFile(inputFile1);

       while (!reader1.AtEof)
           result1.Append(reader1.FetchChar(out dummy));
   }
   */

#endif

                log.ClearEventHandlers();

#if DEBUG

                var visitor = new TerminalVisitor();
                resultTree.Accept(visitor.AsVisitor());
                if (!string.Equals(result1.ToString(), visitor.ResultBuilder.ToString(), StringComparison.Ordinal)) {
                    result.AppendLine("<<XXXX>> Different!");
                    result.AppendLine(result1.ToString());
                    result.AppendLine("<<XXXX>> Different!");
                    result.AppendLine(visitor.ResultBuilder.ToString());

                    //var visitor1 = new StructureVisitor();
                    //var options1 = new StructureVisitorOptions();

                    //result.AppendLine("<<XXXX>> Tree");
                    //VisitorHelper.AcceptVisitor(resultTree, visitor1, options1);
                    //result.AppendLine(options1.ResultBuilder.ToString());


                    return result;
                }



                //return resultTree;
                var transformVisitor = new TreeTransformer(new ProjectRoot()) {
                    LogManager = environment.Log
                };
                resultTree.Accept(transformVisitor.AsVisitor());

#endif

            }



            return result;
        }
    }
}
