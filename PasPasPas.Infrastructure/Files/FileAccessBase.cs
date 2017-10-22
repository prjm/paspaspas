using System;
using System.Collections.Generic;

namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///    abstract base class for file access
    /// </summary>
    public abstract class FileAccessBase : IFileAccess {

        private IDictionary<IFileReference, IBufferReadable> mockups =
            new Dictionary<IFileReference, IBufferReadable>();

        /// <summary>
        ///     create a new file access base clase
        /// </summary>
        protected FileAccessBase() { }

        /// <summary>
        ///     open a file for reading
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns>opened file</returns>
        public IBufferReadable OpenFileForReading(IFileReference path) {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            if (mockups.ContainsKey(path))
                return mockups[path];

            return DoOpenFileForReading(path ?? throw new ArgumentNullException(nameof(path)));
        }

        /// <summary>
        ///     open a text file for reading
        /// </summary>
        /// <param name="path">path to the file</param>
        /// <returns>input file</returns>
        protected abstract IBufferReadable DoOpenFileForReading(IFileReference path);

        /// <summary>
        ///     test if a given files exists
        /// </summary>
        /// <param name="filePath">file path</param>
        /// <returns><c>true</c> if the file exists</returns>
        public bool FileExists(IFileReference filePath) {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            if (mockups.ContainsKey(filePath))
                return true;

            return DoCheckIfFileExists(filePath);
        }

        /// <summary>
        ///     checks if a file exists
        /// </summary>
        /// <param name="file">file to check</param>
        /// <returns><c>true</c> if the file exists</returns>
        protected abstract bool DoCheckIfFileExists(IFileReference file);

        /// <summary>
        ///     create a new reference to a file
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns>file reference</returns>
        public abstract IFileReference ReferenceToFile(string path);

        /// <summary>
        ///     add a mockup file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public void AddMockupFile(IFileReference path, IBufferReadable content) {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            if (content == null)
                throw new ArgumentNullException(nameof(content));

            mockups.Add(path, content);
        }
    }
}
