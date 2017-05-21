using System.Collections.Generic;

namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     file buffer implementatio
    /// </summary>
    public class FileBuffer {

        private readonly IDictionary<FileReference, FileBufferItem> items;

        /// <summary>
        ///     create a new file buffer
        /// </summary>
        public FileBuffer()
            => items = new Dictionary<FileReference, FileBufferItem>();


    }
}
