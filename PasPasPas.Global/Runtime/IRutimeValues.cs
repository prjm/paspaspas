using SharpFloat.FloatingPoint;

namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     helper for constant operations
    /// </summary>
    public interface IRuntimeValues {

        /// <summary>
        ///     integer calculator with type scaling
        /// </summary>
        IIntegerCalculator ScaledIntegerCalculator { get; }

        /// <summary>
        ///     calculator for extended values
        /// </summary>
        IFloatCalculator FloatCalculator { get; }

        /// <summary>
        ///     calculator for boolean values
        /// </summary>
        IBooleanCalculator BooleanCalculator { get; }

        /// <summary>
        ///     calculator for sting value
        /// </summary>
        IStringCalculator StringCalculator { get; }

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
        IValue ToScaledIntegerValue(sbyte number);

        /// <summary>
        ///     get the value for a given number
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        IValue ToScaledIntegerValue(byte number);

        /// <summary>
        ///     get the value for a given number
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        IValue ToScaledIntegerValue(short number);

        /// <summary>
        ///     get the value for a given number
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        IValue ToScaledIntegerValue(ushort number);

        /// <summary>
        ///     get the value for a given number
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        IValue ToScaledIntegerValue(int number);

        /// <summary>
        ///     get the value for a given number
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        IValue ToScaledIntegerValue(uint number);

        /// <summary>
        ///     get the value for a given number
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        IValue ToScaledIntegerValue(long number);

        /// <summary>
        ///     get the value for a given number
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        IValue ToScaledIntegerValue(ulong number);

        /// <summary>
        ///     convert a string to a runtime value object
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        IValue ToUnicodeString(string text);

        /// <summary>
        ///     convert a char to a runtime value object
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        IValue ToWideCharValue(char character);



        /// <summary>
        ///     convert a double to the appropriate runtime constant
        /// </summary>
        /// <param name="value">parsed value</param>
        /// <returns>constant value</returns>
        IValue ToExtendedValue(in ExtF80 value);
    }
}