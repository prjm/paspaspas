namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     statement kind
    /// </summary>
    public enum StructuredStatementKind {


        /// <summary>
        ///     unknown statement
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     raise statement
        /// </summary>
        Raise = 1,

        /// <summary>
        ///     at statement
        /// </summary>
        RaiseAtOnly = 2,

        /// <summary>
        ///     raise at statement
        /// </summary>
        RaiseAt = 3,

        /// <summary>
        ///     try / finally statement
        /// </summary>
        TryFinally = 4,

        /// <summary>
        ///     try / except statement
        /// </summary>
        TryExcept = 5,

        /// <summary>
        ///     except / else statement
        /// </summary>
        ExceptElse = 6,

        /// <summary>
        ///     except handler
        /// </summary>
        ExceptOn = 7,

        /// <summary>
        ///     with statement
        /// </summary>
        With = 8,

        /// <summary>
        ///     for / to statement
        /// </summary>
        ForTo = 9,

        /// <summary>
        ///     for / downto statement
        /// </summary>
        ForDownTo = 10,

        /// <summary>
        ///     for / in statement
        /// </summary>
        ForIn = 11,

        /// <summary>
        ///     while statement
        /// </summary>
        While = 12,

        /// <summary>
        ///     repeat statement
        /// </summary>
        Repeat = 13,
    }
}
