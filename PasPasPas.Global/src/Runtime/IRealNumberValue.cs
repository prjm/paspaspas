using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     real value
    /// </summary>
    public interface IRealNumberValue : INumericalValue {

        /// <summary>
        ///     get the real type for this constant
        /// </summary>
        IRealType RealType { get; }

    }
}
