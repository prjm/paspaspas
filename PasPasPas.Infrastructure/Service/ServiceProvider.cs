using System;
using System.Collections.Generic;

namespace PasPasPas.Infrastructure.Service {

    /// <summary>
    ///     main dependency injection facility
    /// </summary>
    public class ServiceProvider {

        private IDictionary<Guid, IDictionary<Guid, IService>> services
            = new Dictionary<Guid, IDictionary<Guid, IService>>();

        /// <summary>
        ///     resolution policy
        /// </summary>
        public IServiceResolutionPolicy Resolution { get; set; }

        /// <summary>
        ///     create a new service provider
        /// </summary>
        public ServiceProvider() {
            Resolution = new StandardServiceResolutionPolicy();
        }

        /// <summary>
        ///     register a service
        /// </summary>
        /// <param name="provider"></param>
        public void Register(IService provider) {
            IDictionary<Guid, IService> servicesForClass;

            if (!services.TryGetValue(provider.ServiceClassId, out servicesForClass)) {
                servicesForClass = new Dictionary<Guid, IService>();
                services.Add(provider.ServiceClassId, servicesForClass);
            }

            if (servicesForClass.ContainsKey(provider.ServiceId))
                throw new ArgumentException("Service " + provider.ServiceName + " already registered.");

            var serviceBase = provider as ServiceBase;

            if (serviceBase != null)
                serviceBase.RegisterServiceProvider(this);
        }

        /// <summary>
        ///     Resolve a service
        /// </summary>
        /// <param name="serviceClass">service class</param>
        /// <param name="context">context object</param>
        /// <returns></returns>
        public IService Resolve(Guid serviceClass, object context = null) {
            IDictionary<Guid, IService> servicesForClass;
            if (!services.TryGetValue(serviceClass, out servicesForClass)) {
                throw new ArgumentException("Invalid service class " + serviceClass);
            }

            var serviceId = Resolution.ResolveService(serviceClass, servicesForClass, context);

            if (serviceId == Guid.Empty) {
                throw new ArgumentException("No service resolved for service class " + serviceClass);
            }

            return servicesForClass[serviceId];
        }
    }
}
