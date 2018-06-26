using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     array index definition
    /// </summary>
    public class ArrayIndexSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new array index
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="dotDot"></param>
        /// <param name="comma"></param>
        /// <param name="endIndex"></param>
        public ArrayIndexSymbol(ConstantExpressionSymbol startIndex, Terminal dotDot, ConstantExpressionSymbol endIndex, Terminal comma) {
            StartIndex = startIndex;
            DotDot = dotDot;
            Comma = comma;
            EndIndex = endIndex;
        }

        /// <summary>
        ///     start index
        /// </summary>
        public ConstantExpressionSymbol StartIndex { get; }

        /// <summary>
        ///     end index
        /// </summary>
        public ConstantExpressionSymbol EndIndex { get; }

        /// <summary>
        ///     dot-dot symbol
        /// </summary>
        public Terminal DotDot { get; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => StartIndex.GetSymbolLength() +
               DotDot.GetSymbolLength() +
               EndIndex.GetSymbolLength() +
               Comma.GetSymbolLength();

        /// <summary>
        ///     comma
        /// </summary>
        public Terminal Comma { get; }

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