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

        /// <summary>
        ///     invert this integer value
        /// </summary>
        /// <returns>inverted value</returns>
        IValue Not();

        /// <summary>
        ///     bitwise and
        /// </summary>
        /// <param name="valueToAnd">operand</param>
        /// <returns>result of bitwise and</returns>
        IValue And(IValue valueToAnd);

        /// <summary>
        ///     bitwise or
        /// </summary>
        /// <param name="valueToOr">operand</param>
        /// <returns>result of bitwise or</returns>
        IValue Or(IValue valueToOr);

        /// <summary>
        ///     bitwise xor
        /// </summary>
        /// <param name="valueToXor">operand</param>
        /// <returns>result of bitwise xor</returns>
        IValue Xor(IValue valueToXor);
    }
}
