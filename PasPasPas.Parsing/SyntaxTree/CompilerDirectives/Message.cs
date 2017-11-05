using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     message directive
    /// </summary>
    public class Message : CompilerDirectiveBase {

        /// <summary>
        ///     message text
        /// </summary>
        public string MessageText { get; set; }

        /// <summary>
        ///     message type
        /// </summary>
        public MessageSeverity MessageType { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

    }
}
