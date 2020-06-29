namespace PasPasPas.Typings.Serialization {

    /// <summary>
    ///     stored constant kind
    /// </summary>
    public enum StoredConstantKind : int {

        /// <summary>
        ///     undefined constant
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     integer constant
        /// </summary>
        IntegerConstant = 1,

        /// <summary>
        ///     real constant
        /// </summary>
        RealConstant = 2,

        /// <summary>
        ///     boolean constant
        /// </summary>
        BooleanConstant = 3,

        /// <summary>
        ///     char constant
        /// </summary>
        CharConstant = 4,

        /// <summary>
        ///     string constant
        /// </summary>
        StringConstant = 5,
    }
}
