namespace PasPasPas.Infrastructure.Service {

    /// <summary>
    ///     base class for services
    /// </summary>
    public abstract class ServiceBase : IObjectBase {

        /// <summary>
        ///     services
        /// </summary>
        private ServiceProvider services = null;

        /// <summary>
        ///     Services
        /// </summary>
        public ServiceProvider Services
            => services;

        /// <summary>
        ///     register a service provider
        /// </summary>
        /// <param name="provider"></param>
        public void RegisterServiceProvider(ServiceProvider provider) {
            services = provider;
        }

    }
}
