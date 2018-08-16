using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     parameter for a method call
    /// </summary>
    public class ParameterSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new parameter symbol
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="assignmentSymbol"></param>
        /// <param name="expression"></param>
        /// <param name="comma"></param>
        public ParameterSymbol(IdentifierSymbol parameterName, Terminal assignmentSymbol, FormattedExpressionSymbol expression, Terminal comma) {
            ParameterName = parameterName;
            AssignmentSymbol = assignmentSymbol;
            Expression = expression;
            Comma = comma;
        }

        /// <summary>
        ///     parameter expression
        /// </summary>
        public FormattedExpressionSymbol Expression { get; }

        /// <summary>
        ///     parameter name
        /// </summary>
        public IdentifierSymbol ParameterName { get; }

        /// <summary>
        ///     assignment
        /// </summary>
        public Terminal AssignmentSymbol { get; }

        /// <summary>
        ///     comma
        /// </summary>
        public Terminal Comma { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, ParameterName, visitor);
            AcceptPart(this, AssignmentSymbol, visitor);
            AcceptPart(this, Expression, visitor);
            AcceptPart(this, Comma, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ParameterName.GetSymbolLength() +
                AssignmentSymbol.GetSymbolLength() +
                Expression.GetSymbolLength() +
                Comma.GetSymbolLength();

    }
}
