using PasPasPas.Globals.Files;

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
            => SearchPaths = new DerivedListOptionCollection<FileReference>(baseOptions?.SearchPaths);

        /// <summary>
        ///     search paths
        /// </summary>
        public DerivedListOptionCollection<FileReference> SearchPaths { get; internal set; }


        /// <summary>
        ///     clear path options
        /// </summary>
        public void Clear()
            => SearchPaths.ResetToDefault();
    }
}
