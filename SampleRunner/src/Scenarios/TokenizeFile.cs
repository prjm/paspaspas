using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using PasPasPas.Api;
using PasPasPas.Globals.Environment;

namespace SampleRunner.Scenarios {

    internal class TokenInfo {

        public TokenInfo(ulong tokenCount, ulong length) {
            TokenCount = tokenCount;
            TokenLength = length;
        }

        public ulong TokenCount { get; set; }
        public ulong TokenLength { get; set; }

    }

    public static class TokenizeFile {

        public static void Run(TextWriter b, IParserEnvironment environment, string testPath, int reapeat) {
            var registry = new Dictionary<int, TokenInfo>();

            for (var i = 0; i < reapeat; i++) {
                var tokenizerApi = new TokenizerApi(environment);
                var data = tokenizerApi.Readers.CreateInputForPath(testPath);
                using (var tokenizer = tokenizerApi.CreateTokenizer(data)) {

                    while (!tokenizer.AtEof) {
                        tokenizer.FetchNextToken();

                        var token = tokenizer.CurrentToken;
                        var kind = token.Kind;
                        var length = (ulong)token.Value.Length;

                        if (registry.TryGetValue(kind, out var value)) {
                            value.TokenCount += 1;
                            value.TokenLength += length;
                        }
                        else
                            registry.Add(kind, new TokenInfo(1, length));
                    }
                }
            }

            foreach (var entry in registry.OrderByDescending(t => t.Value.TokenLength))
                b.WriteLine($"{entry.Key.ToString(CultureInfo.InvariantCulture)} => {entry.Value.TokenCount}, {entry.Value.TokenLength}");

        }
    }
}
