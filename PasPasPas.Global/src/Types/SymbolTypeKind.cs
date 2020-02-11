namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     symbol type kind
    /// </summary>
    public enum SymbolTypeKind : byte {

        /// <summary>
        ///     undefined symbol kind
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     type definition
        /// </summary>
        TypeDefinition = 1,

        /// <summary>
        ///     routine group
        /// </summary>
        RoutineGroup = 2,

        /// <summary>
        ///     constant value
        /// </summary>
        Constant = 3,

        /// <summary>
        ///     invocation result
        /// </summary>
        InvocationResult = 4,
    }
}
