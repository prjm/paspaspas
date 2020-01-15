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
        IOldTypeReference ToScaledIntegerValue(sbyte number);

        /// <summary>
        ///     get the value for a byte
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        IOldTypeReference ToScaledIntegerValue(byte number);

        /// <summary>
        ///     get the value for a given number
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        IOldTypeReference ToScaledIntegerValue(short number);

        /// <summary>
        ///     get the value for a given number
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        IOldTypeReference ToScaledIntegerValue(ushort number);

        /// <summary>
        ///     get the value for a given number
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        IOldTypeReference ToScaledIntegerValue(int number);

        /// <summary>
        ///     get the value for a given number
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        IOldTypeReference ToScaledIntegerValue(uint number);

        /// <summary>
        ///     get the value for a given number
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        IOldTypeReference ToScaledIntegerValue(long number);

        /// <summary>
        ///     get the value for a given number
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        IOldTypeReference ToScaledIntegerValue(ulong number);

        /// <summary>
        ///     get the fixed value type for a give number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        IOldTypeReference ToIntegerValue(sbyte number);

        /// <summary>
        ///     get the fixed value type for a give number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        IOldTypeReference ToIntegerValue(byte number);

        /// <summary>
        ///     get the fixed value type for a give number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        IOldTypeReference ToIntegerValue(short number);

        /// <summary>
        ///     get the fixed value type for a give number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        IOldTypeReference ToIntegerValue(ushort number);

        /// <summary>
        ///     get the fixed value type for a give number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        IOldTypeReference ToIntegerValue(int number);

        /// <summary>
        ///     get the fixed value type for a give number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        IOldTypeReference ToIntegerValue(uint number);

        /// <summary>
        ///     get the fixed value type for a give number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        IOldTypeReference ToIntegerValue(long number);

        /// <summary>
        ///     get the fixed value type for a given number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        IOldTypeReference ToIntegerValue(ulong number);

        /// <summary>
        ///     get a native integer value for a given number
        /// </summary>
        /// <param name="number"></param>
        /// <param name="typeRegistry"></param>
        /// <returns></returns>
        IOldTypeReference ToNativeInt(IOldTypeReference number, ITypeRegistry typeRegistry);

    }
}