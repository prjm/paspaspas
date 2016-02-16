using PasPasPas.Api.Input;
using PasPasPas.Internal;
using PasPasPas.Internal.Log;
using PasPasPas.Internal.Parser;
using PasPasPas.Internal.Tokenizer;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasPasPas.Api {

    /// <summary>
    ///     public interface for a pascal paser
    /// </summary>
    public class PascalParser {

        private IList<LogMessage> messages
            = new List<LogMessage>();

        /// <summary>
        ///     parser input
        /// </summary>
        public IParserInput Input { get; set; }

        /// <summary>
        ///     log messages
        /// </summary>
        public IList<LogMessage> Messages
            => messages;

        /// <summary>
        ///     test if there are error messages
        /// </summary>
        /// <returns><c>true</c> if there are error messages</returns>
        public bool HasErrors()
            => messages.Any(t => t.Level == LogLevel.Error);

        /// <summary>
        ///     prints out the grammer
        /// </summary>
        /// <param name="result">accepted grammar</param>
        public static void PrintGrammar(StringBuilder result) {
            StandardParser.PrintGrammar(result);
        }

        /// <summary>
        ///     save a log message
        /// </summary>
        /// <param name="message"></param>
        public void ProcessMessage(LogMessage message) {
            messages.Add(message);
        }

        /// <summary>
        ///     run the parser
        /// </summary>
        public ISyntaxPart Run() {
            IPascalTokenizer tokenizer = CreateTokenizer();
            IPascalParser parser = new StandardParser();
            parser.BaseTokenizer = tokenizer;
            return parser.Parse();
        }

        private IPascalTokenizer CreateTokenizer() {
            var result = new StandardTokenizer();
            result.LogMessage += LogMessage; ;
            result.Input = Input;
            return result;
        }

        /// <summary>
        ///     log a message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogMessage(object sender, LogMessageEventArgs e) {
            messages.Add(e.Message);
        }
    }
}
