using PasPasPas.Building.Definition;
using PasPasPas.Building.Engine;
using PasPasPas.Building.Tasks;
using PasPasPas.DesktopPlatform;
using PasPasPas.Infrastructure.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserRunner {


    class Program {

        static void Main(string[] args) {

            var path = @"C:\temp\Testfiles\";

            Console.WriteLine("PasPasPasParserRunner.");
            Console.WriteLine();


            var project = new Project() { Name = "ParserRunner" };
            var target = new Target() { Name = "run" };
            var task = new PasPasPasParserTask();
            var settings = new SettingGroup();

            foreach (var filePath in Directory.GetFiles(path, "*.pas")) {
                var inputFiles = new FilesSetting();
                inputFiles.Name = filePath;
                inputFiles.Path = filePath;
                settings.Items.Add(inputFiles);
                task.Path.Items.Add(new SettingReference() { ReferenceName = inputFiles.Name });
            }

            project.Settings.Add(settings);
            project.Targets.Add(target);
            target.Tasks.Add(task);

            var projectBuilder = new ProjectBuilder();
            var buildSettings = new BuildSettings();

            buildSettings.Targets.Add(target.Name);
            buildSettings.FileSystemAccess = new StandardFileAccess();

            var result = projectBuilder.BuildProject(project, buildSettings);

            foreach (var buildResult in result)
                Console.WriteLine(buildResult.ToString());

        }
    }
}
