﻿using PasPasPas.Infrastructure.Input;

namespace PasPasPas.DesktopPlatform {

    /// <summary>
    ///     standard file access
    /// </summary>
    public class StandardFileAccess : FileAccessBase {

        /// <summary>
        ///     check if a file exists
        /// </summary>
        /// <param name="file">file to check</param>
        /// <returns><c>true</c> if the file exists</returns>
        protected override bool DoCheckIfFileExists(IFileReference file)
            => System.IO.File.Exists(file.Path);

        /// <summary>
        ///     open a textfile for reading
        /// </summary>
        /// <param name="path">path to the file</param>
        /// <returns>opened file</returns>
        protected override IParserInput DoOpenFileForReading(IFileReference path)
            => new FileInput(path);


    }
}