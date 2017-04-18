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
    }
}
