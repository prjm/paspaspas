namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     known basic types
    /// </summary>
    public enum BaseType : byte {

        /// <summary>
        ///     unknown type
        /// </summary>
        Unkown = 0,

        /// <summary>
        ///     error type
        /// </summary>
        Error = 1,

        /// <summary>
        ///     type alias
        /// </summary>
        TypeAlias = 2,

        /// <summary>
        ///     subrange type
        /// </summary>
        Subrange = 3,

        /// <summary>
        ///     boolean type
        /// </summary>
        Boolean = 4,

        /// <summary>
        ///     ANSI char
        /// </summary>
        Char = 5,

        /// <summary>
        ///     string type
        /// </summary>
        String = 6,

        /// <summary>
        ///     integer type
        /// </summary>
        Integer = 7,

        /// <summary>
        ///     real type
        /// </summary>
        Real = 8,

        /// <summary>
        ///     pointer type
        /// </summary>
        Pointer = 9,

        /// <summary>
        ///     enumerated data type
        /// </summary>
        Enumeration = 10,

        /// <summary>
        ///     unit type
        /// </summary>
        Unit = 11,

    }
}
