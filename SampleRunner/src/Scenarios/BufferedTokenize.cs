using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using PasPasPas.Api;

namespace SampleRunner.Scenarios {

    /// <summary>
    ///     demo: buffered tokenizer
    /// </summary>
    public static class BufferedTokenizeFile {

        /// <summary>
        ///     run the demo
        /// </summary>
        /// <param name="b"></param>
        /// <param name="testPath"></param>
        /// <param name="reapeat"></param>
        public static void Run(TextWriter b, string testPath, int reapeat) {
            var registry = new Dictionary<int, Tuple<ulong, long>>();

            for (var i = 0; i < reapeat; i++) {
                using (var tokenizer = CommonApi.CreateBufferedTokenizer(testPath)) {

                    while (!tokenizer.AtEof) {
                        var token = tokenizer.CurrentToken;
                        var kind = token.Kind;
                        var length = token.Value.Length;

                        if (registry.TryGetValue(kind, out var value))
                            registry[kind] = new Tuple<ulong, long>(1 + value.Item1, length + value.Item2);
                        else
                            registry.Add(kind, Tuple.Create<ulong, long>(1, length));

                        tokenizer.FetchNextToken();
                    }
                }
            }

            foreach (var entry in registry.OrderByDescending(t => t.Value.Item2))
                b.WriteLine($"{entry.Key.ToString(CultureInfo.InvariantCulture)} => {entry.Value.ToString()}");

        }
    }
}
