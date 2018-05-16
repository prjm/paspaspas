namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     type of a compilation unit
    /// </summary>
    public enum CompilationUnitType {

        /// <summary>
        ///     unknown compilation unit
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     standard unit (common file extension: <code>.pas</code>)
        /// </summary>
        Unit,

        /// <summary>
        ///     package (common file extension: <code>.bpr</code>)
        /// </summary>
        Package,

        /// <summary>
        ///     library
        /// </summary>
        Library,

        /// <summary>
        ///     program (common file extension: <code>.dpr</code>)
        /// </summary>
        Program,

    }
}
