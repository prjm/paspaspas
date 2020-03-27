using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     interface for char values
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

        /// <summary>
        ///     char type kind
        /// </summary>
        CharTypeKind Kind { get; }
    }
}
