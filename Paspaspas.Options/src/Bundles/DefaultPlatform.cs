using PasPasPas.Globals.Environment;

namespace PasPasPas.Options.Bundles {

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
        public DefaultPlatform(IEnvironment environment) : base(PlatformKey.Default) {
            DefaultOptions = new OptionSet(environment);
            Configurations.Add(DefaultConfigurationName, DefaultOptions);
        }

        /// <summary>
        ///     default options
        /// </summary>
        public OptionSet DefaultOptions { get; }
    }
}
