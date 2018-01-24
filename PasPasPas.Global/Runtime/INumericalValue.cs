namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     interface for numerical values
    /// </summary>
    public interface INumericalValue : IValue {

        /// <summary>
        ///     test if the number is negative
        /// </summary>
        bool IsNegative { get; }

        /// <summary>
        ///    negate this value
        /// </summary>
        /// <remarks>can lead to integer overflow</remarks>
        /// <returns>negated value</returns>
        IValue Negate();

        /// <summary>
        ///     add another number
        /// </summary>
        /// <param name="append">number to add</param>
        /// <returns>result</returns>
        IValue Add(IValue append);

        /// <summary>
        ///     subtract another number
        /// </summary>
        /// <param name="subtrahend">number to subtract</param>
        /// <returns>result</returns>
        IValue Subtract(IValue subtrahend);

        /// <summary>
        ///     multiply this number with another number
        /// </summary>
        /// <param name="multiplier">number to multiply with</param>
        /// <returns>result</returns>
        IValue Multiply(IValue multiplier);

    }
}