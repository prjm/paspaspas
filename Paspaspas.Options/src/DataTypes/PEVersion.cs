namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     pe version information
    /// </summary>
    public class PEVersion {

        /// <summary>
        ///     create a new pe version
        /// </summary>
        /// <param name="baseVersion">base version</param>
        public PEVersion(PEVersion baseVersion) {
            MajorVersion = new DerivedValueOption<int>(baseVersion?.MajorVersion);
            MinorVersion = new DerivedValueOption<int>(baseVersion?.MinorVersion);
        }

        /// <summary>
        ///     major version
        /// </summary>
        public DerivedValueOption<int> MajorVersion { get; }

        /// <summary>
        ///     minor version
        /// </summary>
        public DerivedValueOption<int> MinorVersion { get; }

        /// <summary>
        ///     clear values
        /// </summary>
        public void Clear() {
            MajorVersion.ResetToDefault();
            MinorVersion.ResetToDefault();
        }
    }
}