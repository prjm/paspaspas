using PasPasPas.Api.Options;

namespace PasPasPas.Internal.Options {

    /// <summary>
    ///     default platform
    /// </summary>
    public class DefaultPlatform : Platform {

        /// <summary>
        ///     default options
        /// </summary>
        public const string DefaultConfigurationName
            = "Default";

        /// <summary>
        ///     create a new platform
        /// </summary>
        public DefaultPlatform() : base(PlatformKey.Default) {
            DefaultOptions = new OptionSet();
            Configurations.Add(DefaultConfigurationName, DefaultOptions);
        }

        /// <summary>
        ///     default options
        /// </summary>
        public OptionSet DefaultOptions { get; }
    }
}
