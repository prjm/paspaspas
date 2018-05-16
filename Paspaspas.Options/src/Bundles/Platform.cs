using System.Collections.Generic;

namespace PasPasPas.Options.Bundles {

    /// <summary>
    ///     platform-specific compiler settings
    /// </summary>
    public abstract class Platform {

        /// <summary>
        ///     platform key
        /// </summary>
        private readonly PlatformKey key;

        /// <summary>
        ///     Platform key
        /// </summary>
        public PlatformKey Key
            => key;

        /// <summary>
        ///     supported configurations
        /// </summary>
        public IDictionary<string, OptionSet> Configurations { get; }
            = new Dictionary<string, OptionSet>();

        /// <summary>
        ///     create a new platform entry
        /// </summary>
        /// <param name="key">platform key</param>        
        protected Platform(PlatformKey key) {
            this.key = key;
        }

    }
}