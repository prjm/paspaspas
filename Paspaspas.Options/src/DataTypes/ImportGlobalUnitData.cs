namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     compiler option for inter-unit variable access
    /// </summary>
    public enum ImportGlobalUnitData {

        /// <summary>
        ///     option undefined,
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     import unit data via references
        /// </summary>
        DoImport = 1,

        /// <summary>
        ///     do not import data via references
        /// </summary>
        NoImport = 2,
    }
}
