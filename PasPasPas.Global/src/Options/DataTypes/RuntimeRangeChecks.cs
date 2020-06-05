#nullable disable
namespace PasPasPas.Globals.Options.DataTypes {

    /// <summary>
    ///     flag to generate runtime range checks
    /// </summary>
    public enum RuntimeRangeCheckMode {

        /// <summary>
        ///     undefined flag state
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     enable range checks
        /// </summary>
        EnableRangeChecks = 1,

        /// <summary>
        ///     disable range checks
        /// </summary>
        DisableRangeChecks = 2,
    }
}