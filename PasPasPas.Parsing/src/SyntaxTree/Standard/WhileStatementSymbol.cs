#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///  while statement
    /// </summary>
    public class WhileStatementSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new while statement
        /// </summary>
        /// <param name="whileSymbol"></param>
        /// <param name="condition"></param>
        /// <param name="doSymbol"></param>
        /// <param name="statement"></param>
        public WhileStatementSymbol(Terminal whileSymbol, ExpressionSymbol condition, Terminal doSymbol, StatementSymbol statement) {
            WhileSymbol = whileSymbol;
            Condition = condition;
            DoSymbol = doSymbol;
            Statement = statement;
        }

        /// <summary>
        ///     condition
        /// </summary>
        public ExpressionSymbol Condition { get; }

        /// <summary>
        ///     statement
        /// </summary>
        public StatementSymbol Statement { get; }

        /// <summary>
        ///     do symbol
        /// </summary>
        public Terminal DoSymbol { get; }

        /// <summary>
        ///     while symbol
        /// </summary>
        public Terminal WhileSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, WhileSymbol, visitor);
            AcceptPart(this, Condition, visitor);
            AcceptPart(this, DoSymbol, visitor);
            AcceptPart(this, Statement, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => WhileSymbol.GetSymbolLength() +
                Condition.GetSymbolLength() +
                DoSymbol.GetSymbolLength() +
                Statement.GetSymbolLength();


    }
}