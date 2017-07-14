using PasPasPas.DesktopPlatform;
using PasPasPas.Api;
using System.Collections.Generic;
using System.Linq;

namespace ParserRunner {


    class Program {

        static void Main(string[] args) {

            var tokenizerApi = new TokenizerApi(new StandardFileAccess());
            var tempPath = @"C:\temp\Testfiles\spring.pas";
            var tokenizer = tokenizerApi.CreateTokenizerForPath(tempPath);
            var registry = new Dictionary<int, ulong>();

            while (!tokenizer.AtEof) {
                tokenizer.FetchNextToken();

                var kind = tokenizer.CurrentToken.Kind;

                if (registry.TryGetValue(kind, out ulong value))
                    registry[kind] = 1 + value;
                else
                    registry.Add(kind, 1);
            }


            foreach (var entry in registry.OrderByDescending(t => t.Value))
                System.Console.WriteLine($"{entry.Key.ToString()} => {entry.Value.ToString()}");


            /*
            var tokenizer = new StandardTokenizer(services, reader);

            while (tokenizer.HasNextToken()) {
                tokenizer.FetchNextToken();
            }

            */



            return;

            /*

            var path = @"C:\temp\Testfiles\";

            Console.WriteLine("PasPasPasParserRunner.");
            Console.WriteLine();


            var project = new Project() { Name = "ParserRunner" };
            var target = new Target() { Name = "run" };
            var task = new PasPasPasParserTask();
            var settings = new SettingGroup();

            var files1 = Directory.GetFiles(path, "*.pas").Skip(48 * 100).Take(200);
            var files2 = new[] { path + "Spring.pas" };

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
            var result = ProjectBuilder.BuildProject(project, buildSettings);
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
              */
        }
    }
}
