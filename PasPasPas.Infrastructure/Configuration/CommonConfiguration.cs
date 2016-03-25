using System;
using PasPasPas.Infrastructure.Service;

namespace PasPasPas.Infrastructure.Configuration {

    /// <summary>
    ///     common configuration settings
    /// </summary>
    public class CommonConfiguration : ISettingsService, IService {

        /// <summary>
        ///     get service class id
        /// </summary>
        public Guid ServiceClassId => StandardServices.ConfigurationServiceClass;

        /// <summary>
        ///     get service id
        /// </summary>
        public Guid ServiceId => ConfigurationServiceId;

        private static readonly Guid ConfigurationServiceId
            = new Guid("D000A1D0-AD09-4434-A0BD-F208749853FE");

        /// <summary>
        ///     resolve the service name
        /// </summary>
        public string ServiceName => "CommonConfiguration";

        /// <summary>
        ///     configuration settings
        /// </summary>
        public IConfigurationSettings Settings { get; }
        = new TypedConfigurationSettings();

    }
}
