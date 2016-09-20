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
            StringBuilder result = new StringBuilder();

            foreach (var file in Path.AsFileList()) {

                var logManager = new LogManager();
                var environment = new ParserServices(logManager);
                var log = new LogTarget();
                environment.Options = new OptionSet(settings.FileSystemAccess);

                StandardParser parser = new StandardParser(environment);
                using (var inputFile = settings.FileSystemAccess.OpenFileForReading(file))
                using (var reader = new StackedFileReader()) {
                    result.AppendLine("-----------------------<< " + file.Path);
                    reader.AddFile(inputFile);
                    parser.BaseTokenizer = new StandardTokenizer(environment, reader);
                    var hasError = false;

                    log.ProcessMessage += (x, y) => {
                        result.Append(y.Message.MessageID.ToString() + Environment.NewLine);
                        hasError = hasError ||
                        y.Message.Severity == MessageSeverity.Error ||
                        y.Message.Severity == MessageSeverity.FatalError;
                    };

                    parser.Parse();

                    log.ClearEventHandlers();

                    /*
                    var visitor = new TerminalVisitor();
                    var options = new TerminalVisitorOptions();
                    result.Accept(visitor, options);
                    Assert.AreEqual(output, options.ResultBuilder.ToString());
                    Assert.AreEqual(string.Empty, errorText);
                    Assert.IsFalse(hasError);
                    */
                }

            }

            return result.ToString();
        }
    }
}
