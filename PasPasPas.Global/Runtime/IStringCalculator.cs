namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     calculator for string values
    /// </summary>
    public interface IStringCalculator {

        /// <summary>
        ///     concatenate two strings
        /// </summary>
        /// <param name="value1">first string</param>
        /// <param name="value2">second string</param>
        /// <returns>concatenated string</returns>
        IValue Concat(IValue value1, IValue value2);

        /// <summary>
        ///     compare equality
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IValue Equal(IValue left, IValue right);

        /// <summary>
        ///     compare inequality
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
        IValue GreaterThen(IValue left, IValue right);

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IValue LessThenOrEqual(IValue left, IValue right);

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IValue GreaterThenEqual(IValue left, IValue right);
    }
}

