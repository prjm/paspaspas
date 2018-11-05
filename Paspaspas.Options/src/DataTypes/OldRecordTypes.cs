namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     switch for old packed record types
    /// </summary>
    public enum OldRecordTypeMode {

        /// <summary>
        ///     undefined flag state
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     disable old type layout
        /// </summary>
        DisableOldRecordPacking = 1,

        /// <summary>
        ///     enable old type layout
        /// </summary>
        EnableOldRecordPacking = 2,
    }
}