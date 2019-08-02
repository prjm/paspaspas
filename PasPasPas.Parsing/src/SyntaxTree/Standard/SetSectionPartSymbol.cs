using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     set section part
    /// </summary>
    public class SetSectionPartSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new set section
        /// </summary>
        /// <param name="setExpression"></param>
        /// <param name="continuation"></param>
        public SetSectionPartSymbol(ExpressionSymbol setExpression, Terminal continuation) {
            SetExpression = setExpression;
            Continuation = continuation;
        }

        /// <summary>
        ///     continuation
        /// </summary>
        public Terminal Continuation { get; }

        /// <summary>
        ///     set expression
        /// </summary>
        public ExpressionSymbol SetExpression { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, SetExpression, visitor);
            AcceptPart(this, Continuation, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => SetExpression.GetSymbolLength() +
                Continuation.GetSymbolLength();


    }
}