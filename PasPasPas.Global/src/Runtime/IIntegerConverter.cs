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
        /// <param name="typeDef">type definition</param>
        /// <returns></returns>
        IValue ToIntegerValue(ITypeDefinition typeDef, sbyte number);

        /// <summary>
        ///     get the fixed value type for a give number
        /// </summary>
        /// <param name="number"></param>
        /// <param name="typeDef">type definition</param>
        /// <returns></returns>
        IValue ToIntegerValue(ITypeDefinition typeDef, byte number);

        /// <summary>
        ///     get the fixed value type for a give number
        /// </summary>
        /// <param name="number"></param>
        /// <param name="typeDef"></param>
        /// <returns></returns>
        IValue ToIntegerValue(ITypeDefinition typeDef, short number);

        /// <summary>
        ///     get the fixed value type for a give number
        /// </summary>
        /// <param name="number"></param>
        /// <param name="typeDef"></param>
        /// <returns></returns>
        IValue ToIntegerValue(ITypeDefinition typeDef, ushort number);

        /// <summary>
        ///     get the fixed value type for a give number
        /// </summary>
        /// <param name="number"></param>
        /// <param name="typeDef"></param>
        /// <returns></returns>
        IValue ToIntegerValue(ITypeDefinition typeDef, int number);

        /// <summary>
        ///     get the fixed value type for a give number
        /// </summary>
        /// <param name="number"></param>
        /// <param name="typeDef"></param>
        /// <returns></returns>
        IValue ToIntegerValue(ITypeDefinition typeDef, uint number);

        /// <summary>
        ///     get the fixed value type for a give number
        /// </summary>
        /// <param name="number"></param>
        /// <param name="typeDef"></param>
        /// <returns></returns>
        IValue ToIntegerValue(ITypeDefinition typeDef, long number);

        /// <summary>
        ///     get the fixed value type for a given number
        /// </summary>
        /// <param name="number"></param>
        /// <param name="typeDef"></param>
        /// <returns></returns>
        IValue ToIntegerValue(ITypeDefinition typeDef, ulong number);

        /// <summary>
        ///     get a native integer value for a given number
        /// </summary>
        /// <param name="number"></param>
        /// <param name="typeRegistry"></param>
        /// <returns></returns>
        IOldTypeReference ToNativeInt(IOldTypeReference number, ITypeRegistry typeRegistry);

    }
}