namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     path specific option
    /// </summary>
    public class PathOptionSet {

        /// <summary>
        ///     Create new path tions
        /// </summary>
        /// <param name="baseOptions">base options</param>
        public PathOptionSet(PathOptionSet baseOptions) {
            SearchPaths = new DerivedListOption<string>(baseOptions?.SearchPaths);
        }

        /// <summary>
        ///     search paths
        /// </summary>
        public DerivedListOption<string> SearchPaths { get; internal set; }


        /// <summary>
        ///     reset on new unitr
        /// </summary>
        public void ResetOnNewUnit() {
        }

        /// <summary>
        ///     clear path options
        /// </summary>
        public void Clear() {
            SearchPaths.ResetToDefault();
        }
    }
}
