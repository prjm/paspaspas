using PasPasPas.Globals.Files;

namespace PasPasPas.Globals.Options {

    /// <summary>
    ///     path options
    /// </summary>
    public interface IPathOptions {

        /// <summary>
        ///     search paths
        /// </summary>
        IEnumerableOptionCollection<FileReference> SearchPaths { get; }

        /// <summary>
        ///     clear path options
        /// </summary>
        void Clear();
    }
}
