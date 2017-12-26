namespace PasPasPas.Infrastructure.Common {

    /// <summary>
    ///     helper for constant operations
    /// </summary>
    public interface IRuntimeValues {

        /// <summary>
        ///     get special constants
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IValue this[SpecialConstantKind index] { get; }

        /// <summary>
        ///     get the value for a given number
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        IValue ToIntegerValue(sbyte number);

        /// <summary>
        ///     convert a char to a runtime value object
        /// </summary>
        /// <param name="singleChar"></param>
        /// <returns></returns>
        IValue ToValue(char singleChar);

        /// <summary>
        ///     convert a string to a runtime value object
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        IValue ToValue(string text);

        /// <summary>
        ///     negate a value
        /// </summary>
        /// <param name="value">value to negage</param>
        /// <returns></returns>
        object Negate(object value);

        /// <summary>
        ///     convert an integer to the appropriate runtime constant
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        object ToConstantInt(long result);

        /// <summary>
        ///     convert a double to the appropriate runtime constant
        /// </summary>
        /// <param name="value">parsed value</param>
        /// <returns>constant value</returns>
        IValue ToRealValue(double value);
    }
}