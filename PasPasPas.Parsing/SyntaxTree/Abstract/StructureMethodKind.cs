namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     method kind
    /// </summary>
    public enum StructureMethodKind {

        /// <summary>
        ///     undefined method kind
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     function
        /// </summary>
        Function = 1,

        /// <summary>
        ///     procedure
        /// </summary>
        Procedure = 2,

        /// <summary>
        ///     constructor
        /// </summary>
        Constructor = 3,

        /// <summary>
        ///     destructor
        /// </summary>
        Destructor = 4,

        /// <summary>
        ///     operator
        /// </summary>
        Operator = 5,
    }
}
