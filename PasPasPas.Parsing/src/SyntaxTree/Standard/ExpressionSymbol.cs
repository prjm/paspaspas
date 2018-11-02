using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     expression
    /// </summary>
    public class ExpressionSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new expression object
        /// </summary>
        /// <param name="closureExpression"></param>
        /// <param name="comma"></param>
        public ExpressionSymbol(ClosureExpressionSymbol closureExpression, Terminal comma) {
            ClosureExpression = closureExpression;
            Comma = comma;
        }

        /// <summary>
        ///     create a new expression
        /// </summary>
        /// <param name="leftOperand"></param>
        public ExpressionSymbol(ISyntaxPart leftOperand)
            => LeftOperand = leftOperand;

        /// <summary>
        ///     create a new expression
        /// </summary>
        /// <param name="leftOperand"></param>
        /// <param name="operator"></param>
        /// <param name="rightOperand"></param>
        /// <param name="comma"></param>
        public ExpressionSymbol(ISyntaxPart leftOperand, Terminal @operator, ISyntaxPart rightOperand, Terminal comma) : this(leftOperand) {
            Operator = @operator;
            RightOperand = rightOperand;
            Comma = comma;
        }

        /// <summary>
        ///     closure expression
        /// </summary>
        public ClosureExpressionSymbol ClosureExpression { get; }

        /// <summary>
        ///     relational operator kind
        /// </summary>
        public int Kind
            => Operator.GetSymbolKind();

        /// <summary>
        ///     simple expression
        /// </summary>
        public ISyntaxPart LeftOperand { get; }

        /// <summary>
        ///     right operand
        /// </summary>
        public ISyntaxPart RightOperand { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, ClosureExpression, visitor);
            AcceptPart(this, LeftOperand, visitor);
            AcceptPart(this, Operator, visitor);
            AcceptPart(this, RightOperand, visitor);
            AcceptPart(this, Comma, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     comma
        /// </summary>
        public Terminal Comma { get; }

        /// <summary>
        ///     operator
        /// </summary>
        public Terminal Operator { get; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ClosureExpression.GetSymbolLength() +
                Comma.GetSymbolLength() +
                LeftOperand.GetSymbolLength() +
                Operator.GetSymbolLength() +
                RightOperand.GetSymbolLength();
    }
}
