#nullable disable
using PasPasPas.Globals.Options;

namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     pe version information
    /// </summary>
    public class PEVersion : IPEVersion {

        /// <summary>
        ///     create a new pe version
        /// </summary>
        /// <param name="baseVersion">base version</param>
        public PEVersion(IPEVersion baseVersion) {
            MajorVersion = new DerivedValueOption<int>(baseVersion?.MajorVersion);
            MinorVersion = new DerivedValueOption<int>(baseVersion?.MinorVersion);
        }

        /// <summary>
        ///     major version
        /// </summary>
        public IOption<int> MajorVersion { get; }

        /// <summary>
        ///     minor version
        /// </summary>
        public IOption<int> MinorVersion { get; }

        /// <summary>
        ///     clear values
        /// </summary>
        public void Clear() {
            MajorVersion.ResetToDefault();
            MinorVersion.ResetToDefault();
        }
    }
}