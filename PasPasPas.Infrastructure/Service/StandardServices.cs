using PasPasPas.Infrastructure.Input;
using PasPasPas.Infrastructure.Log;
using System;

namespace PasPasPas.Infrastructure.Service {

    /// <summary>
    ///     standard service 
    /// </summary>
    public class StandardServices {

        /// <summary>
        ///     log manager
        /// </summary>
        public ILogManager LogManager { get; }
            = new LogManager();

        /// <summary>
        ///     file access
        /// </summary>
        public IFileAccess FileAccess { get; set; }

        /// <summary>
        ///     service class for common configuration settings
        /// </summary>
        public static readonly Guid ConfigurationServiceClass
            = new Guid("9716AB7F-3FBD-4EF6-B61B-A6D7D3255B1E");

    }
}
