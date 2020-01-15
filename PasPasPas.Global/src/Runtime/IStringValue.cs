namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     string value
    /// </summary>
    public interface IStringValue : IOldTypeReference {

        /// <summary>
        ///     get string value
        /// </summary>
        string AsUnicodeString { get; }

        /// <summary>
        ///     string length (measured in characters of the underlying char type)
        /// </summary>
        int NumberOfCharElements { get; }

        /// <summary>
        ///     char value at a given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IOldTypeReference CharAt(int index);
    }
}
