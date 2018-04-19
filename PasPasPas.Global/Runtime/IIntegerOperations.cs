namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     integer calculator
    /// </summary>
    public interface IIntegerOperations :
        IArithmeticOperations, ILogicalOperations, IRelationalOperations {

        /// <summary>
        ///     invalid integer
        /// </summary>
        IValue Invalid { get; }

        /// <summary>
        ///     integer overflow
        /// </summary>
        IValue Overflow { get; }

        /// <summary>
        ///     integer division
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        IValue Divide(IValue dividend, IValue divisor);

        /// <summary>
        ///     integer remainder
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        IValue Modulo(IValue dividend, IValue divisor);

        /// <summary>
        ///     shift left
        /// </summary>
        /// <param name="firstOperand"></param>
        /// <param name="secondOperand"></param>
        /// <returns></returns>
        IValue Shl(IValue firstOperand, IValue secondOperand);

        /// <summary>
        ///     shift right
        /// </summary>
        /// <param name="firstOperand"></param>
        /// <param name="secondOperand"></param>
        /// <returns></returns>
        IValue Shr(IValue firstOperand, IValue secondOperand);


        /// <summary>
        ///     get the value for a signed byte
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        IValue ToScaledIntegerValue(sbyte number);

        /// <summary>
        ///     get the value for a byte
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        IValue ToScaledIntegerValue(byte number);

        /// <summary>
        ///     get the value for a given number
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        IValue ToScaledIntegerValue(short number);

        /// <summary>
        ///     get the value for a given number
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        IValue ToScaledIntegerValue(ushort number);

        /// <summary>
        ///     get the value for a given number
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        IValue ToScaledIntegerValue(int number);

        /// <summary>
        ///     get the value for a given number
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        IValue ToScaledIntegerValue(uint number);

        /// <summary>
        ///     get the value for a given number
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        IValue ToScaledIntegerValue(long number);

        /// <summary>
        ///     get the value for a given number
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        IValue ToScaledIntegerValue(ulong number);

    }
}
