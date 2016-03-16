namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     flag for precission excess under x64
    /// </summary>
    public enum ExcessPrecisionForResults {

        /// <summary>
        ///     undefined precision excess
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     disable excess
        /// </summary>
        EnableExcess = 1,

        /// <summary>
        ///     enable excess
        /// </summary>
        DisableExcess = 2,
    }
}