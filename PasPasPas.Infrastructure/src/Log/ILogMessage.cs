﻿namespace PasPasPas.Infrastructure.Log {

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
        System.Guid MessageID { get; }

        /// <summary>
        ///     group id
        /// </summary>
        System.Guid GroupID { get; }

    }
}