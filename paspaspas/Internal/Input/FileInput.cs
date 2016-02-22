using System;
using System.IO;
using System.Text;

namespace PasPasPas.Internal.Input {

    /// <summary>
    ///     file based input for the parser
    /// </summary>
    public class FileInput : InputBase, IDisposable {

        private StreamReader reader = null;

        /// <summary>
        ///     input file
        /// </summary>
        public string FileName { get; set; }

        private StreamReader Reader
        {
            get
            {
                if (reader != null)
                    return reader;

                reader = CreateAndInitializeReader();
                return reader;
            }
        }

        /// <summary>
        /// Detects the byte order mark of a file and returns
        /// an appropriate encoding for the file.
        /// </summary>
        /// <param name="srcFile"></param>
        /// <returns></returns>
        public static Encoding GetFileEncoding(string srcFile) {
            Encoding enc = Encoding.Default;

            // *** Detect byte order mark if any - otherwise assume default
            byte[] buffer = new byte[5];
            FileStream file = new FileStream(srcFile, FileMode.Open);
            file.Read(buffer, 0, 5);
            file.Close();

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


        private StreamReader CreateAndInitializeReader() {
            var result = new StreamReader(FileName, GetFileEncoding(FileName));
            return result;
        }


        /// <summary>
        ///     test if end of file is reached
        /// </summary>
        protected override bool IsSourceAtEof
            => Reader.EndOfStream;

        /// <summary>
        ///     get the next input characterfrom trhefile
        /// </summary>
        /// <returns></returns>
        protected override char NextCharFromSource()
            => (char)Reader.Read();

        #region IDisposable Support
        private bool disposedValue = false;

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
        }

        #endregion

    }
}
