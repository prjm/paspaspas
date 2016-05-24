﻿using Autofac;
using P3Ide.ViewModel.MainWindow;
using P3Ide.ViewModel.PascalProject;
using P3Ide.ViewModel.Projects;
using P3Ide.ViewModel.StandardFiles;
using System.Windows;

namespace P3Ide {
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application {

        /// <summary>
        ///     start application
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<FileNewMenuItem>().As<IMainMenuItem>();
            containerBuilder.RegisterType<FileOpenMenuItem>().As<IMainMenuItem>();
            containerBuilder.RegisterType<FileExitMenuItem>().As<IMainMenuItem>();

            containerBuilder.RegisterType<MainViewModel>().As<IMainViewModel>();
            containerBuilder.RegisterType<MainMenuViewModel>().As<IMainMenuViewModel>();
            containerBuilder.RegisterType<StandardEditorCapabilities>().As<IEditorCapabilites>().SingleInstance();
            containerBuilder.RegisterType<EditorWorkspace>().As<IEditorWorkspace>().SingleInstance();
            containerBuilder.RegisterType<EditorRegistry>().As<IEditorRegistry>().SingleInstance();

            containerBuilder.RegisterType<SupportedPascalProject>().As<ISupportedProjectType>().SingleInstance();
            containerBuilder.RegisterType<TextFileType>().As<ISupportedFileType>().SingleInstance();
            containerBuilder.RegisterType<PascalFileType>().As<ISupportedFileType>().SingleInstance();

            var container = containerBuilder.Build();

            using (var scope = container.BeginLifetimeScope()) {
                var mainWindow = new MainIdeWindow();
                var mainViewModel = container.Resolve<IMainViewModel>();
                mainWindow.Show();
                mainWindow.DataContext = mainViewModel;
            }
        }

    }
}
