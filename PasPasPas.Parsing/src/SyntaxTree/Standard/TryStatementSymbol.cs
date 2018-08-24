using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     try statement
    /// </summary>
    public class TryStatementSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new try statement
        /// </summary>
        /// <param name="trySymbol"></param>
        /// <param name="statements"></param>
        /// <param name="exceptSymbol"></param>
        /// <param name="handlers"></param>
        /// <param name="finallySymbol"></param>
        /// <param name="finallyStatements"></param>
        /// <param name="endSymbol"></param>
        public TryStatementSymbol(Terminal trySymbol, StatementList statements, Terminal exceptSymbol, ExceptHandlersSymbol handlers, Terminal finallySymbol, StatementList finallyStatements, Terminal endSymbol) {
            TrySymbol = trySymbol;
            Try = statements;
            ExceptSymbol = exceptSymbol;
            Handlers = handlers;
            Finally = finallyStatements;
            FinallySymbol = finallySymbol;
            EndSymbol = endSymbol;
        }

        /// <summary>
        ///     finally part
        /// </summary>
        public StatementList Finally { get; }

        /// <summary>
        ///     except handlers
        /// </summary>
        public ExceptHandlersSymbol Handlers { get; }

        /// <summary>
        ///     try part
        /// </summary>
        public StatementList Try { get; }

        /// <summary>
        ///     try symbol
        /// </summary>
        public Terminal TrySymbol { get; }

        /// <summary>
        ///     except symbol
        /// </summary>
        public Terminal ExceptSymbol { get; }

        /// <summary>
        ///     finally symbol
        /// </summary>
        public Terminal FinallySymbol { get; }

        /// <summary>
        ///     end symbol
        /// </summary>
        public Terminal EndSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor to use</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, TrySymbol, visitor);
            AcceptPart(this, Try, visitor);
            AcceptPart(this, ExceptSymbol, visitor);
            AcceptPart(this, Handlers, visitor);
            AcceptPart(this, FinallySymbol, visitor);
            AcceptPart(this, Finally, visitor);
            AcceptPart(this, EndSymbol, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => TrySymbol.GetSymbolLength() +
                Try.GetSymbolLength() +
                ExceptSymbol.GetSymbolLength() +
                Handlers.GetSymbolLength() +
                FinallySymbol.GetSymbolLength() +
                Finally.GetSymbolLength() +
                EndSymbol.GetSymbolLength();

    }
}