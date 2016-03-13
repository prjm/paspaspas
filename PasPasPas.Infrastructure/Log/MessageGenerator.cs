
namespace PasPasPas.Infrastructure.Log {

    using System;


    /// <summary>
    ///     event arguments to log messages
    /// </summary>
    public class LogMessageEventArgs : EventArgs {

        private LogMessage message;

        /// <summary>
        ///     creates a new log message event argument object
        /// </summary>
        /// <param name="message">message to log</param>
        public LogMessageEventArgs(LogMessage message) {
            this.message = message;
        }

        /// <summary>
        ///     log message
        /// </summary>
        public LogMessage Message
            => message;
    }

    /// <summary>
    ///     message generating object
    /// </summary>
    public class MessageGenerator {

        /// <summary>
        ///     log message event
        /// </summary>
        public event EventHandler<LogMessageEventArgs> LogMessage;

        /// <summary>
        ///     log a message
        /// </summary>
        /// <param name="message">message to log</param>
        protected void OnLogMessage(LogMessage message) {
            if (LogMessage != null) {
                LogMessage(this, new LogMessageEventArgs(message));
            }
        }

        /// <summary>
        ///     log an error
        /// </summary>
        /// <param name="messageId">message id</param>
        /// <param name="data">message data</param>
        protected void LogError(int messageId, params object[] data) {
            var message = new LogMessage(messageId, data) { Level = LogLevel.Error };
            OnLogMessage(message);
        }

    }

}
