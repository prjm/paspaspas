namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     current part of the unit
    /// </summary>
    public enum UnitMode {

        /// <summary>
        ///     unknown part
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     interface
        /// </summary>
        Interface = 1,

        /// <summary>
        ///     implementation
        /// </summary>
        Implementation = 2,

        /// <summary>
        ///     package - requires
        /// </summary>
        Contains = 3,

        /// <summary>
        ///     package - contains
        /// </summary>
        Requires = 4,

        /// <summary>
        ///     program
        /// </summary>
        Program = 5,

        /// <summary>
        ///     library
        /// </summary>
        Library = 6
    }
}
