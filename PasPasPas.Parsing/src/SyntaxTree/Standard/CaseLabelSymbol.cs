using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     case label
    /// </summary>
    public class CaseLabelSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new case label symbol
        /// </summary>
        /// <param name="startExpression"></param>
        /// <param name="dots"></param>
        /// <param name="endExpression"></param>
        /// <param name="comma"></param>
        public CaseLabelSymbol(ExpressionSymbol startExpression, Terminal dots, ExpressionSymbol endExpression, Terminal comma) {
            StartExpression = startExpression;
            Dots = dots;
            EndExpression = endExpression;
            Comma = comma;
        }

        /// <summary>
        ///     end expression
        /// </summary>
        public ExpressionSymbol EndExpression { get; }

        /// <summary>
        ///     start expression
        /// </summary>
        public ExpressionSymbol StartExpression { get; }

        /// <summary>
        ///     comma
        /// </summary>
        public Terminal Comma { get; }

        /// <summary>
        ///     dot symbol
        /// </summary>
        public Terminal Dots { get; }


        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, StartExpression, visitor);
            AcceptPart(this, Dots, visitor);
            AcceptPart(this, EndExpression, visitor);
            AcceptPart(this, Comma, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => StartExpression.GetSymbolLength() +
               Dots.GetSymbolLength() +
               EndExpression.GetSymbolLength() +
               Comma.GetSymbolLength();

    }
}