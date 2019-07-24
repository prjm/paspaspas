using System.IO;
using PasPasPas.Api;

namespace SampleRunner.Scenarios {

    public static class ReadFile {

        public static void Run(TextWriter b, string file, int repeat) {
            var count = 0L;
            var env = Factory.CreateEnvironment();
            var api = Factory.CreateReaderApi(env);
            var data = api.CreateInputForPath(file);

            for (var i = 0; i < repeat; i++) {
                using (var reader = api.CreateReader(data)) {

                    while (!reader.AtEof) {
                        reader.NextChar();
                        count++;
                    }
                }

                b.WriteLine($"{count} characters read.");
            }
        }
    }
}
