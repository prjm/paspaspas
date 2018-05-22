using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     array index definition
    /// </summary>
    public class ArrayIndexSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     start index
        /// </summary>
        public ConstantExpression StartIndex { get; set; }

        /// <summary>
        ///     end index
        /// </summary>
        public ISyntaxPart EndIndex { get; set; }

        /// <summary>
        ///     dot-dot symbol
        /// </summary>
        public Terminal DotDot { get; set; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public int Length
            => StartIndex.Length + DotDot.Length + EndIndex.Length + Comma.Length;

        /// <summary>
        ///     comma
        /// </summary>
        public Terminal Comma { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, StartIndex, visitor);
            AcceptPart(this, DotDot, visitor);
            AcceptPart(this, EndIndex, visitor);
            AcceptPart(this, Comma, visitor);
            visitor.EndVisit(this);
        }

    }
}