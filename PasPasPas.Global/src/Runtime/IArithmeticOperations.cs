namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     arithmetic operations
    /// </summary>
    public interface IArithmeticOperations {

        /// <summary>
        ///     negate a number
        /// </summary>
        /// <param name="number">input number</param>
        /// <returns>negative number</returns>
        IOldTypeReference Negate(IOldTypeReference number);

        /// <summary>
        ///     provide an identity function
        /// </summary>
        /// <param name="number">input number</param>
        /// <returns>same number</returns>
        IOldTypeReference Identity(IOldTypeReference number);

        /// <summary>
        ///     add two numbers
        /// </summary>
        /// <param name="augend">number</param>
        /// <param name="addend">number to add</param>
        /// <returns>sum</returns>
        IOldTypeReference Add(IOldTypeReference augend, IOldTypeReference addend);

        /// <summary>
        ///     subtract two numbers
        /// </summary>
        /// <param name="minuend"></param>
        /// <param name="subtrahend"></param>
        /// <returns></returns>
        IOldTypeReference Subtract(IOldTypeReference minuend, IOldTypeReference subtrahend);

        /// <summary>
        ///     multiply two numbers
        /// </summary>
        /// <param name="multiplicand"></param>
        /// <param name="intMultiplier"></param>
        /// <returns></returns>
        IOldTypeReference Multiply(IOldTypeReference multiplicand, IOldTypeReference intMultiplier);

    }
}