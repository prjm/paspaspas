using System;
using System.Diagnostics;
using System.Text;
using PasPasPas.Api;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.ObjectPooling;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing;
using PasPasPas.Typings.Common;
using SampleRunner.Scenarios;

namespace SampleRunner {

    /// <summary>
    ///     This sample runner is used to test different feature sets
    ///     of the p3 library.
    /// </summary>
    internal class Program {

        private static void Main() {

            var testPath = @"C:\temp\Testfiles\spring.pas";
            var mode = SampleMode.ParseFile;
            var repeat = 1;
            var result = new StringBuilder();
            var environment = new DefaultEnvironment();
            Action<StringBuilder> action;

            action = PrepareSample(environment, testPath, mode, repeat);
            RunSample(environment, result, action);
            Console.WriteLine(result.ToString());
            Console.ReadLine();
        }

        private static string GetCacheName(object data)
            => $"[{data.GetType().ToString()}]";

        private static void RunSample(IParserEnvironment environment, StringBuilder result, Action<StringBuilder> action) {
            var timer = new Stopwatch();
            var status = new SystemInfo();
            timer.Start();
            action(result);
            timer.Stop();
            status = new SystemInfo(status);

            result.AppendLine(new string('.', 80));

            foreach (var entry in environment.Entries) {
                var name = GetCacheName(entry);
                if (entry is ILookupFunction fn)
                    result.AppendLine(name + ": " + fn.Table.Count);
                else if (entry is ObjectPool pool)
                    result.AppendLine(name + ": " + pool.Count);
                else if (entry is IEnvironmentItem sc) {
                    var count = sc.Count;
                    if (count < 0)
                        result.AppendLine(name);
                    else
                        result.AppendLine(name + ": " + sc.Count);
                }
                else
                    result.AppendLine(name);
            }

            result.AppendLine(new string('-', 80));
            result.AppendLine($"{timer.ElapsedTicks} ticks required ({timer.Elapsed.TotalMilliseconds}).");
            result.AppendLine($"{status.WorkingSet} bytes required.");
            result.AppendLine($"{status.CollectionCount0} collections level 0.");
            result.AppendLine($"{status.CollectionCount1} collections level 1.");
            result.AppendLine($"{status.CollectionCount2} collections level 2.");
        }

        private static Action<StringBuilder> PrepareSample(ITypedEnvironment environment, string testPath, SampleMode mode, int repeat) {
            Action<StringBuilder> action;

            switch (mode) {

                case SampleMode.ReadFile:
                    action = (b) => ReadFile.Run(b, environment, testPath, repeat);
                    break;

                case SampleMode.TokenizerFile:
                    action = (b) => TokenizeFile.Run(b, environment, testPath, repeat);
                    break;

                case SampleMode.BufferedTokenizeFile:
                    action = (b) => BufferedTokenizeFile.Run(b, environment, testPath, repeat);
                    break;

                case SampleMode.ParseFile:
                    action = (b) => ParseFile.Run(b, environment, testPath, repeat);
                    break;

                case SampleMode.CreateAbstractSyntaxTree:
                    action = (b) => CreateAst.Run(b, environment, testPath, repeat);
                    break;

                case SampleMode.TypeAnnotateFile:
                    action = (b) => TypeAnnotateFile.Run(b, environment, testPath, repeat);
                    break;

                default:
                    action = (b) => b.AppendLine("Unknown mode.");
                    break;

            }

            return action;
        }
    }
}
