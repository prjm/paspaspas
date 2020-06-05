#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     disp id directive
    /// </summary>
    public class DispIdSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new dispid  symbol
        /// </summary>
        /// <param name="dispId"></param>
        /// <param name="expression"></param>
        /// <param name="semicolon"></param>
        public DispIdSymbol(Terminal dispId, ExpressionSymbol expression, Terminal semicolon) {
            DispId = dispId;
            DispExpression = expression;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     disp id expression
        /// </summary>
        public ExpressionSymbol DispExpression { get; }

        /// <summary>
        ///     disp id
        /// </summary>
        public Terminal DispId { get; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => DispId.GetSymbolLength() +
               DispExpression.GetSymbolLength() +
               Semicolon.GetSymbolLength();

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, DispId, visitor);
            AcceptPart(this, DispExpression, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }


    }
}