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


    }
}