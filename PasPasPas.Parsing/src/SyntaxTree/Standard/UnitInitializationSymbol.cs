using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     unit initialization part
    /// </summary>
    public class UnitInitializationSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new init symbol
        /// </summary>
        /// <param name="initSymbol"></param>
        /// <param name="statements"></param>
        /// <param name="finalization"></param>
        public UnitInitializationSymbol(Terminal initSymbol, StatementList statements, UnitFinalizationSymbol finalization) {
            InitSymbol = initSymbol;
            Statements = statements;
            Finalization = finalization;
        }

        /// <summary>
        ///     unit finalization
        /// </summary>
        public UnitFinalizationSymbol Finalization { get; }

        /// <summary>
        ///     initialization statements
        /// </summary>
        public StatementList Statements { get; }

        /// <summary>
        ///     init symbol
        /// </summary>
        public Terminal InitSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, InitSymbol, visitor);
            AcceptPart(this, Statements, visitor);
            AcceptPart(this, Finalization, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => InitSymbol.GetSymbolLength() +
                Statements.GetSymbolLength() +
                Finalization.GetSymbolLength();

    }
}