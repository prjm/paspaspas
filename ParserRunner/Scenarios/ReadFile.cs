using System.Text;
using PasPasPas.Api;
using PasPasPas.DesktopPlatform;

namespace SampleRunner.Scenarios {

    public class ReadFile {

        public static void Run(StringBuilder b, string file, int repeat) {
            var count = 0L;
            for (var i = 0; i < repeat; i++) {
                var readerApi = new ReaderApi(new StandardFileAccess());
                var reader = readerApi.CreateReaderForPath(file);

                while (!reader.AtEof) {
                    reader.NextChar();
                    count++;
                }
            }

            b.AppendLine($"{count} characters read.");
        }
    }
}
