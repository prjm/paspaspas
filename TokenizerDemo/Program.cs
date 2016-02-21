using PasPasPas.Api;
using PasPasPas.Api.Input;
using PasPasPas.Internal.Input;
using PasPasPas.Internal.Tokenizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenizerDemo {
    class Program {

        public static IList<string> FindFiles(string path)
            => new List<string>(Directory.EnumerateFiles(path, "*.pas"));

        static void Main(string[] args) {
            var path = "C:\\Users\\Bastian\\Documents\\Visual Studio 2015\\Projects\\paspaspas\\Testfiles";
            var files = FindFiles(path);
            var result = new Dictionary<string, int>();
            var before = new List<string>();

            foreach (var file in files) {
                Console.WriteLine(file);

                using (FileInput input = new FileInput()) {
                    input.FileName = file;
                    var tokenizer = new StandardTokenizer();
                    tokenizer.Input = input;

                    while (tokenizer.HasNextToken()) {
                        var token = tokenizer.FetchNextToken();

                        if (before.Count >= 10)
                            before.RemoveAt(0);

                        before.Add(token.Value);

                        if (token.Kind != PascalToken.Undefined)
                            continue;

                        for (var x = 0; x < before.Count; x++) {
                            Console.Write(before[x]);
                            Console.Write(" ");
                        }
                        Console.WriteLine();

                        var key = token.Kind.ToString() + ": " + token.Value;
                        int count;
                        if (!result.TryGetValue(key, out count))
                            count = 0;

                        count++;

                        result[key] = count;
                    }
                }

            }


            var list = new List<KeyValuePair<string, int>>(result.Count);
            list.AddRange(result);
            list.Sort((l, r) => l.Value - r.Value);

            foreach (var entry in result) {
                Console.WriteLine(entry.Value + ": " + entry.Key);
            }
        }
    }
}
