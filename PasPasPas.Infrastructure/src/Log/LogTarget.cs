#nullable disable
using System;
using System.Collections.Generic;
using PasPasPas.Globals.Log;

namespace PasPasPas.Infrastructure.Log {

    /// <summary>
    ///     event arguments for a log message event
    /// </summary>
    public class LogMessageEventArgs : EventArgs {

        /// <summary>
        ///     create a new log message event
        /// </summary>
        /// <param name="message"></param>
        public LogMessageEventArgs(ILogMessage message)
            => Message = message ?? throw new ArgumentNullException(nameof(message));

        /// <summary>
        ///     log message
        /// </summary>
        public ILogMessage Message { get; }
    }

    /// <summary>
    ///     log target
    /// </summary>
    public class LogTarget : ILogTarget {

        /// <summary>
        ///     callback for log messages
        /// </summary>
        public event EventHandler<LogMessageEventArgs> ProcessMessage;

        /// <summary>
        ///     process a message
        /// </summary>
        /// <param name="message">message to process</param>
        public virtual void HandleMessage(ILogMessage message) => ProcessMessage?.Invoke(this, new LogMessageEventArgs(message));

        /// <summary>
        ///     log target was registered at manager
        /// </summary>
        /// <param name="logManager"></param>
        public void RegisteredAt(ILogManager logManager) {
            //..
        }

        /// <summary>
        ///     log target was unregistered at manager
        /// </summary>
        /// <param name="logManager"></param>
        public void UnregisteredAt(ILogManager logManager) {
            //..
        }

        /// <summary>
        ///     clear  event handlers
        /// </summary>
        public void ClearEventHandlers() => ProcessMessage = null;
    }

    /// <summary>
    ///     log target which save messages in a list
    /// </summary>
    public class ListLogTarget : LogTarget {

        /// <summary>
        ///     message list
        /// </summary>
        public IList<ILogMessage> Messages { get; }
            = new List<ILogMessage>();

        /// <summary>
        ///     adds a message to the list
        /// </summary>
        /// <param name="message">message to add</param>
        public override void HandleMessage(ILogMessage message) {
            base.HandleMessage(message);
            Messages.Add(message);
        }

    }

}
