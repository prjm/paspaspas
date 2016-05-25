using System;
using System.Globalization;
using System.Resources;

namespace PasPasPas.Infrastructure.Log {

    /// <summary>
    ///     standard log source
    /// </summary>
    public class LogSource : ILogSource {

        private readonly ILogManager manager;
        private readonly Guid group;
        private ResourceManager resourceManager;

        /// <summary>
        ///     create a new log source
        /// </summary>
        /// <param name="logManager">used log manager</param>
        /// <param name="groupId">message group id</param>
        /// <param name="resources">resizrces</param>
        public LogSource(ILogManager logManager, Guid groupId, ResourceManager resources) {
            manager = logManager;
            group = groupId;
            resourceManager = resources;
        }

        /// <summary>
        ///     process a log message
        /// </summary>
        /// <param name="message"></param>
        public void ProcessMessage(ILogMessage message) {
            manager.RouteMessage(this, message);
        }

        /// <summary>
        ///     generate an error message
        /// </summary>
        /// <param name="id">message id</param>
        /// <param name="data">message parameters</param>
        public void Error(Guid id, params object[] data) {
            string text = resourceManager.GetString(string.Format(CultureInfo.InvariantCulture, "R_{0}", id.ToString("N").ToUpperInvariant()));
            ProcessMessage(new LogMessage(MessageSeverity.Error, group, id, text, data));
        }

    }
}
