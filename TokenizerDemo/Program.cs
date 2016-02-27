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
            var result = new Dictionary<long, long>();

            foreach (var file in files) {

                using (FileInput input = new FileInput()) {
                    input.FileName = file;
                    var baseTokenizer = new StandardTokenizer();
                    var tokenizer = new TokenizerWithLookahead();
                    baseTokenizer.Input = input;
                    tokenizer.BaseTokenizer = baseTokenizer;

                    while (tokenizer.HasNextToken()) {
                        var token = tokenizer.FetchNextToken();
                        long key = token.Kind; // + ": " + token.Value;
                        long count;
                        if (!result.TryGetValue(key, out count))
                            count = 0;

                        count++;

                        result[key] = count;
                    }
                }

            }


            var list = new List<KeyValuePair<long, long>>(result.Count);
            list.AddRange(result);
            list.Sort((l, r) => Math.Sign(l.Value - r.Value));

            foreach (var entry in list) {
                Console.WriteLine(entry.Value + ": " + entry.Key);
            }


            MacroProcessor.Info();
        }
    }
}
