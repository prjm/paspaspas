namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     global interface for integer values
    /// </summary>
    /// <remarks>
    ///     currently, integer values up to 8 byte length are supported
    /// </remarks>
    public interface IIntegerValue {

        /// <summary>
        ///     get the value as unsigned long
        /// </summary>
        ulong AsUnsignedLong { get; }

        /// <summary>
        ///     test if the number is negative
        /// </summary>
        bool IsNegative { get; }

        /// <summary>
        ///    negate this value
        /// </summary>
        /// <remarks>can lead to integer overflow</remarks>
        /// <returns>negated value</returns>
        IValue Negate();

    }
}
