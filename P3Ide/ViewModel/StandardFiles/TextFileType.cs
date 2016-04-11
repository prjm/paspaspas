using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3Ide.ViewModel.StandardFiles {

    /// <summary>
    ///     text file type
    /// </summary>
    public class TextFileType : ISupportedFileType {

        /// <summary>
        ///     file description
        /// </summary>
        public string FileDescription
            => "Text file";

        /// <summary>
        ///     file extension
        /// </summary>
        public string FileExtension
            => ".txt";

    }
}
