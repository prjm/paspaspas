namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     arithmetic operations
    /// </summary>
    public interface IArithmeticOperations {

        /// <summary>
        ///     negate a number
        /// </summary>
        /// <param name="number">input number</param>
        /// <returns>negative number</returns>
        ITypeReference Negate(ITypeReference number);

        /// <summary>
        ///     provide an identity function
        /// </summary>
        /// <param name="number">input number</param>
        /// <returns>same number</returns>
        ITypeReference Identity(ITypeReference number);

        /// <summary>
        ///     add two numbers
        /// </summary>
        /// <param name="augend">number</param>
        /// <param name="addend">number to add</param>
        /// <returns>sum</returns>
        ITypeReference Add(ITypeReference augend, ITypeReference addend);

        /// <summary>
        ///     subtract two numbers
        /// </summary>
        /// <param name="minuend"></param>
        /// <param name="subtrahend"></param>
        /// <returns></returns>
        ITypeReference Subtract(ITypeReference minuend, ITypeReference subtrahend);

        /// <summary>
        ///     multiply two numbers
        /// </summary>
        /// <param name="multiplicand"></param>
        /// <param name="intMultiplier"></param>
        /// <returns></returns>
        ITypeReference Multiply(ITypeReference multiplicand, ITypeReference intMultiplier);

    }
}