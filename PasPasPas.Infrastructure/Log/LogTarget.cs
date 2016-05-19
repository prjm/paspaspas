using System;

namespace PasPasPas.Infrastructure.Log {

    /// <summary>
    ///     event arguments for a log message event
    /// </summary>
    public class LogMessageEvent : EventArgs {

        /// <summary>
        ///     logged message
        /// </summary>
        private ILogMessage message;

        /// <summary>
        ///     create a new log message event
        /// </summary>
        /// <param name="message"></param>
        public LogMessageEvent(ILogMessage message) {
            this.message = message;
        }

        /// <summary>
        ///     log message
        /// </summary>
        public ILogMessage Message => message;
    }

    /// <summary>
    ///     log target
    /// </summary>
    public class LogTarget : ILogTarget {

        /// <summary>
        ///     callback for log messages
        /// </summary>
        public event EventHandler<LogMessageEvent> ProcessMessage;

        /// <summary>
        ///     process a message
        /// </summary>
        /// <param name="message">message to process</param>
        public void HandleMessage(ILogMessage message) {
            if (ProcessMessage != null)
                ProcessMessage(this, new LogMessageEvent(message));
        }

        /// <summary>
        ///     log target was registered at manager
        /// </summary>
        /// <param name="logManager"></param>
        public void RegisteredAt(LogManager logManager) {
            //..
        }

        /// <summary>
        ///     log target was unregistered at manager
        /// </summary>
        /// <param name="logManager"></param>
        public void UnregisteredAt(LogManager logManager) {
            //..
        }
    }
}
