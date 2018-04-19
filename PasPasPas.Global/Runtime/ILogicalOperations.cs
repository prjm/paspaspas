namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     provides a set of logical operations
    /// </summary>
    public interface ILogicalOperations {

        /// <summary>
        ///     <c>and</c> operation
        /// </summary>
        /// <param name="firstOperand"></param>
        /// <param name="secondOperand"></param>
        /// <returns></returns>
        IValue And(IValue firstOperand, IValue secondOperand);

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
        /// <param name="value">value to invert</param>
        /// <returns>inverted value</returns>
        IValue Not(IValue value);

    }
}