using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     raise statement
    /// </summary>
    public class RaiseStatementSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new raise statement
        /// </summary>
        /// <param name="raiseSymbol"></param>
        /// <param name="atSymbol"></param>
        /// <param name="raiseExpression"></param>
        /// <param name="atExpression"></param>
        public RaiseStatementSymbol(Terminal raiseSymbol, Terminal atSymbol, ExpressionSymbol raiseExpression, ExpressionSymbol atExpression) {
            RaiseSymbol = raiseSymbol;
            AtSymbol = atSymbol;
            RaiseExpression = raiseExpression;
            AtExpression = atExpression;
        }

        /// <summary>
        ///     expression
        /// </summary>
        public ExpressionSymbol RaiseExpression { get; }

        /// <summary>
        ///     at expression
        /// </summary>
        public ExpressionSymbol AtExpression { get; }

        /// <summary>
        ///     at symbol
        /// </summary>
        public Terminal AtSymbol { get; }

        /// <summary>
        ///     raise symbol
        /// </summary>
        public Terminal RaiseSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, RaiseSymbol, visitor);
            AcceptPart(this, RaiseExpression, visitor);
            AcceptPart(this, AtSymbol, visitor);
            AcceptPart(this, AtExpression, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => RaiseSymbol.GetSymbolLength() +
                AtSymbol.GetSymbolLength() +
                RaiseExpression.GetSymbolLength() +
                AtExpression.GetSymbolLength();
    }
}