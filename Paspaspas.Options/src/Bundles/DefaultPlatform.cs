#nullable disable
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Options;

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
        public DefaultPlatform(IEnvironment environment, IInputResolver resolver) : base(PlatformKey.Default) {
            DefaultOptions = new OptionSet(resolver, environment);
            Configurations.Add(DefaultConfigurationName, DefaultOptions);
        }

        /// <summary>
        ///     default options
        /// </summary>
        public IOptionSet DefaultOptions { get; }
    }
}
