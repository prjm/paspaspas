namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     calculator for boolean values
    /// </summary>
    public interface IBooleanCalculator {

        /// <summary>
        ///     <c>and</c> operation
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        IValue And(IValue value1, IValue value2);

        /// <summary>
        ///     <c>or</c> operation
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        IValue Or(IValue value1, IValue value2);

        /// <summary>
        ///     <c>xor</c> operation
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        IValue Xor(IValue value1, IValue value2);

        /// <summary>
        ///     <c>not</c> operation
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IValue Not(IValue value);

        /// <summary>
        ///     <c>==</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IValue Equal(IValue left, IValue right);

        /// <summary>
        ///     <c>&lt;&gt;</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IValue NotEquals(IValue left, IValue right);

        /// <summary>
        ///     <c>&lt;</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IValue LessThen(IValue left, IValue right);

        /// <summary>
        ///     <c>&gt;</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IValue GreaterThen(IValue left, IValue right);

        /// <summary>
        ///     <c>&lt;=</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IValue LessThenOrEqual(IValue left, IValue right);

        /// <summary>
        ///     <c>&gt;=</c>
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IValue GreaterThenEqual(IValue left, IValue right);
    }
}
