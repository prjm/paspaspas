using P3Ide.ViewModel.PascalProject;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Infrastructure.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3Ide.ViewModel.MainWindow {

    /// <summary>
    ///     view model for docked windows
    /// </summary>
    public class PascalEditorViewModel : EditorViewModel {

        /// <summary>
        ///     create a new editor view model
        /// </summary>
        /// <param name="capabilities"></param>
        /// <param name="workspace">workspace</param>
        public PascalEditorViewModel(IEditorCapabilites capabilities, IEditorWorkspace workspace) {
            Capabilities = capabilities;
            Workspace = workspace;
        }

        /// <summary>
        ///     capabilities
        /// </summary>
        public IEditorCapabilites Capabilities { get; }

        /// <summary>
        ///     editor content
        /// </summary>
        public override string Content
        {
            get
            {
                return base.Content;
            }

            set
            {
                base.Content = value;
                var fileType = Capabilities.SupportedFileTypes.First(t => string.Equals(t.FileExtension, ".pas", StringComparison.OrdinalIgnoreCase));

                // this is only a workaround
                // and in future, there should be a project model for this
                var workaround = (PascalFileType)fileType;
                var logManager = new LogManager();

                foreach (var toolWindow in Workspace.ToolWindows) {
                    var logTarget = toolWindow as ILogTarget;
                    if (logTarget != null)
                        logManager.RegisterTarget(logTarget);
                }

                using (var singleFile = new StringInput(value, "C:\\temp\\dummy_1.pas")) {
                    workaround.ParseSingleFile(singleFile, logManager);
                }

                foreach (var toolWindow in Workspace.ToolWindows) {
                    var logTarget = toolWindow as ILogTarget;
                    if (logTarget != null)
                        logManager.UnregisterTarget(logTarget);
                }
            }
        }

        /// <summary>
        ///     workspace
        /// </summary>
        public IEditorWorkspace Workspace { get; }
    }
}
