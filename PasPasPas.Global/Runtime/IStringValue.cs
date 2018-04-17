namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     string value
    /// </summary>
    public interface IStringValue : IValue {

        /// <summary>
        ///     get string value
        /// </summary>
        string AsUnicodeString { get; }

    }
}
