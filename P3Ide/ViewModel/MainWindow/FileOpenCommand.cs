using Microsoft.Win32;
using System;
using System.Windows.Input;

namespace P3Ide.ViewModel.MainWindow {

    /// <summary>
    ///     command to open a file
    /// </summary>
    public class FileOpenCommand : ICommand {
        private IEditorCapabilites capabilities;

        public FileOpenCommand(IEditorCapabilites capabilities) {
            this.capabilities = capabilities;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) {
            var openFileDialog = new OpenFileDialog();

            string filterString = string.Empty;

            foreach (var projectType in capabilities.SupportedProjectTypes) {
                filterString += projectType.ProjectDescription + "|*." + projectType.ProjectExtension;
            }

            openFileDialog.Filter = filterString;

            if (openFileDialog.ShowDialog() == true) {
                //..
            }
        }
    }
}