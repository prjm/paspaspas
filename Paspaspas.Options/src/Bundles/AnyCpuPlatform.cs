using PasPasPas.Globals.Options;

namespace PasPasPas.Options.Bundles {

    /// <summary>
    ///     any cpu platform
    /// </summary>
    public class AnyCpuPlatform : Platform {

        /// <summary>
        ///     creates a new platform for any cpu
        /// </summary>
        /// <param name="optionRoot">default options</param>
        internal AnyCpuPlatform(IOptionSet optionRoot) : base(PlatformKey.AnyCpu) {
            Configurations.Add(OptionSet.DebugConfigurationName, new OptionSet(optionRoot));
            Configurations.Add(OptionSet.ReleaseConfigurationName, new OptionSet(optionRoot));
        }

    }
}
