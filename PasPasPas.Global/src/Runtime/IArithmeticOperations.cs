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
        IValue Negate(IValue number);

        /// <summary>
        ///     provide an identity function
        /// </summary>
        /// <param name="number">input number</param>
        /// <returns>same number</returns>
        IValue Identity(IValue number);

        /// <summary>
        ///     add two numbers
        /// </summary>
        /// <param name="augend">number</param>
        /// <param name="addend">number to add</param>
        /// <returns>sum</returns>
        IValue Add(IValue augend, IValue addend);

        /// <summary>
        ///     subtract two numbers
        /// </summary>
        /// <param name="minuend"></param>
        /// <param name="subtrahend"></param>
        /// <returns></returns>
        IValue Subtract(IValue minuend, IValue subtrahend);

        /// <summary>
        ///     multiply two numbers
        /// </summary>
        /// <param name="multiplicand"></param>
        /// <param name="intMultiplier"></param>
        /// <returns></returns>
        IValue Multiply(IValue multiplicand, IValue intMultiplier);

    }
}