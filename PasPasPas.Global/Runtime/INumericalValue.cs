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
        /// <param name="numberToAdd">number to add</param>
        /// <returns>result</returns>
        IValue Add(IValue numberToAdd);

        /// <summary>
        ///     subtract another number
        /// </summary>
        /// <param name="numberToSubtract">number to subtract</param>
        /// <returns>result</returns>
        IValue Subtract(IValue numberToSubtract);
    }
}