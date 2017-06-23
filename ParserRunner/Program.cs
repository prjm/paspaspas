using PasPasPas.Building.Definition;
using PasPasPas.Building.Engine;
using PasPasPas.Building.Tasks;
using PasPasPas.DesktopPlatform;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Parsing.Tokenizer;
using PasPasPas.Parsing.Parser;
using PasPasPas.Infrastructure.Log;

namespace ParserRunner {


    class Program {

        static void Main(string[] args) {
            var buffer = new FileBuffer();
            var reference = new FileReference("test.pas");
            var tempPath = @"C:\temp\Testfiles\spring.pas";
            var content = new DesktopFileReadable(new FileReference(tempPath));
            var logManager = new LogManager();
            var services = new ParserServices(logManager);

            buffer.Add(reference, content);

            var reader = new StackedFileReader(buffer);
            reader.AddFileToRead(reference);

            var tokenizer = new StandardTokenizer(services, reader);

            while (tokenizer.HasNextToken()) {
                tokenizer.FetchNextToken();
            }


            return;


            var path = @"C:\temp\Testfiles\";

            Console.WriteLine("PasPasPasParserRunner.");
            Console.WriteLine();


            var project = new Project() { Name = "ParserRunner" };
            var target = new Target() { Name = "run" };
            var task = new PasPasPasParserTask();
            var settings = new SettingGroup();

            IEnumerable<string> files1 = Directory.GetFiles(path, "*.pas").Skip(48 * 100).Take(200);
            string[] files2 = new[] { path + "Spring.pas" };

            foreach (var filePath in files2) {
                var inputFiles = new FilesSetting() {
                    Name = filePath,
                    Path = filePath
                };
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

#if DEBUG

            var p = Process.GetCurrentProcess();
            p.Refresh();
            Console.WriteLine("Duration: " + watch.ElapsedMilliseconds.ToString());
            Console.WriteLine("Processor time: " + p.TotalProcessorTime);
            Console.WriteLine("Memory: " + p.WorkingSet64);


            foreach (var buildResult in result)
                Console.WriteLine(buildResult.ToString());


#endif

        }
    }
}
