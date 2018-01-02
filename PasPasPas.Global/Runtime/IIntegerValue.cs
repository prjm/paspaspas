namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     global interface for integer values
    /// </summary>
    /// <remarks>
    ///     currently, integer values up to 8 byte length are supported
    /// </remarks>
    public interface IIntegerValue : INumericalValue {

        /// <summary>
        ///     get the value as unsigned long
        /// </summary>
        ulong AsUnsignedLong { get; }

    }
}
