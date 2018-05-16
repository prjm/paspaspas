namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     specifier kind
    /// </summary>
    public enum MethodDirectiveSpecifierKind {

        /// <summary>
        ///     unknown kind
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     dependency specification
        /// </summary>
        Dependency = 1,

        /// <summary>
        ///     delayed dependency specification
        /// </summary>
        Delayed = 2,

        /// <summary>
        ///     index specification
        /// </summary>
        Index = 3,

        /// <summary>
        ///     name specification
        /// </summary>
        Name = 4,
    }
}