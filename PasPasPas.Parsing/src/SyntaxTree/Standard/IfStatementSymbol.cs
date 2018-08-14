using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     if statement
    /// </summary>
    public class IfStatementSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new if statement
        /// </summary>
        /// <param name="ifSymbol"></param>
        /// <param name="condition"></param>
        /// <param name="thenSymbol"></param>
        /// <param name="thenPart"></param>
        /// <param name="elseSymbol"></param>
        /// <param name="elsePart"></param>
        public IfStatementSymbol(Terminal ifSymbol, ExpressionSymbol condition, Terminal thenSymbol, Statement thenPart, Terminal elseSymbol, Statement elsePart) {
            IfSymbol = ifSymbol;
            Condition = condition;
            ThenSymbol = thenSymbol;
            ThenPart = thenPart;
            ElseSymbol = elseSymbol;
            ElsePart = elsePart;
        }

        /// <summary>
        ///     condition
        /// </summary>
        public ExpressionSymbol Condition { get; }

        /// <summary>
        ///     else part
        /// </summary>
        public Statement ElsePart { get; }

        /// <summary>
        ///     then part
        /// </summary>
        public Statement ThenPart { get; }
        public Terminal IfSymbol { get; set; }
        public Terminal ThenSymbol { get; set; }
        public Terminal ElseSymbol { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, IfSymbol, visitor);
            AcceptPart(this, Condition, visitor);
            AcceptPart(this, ThenSymbol, visitor);
            AcceptPart(this, ThenPart, visitor);
            AcceptPart(this, ElseSymbol, visitor);
            AcceptPart(this, ElsePart, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => IfSymbol.GetSymbolLength() +
                Condition.GetSymbolLength() +
                ThenSymbol.GetSymbolLength() +
                ThenPart.GetSymbolLength() +
                ElseSymbol.GetSymbolLength() +
                ElsePart.GetSymbolLength();
    }
}