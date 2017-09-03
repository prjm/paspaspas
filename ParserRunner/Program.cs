using PasPasPas.DesktopPlatform;
using PasPasPas.Api;
using System.Collections.Generic;
using System.Linq;
using System;
using PasPasPas.Infrastructure.Environment;

namespace ParserRunner {


    class Program {

        static void Main(string[] args) {

            var registry = new Dictionary<int, Tuple<ulong, long>>();

            for (int i = 0; i < 10; i++) {
                var tokenizerApi = new TokenizerApi(new StandardFileAccess());
                var tempPath = @"C:\temp\Testfiles\spring.pas";
                var tokenizer = tokenizerApi.CreateTokenizerForPath(tempPath);

                while (!tokenizer.AtEof) {
                    tokenizer.FetchNextToken();

                    var token = tokenizer.CurrentToken;
                    var kind = token.Kind;
                    int length = token.Value.Length;

                    if (registry.TryGetValue(kind, out Tuple<ulong, long> value))
                        registry[kind] = new Tuple<ulong, long>(1 + value.Item1, length + value.Item2);
                    else
                        registry.Add(kind, Tuple.Create<ulong, long>(1, length));
                }
            }

            foreach (var entry in registry.OrderByDescending(t => t.Value.Item2))
                System.Console.WriteLine($"{entry.Key.ToString()} => {entry.Value.ToString()}");

            Console.WriteLine(new string('.', 80));


            foreach (var entry in StaticEnvironment.Entries)
                if (entry is ILookupFunction fn)
                    Console.WriteLine(entry.GetType().FullName + ": " + fn.Table.Count);


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
