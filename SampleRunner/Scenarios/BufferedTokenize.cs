using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PasPasPas.Api;
using PasPasPas.Parsing;

namespace SampleRunner.Scenarios {

    public static class BufferedTokenizeFile {

        public static void Run(StringBuilder b, IParserEnvironment environment, string testPath, int reapeat) {
            var registry = new Dictionary<int, Tuple<ulong, long>>();

            for (var i = 0; i < reapeat; i++) {
                var tokenizerApi = new TokenizerApi(environment);
                using (var tokenizer = tokenizerApi.CreateBufferedTokenizerForPath(testPath)) {

                    while (!tokenizer.AtEof) {
                        var token = tokenizer.CurrentToken;
                        var kind = token.Kind;
                        var length = token.Value.Length;

                        if (registry.TryGetValue(kind, out Tuple<ulong, long> value))
                            registry[kind] = new Tuple<ulong, long>(1 + value.Item1, length + value.Item2);
                        else
                            registry.Add(kind, Tuple.Create<ulong, long>(1, length));

                        tokenizer.FetchNextToken();
                    }
                }
            }

            foreach (var entry in registry.OrderByDescending(t => t.Value.Item2))
                b.AppendLine($"{entry.Key.ToString()} => {entry.Value.ToString()}");

        }
    }
}
