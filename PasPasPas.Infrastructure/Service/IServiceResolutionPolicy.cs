using System;
using System.Collections.Generic;

namespace PasPasPas.Infrastructure.Service {

    /// <summary>
    ///     resolve a service for a service class
    /// </summary>
    public interface IServiceResolutionPolicy {

        /// <summary>
        ///     resolve a service for a given class
        /// </summary>
        /// <param name="serviceClass">service class</param>
        /// <param name="registeredServices">registered services</param>
        /// <param name="context">context object</param>
        /// <returns></returns>
        Guid ResolveService(Guid serviceClass, IDictionary<Guid, IService> registeredServices, object context);

    }
}
