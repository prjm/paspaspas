namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     integer calculator
    /// </summary>
    public interface IIntegerCalculator {

        /// <summary>
        ///     add two numbers
        /// </summary>
        /// <param name="augend"></param>
        /// <param name="addend"></param>
        /// <returns></returns>
        IValue Add(IValue augend, IValue addend);

        /// <summary>
        ///     logical and
        /// </summary>
        /// <param name="firstOperand"></param>
        /// <param name="secondOperand"></param>
        /// <returns></returns>
        IValue And(IValue firstOperand, IValue secondOperand);

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
        ///     multiply two numbers
        /// </summary>
        /// <param name="multiplicand"></param>
        /// <param name="intMultiplier"></param>
        /// <returns></returns>
        IValue Multiply(IValue multiplicand, IValue intMultiplier);

        /// <summary>
        ///     negate a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        IValue Negate(IValue number);

        /// <summary>
        ///     invert a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        IValue Not(IValue number);

        /// <summary>
        ///     logical or
        /// </summary>
        /// <param name="firstOperand"></param>
        /// <param name="secondOperand"></param>
        /// <returns></returns>
        IValue Or(IValue firstOperand, IValue secondOperand);

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
        ///     subtract two numbers
        /// </summary>
        /// <param name="minuend"></param>
        /// <param name="subtrahend"></param>
        /// <returns></returns>
        IValue Subtract(IValue minuend, IValue subtrahend);

        /// <summary>
        ///     bitwise xor
        /// </summary>
        /// <param name="firstOperand"></param>
        /// <param name="secondOperand"></param>
        /// <returns></returns>
        IValue Xor(IValue firstOperand, IValue secondOperand);

    }
}
