namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     assembler statement kind
    /// </summary>
    public enum AssemblerStatementKind {

        /// <summary>
        ///     undefined kind
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     params pseudo-op
        /// </summary>
        ParamsOperation = 1,

        /// <summary>
        ///     pushenv pseudo-op
        /// </summary>
        PushEnvOperation = 2,

        /// <summary>
        ///     savenec pseudo-op
        /// </summary>
        SaveEnvOperation = 3,

        /// <summary>
        ///     noframe pseudo-op
        /// </summary>
        NoFrameOperation = 4,
    }
}