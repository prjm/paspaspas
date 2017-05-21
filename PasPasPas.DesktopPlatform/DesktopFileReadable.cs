using System;
using System.IO;
using System.Text;
using PasPasPas.Infrastructure.Files;

namespace PasPasPas.DesktopPlatform {

    /// <summary>
    ///     file readable buffer
    /// </summary>
    public class DesktopFileReadable : IBufferReadable {

        private readonly IFileReference file;

        /// <summary>
        ///     Create a new file readable by reference
        /// </summary>
        /// <param name="fileReference">file reference</param>
        public DesktopFileReadable(IFileReference fileReference)
            => file = fileReference;

        private const int DefaultFileStreamBufferSize = 4096;

        /// <summary>
        ///     read the file
        /// </summary>
        /// <param name="item">buffer item</param>
        public void ToBufferItem(FileBufferItem item) {
            Encoding encoding = Encoding.UTF8;
            StringBuilder sb = item.Data;

            var bufferSize = encoding.GetMaxCharCount(DefaultFileStreamBufferSize);
            var count = bufferSize;

            char[] buffer = new char[bufferSize];

            sb.Clear();

            using (var fileStream = new FileStream(file.Path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultFileStreamBufferSize, FileOptions.SequentialScan))
            using (var reader = new StreamReader(fileStream, encoding, true, DefaultFileStreamBufferSize, false)) {

                while (count > 0) {
                    count = reader.Read(buffer, 0, bufferSize);
                    sb.Append(buffer, 0, count);
                }

            }
        }
    }
}
