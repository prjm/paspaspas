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
        ITypeReference ToScaledIntegerValue(sbyte number);

        /// <summary>
        ///     get the value for a byte
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        ITypeReference ToScaledIntegerValue(byte number);

        /// <summary>
        ///     get the value for a given number
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        ITypeReference ToScaledIntegerValue(short number);

        /// <summary>
        ///     get the value for a given number
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        ITypeReference ToScaledIntegerValue(ushort number);

        /// <summary>
        ///     get the value for a given number
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        ITypeReference ToScaledIntegerValue(int number);

        /// <summary>
        ///     get the value for a given number
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        ITypeReference ToScaledIntegerValue(uint number);

        /// <summary>
        ///     get the value for a given number
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        ITypeReference ToScaledIntegerValue(long number);

        /// <summary>
        ///     get the value for a given number
        /// </summary>
        /// <param name="number">given number</param>
        /// <returns>converted value</returns>
        ITypeReference ToScaledIntegerValue(ulong number);

        /// <summary>
        ///     get the fixed value type for a give number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        ITypeReference ToIntegerValue(sbyte number);

        /// <summary>
        ///     get the fixed value type for a give number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        ITypeReference ToIntegerValue(byte number);

        /// <summary>
        ///     get the fixed value type for a give number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        ITypeReference ToIntegerValue(short number);

        /// <summary>
        ///     get the fixed value type for a give number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        ITypeReference ToIntegerValue(ushort number);

        /// <summary>
        ///     get the fixed value type for a give number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        ITypeReference ToIntegerValue(int number);

        /// <summary>
        ///     get the fixed value type for a give number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        ITypeReference ToIntegerValue(uint number);

        /// <summary>
        ///     get the fixed value type for a give number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        ITypeReference ToIntegerValue(long number);

        /// <summary>
        ///     get the fixed value type for a given number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        ITypeReference ToIntegerValue(ulong number);

        /// <summary>
        ///     get a native integer value for a given number
        /// </summary>
        /// <param name="number"></param>
        /// <param name="typeRegistry"></param>
        /// <returns></returns>
        ITypeReference ToNativeInt(ITypeReference number, ITypeRegistry typeRegistry);

    }
}