namespace PasPasPas.Infrastructure.Service {

    /// <summary>
    ///     basic interface for services
    /// </summary>
    public interface IService {

        /// <summary>
        ///     service id
        /// </summary>
        System.Guid ServiceId { get; }

        /// <summary>
        ///     service class id
        /// </summary>
        System.Guid ServiceClassId { get; }

        /// <summary>
        ///     service name
        /// </summary>
        string ServiceName { get; }

    }
}
