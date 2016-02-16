﻿using PasPasPas.Api.Options;

namespace PasPasPas.Internal.Options {

    /// <summary>
    ///     any cpu platfrom
    /// </summary>
    public class AnyCpuPlatform : Platform {

        /// <summary>
        ///     creates a new platform for any cpu
        /// </summary>                                            
        /// <param name="optionRoot">default options</param>
        public AnyCpuPlatform(OptionSet optionRoot) : base(PlatformKey.AnyCPU) {
            Configurations.Add(OptionSet.DebugConfigurationName, new OptionSet(optionRoot));
            Configurations.Add(OptionSet.ReleaseConfigurationName, new OptionSet(optionRoot));

        }

    }
}
