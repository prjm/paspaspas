namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     string value
    /// </summary>
    public interface IStringValue : ITypeReference {

        /// <summary>
        ///     get string value
        /// </summary>
        string AsUnicodeString { get; }

        /// <summary>
        ///     string length (measured in characters of the underlying char type)
        /// </summary>
        int NumberOfCharElements { get; }
    }
}
