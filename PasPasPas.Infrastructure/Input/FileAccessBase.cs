using System;
using System.Collections.Generic;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Infrastructure.Input {

    using FileDictionary = IDictionary<string, IBufferReadable>;

    /// <summary>
    ///    abstract base class for file access
    /// </summary>
    public abstract class FileAccessBase : IFileAccess {

        private readonly Lazy<FileDictionary> mockupFiles;

        private FileDictionary CreateFileDictionary()
            => new Dictionary<string, IBufferReadable>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        ///     create a new file access base clase
        /// </summary>
        protected FileAccessBase()
            => mockupFiles = new Lazy<FileDictionary>(CreateFileDictionary);

        /// <summary>
        ///     open a file for reading
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns>opened file</returns>
        public IBufferReadable OpenFileForReading(IFileReference path) {

            if (path == null)
                ExceptionHelper.ArgumentIsNull(nameof(path));

            IBufferReadable result;

            if (mockupFiles.IsValueCreated && mockupFiles.Value.TryGetValue(path.FileName, out result))
                return result;

            result = DoOpenFileForReading(path);
            return result;
        }


        /// <summary>
        ///     open a text file for reading
        /// </summary>
        /// <param name="path">path to the file</param>
        /// <returns>input file</returns>
        protected abstract IBufferReadable DoOpenFileForReading(IFileReference path);

        /// <summary>
        ///     add a one-time mockup-file
        /// </summary>
        /// <param name="input">file to add</param>
        /// <param name="path">file path</param>
        public void AddOneTimeMockup(IFileReference path, IBufferReadable input) {

            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var fileName = path.FileName;

            if (string.IsNullOrEmpty(fileName))
                throw new InvalidOperationException("Undefined filename");

            if (mockupFiles.Value.ContainsKey(fileName))
                throw new InvalidOperationException(StringUtils.Invariant($"Duplicate mockup file {path}"));

            mockupFiles.Value.Add(fileName, input);
        }

        /// <summary>
        ///     test if a given files exists
        /// </summary>
        /// <param name="filePath">file path</param>
        /// <returns><c>true</c> if the file exists</returns>
        public bool FileExists(IFileReference filePath) {

            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            var fileNameWithoutPath = filePath.FileName;
            if (mockupFiles.IsValueCreated && mockupFiles.Value.ContainsKey(fileNameWithoutPath))
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
        ///     clear mockup files
        /// </summary>
        public void ClearMockups()
            => mockupFiles.Value.Clear();

    }
}
