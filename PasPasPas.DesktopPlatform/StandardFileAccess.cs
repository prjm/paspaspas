using PasPasPas.Infrastructure.Input;

namespace PasPasPas.DesktopPlatform {

    /// <summary>
    ///     standard file access
    /// </summary>
    public class StandardFileAccess : FileAccessBase {

        /// <summary>
        ///     open a textfile for reading
        /// </summary>
        /// <param name="path">path to the file</param>
        /// <returns>opened file</returns>
        protected override IParserInput DoOpenFileForReading(IFileReference path)
            => new FileInput(path);


    }
}
