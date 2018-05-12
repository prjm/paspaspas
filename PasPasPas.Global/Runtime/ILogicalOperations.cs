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
        ITypeReference AndOperator(ITypeReference firstOperand, ITypeReference secondOperand);

        /// <summary>
        ///     <c>or</c> operation
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        ITypeReference Or(ITypeReference value1, ITypeReference value2);

        /// <summary>
        ///     <c>xor</c> operation
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        ITypeReference XorOperator(ITypeReference value1, ITypeReference value2);

        /// <summary>
        ///     <c>not</c> operation
        /// </summary>
        /// <param name="value">value to invert</param>
        /// <returns>inverted value</returns>
        ITypeReference Not(ITypeReference value);

    }
}