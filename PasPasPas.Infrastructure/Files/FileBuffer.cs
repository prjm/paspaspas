using System;
using System.Collections.Generic;
using PasPasPas.Infrastructure.Utils;

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

        /// <summary>
        ///     add content to the bzffer
        /// </summary>
        /// <param name="reference">file path</param>
        /// <param name="content">content</param>
        public void Add(FileReference reference, IBufferReadable content) {
            if (items.ContainsKey(reference))
                ExceptionHelper.DuplicateKeyInDictionary(reference, nameof(reference));

            var item = new FileBufferItem();
            content.ToBufferItem(item);
            items.Add(reference, item);
        }

        /// <summary>
        ///     get buffer content by file reference
        /// </summary>
        /// <param name="index">file index</param>
        /// <returns></returns>
        public FileBufferItem this[FileReference index] {
            get {
                FileBufferItem result;
                if (!items.TryGetValue(index, out result))
                    ExceptionHelper.ArgumetOutOfRange(index, nameof(index));
                return result;
            }
        }
    }
}
