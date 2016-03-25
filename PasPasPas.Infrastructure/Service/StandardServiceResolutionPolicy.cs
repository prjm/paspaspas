using System;
using System.Collections.Generic;
using System.Linq;

namespace PasPasPas.Infrastructure.Service {

    /// <summary>
    ///     provides a standard service resolution policy
    /// </summary>
    public class StandardServiceResolutionPolicy : IServiceResolutionPolicy {

        /// <summary>
        ///     resolve a service
        /// </summary>
        /// <param name="serviceClass">service class</param>
        /// <param name="registeredServices">registered services</param>
        /// <param name="context">context object</param>
        /// <returns>resolved service</returns>
        public Guid ResolveService(Guid serviceClass, IDictionary<Guid, IService> registeredServices, object context) {
            if (registeredServices.Count == 0)
                return Guid.Empty;

            if (registeredServices.Count == 1)
                return registeredServices.Keys.First();

            throw new InvalidOperationException("Service class " + serviceClass + " contains multiple services");
        }
    }
}
