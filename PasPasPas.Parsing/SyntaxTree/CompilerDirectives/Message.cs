using PasPasPas.Infrastructure.Log;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     message directive
    /// </summary>
    public class Message : SyntaxPartBase {

        /// <summary>
        ///     message text
        /// </summary>
        public string MessageText { get; set; }

        /// <summary>
        ///     message type
        /// </summary>
        public MessageSeverity MessageType { get; set; }
    }
}
