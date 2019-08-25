using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using PasPasPas.Api;
using PasPasPas.Globals.Environment;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.ObjectPooling;
using PasPasPas.Infrastructure.Utils;
using SampleRunner.Scenarios;

namespace SampleRunner {

    /// <summary>
    ///     This sample runner is used to test different feature sets
    ///     of the p3 library.
    /// </summary>
    internal class Program {

        private static void Main() {

            var testPath = @"D:\temp\testfiles\q\q.dpr";
            //var testPath = @"C:\temp\Testfiles\all";
            var mode = SampleMode.TypeAnnotateFile;
            var repeat = 1;
            var result = System.Console.Out;
            var environment = Factory.CreateEnvironment();
            var useHistograms = false;
            var logListener = new ConsoleLogListener(result);
            var action = PrepareSample(environment, testPath, mode, repeat, useHistograms);

            environment.Log.RegisterTarget(logListener);
            RunSample(environment, result, action, useHistograms);

            Console.ReadLine();
        }

        private static string GetCacheName(object data)
            => $"[{data.GetType().ToString()}]";

        private static void RunSample(IParserEnvironment environment, TextWriter result, Action<TextWriter> action, bool useHistogram) {
            var timer = new Stopwatch();
            var status = new SystemInfo();
            var printer = new HtmlHistogramPrinter();
            timer.Start();
            action(result);
            timer.Stop();
            status = new SystemInfo(status);

            result.WriteLine(new string('.', 80));

            foreach (var entry in environment.Entries) {
                var name = GetCacheName(entry);
                if (entry is ILookupFunction fn)
                    result.WriteLine(name + ": " + fn.Table.Count);
                else if (entry is ObjectPool pool)
                    result.WriteLine(name + ": " + pool.Count);
                else if (entry is Histograms hist)
                    result.WriteLine(name + ": " + hist.Count);
                else if (entry is IEnvironmentItem sc) {
                    var count = sc.Count;
                    if (count < 0)
                        result.WriteLine(name);
                    else
                        result.WriteLine(name + ": " + sc.Count);
                }
                else
                    result.WriteLine(name);
            }

            if (useHistogram) {
                Histograms.Print(printer);
                printer.Render(@"C:\TEMP\HIST.HTML");
            }

            result.WriteLine(new string('-', 80));
            result.WriteLine($"{timer.ElapsedTicks} ticks required ({timer.Elapsed.TotalMilliseconds}).");
            result.WriteLine($"{status.WorkingSet} bytes required.");
            result.WriteLine($"{status.CollectionCount0} collections level 0.");
            result.WriteLine($"{status.CollectionCount1} collections level 1.");
            result.WriteLine($"{status.CollectionCount2} collections level 2.");
        }

        private static Action<TextWriter> PrepareSample(IAssemblyBuilderEnvironment environment, string testPath, SampleMode mode, int repeat, bool useHistograms) {

            Histograms.Enable = useHistograms;

            var action = default(Action<TextWriter>);
            var actions = new List<Action<TextWriter>>();
            var files = new List<string>();

            if (Directory.Exists(testPath)) {
                foreach (var file in Directory.GetFiles(testPath, "*.pas").OrderBy(t => Path.GetFileName(t)))
                    if (File.Exists(file))
                        files.Add(file);
            }
            else if (File.Exists(testPath)) {
                files.Add(testPath);
            }
            else throw new FileNotFoundException("File or path not found.", testPath);

            foreach (var file in files.Take(500)) {

                switch (mode) {

                    case SampleMode.ReadFile:
                        action = (b) => ReadFile.Run(b, file, repeat);
                        break;

                    case SampleMode.TokenizerFile:
                        action = (b) => TokenizeFile.Run(b, file, repeat);
                        break;

                    case SampleMode.BufferedTokenizeFile:
                        action = (b) => BufferedTokenizeFile.Run(b, file, repeat);
                        break;

                    case SampleMode.ParseFile:
                        action = (b) => ParseFile.Run(b, file, repeat);
                        break;

                    case SampleMode.CreateAbstractSyntaxTree:
                        action = (b) => CreateAst.Run(b, environment, file, repeat);
                        break;

                    case SampleMode.TypeAnnotateFile:
                        action = (b) => TypeAnnotateFile.Run(b, environment, file, repeat);
                        break;

                    case SampleMode.FindConstants:
                        action = (b) => ConstantValueFinder.Run(b, environment, file, repeat);
                        break;

                    case SampleMode.CreateAssembly:
                        action = (b) => CreateAssembly.Run(b, environment, file, repeat);
                        break;

                    default:
                        action = (b) => b.WriteLine("Unknown mode.");
                        break;

                }

                actions.Add(action);

            };

            return (tw) => {
                foreach (var a in actions) {
                    try {
                        a(tw);
                    }
                    catch (Exception e) {
                        tw.WriteLine(e);
                        throw;
                    }
                }
            };
        }
    }
}
