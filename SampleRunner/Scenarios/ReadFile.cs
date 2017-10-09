using System.Text;
using PasPasPas.Api;
using PasPasPas.DesktopPlatform;
using PasPasPas.Infrastructure.Environment;

namespace SampleRunner.Scenarios {

    public class ReadFile {

        public static void Run(StringBuilder b, IBasicEnvironment environment, string file, int repeat) {
            var count = 0L;
            for (var i = 0; i < repeat; i++) {
                var readerApi = new ReaderApi(environment);
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
