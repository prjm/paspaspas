namespace PasPasPas.Infrastructure.Configuration {

    /// <summary>
    ///     basic interface for common settings
    /// </summary>
    public interface ISettingsService {

        /// <summary>
        ///     get settings
        /// </summary>
        IConfigurationSettings Settings { get; }

    }
}