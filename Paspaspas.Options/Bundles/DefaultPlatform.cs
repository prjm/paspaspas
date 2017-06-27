using PasPasPas.Infrastructure.Files;
using PasPasPas.Infrastructure.Input;

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
        public DefaultPlatform(IFileAccess fileAccess) : base(PlatformKey.Default) {
            DefaultOptions = new OptionSet(fileAccess);
            Configurations.Add(DefaultConfigurationName, DefaultOptions);
        }

        /// <summary>
        ///     default options
        /// </summary>
        public OptionSet DefaultOptions { get; }
    }
}
