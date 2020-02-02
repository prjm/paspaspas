using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     integer converter
    /// </summary>
    public interface IIntegerConverter {

        /// <summary>
        ///     get the value for a signed byte
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        IValue ToScaledIntegerValue(sbyte number);

        /// <summary>
        ///     get the value for a byte
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
        ///     get the fixed value type for a give number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        IValue ToIntegerValue(sbyte number);

        /// <summary>
        ///     get the fixed value type for a give number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        IValue ToIntegerValue(byte number);

        /// <summary>
        ///     get the fixed value type for a give number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        IValue ToIntegerValue(short number);

        /// <summary>
        ///     get the fixed value type for a give number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        IValue ToIntegerValue(ushort number);

        /// <summary>
        ///     get the fixed value type for a give number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        IValue ToIntegerValue(int number);

        /// <summary>
        ///     get the fixed value type for a give number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        IValue ToIntegerValue(uint number);

        /// <summary>
        ///     get the fixed value type for a give number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        IValue ToIntegerValue(long number);

        /// <summary>
        ///     get the fixed value type for a given number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        IValue ToIntegerValue(ulong number);

        /// <summary>
        ///     get a native integer value for a given number
        /// </summary>
        /// <param name="number"></param>
        /// <param name="typeRegistry"></param>
        /// <returns></returns>
        IValue ToNativeInt(IValue number, ITypeRegistry typeRegistry);

    }
}