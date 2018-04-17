namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     integer value
    /// </summary>
    public interface IIntegerValue : INumericalValue {

        /// <summary>
        ///     signed value
        /// </summary>
        long SignedValue { get; }

        /// <summary>
        ///     unsigned value
        /// </summary>
        ulong UnsignedValue { get; }
    }
}
