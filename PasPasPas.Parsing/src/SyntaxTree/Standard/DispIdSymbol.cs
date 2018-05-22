using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     disp id directive
    /// </summary>
    public class DispIdSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     disp id expression
        /// </summary>
        public Expression DispExpression { get; set; }

        /// <summary>
        ///     disp id
        /// </summary>
        public Terminal DispId { get; set; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public int Length
            => DispId.Length + DispExpression.Length + Semicolon.Length;

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, DispId, visitor);
            AcceptPart(this, DispExpression, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }


    }
}