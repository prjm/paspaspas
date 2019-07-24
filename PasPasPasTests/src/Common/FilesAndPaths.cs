using System.Collections.Generic;

namespace PasPasPasTests.src.Common {
    public class FilesAndPaths {

        private readonly Dictionary<string, string> data
            = new Dictionary<string, string>();

        /// <summary>
        ///     create a new set of files and paths
        /// </summary>
        /// <param name="p"></param>
        public FilesAndPaths(params (string name, string content)[] p) {
            foreach (var (name, content) in p)
                data.Add(name, content);
        }
    }
}
