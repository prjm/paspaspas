using System;
using System.Text;
using PasPasPas.Api;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Parsing;
using PasPasPas.Typings.Common;
using SampleRunner.Scenarios;

namespace SampleRunner {

    /// <summary>
    ///     the sample runner is used to test different feature sets
    ///     of the p3 library
    /// </summary>
    class Program {

        static void Main() {

            var testPath = @"C:\temp\Testfiles\spring.pas";
            var mode = SampleMode.TypeAnnotateFile;
            var repeat = 1;
            var result = new StringBuilder();
            var environment = new DefaultEnvironment();
            Action<StringBuilder> action;

            action = PrepareSample(environment, testPath, mode, repeat);
            RunSample(environment, result, action);
            Console.WriteLine(result.ToString());
        }

        private static string GetCacheName(object data) {
            if (data is IEnvironmentItem item)
                return $"[{item.Caption}]";
            else if (data is ObjectPool pool)
                return $"[{pool.PoolName}]";
            else
                return $"[{data.GetType().ToString()}]";
        }

        private static void RunSample(IParserEnvironment environment, StringBuilder result, Action<StringBuilder> action) {
            var timer = new ExecutionTimer();
            timer.Start();
            action(result);
            timer.Stop();
            GC.Collect();

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
            result.AppendLine($"{timer.TickCount} ticks required ({timer.Duration.TotalMilliseconds}).");
            result.AppendLine($"{GC.CollectionCount(0)} collections level 0.");
            result.AppendLine($"{GC.CollectionCount(1)} collections level 1.");
            result.AppendLine($"{GC.CollectionCount(2)} collections level 2.");
        }

        private static Action<StringBuilder> PrepareSample(ITypedEnvironment environment, string testPath, SampleMode mode, int repeat) {
            Action<StringBuilder> action;

            switch (mode) {

                case SampleMode.ReadFile:
                    action = (b) => Scenarios.ReadFile.Run(b, environment, testPath, repeat);
                    break;

                case SampleMode.TokenizerFile:
                    action = (b) => Scenarios.TokenizeFile.Run(b, environment, testPath, repeat);
                    break;

                case SampleMode.BufferedTokenizeFile:
                    action = (b) => Scenarios.BufferedTokenizeFile.Run(b, environment, testPath, repeat);
                    break;

                case SampleMode.ParseFile:
                    action = (b) => Scenarios.ParseFile.Run(b, environment, testPath, repeat);
                    break;

                case SampleMode.CreateAbstractSyntaxTree:
                    action = (b) => Scenarios.CreateAst.Run(b, environment, testPath, repeat);
                    break;

                case SampleMode.TypeAnnotateFile:
                    action = (b) => Scenarios.TypeAnnotateFile.Run(b, environment, testPath, repeat);
                    break;

                default:
                    action = (b) => b.AppendLine("Unknown mode.");
                    break;

            }

            return action;
        }
    }
}
