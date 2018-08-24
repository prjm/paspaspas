using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     pointer type specification
    /// </summary>
    public class PointerTypeSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new pointer type symbol
        /// </summary>
        /// <param name="pointerSymbol"></param>
        public PointerTypeSymbol(Terminal pointerSymbol)
            => PointerSymbol = pointerSymbol;

        /// <summary>
        ///     create a new pointer type symbol
        /// </summary>
        /// <param name="pointerSymbol"></param>
        /// <param name="typeSpecification"></param>
        public PointerTypeSymbol(Terminal pointerSymbol, TypeSpecificationSymbol typeSpecification) : this(pointerSymbol)
            => TypeSpecification = typeSpecification;

        /// <summary>
        ///     true if a generic pointer type is found
        /// </summary>
        public bool GenericPointer
            => PointerSymbol.GetSymbolKind() == TokenKind.Pointer;

        /// <summary>
        ///     type specification for non generic pointers
        /// </summary>
        public TypeSpecificationSymbol TypeSpecification { get; }

        /// <summary>
        ///     pointer symbol
        /// </summary>
        public Terminal PointerSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, PointerSymbol, visitor);
            AcceptPart(this, TypeSpecification, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => PointerSymbol.GetSymbolLength() +
                TypeSpecification.GetSymbolLength();
    }
}