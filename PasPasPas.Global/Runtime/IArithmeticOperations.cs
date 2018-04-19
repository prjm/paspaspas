namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     arithmetic operations
    /// </summary>
    public interface IArithmeticOperations {

        /// <summary>
        ///     negate a number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        IValue Negate(IValue number);

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