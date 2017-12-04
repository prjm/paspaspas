namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     scope entry kind
    /// </summary>
    public enum ScopeEntryKind {

        /// <summary>
        ///     unknown scope entry
        /// </summary>
        Unknown,

        /// <summary>
        ///     type name
        /// </summary>
        TypeName,

        /// <summary>
        ///     variable name
        /// </summary>
        DeclaredVariable,

        /// <summary>
        ///     unit reference
        /// </summary>
        UnitReference,

        /// <summary>
        ///     callable object method
        /// </summary>
        ObjectMethod,

        /// <summary>
        ///     enum value
        /// </summary>
        EnumValue
    }
}