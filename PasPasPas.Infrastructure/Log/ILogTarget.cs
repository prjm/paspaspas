namespace PasPasPas.Infrastructure.Log {

    /// <summary>
    ///     target for log messages
    /// </summary>
    public interface ILogTarget {

        /// <summary>
        ///     unregister this target
        /// </summary>
        /// <param name="logManager"></param>
        void UnregisteredAt(LogManager logManager);

        /// <summary>
        ///     register this target
        /// </summary>
        /// <param name="logManager"></param>
        void RegisteredAt(LogManager logManager);

        /// <summary>
        ///     handle a message
        /// </summary>
        /// <param name="message">mesasge</param>
        void HandleMessage(ILogMessage message);
    }
}