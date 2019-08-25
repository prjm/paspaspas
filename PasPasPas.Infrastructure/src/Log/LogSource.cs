using System;
using PasPasPas.Globals.Log;

namespace PasPasPas.Infrastructure.Log {

    /// <summary>
    ///     standard log source
    /// </summary>
    public class LogSource : ILogSource {
        private readonly uint group;

        /// <summary>
        ///     associated log manager
        /// </summary>
        public ILogManager Manager { get; }

        /// <summary>
        ///     create a new log source
        /// </summary>
        /// <param name="logManager">used log manager</param>
        /// <param name="groupId">message group id</param>
        public LogSource(ILogManager logManager, uint groupId) {
            if (groupId == default)
                throw new ArgumentOutOfRangeException(nameof(groupId));

            Manager = logManager ?? throw new ArgumentNullException(nameof(logManager));
            group = groupId;
        }

        /// <summary>
        ///     process a log message
        /// </summary>
        /// <param name="message"></param>
        public void ProcessMessage(ILogMessage message) {

            if (message == null)
                throw new ArgumentNullException(nameof(message));

            Manager.RouteMessage(this, message);
        }

        /// <summary>
        ///     generate an error message
        /// </summary>
        /// <param name="id">message id</param>
        /// <param name="data">message parameters</param>
        public void LogError(uint id, params object[] data)
            => MessageForId(MessageSeverity.Error, id, data);

        /// <summary>
        ///     generate a warning message
        /// </summary>
        /// <param name="id">message id</param>
        /// <param name="data">message parameters</param>
        public void LogWarning(uint id, params object[] data)
            => MessageForId(MessageSeverity.Warning, id, data);

        /// <summary>
        ///     generate a fatal error message
        /// </summary>
        /// <param name="id">message id</param>
        /// <param name="data">message parameters</param>
        public void LogFatalError(uint id, params object[] data)
            => MessageForId(MessageSeverity.FatalError, id, data);

        /// <summary>
        ///     generate a hint message
        /// </summary>
        /// <param name="id">message id</param>
        /// <param name="data">message parameters</param>
        public void LogHint(uint id, params object[] data)
            => MessageForId(MessageSeverity.Hint, id, data);


        /// <summary>
        ///     generate an information message
        /// </summary>
        /// <param name="id">message id</param>
        /// <param name="data">message parameters</param>
        public void Information(uint id, params object[] data)
            => MessageForId(MessageSeverity.Information, id, data);

        /// <summary>
        ///        log a debug message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public void LogDebug(uint id, params object[] data)
            => MessageForId(MessageSeverity.Debug, id, data);

        private void MessageForId(MessageSeverity severity, uint id, object[] data)
            => ProcessMessage(new LogMessage(severity, group, id, data));


    }
}
