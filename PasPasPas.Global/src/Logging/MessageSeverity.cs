namespace PasPasPas.Globals.Log {

    /// <summary>
    ///     message severity
    /// </summary>
    public enum MessageSeverity {

        /// <summary>
        ///     undefined severity
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     debug message
        /// </summary>
        Debug = 1,

        /// <summary>
        ///     informational message
        /// </summary>
        Information = 2,

        /// <summary>
        ///     warning message
        /// </summary>
        Warning = 3,

        /// <summary>
        ///     error message
        /// </summary>
        Error = 4,

        /// <summary>
        ///     fatal error message
        /// </summary>
        FatalError = 5,

        /// <summary>
        ///     hint message
        /// </summary>
        Hint = 6,
    }
}