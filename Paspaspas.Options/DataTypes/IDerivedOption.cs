namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     common interface for derived settings
    /// </summary>
    public interface IDerivedOption {

        /// <summary>
        ///     test if the default is overriden
        /// </summary>
        bool OverwritesDefaultValue { get; }

        /// <summary>
        ///     resets to default
        /// </summary>
        void ResetToDefault();

    }
}
