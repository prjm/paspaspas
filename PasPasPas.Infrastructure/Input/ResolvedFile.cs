namespace PasPasPas.Infrastructure.Input {

    /// <summary>
    ///     definition of a resolved file
    /// </summary>
    public class ResolvedFile {

        /// <summary>
        ///     current directory
        /// </summary>
        public string CurrentDirectory { get; set; }

        /// <summary>
        ///     flag, <c>true</c> if the file exists
        /// </summary>
        public bool IsResolved { get; set; }

        /// <summary>
        ///     path to resolve
        /// </summary>
        public string PathToResolve { get; set; }

        /// <summary>
        ///     target path
        /// </summary>
        public string TargetPath { get; set; }
    }
}