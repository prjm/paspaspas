namespace PasPasPas.Infrastructure.Configuration {

    /// <summary>
    ///     common configuration settings
    /// </summary>
    public class CommonConfiguration : ISettingsService {

        /// <summary>
        ///     configuration settings
        /// </summary>
        public IConfigurationSettings Settings { get; }
        = new TypedConfigurationSettings();

    }
}
