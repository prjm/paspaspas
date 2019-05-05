using PasPasPas.Globals.Log;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     message directive
    /// </summary>
    public class Message : CompilerDirectiveBase {
        private readonly Terminal symbol;
        private readonly Terminal kind;
        private readonly Terminal text;

        /// <summary>
        ///     create a new message directive
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="kind"></param>
        /// <param name="text"></param>
        /// <param name="message"></param>
        /// <param name="textValue"></param>
        public Message(Terminal symbol, Terminal kind, Terminal text, MessageSeverity message, string textValue) {
            this.symbol = symbol;
            this.kind = kind;
            this.text = text;
            MessageType = message;
            MessageText = textValue;
        }

        /// <summary>
        ///     message text
        /// </summary>
        public string MessageText { get; }

        /// <summary>
        ///     message type
        /// </summary>
        public MessageSeverity MessageType { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, symbol, visitor);
            AcceptPart(this, kind, visitor);
            AcceptPart(this, text, visitor);
            visitor.EndVisit(this);
        }

    }
}
