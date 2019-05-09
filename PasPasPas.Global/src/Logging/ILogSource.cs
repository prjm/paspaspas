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
        ///     create a warning message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        void LogFatalError(Guid id, params object[] data);

        /// <summary>
        ///     log an error message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        void LogError(Guid id, params object[] data);

        /// <summary>
        ///     create a warning message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        void LogWarning(Guid id, params object[] data);

        /// <summary>
        ///     create a hint message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        void LogHint(Guid id, params object[] data);
    }
}
