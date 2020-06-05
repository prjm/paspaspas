#nullable disable
using System.IO;
using PasPasPas.Api;

namespace SampleRunner.Scenarios {

    /// <summary>
    ///     read file demo
    /// </summary>
    public static class ReadFile {

        /// <summary>
        ///     read a simple file
        /// </summary>
        /// <param name="b"></param>
        /// <param name="file"></param>
        /// <param name="repeat"></param>
        public static void Run(TextWriter b, string file, int repeat) {
            var count = 0L;

            for (var i = 0; i < repeat; i++) {
                using (var reader = CommonApi.CreateReaderForFiles(file)) {

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
