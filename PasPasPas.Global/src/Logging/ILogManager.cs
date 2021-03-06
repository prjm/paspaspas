﻿#nullable disable
namespace PasPasPas.Globals.Log {

    /// <summary>
    ///     log message router
    /// </summary>
    public interface ILogManager {

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

        /// <summary>
        ///     create a new log source
        /// </summary>
        /// <param name="logGroup"></param>
        /// <returns></returns>
        ILogSource CreateLogSource(uint logGroup);
    }
}