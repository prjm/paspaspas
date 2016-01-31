
using PasPasPas.Api;

namespace PasPasPas.Internal {

    /// <summary>
    ///     helper class for message data
    /// </summary>
    public static class MessageData {

        /// <summary>
        ///     unexpected eof
        /// </summary>
        public const int UnexpectedEndOfFile = 1000;

        /// <summary>
        ///     incomplete hex number
        /// </summary>
        public const int IncompleteHexNumber = 1001;

        /// <summary>
        ///     incomplete identifier
        /// </summary>
        public const int IncompleteIdentifier = 1002;

        /// <summary>
        ///     undefined input
        /// </summary>
        public const int UndefinedInputToken = 1003;

        /// <summary>
        ///     unexptected token
        /// </summary>
        public const int UnexpectedToken = 2000;

        /// <summary>
        ///    format message level 
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static object FormatMessageLevel(LogLevel level) {
            switch (level) {
                case LogLevel.Error:
                    return "Error";
                case LogLevel.Warning:
                    return "Warning";
                case LogLevel.Information:
                    return "Information";
            }
            return "Undefined";
        }
    }
}
