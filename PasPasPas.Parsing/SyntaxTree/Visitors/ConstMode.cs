namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     constand declaration mode
    /// </summary>
    public enum ConstMode {

        /// <summary>
        ///     undefined mode
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     constants
        /// </summary>
        Const = 1,

        /// <summary>
        ///     resource strings
        /// </summary>
        ResourceString = 2

    }
}