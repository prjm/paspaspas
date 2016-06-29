using P3Ide.ViewModel.StandardFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P3Ide.ViewModel.MainWindow;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.Parser;
using PasPasPas.Options.Bundles;
using PasPasPas.DesktopPlatform;

namespace P3Ide.ViewModel.PascalProject {

    /// <summary>
    ///     supported file type: pascal file
    /// </summary>
    public class PascalFileType : ISupportedFileType {

        /// <summary>
        ///     file description
        /// </summary>
        public string FileDescription
            => "Pascal File";

        /// <summary>
        ///     file extensions
        /// </summary>
        public string FileExtension
            => ".pas";

        /// <summary>
        ///     editor
        /// </summary>
        /// <param name="registry"></param>
        public void RegisterEditor(IEditorRegistry registry) {
            EditorCreator editor = (viewModel, workspace) => {
                return new PascalEditorViewModel(viewModel, workspace);
            };
            registry.RegisterFileType(FileExtension, editor);
        }

        internal void ParseSingleFile(StringInput singleFile, LogManager log) {
            var env = new ParserServices(log);
            env.Options = new OptionSet(new StandardFileAccess());

            using (var reader = new StackedFileReader()) {
                reader.AddFile(singleFile);
                var parser = new CompilerDirectiveParser(env, reader);
                parser.IncludeInput = reader;
                while (!reader.AtEof) {
                    parser.ParseCompilerDirective();
                }
            }
        }
    }
}
