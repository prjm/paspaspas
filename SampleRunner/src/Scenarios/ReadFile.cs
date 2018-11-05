using System.IO;
using PasPasPas.Api;
using PasPasPas.Infrastructure.Environment;

namespace SampleRunner.Scenarios {

    public static class ReadFile {

        public static void Run(TextWriter b, IBasicEnvironment environment, string file, int repeat) {
            var count = 0L;
            for (var i = 0; i < repeat; i++) {
                using (var reader = ReaderApi.CreateReaderForPath(file)) {

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
