using System.Collections.Immutable;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     rtti control specifier
    /// </summary>
    public class RttiControlSpecifier : CompilerDirectiveBase {
        private readonly Terminal openParen;
        private readonly Terminal openBraces;
        private ImmutableArray<RttiVisibilityItem> immutableArray;
        private readonly Terminal closeBraces;
        private readonly Terminal closeParen;

        /// <summary>
        ///     create a new rtti control specifier
        /// </summary>
        /// <param name="openParen"></param>
        /// <param name="openBraces"></param>
        /// <param name="immutableArray"></param>
        /// <param name="closeBraces"></param>
        /// <param name="closeParen"></param>
        public RttiControlSpecifier(Terminal openParen, Terminal openBraces, ImmutableArray<RttiVisibilityItem> immutableArray, Terminal closeBraces, Terminal closeParen) {
            this.openParen = openParen;
            this.openBraces = openBraces;
            this.immutableArray = immutableArray;
            this.closeBraces = closeBraces;
            this.closeParen = closeParen;
        }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor"></param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, openParen, visitor);
            AcceptPart(this, openBraces, visitor);

            foreach (var item in immutableArray)
                AcceptPart(this, item, visitor);

            AcceptPart(this, closeBraces, visitor);
            AcceptPart(this, closeParen, visitor);
            visitor.EndVisit(this);
        }
    }
}
