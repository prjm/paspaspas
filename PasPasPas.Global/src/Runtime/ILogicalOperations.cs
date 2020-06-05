#nullable disable
namespace PasPasPas.Globals.Runtime {

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
        IValue AndOperator(IValue firstOperand, IValue secondOperand);

        /// <summary>
        ///     <c>or</c> operation
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        IValue OrOperator(IValue value1, IValue value2);

        /// <summary>
        ///     <c>xor</c> operation
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        IValue XorOperator(IValue value1, IValue value2);

        /// <summary>
        ///     <c>not</c> operation
        /// </summary>
        /// <param name="value">value to invert</param>
        /// <returns>inverted value</returns>
        IValue NotOperator(IValue value);

    }
}