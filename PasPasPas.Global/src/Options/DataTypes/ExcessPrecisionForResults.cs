#nullable disable
namespace PasPasPas.Globals.Options.DataTypes {

    /// <summary>
    ///     flag for precision excess under x64
    /// </summary>
    public enum ExcessPrecisionForResult {

        /// <summary>
        ///     undefined precision excess
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     disable excess
        /// </summary>
        EnableExcess = 1,

        /// <summary>
        ///     enable excess
        /// </summary>
        DisableExcess = 2,
    }
}