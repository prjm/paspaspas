#nullable disable
using System.Collections.Generic;

namespace PasPasPas.Globals.Log {

    /// <summary>
    ///     interface for log messages (immutable)
    /// </summary>
    public interface ILogMessage {


        /// <summary>
        ///     message severity
        /// </summary>
        MessageSeverity Severity { get; }

        /// <summary>
        ///     message id
        /// </summary>
        uint MessageID { get; }

        /// <summary>
        ///     message group id
        /// </summary>
        uint GroupID { get; }

        /// <summary>
        ///     data
        /// </summary>
        IList<object> Data { get; }
    }
}