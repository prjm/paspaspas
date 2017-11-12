using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     set section part
    /// </summary>
    public class SetSectnPart : StandardSyntaxTreeBase {

        /// <summary>
        ///     continuation
        /// </summary>
        public int Continuation { get; set; }

        /// <summary>
        ///     set expression
        /// </summary>
        public Expression SetExpression { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }


    }
}