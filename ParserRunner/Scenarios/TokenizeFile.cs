using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PasPasPas.Api;
using PasPasPas.DesktopPlatform;
using PasPasPas.Infrastructure.Environment;

namespace SampleRunner.Scenarios {

    public static class TokenizeFile {

        public static void Run(StringBuilder b, string testPath, int reapeat) {
            var registry = new Dictionary<int, Tuple<ulong, long>>();

            for (var i = 0; i < reapeat; i++) {
                var tokenizerApi = new TokenizerApi(new StandardFileAccess());
                using (var tokenizer = tokenizerApi.CreateTokenizerForPath(testPath)) {

                    while (!tokenizer.AtEof) {
                        tokenizer.FetchNextToken();

                        var token = tokenizer.CurrentToken;
                        var kind = token.Kind;
                        var length = token.Value.Length;

                        if (registry.TryGetValue(kind, out Tuple<ulong, long> value))
                            registry[kind] = new Tuple<ulong, long>(1 + value.Item1, length + value.Item2);
                        else
                            registry.Add(kind, Tuple.Create<ulong, long>(1, length));
                    }
                }
            }

            foreach (var entry in registry.OrderByDescending(t => t.Value.Item2))
                b.AppendLine($"{entry.Key.ToString()} => {entry.Value.ToString()}");

            b.AppendLine(new string('.', 80));

            foreach (var entry in StaticEnvironment.Entries)
                if (entry is ILookupFunction fn)
                    b.AppendLine(entry.GetType().Name + ": " + fn.Table.Count);
                else if (entry is ObjectPool pool)
                    b.AppendLine(entry.ToString() + ": " + pool.Count);

        }
    }
}
