#nullable disable
namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     meta types
    /// </summary>
    public enum MetaTypeKind {

        /// <summary>
        ///     undefined type
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     const type
        /// </summary>
        Const = 1,

        /// <summary>
        ///     unicode string
        /// </summary>
        UnicodeString = 2,

        /// <summary>
        ///     wide string
        /// </summary>
        WideString = 3,

        /// <summary>
        ///     short string
        /// </summary>
        ShortString = 4,

        /// <summary>
        ///     ANSI string
        /// </summary>
        AnsiString = 5,

        /// <summary>
        ///     string
        /// </summary>
        StringType = 6,

        /// <summary>
        ///     named type
        /// </summary>
        NamedType = 7,

        /// <summary>
        ///     generic pointer type
        /// </summary>
        PointerType = 8,

        /// <summary>
        ///     short string (System.ShortString) with length 255
        /// </summary>
        ShortStringDefault = 9,

    }

}