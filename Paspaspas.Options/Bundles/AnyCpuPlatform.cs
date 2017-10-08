namespace PasPasPas.Options.Bundles {

    /// <summary>
    ///     any cpu platfrom
    /// </summary>
    public class AnyCpuPlatform : Platform {

        /// <summary>
        ///     creates a new platform for any cpu
        /// </summary>                                            
        /// <param name="optionRoot">default options</param>
        public AnyCpuPlatform(OptionSet optionRoot) : base(PlatformKey.AnyCpu) {
            Configurations.Add(OptionSet.DebugConfigurationName, new OptionSet(optionRoot, optionRoot.Environment));
            Configurations.Add(OptionSet.ReleaseConfigurationName, new OptionSet(optionRoot, optionRoot.Environment));
        }

    }
}
