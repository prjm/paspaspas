namespace PasPasPas.Globals.Runtime {

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
        TypeName = 3

    }

}
