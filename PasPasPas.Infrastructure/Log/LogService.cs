namespace PasPasPas.Infrastructure.Log {

    using System.Collections.Generic;

    /// <summary>
    ///     general log provider
    /// </summary>
    public class LogService : ILogService {

        /// <summary>
        ///     list of messages
        /// </summary>
        private List<LogMessage> messages
            = new List<LogMessage>();

        /// <summary>
        ///     logs a  message
        /// </summary>
        /// <param name="message">message to log</param>
        public void Log(LogMessage message) {
            messages.Add(message);
        }
    }
}
