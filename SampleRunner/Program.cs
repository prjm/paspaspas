using System;
using System.Text;
using PasPasPas.Infrastructure.Environment;

namespace SampleRunner {

    public enum SampleMode {
        Undefined,
        ReadFile,
        TokenizerFile,
        BufferedTokenizeFile,
        ParseFile
    }

    class Program {

        static void Main(string[] args) {

            var testPath = @"C:\temp\Testfiles\spring.pas";
            var mode = SampleMode.ParseFile;
            var repeat = 1;
            var result = new StringBuilder();
            Action<StringBuilder> action;

            action = PrepareSample(testPath, mode, repeat);
            RunSample(result, action);
            Console.WriteLine(result.ToString());
        }

        private static string GetCacheName(object data) {
            if (data is IStaticCacheItem item)
                return $"[{item.Caption}]";
            else
                return string.Concat(data.ToString(), '*');
        }

        private static void RunSample(StringBuilder result, Action<StringBuilder> action) {
            var timer = new ExecutionTimer();
            timer.Start();
            action(result);
            timer.Stop();
            GC.Collect();

            result.AppendLine(new string('.', 80));

            foreach (var entry in StaticEnvironment.Entries) {
                var name = GetCacheName(entry);
                if (entry is ILookupFunction fn)
                    result.AppendLine(name + ": " + fn.Table.Count);
                else if (entry is ObjectPool pool)
                    result.AppendLine(name + ": " + pool.Count);
                else if (entry is IManualStaticCache sc)
                    result.AppendLine(name + ": " + sc.Count);
                else
                    result.AppendLine(name);
            }

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

                case SampleMode.ParseFile:
                    action = (b) => Scenarios.ParseFile.Run(b, testPath, repeat);
                    break;

                default:
                    action = (b) => b.AppendLine("Unknown mode.");
                    break;

            }

            return action;
        }
    }
}
