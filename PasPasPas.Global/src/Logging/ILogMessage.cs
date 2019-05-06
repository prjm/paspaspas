using System;

namespace PasPasPas.Globals.Log {

    /// <summary>
    ///     a common log message (immutable)
    /// </summary>
    public interface ILogMessage {


        /// <summary>
        ///     message severity
        /// </summary>
        MessageSeverity Severity { get; }

        /// <summary>
        ///     message id
        /// </summary>
        Guid MessageID { get; }

        /// <summary>
        ///     message group id
        /// </summary>
        uint GroupID { get; }

    }
}