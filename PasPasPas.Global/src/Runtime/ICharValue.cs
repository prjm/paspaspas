namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     widechar runtime value (utf-16)
    /// </summary>
    public interface ICharValue : IOrdinalValue {

        /// <summary>
        ///     get the wide char value
        /// </summary>
        char AsWideChar { get; }

        /// <summary>
        ///     get the ANSI char value
        /// </summary>
        byte AsAnsiChar { get; }

        /// <summary>
        ///     string value
        /// </summary>
        string AsUnicodeString { get; }
    }
}
