using PasPasPas.Globals.Files;
using PasPasPas.Globals.Options;

namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     path specific option
    /// </summary>
    public class PathOptionSet : IPathOptions {

        /// <summary>
        ///     Create new path options
        /// </summary>
        /// <param name="baseOptions">base options</param>
        public PathOptionSet(IPathOptions baseOptions)
            => SearchPaths = new DerivedListOptionCollection<FileReference>(baseOptions?.SearchPaths);

        /// <summary>
        ///     search paths
        /// </summary>
        public IEnumerableOption<FileReference> SearchPaths { get; internal set; }


        /// <summary>
        ///     clear path options
        /// </summary>
        public void Clear()
            => SearchPaths.ResetToDefault();
    }
}
