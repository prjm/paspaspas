using System;
using System.Text;
using PasPasPas.Infrastructure.Environment;

namespace SampleRunner {

    public enum SampleMode {
        Undefined,
        ReadFile,
        TokenizerFile,
        BufferedTokenizeFile
    }

    class Program {

        static void Main(string[] args) {

            var testPath = @"C:\temp\Testfiles\spring.pas";
            var mode = SampleMode.BufferedTokenizeFile;
            var repeat = 1;
            var result = new StringBuilder();
            Action<StringBuilder> action;

            action = PrepareSample(testPath, mode, repeat);
            RunSample(result, action);
            Console.WriteLine(result.ToString());

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

        private static void RunSample(StringBuilder result, Action<StringBuilder> action) {
            var timer = new ExecutionTimer();
            timer.Start();
            action(result);
            timer.Stop();
            GC.Collect();

            result.AppendLine(new string('.', 80));

            foreach (var entry in StaticEnvironment.Entries)
                if (entry is ILookupFunction fn)
                    result.AppendLine(entry.GetType().Name + ": " + fn.Table.Count);
                else if (entry is ObjectPool pool)
                    result.AppendLine(entry.ToString() + ": " + pool.Count);
                else if (entry is IManualStaticCache sc)
                    result.AppendLine(entry.ToString() + ": " + sc.Count);

            result.AppendLine(new string('-', 80));
            result.AppendLine($"{timer.TickCount} ticks required ({timer.Duration.TotalMilliseconds}).");
            result.AppendLine($"{GC.CollectionCount(0)} collections level 0.");
            result.AppendLine($"{GC.CollectionCount(1)} collections level 1.");
            result.AppendLine($"{GC.CollectionCount(2)} collections level 2.");
        }

        private static Action<StringBuilder> PrepareSample(string testPath, SampleMode mode, int repeat) {
            Action<StringBuilder> action;

            switch (mode) {

                case SampleMode.ReadFile:
                    action = (b) => Scenarios.ReadFile.Run(b, testPath, repeat);
                    break;

                case SampleMode.TokenizerFile:
                    action = (b) => Scenarios.TokenizeFile.Run(b, testPath, repeat);
                    break;

                case SampleMode.BufferedTokenizeFile:
                    action = (b) => Scenarios.BufferedTokenizeFile.Run(b, testPath, repeat);
                    break;

                default:
                    action = (b) => b.AppendLine("Unknown mode.");
                    break;

            }

            return action;
        }
    }
}
