using PasPasPas.Infrastructure.Files;

namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     path specific option
    /// </summary>
    public class PathOptionSet {

        /// <summary>
        ///     Create new path options
        /// </summary>
        /// <param name="baseOptions">base options</param>
        public PathOptionSet(PathOptionSet baseOptions)
            => SearchPaths = new DerivedListOption<FileReference>(baseOptions?.SearchPaths);

        /// <summary>
        ///     search paths
        /// </summary>
        public DerivedListOption<FileReference> SearchPaths { get; internal set; }


        /// <summary>
        ///     clear path options
        /// </summary>
        public void Clear()
            => SearchPaths.ResetToDefault();
    }
}
