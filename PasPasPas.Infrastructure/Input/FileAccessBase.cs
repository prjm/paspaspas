using System;
using System.Collections.Generic;
using System.IO;

namespace PasPasPas.Infrastructure.Input {

    /// <summary>
    ///    base class for file access
    /// </summary>
    public abstract class FileAccessBase : IFileAccess {

        private Lazy<IDictionary<string, IParserInput>> mockupFiles
            = new Lazy<IDictionary<string, IParserInput>>(() => new Dictionary<string, IParserInput>(StringComparer.OrdinalIgnoreCase));

        /// <summary>
        ///     open a file for reading
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns>opened file</returns>
        public IParserInput OpenFileForReading(string path) {
            IParserInput result;

            if (mockupFiles.IsValueCreated && mockupFiles.Value.TryGetValue(path, out result))
                return result;

            result = DoOpenFileForReading(path);
            return result;
        }


        /// <summary>
        ///     open a text file for reading
        /// </summary>
        /// <param name="path">path to the file</param>
        /// <returns>input file</returns>
        protected abstract IParserInput DoOpenFileForReading(string path);

        /// <summary>
        ///     add a one-time mockup-file
        /// </summary>
        /// <param name="path">pseud-path</param>
        /// <param name="input">file to add</param>
        public void AddOneTimeMockup(string path, IParserInput input) {
            mockupFiles.Value.Add(path, input);
        }

        /// <summary>
        ///     test if a given files exists
        /// </summary>
        /// <param name="filePath">file path</param>
        /// <returns><c>true</c> if the file exists</returns>
        public bool FileExists(string filePath) {
            var fileNameWithoutPath = Path.GetFileName(filePath);
            if (mockupFiles.IsValueCreated && mockupFiles.Value.ContainsKey(fileNameWithoutPath))
                return true;

            return File.Exists(filePath);
        }

        /// <summary>
        ///     clear mockups
        /// </summary>
        public void ClearMockups() {
            mockupFiles.Value.Clear();
        }
    }
}
