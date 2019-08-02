using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     finalization part
    /// </summary>
    public class UnitFinalizationSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new unit finalization symbol
        /// </summary>
        /// <param name="finalizationSymbol"></param>
        /// <param name="statements"></param>
        public UnitFinalizationSymbol(Terminal finalizationSymbol, StatementList statements) {
            FinalizationSymbol = finalizationSymbol;
            Statements = statements;
        }

        /// <summary>
        ///     final statements
        /// </summary>
        public StatementList Statements { get; }

        /// <summary>
        ///     finalization symbol
        /// </summary>
        public Terminal FinalizationSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, FinalizationSymbol, visitor);
            AcceptPart(this, Statements, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => FinalizationSymbol.GetSymbolLength() +
                Statements.GetSymbolLength();

    }

}