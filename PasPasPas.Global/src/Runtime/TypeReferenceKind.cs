namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     distinction between different type reference kinds
    /// </summary>
    public enum TypeReferenceKind {

        /// <summary>
        ///     undefined reference
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     reference to a constant value
        /// </summary>
        ConstantValue = 1,

        /// <summary>
        ///     reference to a runtime value
        /// </summary>
        DynamicValue = 2,

        /// <summary>
        ///     reference to a type
        /// </summary>
        TypeName = 3,

        /// <summary>
        ///     result of a method invocation
        /// </summary>
        InvocationResult = 4
    }

}
