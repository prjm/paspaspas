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
        ///     hidden type
        /// </summary>
        Hidden = 1,

        /// <summary>
        ///     error type
        /// </summary>
        Error = 2,

        /// <summary>
        ///     type alias
        /// </summary>
        TypeAlias = 3,

        /// <summary>
        ///     subrange type
        /// </summary>
        Subrange = 4,

        /// <summary>
        ///     boolean type
        /// </summary>
        Boolean = 5,

        /// <summary>
        ///     ANSI char
        /// </summary>
        Char = 6,

        /// <summary>
        ///     string type
        /// </summary>
        String = 7,

        /// <summary>
        ///     integer type
        /// </summary>
        Integer = 8,

        /// <summary>
        ///     real type
        /// </summary>
        Real = 9,

        /// <summary>
        ///     pointer type
        /// </summary>
        Pointer = 10,

        /// <summary>
        ///     enumerated data type
        /// </summary>
        Enumeration = 11,

        /// <summary>
        ///     unit type
        /// </summary>
        Unit = 12,

        /// <summary>
        ///     array type
        /// </summary>
        Array = 13,

        /// <summary>
        ///     file type
        /// </summary>
        File = 14,

        /// <summary>
        ///     set type
        /// </summary>
        Set = 15,

        /// <summary>
        ///     meta class type
        /// </summary>
        MetaClass = 16,

        /// <summary>
        ///     generic type parameter
        /// </summary>
        GenericTypeParameter = 16,

        /// <summary>
        ///     routine type
        /// </summary>
        Routine = 17,

        /// <summary>
        ///     structured type
        /// </summary>
        Structured = 18,

    }
}
