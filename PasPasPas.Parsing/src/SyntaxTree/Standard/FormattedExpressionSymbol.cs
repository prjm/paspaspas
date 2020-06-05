#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     formatted expression
    /// </summary>
    public class FormattedExpressionSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     ceate a new formatted expression
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="colon1"></param>
        /// <param name="width"></param>
        /// <param name="colon2"></param>
        /// <param name="decimals"></param>
        public FormattedExpressionSymbol(ExpressionSymbol expression, Terminal colon1, ExpressionSymbol width, Terminal colon2, ExpressionSymbol decimals) {
            Expression = expression;
            Colon1 = colon1;
            Width = width;
            Colon2 = colon2;
            Decimals = decimals;
        }

        /// <summary>
        ///     decimals subexpression
        /// </summary>
        public ExpressionSymbol Decimals { get; }

        /// <summary>
        ///     width subexpression
        /// </summary>
        public ExpressionSymbol Width { get; }

        /// <summary>
        ///     base expression
        /// </summary>
        public ExpressionSymbol Expression { get; }

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal Colon2 { get; }

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal Colon1 { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Expression, visitor);
            AcceptPart(this, Colon1, visitor);
            AcceptPart(this, Width, visitor);
            AcceptPart(this, Colon2, visitor);
            AcceptPart(this, Decimals, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Expression.GetSymbolLength() +
                Colon1.GetSymbolLength() +
                Width.GetSymbolLength() +
                Colon2.GetSymbolLength() +
                Decimals.GetSymbolLength();

    }
}