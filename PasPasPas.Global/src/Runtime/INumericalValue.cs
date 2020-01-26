using SharpFloat.FloatingPoint;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     interface for numerical values
    /// </summary>
    public interface INumericalValue : IValue {

        /// <summary>
        ///     test if this value is negative
        /// </summary>
        bool IsNegative { get; }

        /// <summary>
        ///     get the extended value
        /// </summary>
        ExtF80 AsExtended { get; }

    }
}