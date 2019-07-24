﻿using System.Collections.Generic;
using PasPasPas.Globals.Files;
using PasPasPas.Infrastructure.Environment;

namespace PasPasPas.Building.Definition {

    /// <summary>
    ///     reference to one or more files
    /// </summary>
    public class FilesSetting : Setting, IFileReferenceSetting {

        /// <summary>
        ///     input path
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        ///     get a list of referenced files
        /// </summary>
        /// <returns></returns>
        public IList<FileReference> GetReferencedFiles(StringPool pool)
            => new List<FileReference> {
                new FileReference(Path)
            };
    }
}
