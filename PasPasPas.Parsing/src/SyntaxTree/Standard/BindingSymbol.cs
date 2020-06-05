#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     binding directive
    /// </summary>
    public class BindingSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new binding symbol
        /// </summary>
        /// <param name="directive"></param>
        /// <param name="messageExpression"></param>
        /// <param name="semicolon"></param>
        public BindingSymbol(Terminal directive, ExpressionSymbol messageExpression, Terminal semicolon) {
            Directive = directive;
            MessageExpression = messageExpression;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     directive kind
        /// </summary>
        public int Kind
            => Directive.GetSymbolKind();

        /// <summary>
        ///     message expression
        /// </summary>
        public ExpressionSymbol MessageExpression { get; }

        /// <summary>
        ///     directive
        /// </summary>
        public Terminal Directive { get; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Directive.GetSymbolLength() +
                MessageExpression.GetSymbolLength() +
                Semicolon.GetSymbolLength();

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Directive, visitor);
            AcceptPart(this, MessageExpression, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }

    }
}
