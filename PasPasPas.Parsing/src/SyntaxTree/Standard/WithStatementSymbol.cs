#nullable disable
using System.Collections.Immutable;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     with statement
    /// </summary>
    public class WithStatementSymbol : VariableLengthSyntaxTreeBase<ExpressionSymbol> {

        /// <summary>
        ///     create a new with symbol
        /// </summary>
        /// <param name="withSymbol"></param>
        /// <param name="items"></param>
        /// <param name="doSymbol"></param>
        /// <param name="statement"></param>
        public WithStatementSymbol(Terminal withSymbol, ImmutableArray<ExpressionSymbol> items, Terminal doSymbol, StatementSymbol statement) : base(items) {
            WithSymbol = withSymbol;
            DoSymbol = doSymbol;
            Statement = statement;
        }

        /// <summary>
        ///     statement
        /// </summary>
        public StatementSymbol Statement { get; }

        /// <summary>
        ///     do symbol
        /// </summary>
        public Terminal DoSymbol { get; }

        /// <summary>
        ///     with symbol
        /// </summary>
        public Terminal WithSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor to use</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, WithSymbol, visitor);
            AcceptPart(this, visitor);
            AcceptPart(this, DoSymbol, visitor);
            AcceptPart(this, Statement, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => WithSymbol.GetSymbolLength() +
                ItemLength +
                DoSymbol.GetSymbolLength() +
                Statement.GetSymbolLength();

    }
}