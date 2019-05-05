using System.Collections.Generic;
using System.Collections.ObjectModel;
using PasPasPas.Globals.Environment;

namespace PasPasPas.Options.Bundles {


    /// <summary>
    ///     compiler options
    /// </summary>
    public class CompilerOptions {

        /// <summary>
        ///     supported platforms
        /// </summary>
        public IReadOnlyDictionary<PlatformKey, Platform> Platforms { get; }

        /// <summary>
        ///     active platform
        /// </summary>
        public Platform ActivePlatform { get; set; }
            = null;

        /// <summary>
        ///     creates a new set of options
        /// </summary>
        /// <param name="environment">environment</param>
        public CompilerOptions(IEnvironment environment) {
            var platforms = new Dictionary<PlatformKey, Platform>();
            var optionRoot = new DefaultPlatform(environment);
            platforms.Add(PlatformKey.Default, optionRoot);
            platforms.Add(PlatformKey.AnyCpu, new AnyCpuPlatform(optionRoot.DefaultOptions));
            Platforms = new ReadOnlyDictionary<PlatformKey, Platform>(platforms);
        }

    }
}