﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

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
        public CompilerOptions() {
            var platforms = new Dictionary<PlatformKey, Platform>();
            var optionRoot = new DefaultPlatform();
            platforms.Add(PlatformKey.Default, optionRoot);
            platforms.Add(PlatformKey.AnyCpu, new AnyCpuPlatform(optionRoot.DefaultOptions));
            Platforms = new ReadOnlyDictionary<PlatformKey, Platform>(platforms);
        }

    }
}