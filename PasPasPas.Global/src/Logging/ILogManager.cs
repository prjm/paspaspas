using PasPasPas.Globals.Environment;

namespace PasPasPas.Globals.Log {

    /// <summary>
    ///     log message router
    /// </summary>
    public interface ILogManager : IEnvironmentItem {

        /// <summary>
        ///     route a log message
        /// </summary>
        /// <param name="message">message to route</param>
        /// <param name="source">log source</param>
        void RouteMessage(ILogSource source, ILogMessage message);

        /// <summary>
        ///     register a log target
        /// </summary>
        /// <param name="target">log target</param>
        void RegisterTarget(ILogTarget target);

        /// <summary>
        ///     unregister a log target
        /// </summary>
        /// <param name="target">log target</param>
        /// <returns><b>true</b> if a target was unregistered</returns>
        bool UnregisterTarget(ILogTarget target);

    }
}