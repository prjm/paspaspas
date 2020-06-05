#nullable disable
using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     structured type
    /// </summary>
    public class StructTypeSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new structured type symbol
        /// </summary>
        /// <param name="packed"></param>
        /// <param name="part"></param>
        public StructTypeSymbol(Terminal packed, StructTypePart part) {
            PackedSymbol = packed;
            Part = part;
        }

        /// <summary>
        ///     Packed structured type
        /// </summary>
        public bool Packed
            => PackedSymbol.GetSymbolKind() == TokenKind.Packed;

        /// <summary>
        ///     part
        /// </summary>
        public StructTypePart Part { get; }

        /// <summary>
        ///     packed symbol
        /// </summary>
        public Terminal PackedSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, PackedSymbol, visitor);
            AcceptPart(this, Part, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => PackedSymbol.GetSymbolLength() + Part.GetSymbolLength();
    }
}