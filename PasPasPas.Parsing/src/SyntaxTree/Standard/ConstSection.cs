using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     section for constants
    /// </summary>
    public class ConstSection : VariableLengthSyntaxTreeBase<ConstDeclaration> {

        /// <summary>
        ///     create a new const section
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="items"></param>
        public ConstSection(Terminal symbol, ImmutableArray<ConstDeclaration> items) : base(items) {
            ConstSymbol = symbol;
        }

        /// <summary>
        ///     constant symbol
        /// </summary>
        public Terminal ConstSymbol { get; }

        /// <summary>
        ///     kind of constant
        /// </summary>
        public int Kind { get; }
                = TokenKind.Undefined;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, ConstSymbol, visitor);
            AcceptPart(this, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     item length
        /// </summary>
        public override int Length =>
            ConstSymbol.GetSymbolLength() + ItemLength;

    }
}
