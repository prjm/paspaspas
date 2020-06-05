#nullable disable
namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     rounding mode
    /// </summary>
    public enum RealNumberRoundingMode {

        /// <summary>
        ///     undefined rounding mode
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     round to the nearest integral number
        /// </summary>
        ToNearest = 1,

        /// <summary>
        ///     round down
        /// </summary>
        Down = 2,

        /// <summary>
        ///     round up
        /// </summary>
        Up = 3,

        /// <summary>
        ///     truncate
        /// </summary>
        Truncate = 4

    }
}
