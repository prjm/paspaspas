namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     parameter reference kind
    /// </summary>
    public enum ParameterReferenceKind {

        /// <summary>
        ///     undefined parameter kind
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     constant parameter
        /// </summary>
        Const = 1,

        /// <summary>
        ///     parameter by reference
        /// </summary>
        Var = 2,

        /// <summary>
        ///     output parameter
        /// </summary>
        Out = 3
    }
}