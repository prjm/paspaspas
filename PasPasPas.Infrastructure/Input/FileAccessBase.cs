using System;
using System.Collections.Generic;

namespace PasPasPas.Infrastructure.Input {

    /// <summary>
    ///    base class for file access
    /// </summary>
    public abstract class FileAccessBase : IFileAccess {

        private Lazy<IDictionary<string, IParserInput>> mockupFiles
            = new Lazy<IDictionary<string, IParserInput>>(()
                => new Dictionary<string, IParserInput>(StringComparer.OrdinalIgnoreCase));

        /// <summary>
        ///     open a file for reading
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns>opened file</returns>
        public IParserInput OpenFileForReading(IFileReference path) {

            if (path == null)
                throw new ArgumentNullException(nameof(path));

            IParserInput result;

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
        protected abstract IParserInput DoOpenFileForReading(IFileReference path);

        /// <summary>
        ///     add a one-time mockup-file
        /// </summary>
        /// <param name="input">file to add</param>
        public void AddOneTimeMockup(IParserInput input) {

            if (input == null)
                throw new ArgumentNullException(nameof(input));


            IFileReference path = input.FilePath;
            string fileName = path.FileName;

            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("Undefined filename", nameof(path));

            if (mockupFiles.Value.ContainsKey(fileName))
                throw new ArgumentException($"Duplicate mockup file {path}");

            mockupFiles.Value.Add(fileName, input);
        }

        /// <summary>
        ///     test if a given files exists
        /// </summary>
        /// <param name="file">file path</param>
        /// <returns><c>true</c> if the file exists</returns>
        public bool FileExists(IFileReference file) {

            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var fileNameWithoutPath = file.FileName;
            if (mockupFiles.IsValueCreated && mockupFiles.Value.ContainsKey(fileNameWithoutPath))
                return true;

            return DoCheckIfFileExists(file);
        }

        /// <summary>
        ///     checks if a file exists
        /// </summary>
        /// <param name="file">file to check</param>
        /// <returns><c>true</c> if the file exists</returns>
        protected abstract bool DoCheckIfFileExists(IFileReference file);

        /// <summary>
        ///     clear mockups
        /// </summary>
        public void ClearMockups() {
            mockupFiles.Value.Clear();
        }
    }
}
