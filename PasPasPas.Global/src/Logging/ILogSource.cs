using System;

namespace PasPasPas.Globals.Log {

    /// <summary>
    ///     interface for log event generators
    /// </summary>
    public interface ILogSource {

        /// <summary>
        ///     log manager
        /// </summary>
        ILogManager Manager { get; }

        /// <summary>
        ///     log an error message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        void LogError(Guid id, params object[] data);

        /// <summary>
        ///     process a generated message
        /// </summary>
        /// <param name="message">log message</param>
        void ProcessMessage(ILogMessage message);

    }
}
