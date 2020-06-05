#nullable disable
namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     debug information
    /// </summary>
    public enum DebugInformation {

        /// <summary>
        ///     undefined if debug information should be included
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     include debug info
        /// </summary>
        IncludeDebugInformation,

        /// <summary>
        ///     do not generate debug info
        /// </summary>
        NoDebugInfo,
    }
}