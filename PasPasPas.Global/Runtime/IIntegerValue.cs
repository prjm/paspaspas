namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     integer value
    /// </summary>
    public interface IIntegerValue : IValue {

        /// <summary>
        ///     test if this value is negative
        /// </summary>
        bool IsNegative { get; }

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
