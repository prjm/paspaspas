using System.Globalization;
using System.IO;
using PasPasPas.Globals.Log;
using PasPasPas.Infrastructure.Log;

namespace SampleRunner {

    internal class ConsoleLogListener : LogTarget {
        private TextWriter result;

        public ConsoleLogListener(TextWriter result) => this.result = result;

        public override void HandleMessage(ILogMessage message) {
            result.Write(message.MessageID.ToString("X4", CultureInfo.InvariantCulture));
            result.Write(": ");
            if (message.Data != default)
                foreach (var data in message.Data)
                    result.Write(data);
            result.WriteLine();
        }

    }
}