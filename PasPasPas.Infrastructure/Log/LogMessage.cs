using System;
using System.Globalization;

namespace PasPasPas.Infrastructure.Log {

    /// <summary>
    ///     immutable, standard log message
    /// </summary>
    public class LogMessage : ILogMessage {

        private readonly object[] data;
        private readonly Guid group;
        private readonly Guid id;
        private readonly MessageSeverity severity;
        private readonly string text;

        /// <summary>
        ///     create a new log message
        /// </summary>
        /// <param name="messageId">message id</param>
        /// <param name="messageData">message data</param>
        /// <param name="groupId">group id</param>
        /// <param name="messageServerity">severity</param>
        /// <param name="messageText">message text</param>
        public LogMessage(MessageSeverity messageServerity, Guid groupId, Guid messageId, string messageText, params object[] messageData) {
            group = groupId;
            id = messageId;
            text = messageText;
            severity = messageServerity;
            data = messageData;
        }

        /// <summary>
        ///     format log message
        /// </summary>
        public string FormattedMessage => string.Format(CultureInfo.CurrentCulture, text, data);

        /// <summary>
        ///     groud id
        /// </summary>
        public Guid GroupID => group;

        /// <summary>
        ///     message id
        /// </summary>
        public Guid MessageID => id;

        /// <summary>
        ///     severity
        /// </summary>
        public MessageSeverity Severity => severity;

    }
}
