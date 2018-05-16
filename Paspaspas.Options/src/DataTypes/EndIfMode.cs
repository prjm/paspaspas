namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///    if / ifend mode
    /// </summary>
    public enum EndIfMode {

        /// <summary>
        ///     undefined flag state
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     enable legacy mode
        /// </summary>
        LegacyIfEnd = 1,

        /// <summary>
        ///     modern standard mode
        /// </summary>
        Standard = 2,
    }
}