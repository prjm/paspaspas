#nullable disable
namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     string type kind
    /// </summary>
    public enum StringTypeKind {

        /// <summary>
        ///     undefined type kind
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     ANSI string
        /// </summary>
        AnsiString = 1,

        /// <summary>
        ///     wide string type
        /// </summary>
        WideStringType = 2,

        /// <summary>
        ///     UNICODE string type
        /// </summary>
        UnicodeString = 3,

        /// <summary>
        ///     short string type
        /// </summary>
        ShortString = 4,
    }
}