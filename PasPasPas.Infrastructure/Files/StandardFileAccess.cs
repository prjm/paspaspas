namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     standard file access
    /// </summary>
    public class StandardFileAccess : FileAccessBase {

        /// <summary>
        ///     create a new reference to a file
        /// </summary>
        /// <param name="path">file reference</param>
        /// <returns></returns>
        public override IFileReference ReferenceToFile(string path)
            => new FileReference(path);

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
        protected override IBufferReadable DoOpenFileForReading(IFileReference path)
            => new FileBufferReadable(path);


    }
}
