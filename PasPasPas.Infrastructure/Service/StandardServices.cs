using System;

namespace PasPasPas.Infrastructure.Service {

    /// <summary>
    ///     standard service keys
    /// </summary>
    public static class StandardServices {

        /// <summary>
        ///     service class for logging
        /// </summary>
        public static readonly Guid LoggingServiceClass
            = new Guid("068B55FE-BE82-4018-9071-5B1AE052762E");

        /// <summary>
        ///     service class for common configuration settings
        /// </summary>
        public static readonly Guid ConfigurationServiceClass
            = new Guid("9716AB7F-3FBD-4EF6-B61B-A6D7D3255B1E");

        /// <summary>
        ///     service class for compiler configuration settings
        /// </summary>
        public static readonly Guid CompilerConfigurationServiceClass
            = new Guid("4ED72AC0-5137-4907-B9D6-A497D484C8FB");

        /// <summary>
        ///     service class for file access
        /// </summary>
        public static readonly Guid FileAccessServiceClass
            = new Guid("0241B6D7-BFEF-46DE-9588-2FFE83F640A7");

    }
}
