using PasPasPas.Infrastructure.Input;
using System;
using System.IO;
using System.Text;

namespace PasPasPas.DesktopPlatform {

    /// <summary>
    ///     file based input for the parser
    /// </summary>
    public class FileInput : IParserInput {

        /// <summary>
        ///     name of thedefaul encoding
        /// </summary>
        public const string DefaultEncodingSettingName
            = "FileInput.DefaultEncoding";

        /// <summary>
        ///     value of the encoding
        /// </summary>
        public const string DefaultEncodingValue = "utf-8";

        private StreamReader reader = null;
        private FileStream file = null;

        /// <summary>
        ///     dispose the input stream
        /// </summary>
        ~FileInput() {
            Dispose(false);
        }

        /// <summary>
        ///     input file
        /// </summary>
        public IFileReference FilePath
            => filePath;

        /// <summary>
        ///     stream reader
        /// </summary>
        private StreamReader Reader
        {
            get
            {
                Open();
                return reader;
            }
        }

        /// <summary>
        /// Detects the byte order mark of a file and returns
        /// an appropriate encoding for the file.
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <returns></returns>
        public static Encoding GetFileEncoding(string sourceFile) {

            Encoding enc = Encoding.Default;

            // *** Detect byte order mark if any - otherwise assume default
            byte[] buffer = new byte[5];
            using (FileStream inputFile = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                inputFile.Read(buffer, 0, 5);
            }

            if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf)
                enc = Encoding.UTF8;
            else if (buffer[0] == 0xff && buffer[1] == 0xfe)
                enc = Encoding.Unicode;
            else if (buffer[0] == 0xfe && buffer[1] == 0xff)
                enc = Encoding.BigEndianUnicode;
            else if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xfe && buffer[3] == 0xff)
                enc = Encoding.UTF32;
            else if (buffer[0] == 0x2b && buffer[1] == 0x2f && buffer[2] == 0x76)
                enc = Encoding.UTF7;

            return enc;
        }


        /// <summary>
        ///     test if at eof
        /// </summary>
        public bool AtEof
            => Reader.EndOfStream;

        private IFileReference filePath;

        #region IDisposable Support
        private bool disposedValue = false;

        /// <summary>
        ///     creates a new input file
        /// </summary>
        /// <param name="path"></param>
        public FileInput(IFileReference path) {
            filePath = path;
        }

        /// <summary>
        ///     dispose input read
        /// </summary>
        /// <param name="disposing"><c>true</c> if ressources are disposed</param>
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    CloseReader();
                    CloseFileStream();
                }
                disposedValue = true;
            }
        }

        /// <summary>
        ///     Dispose the input string
        /// </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     close reader
        /// </summary>
        public void Close() {
            //..
        }

        private void CloseFileStream() {
            if (file == null)
                return;

            file.Close();
            file = null;
        }

        private void CloseReader() {
            if (reader == null)
                return;

            reader.Close();
            reader = null;
        }

        /// <summary>
        ///     open redaer
        /// </summary>
        public void Open() {
            string path = FilePath.Path;

            if (file == null) {
                file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan);
            }

            if (reader == null) {
                reader = new StreamReader(file, GetFileEncoding(path));
            }
        }

        /// <summary>
        /// read a single char
        /// </summary>
        /// <returns></returns>
        public char NextChar()
            => (char)Reader.Read();
    }

    #endregion

}

