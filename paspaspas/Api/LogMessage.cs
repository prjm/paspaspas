using PasPasPas.Internal;
using System.Collections.Generic;
using System.Globalization;

namespace PasPasPas.Api {

    /// <summary>
    ///     log level
    /// </summary>
    public enum LogLevel {

        /// <summary>
        ///     undefined level
        /// </summary>
        Undefined,

        /// <summary>
        ///     informational message
        /// </summary>
        Information,

        /// <summary>
        ///     warning
        /// </summary>
        Warning,

        /// <summary>
        ///     error
        /// </summary>
        Error
    }

    /// <summary>
    ///     messages
    /// </summary>
    public class LogMessage {

        /// <summary>
        ///     create a new log message
        /// </summary>
        /// <param name="messageId">message id</param>
        /// <param name="data">message data</param>
        public LogMessage(int messageId, object[] data) {
            Id = messageId;
            Data = new List<object>(data);
        }

        /// <summary>
        ///     message data
        /// </summary>
        public IList<object> Data { get; }


        /// <summary>
        ///     message id
        /// </summary>
        public int Id { get; }

        /// <summary>
        ///     log level
        /// </summary>
        public LogLevel Level { get; internal set; }

        /// <summary>
        ///     format message as simple string
        /// </summary>
        /// <returns></returns>
        public string ToSimpleString() =>
            string.Format(CultureInfo.CurrentCulture, "{0} {1}:", MessageData.FormatMessageLevel(Level), Id);
    }
}
