namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     calculator for extended value
    /// </summary>
    public interface IFloatCalculator {

        /// <summary>
        ///     floating point division
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        IValue Divide(IValue dividend, IValue divisor);

        /// <summary>
        ///     negate a floating point value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IValue Negate(IValue value);

        /// <summary>
        ///     add two floating point numbers
        /// </summary>
        /// <param name="value1">first operand</param>
        /// <param name="value2">second operand</param>
        /// <returns>sum</returns>
        IValue Add(IValue value1, IValue value2);

        /// <summary>
        ///     subtract two floating point numbers
        /// </summary>
        /// <param name="value1">first operand</param>
        /// <param name="value2">second operand</param>
        /// <returns>difference</returns>
        IValue Subtract(IValue value1, IValue value2);

        /// <summary>
        ///     multiply two floating point numbers
        /// </summary>
        /// <param name="value1">first operand</param>
        /// <param name="value2">second operand</param>
        /// <returns>difference</returns>
        IValue Multiply(IValue value1, IValue value2);

        /// <summary>
        ///     test equality
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IValue Equal(IValue left, IValue right);

        /// <summary>
        ///     test inequality
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IValue NotEquals(IValue left, IValue right);

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IValue LessThen(IValue left, IValue right);

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IValue GreaterThenEqual(IValue left, IValue right);

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IValue GreaterThen(IValue left, IValue right);

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IValue LessThenOrEqual(IValue left, IValue right);
    }
}
