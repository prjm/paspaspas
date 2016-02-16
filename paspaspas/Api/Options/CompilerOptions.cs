using PasPasPas.Internal.Options;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PasPasPas.Api.Options {

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
        public Platform ActivePlatform
            = null;

        /// <summary>
        ///     creates a new set of options
        /// </summary>
        public CompilerOptions() {
            var platforms = new Dictionary<PlatformKey, Platform>();
            var optionRoot = new DefaultPlatform();
            platforms.Add(PlatformKey.Default, optionRoot);
            platforms.Add(PlatformKey.AnyCPU, new AnyCpuPlatform(optionRoot.DefaultOptions));
            Platforms = new ReadOnlyDictionary<PlatformKey, Platform>(platforms);
        }

    }
}