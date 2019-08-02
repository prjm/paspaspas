using System.Collections.Immutable;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     except handlers
    /// </summary>
    public class ExceptHandlersSymbol : VariableLengthSyntaxTreeBase<ExceptHandlerSymbol> {

        /// <summary>
        ///     create a new except handler symbol
        /// </summary>
        /// <param name="items"></param>
        /// <param name="elseSymbol"></param>
        /// <param name="statements"></param>
        public ExceptHandlersSymbol(ImmutableArray<ExceptHandlerSymbol> items, Terminal elseSymbol, StatementList statements) : base(items) {
            ElseSymbol = elseSymbol;
            Statements = statements;
        }

        /// <summary>
        ///     generic except handler statements
        /// </summary>
        public StatementList Statements { get; }

        /// <summary>
        ///     else symbol
        /// </summary>
        public Terminal ElseSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, visitor);
            AcceptPart(this, ElseSymbol, visitor);
            AcceptPart(this, Statements, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ItemLength +
                ElseSymbol.GetSymbolLength() +
                Statements.GetSymbolLength();

    }
}