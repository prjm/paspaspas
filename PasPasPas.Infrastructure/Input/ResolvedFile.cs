namespace PasPasPas.Infrastructure.Input {

    /// <summary>
    ///     definition of a resolved file
    /// </summary>
    public class ResolvedFile {

        /// <summary>
        ///     current directory
        /// </summary>
        public IFileReference CurrentDirectory { get; set; }

        /// <summary>
        ///     flag, <c>true</c> if the file exists
        /// </summary>
        public bool IsResolved { get; set; }

        /// <summary>
        ///     path to resolve
        /// </summary>
        public IFileReference PathToResolve { get; set; }

        /// <summary>
        ///     target path
        /// </summary>
        public IFileReference TargetPath { get; set; }
    }
}