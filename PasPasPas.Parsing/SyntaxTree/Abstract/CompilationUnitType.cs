namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     type of compilation unit
    /// </summary>
    public enum CompilationUnitType {

        /// <summary>
        ///     unknown compilation unit type
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     unit
        /// </summary>
        Unit,

        /// <summary>
        ///     package
        /// </summary>
        Package,

        /// <summary>
        ///     library
        /// </summary>
        Library,

        /// <summary>
        ///     program
        /// </summary>
        Program,

    }
}
