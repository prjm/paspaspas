using System;
using System.Collections.Generic;
using PasPasPas.Infrastructure.Environment;

namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     standard file access
    /// </summary>
    public class StandardFileAccess : IFileAccess, IEnvironmentItem {

        private IDictionary<IFileReference, IBufferReadable> mockups =
            new Dictionary<IFileReference, IBufferReadable>();

        /// <summary>
        ///     item count
        /// </summary>
        public int Count
            => -1;

        /// <summary>
        ///     file access
        /// </summary>
        public string Caption
            => "StandardFileAccess";

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

        /// <summary>
        ///     create a new reference to a file
        /// </summary>
        /// <param name="path">file reference</param>
        /// <param name="pool">string pool to use</param>
        /// <returns></returns>
        public IFileReference ReferenceToFile(StringPool pool, string path)
            => new FileReference(pool, path);

        /// <summary>
        ///     check if a file exists
        /// </summary>
        /// <param name="file">file to check</param>
        /// <returns><c>true</c> if the file exists</returns>
        protected bool DoCheckIfFileExists(IFileReference file)
            => System.IO.File.Exists(file.Path);

        /// <summary>
        ///     open a textfile for reading
        /// </summary>
        /// <param name="path">path to the file</param>
        /// <returns>opened file</returns>
        protected IBufferReadable DoOpenFileForReading(IFileReference path)
            => new FileBufferReadable(path);


    }
}
