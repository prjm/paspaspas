namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     string value
    /// </summary>
    public interface IStringValue : ITypeReference {

        /// <summary>
        ///     get string value
        /// </summary>
        string AsUnicodeString { get; }

    }
}
