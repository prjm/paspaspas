namespace PasPasPas.Infrastructure.Common {

    /// <summary>
    ///     interface for integer values
    /// </summary>
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
        /// <returns></returns>
        IValue Negate();
    }
}
