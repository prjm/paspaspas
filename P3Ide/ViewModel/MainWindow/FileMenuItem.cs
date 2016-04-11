using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using P3Ide.ViewModel.Base;
using Microsoft.Win32;
using System.Windows;

namespace P3Ide.ViewModel.MainWindow {

    /// <summary>
    ///     file menu item
    /// </summary>
    public abstract class FileMenuItem : IMainMenuItem {

        /// <summary>
        ///     menu command
        /// </summary>
        public abstract ICommand Command { get; }


        /// <summary>
        ///     menu subitems
        /// </summary>
        public ObservableCollection<IMenuViewModel> MenuItems { get; }
            = new ObservableCollection<IMenuViewModel>();

        /// <summary>
        ///     menu tex
        /// </summary>
        public abstract string Text { get; }

        /// <summary>
        ///     menu category
        /// </summary>
        public string TopLevelMenu => "File";

        /// <summary>
        ///     priority
        /// </summary>
        public int Priority { get; protected set; }
            = 0;
    }

    /// <summary>
    ///     menu item to open a file
    /// </summary>
    public class FileOpenMenuItem : FileMenuItem {

        /// <summary>
        ///     create a new file open menu item
        /// </summary>
        /// <param name="capabilities"></param>
        public FileOpenMenuItem(IEditorCapabilites capabilities) {
            Capabilities = capabilities;
        }

        /// <summary>
        /// 
        /// </summary>
        public override ICommand Command
            => new RelayCommand<object>(OpenFile);

        /// <summary>
        ///     open a file
        /// </summary>
        /// <returns></returns>
        private void OpenFile(object parameter) {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Open files or projects";

            string filterString = "";
            foreach (var supportedProjectType in Capabilities.SupportedProjectTypes) {
                filterString = AppendFilter(filterString, supportedProjectType.ProjectDescription, "*" + supportedProjectType.ProjectExtension);
            }
            foreach (var supportedFileType in Capabilities.SupportedFileTypes) {
                filterString = AppendFilter(filterString, supportedFileType.FileDescription, "*" + supportedFileType.FileExtension);
            }
            filterString = AppendFilter(filterString, "All files", "*.*");
            openFileDialog.Filter = filterString;

            if (openFileDialog.ShowDialog() == true) {

            }

        }

        private string AppendFilter(string filterString, string description, string extension) {
            string result = filterString ?? string.Empty;
            if (!string.IsNullOrEmpty(filterString))
                result = filterString + "|";
            result += description + "|" + extension;
            return result;
        }

        /// <summary>
        ///     menu item text
        /// </summary>
        public override string Text
            => "Open";

        /// <summary>
        ///     capabilities
        /// </summary>
        public IEditorCapabilites Capabilities { get; }
    }

    /// <summary>
    ///     exit file menu item
    /// </summary>
    public class FileExitMenuItem : FileMenuItem {

        /// <summary>
        ///     file priority
        /// </summary>
        public FileExitMenuItem() {
            Priority = 999;
        }

        /// <summary>
        ///     exit command
        /// </summary>
        public override ICommand Command
            => new RelayCommand<object>(ExitApplication);


        /// <summary>
        ///     exit application
        /// </summary>
        /// <param name="obj"></param>
        private void ExitApplication(object obj) {
            Application.Current.Shutdown();
        }

        /// <summary>
        ///     menu item text
        /// </summary>
        public override string Text
            => "Exit";
    }
}
