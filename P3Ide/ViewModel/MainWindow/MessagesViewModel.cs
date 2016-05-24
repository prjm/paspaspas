using PasPasPas.Infrastructure.Log;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3Ide.ViewModel.MainWindow {

    /// <summary>
    ///     messages
    /// </summary>
    public class MessagesViewModel : ToolWindowViewModel, ILogTarget {

        /// <summary>
        ///     log messages
        /// </summary>
        public ObservableCollection<ILogMessage> Messages { get; } =
            new ObservableCollection<ILogMessage>();

        public MessagesViewModel() {
            HandleMessage(new LogMessage(MessageSeverity.Information, new Guid(), new Guid(), "xxx", "d"));
        }

        /// <summary>
        ///     handle a message
        /// </summary>
        /// <param name="message"></param>
        public void HandleMessage(ILogMessage message) {
            Messages.Add(message);
        }

        /// <summary>
        ///     register
        /// </summary>
        /// <param name="logManager"></param>
        public void RegisteredAt(LogManager logManager) {
            Messages.Clear();
        }

        /// <summary>
        ///     unregister
        /// </summary>
        /// <param name="logManager"></param>
        public void UnregisteredAt(LogManager logManager) {
            //..
        }
    }
}
