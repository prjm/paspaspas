using System;
using System.Text;
using PasPasPas.DesktopPlatform;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Log;

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
            var env = new StaticEnvironment();
            Action<StringBuilder> action;

            action = PrepareSample(env, testPath, mode, repeat);
            RunSample(env, result, action);
            Console.WriteLine(result.ToString());
        }

        private static string GetCacheName(object data) {
            if (data is IStaticCacheItem item)
                return $"[{item.Caption}]";
            else
                return string.Concat(data.ToString(), '*');
        }

        private static void RunSample(StaticEnvironment env, StringBuilder result, Action<StringBuilder> action) {
            var timer = new ExecutionTimer();
            timer.Start();
            action(result);
            timer.Stop();
            GC.Collect();

            result.AppendLine(new string('.', 80));

            foreach (var entry in env.Entries) {
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

        private static Action<StringBuilder> PrepareSample(StaticEnvironment env, string testPath, SampleMode mode, int repeat) {
            Action<StringBuilder> action;

            env.Register(StaticDependency.FileAccess, new StandardFileAccess());
            env.Register(StaticDependency.LogManager, new LogManager());

            switch (mode) {

                case SampleMode.ReadFile:
                    action = (b) => Scenarios.ReadFile.Run(b, env, testPath, repeat);
                    break;

                case SampleMode.TokenizerFile:
                    action = (b) => Scenarios.TokenizeFile.Run(b, env, testPath, repeat);
                    break;

                case SampleMode.BufferedTokenizeFile:
                    action = (b) => Scenarios.BufferedTokenizeFile.Run(b, env, testPath, repeat);
                    break;

                case SampleMode.ParseFile:
                    action = (b) => Scenarios.ParseFile.Run(b, env, testPath, repeat);
                    break;

                default:
                    action = (b) => b.AppendLine("Unknown mode.");
                    break;

            }

            return action;
        }
    }
}
