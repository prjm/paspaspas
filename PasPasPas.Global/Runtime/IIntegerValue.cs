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

        /// <summary>
        ///     bitwise shift left
        /// </summary>
        /// <param name="valueToShl">number of bits</param>
        /// <returns></returns>
        IValue Shl(IValue valueToShl);

        /// <summary>
        ///     bitwise shift right
        /// </summary>
        /// <param name="valueToShr">number of bits</param>
        /// <returns></returns>
        IValue Shr(IValue valueToShr);


        /// <summary>
        ///     divide this number by another number (integer division)
        /// </summary>
        /// <param name="divisor">number to divide through</param>
        /// <returns>division result</returns>
        IValue Divide(IValue divisor);

        /// <summary>
        ///     divide this number by another number and compute the remainder
        /// </summary>
        /// <param name="divisor"></param>
        /// <returns>remainder of division</returns>
        IValue Modulo(IValue divisor);

    }
}
