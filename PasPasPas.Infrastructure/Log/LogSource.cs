using System;

namespace PasPasPas.Infrastructure.Log {

    /// <summary>
    ///     standard log source
    /// </summary>
    public class LogSource : ILogSource {

        private readonly ILogManager manager;
        private readonly Guid group;

        /// <summary>
        ///     create a new log source
        /// </summary>
        /// <param name="logManager">used log manager</param>
        /// <param name="groupId">message group id</param>
        public LogSource(ILogManager logManager, Guid groupId) {

            if (logManager == null)
                throw new ArgumentNullException(nameof(manager));

            if (groupId == default(Guid))
                throw new ArgumentOutOfRangeException(nameof(groupId));

            manager = logManager;
            group = groupId;
        }

        /// <summary>
        ///     process a log message
        /// </summary>
        /// <param name="message"></param>
        public void ProcessMessage(ILogMessage message) {

            if (message == null)
                throw new ArgumentNullException(nameof(message));

            manager.RouteMessage(this, message);
        }

        /// <summary>
        ///     generate an error message
        /// </summary>
        /// <param name="id">message id</param>
        /// <param name="data">message parameters</param>
        public void Error(Guid id, params object[] data) {
            MessageForGuid(MessageSeverity.Error, id, data);
        }

        /// <summary>
        ///     generate an warning message
        /// </summary>
        /// <param name="id">message id</param>
        /// <param name="data">message parameters</param>
        public void Warning(Guid id, params object[] data) {
            MessageForGuid(MessageSeverity.Warning, id, data);
        }

        /// <summary>
        ///     generate an information message
        /// </summary>
        /// <param name="id">message id</param>
        /// <param name="data">message parameters</param>
        public void Information(Guid id, params object[] data) {
            MessageForGuid(MessageSeverity.Information, id, data);
        }

        private void MessageForGuid(MessageSeverity severity, Guid id, object[] data) {
            ProcessMessage(new LogMessage(severity, group, id, data));
        }

    }
}
