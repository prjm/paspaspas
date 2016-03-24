using PasPasPas.Infrastructure.Configuration;
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

        /// <summary>
        ///     dispose the input stream
        /// </summary>
        ~FileInput() {
            Dispose(false);
        }

        /// <summary>
        ///     input file
        /// </summary>
        public string Path => filePath;

        /// <summary>
        ///     config settings
        /// </summary>
        public IConfigurationSettings Settings { get; set; }

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
        /// <param name="srcFile"></param>
        /// <returns></returns>
        public Encoding GetFileEncoding(string srcFile) {
            Encoding enc = Encoding.GetEncoding(Settings.GetValue(DefaultEncodingSettingName, DefaultEncodingValue));

            // *** Detect byte order mark if any - otherwise assume default
            byte[] buffer = new byte[5];
            using (FileStream file = new FileStream(srcFile, FileMode.Open)) {
                file.Read(buffer, 0, 5);
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


        private StreamReader CreateAndInitializeReader()
            => new StreamReader(Path, GetFileEncoding(Path));


        /// <summary>
        ///     stream position
        /// </summary>
        public long Position
        {
            get
            {
                return reader.BaseStream.Position;
            }

            set
            {
                reader.BaseStream.Seek(Position, SeekOrigin.Begin);
            }
        }

        /// <summary>
        ///     test if at eof
        /// </summary>
        public bool AtEof
            => Reader.EndOfStream;

        #region IDisposable Support
        private bool disposedValue = false;

        private string filePath;

        /// <summary>
        ///     creates a new input file
        /// </summary>
        /// <param name="path"></param>
        public FileInput(string path) {
            filePath = path;
        }

        /// <summary>
        ///     dispose input read
        /// </summary>
        /// <param name="disposing"><c>true</c> if ressources are disposed</param>
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    if (reader != null)
                        reader.Dispose();
                }

                reader = null;
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
            reader.Close();
            reader = null;
        }

        /// <summary>
        ///     open redaer
        /// </summary>
        public void Open() {
            if (reader == null)
                reader = CreateAndInitializeReader();
        }

        /// <summary>
        /// read a single char
        /// </summary>
        /// <returns></returns>
        public char NextChar()
            => (char)reader.Read();
    }

    #endregion

}

