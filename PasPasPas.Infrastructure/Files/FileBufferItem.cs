using System.Text;

namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     item of a file buffer
    /// </summary>
    public class FileBufferItem {

        private readonly StringBuilder data
            = new StringBuilder();

        /// <summary>
        ///     file content
        /// </summary>
        public StringBuilder Data
            => data;

        /// <summary>
        ///     content length (in chars)
        /// </summary>
        public int Length
            => data.Length;

    }
}