using PasPasPas.Api;
using PasPasPas.DesktopPlatform;

namespace ParserRunner.Scenarios {

    public class ReadFile {

        public static void Run(string file, int repeat) {
            for (var i = 0; i < repeat; i++) {
                var readerApi = new ReaderApi(new StandardFileAccess());
                var reader = readerApi.CreateReaderForPath(file);
                var count = 0;

                while (!reader.AtEof) {
                    reader.NextChar();
                    count++;
                }
            }
        }
    }
}
