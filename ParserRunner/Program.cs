using PasPasPas.Building.Definition;
using PasPasPas.Building.Engine;
using PasPasPas.Building.Tasks;
using PasPasPas.DesktopPlatform;
using PasPasPas.Infrastructure.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            IEnumerable<string> files1 = Directory.GetFiles(path, "*.pas").Skip(48 * 100).Take(200);
            string[] files2 = new[] { path + "Demo.pas" };

            foreach (var filePath in files1) {
                var inputFiles = new FilesSetting();
                inputFiles.Name = filePath;
                inputFiles.Path = filePath;
                settings.Items.Add(inputFiles);
                task.Path.Items.Add(new SettingReference() { ReferenceName = inputFiles.Name });
            }

            project.Settings.Add(settings);
            project.Targets.Add(target);
            target.Tasks.Add(task);

            var buildSettings = new BuildSettings();

            buildSettings.Targets.Add(target.Name);
            buildSettings.FileSystemAccess = new StandardFileAccess();

            var watch = new Stopwatch();

            watch.Start();
            IList<object> result = ProjectBuilder.BuildProject(project, buildSettings);
            watch.Stop();

            Console.WriteLine("Completed.");
            Console.ReadLine();

            var p = Process.GetCurrentProcess();
            p.Refresh();
            Console.WriteLine("Duration: " + watch.ElapsedMilliseconds.ToString());
            Console.WriteLine("Processor time: " + p.TotalProcessorTime);
            Console.WriteLine("Memory: " + p.WorkingSet64);

            foreach (var buildResult in result)
                Console.WriteLine(buildResult.ToString());

        }
    }
}
