using System;
using System.Collections.Generic;

namespace PasPasPas.Infrastructure.Log {

    /// <summary>
    ///     immutable, standard log message
    /// </summary>
    public class LogMessage : ILogMessage {

        private readonly object[] data;
        private readonly Guid group;
        private readonly Guid id;
        private readonly MessageSeverity severity;

        /// <summary>
        ///     create a new log message
        /// </summary>
        /// <param name="messageId">message id</param>
        /// <param name="messageData">message data</param>
        /// <param name="groupId">group id</param>
        /// <param name="messageSeverity">severity</param>
        public LogMessage(MessageSeverity messageSeverity, Guid groupId, Guid messageId, params object[] messageData) {

            if (messageSeverity == MessageSeverity.Undefined)
                throw new ArgumentOutOfRangeException(nameof(messageSeverity));

            if (groupId == default)
                throw new ArgumentOutOfRangeException(nameof(groupId));

            if (messageId == default)
                throw new ArgumentOutOfRangeException(nameof(messageId));

            group = groupId;
            id = messageId;
            severity = messageSeverity;
            data = messageData ?? Array.Empty<object>();
        }

        /// <summary>
        ///     message group id
        /// </summary>
        public Guid GroupID
            => group;

        /// <summary>
        ///     message id
        /// </summary>
        public Guid MessageID
            => id;

        /// <summary>
        ///     severity
        /// </summary>
        public MessageSeverity Severity
            => severity;


        /// <summary>
        ///     get message data
        /// </summary>
        public IList<object> Data
            => data;

    }
}
