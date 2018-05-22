using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     array type definition
    /// </summary>
    public class ArrayTypeSymbol : VariableLengthSyntaxTreeBase<ArrayIndexSymbol> {

        /// <summary>
        ///     true if the array is of type <c>array of const</c>
        /// </summary>
        public bool ArrayOfConst
            => ConstSymbol.Kind == TokenKind.Const;

        /// <summary>
        ///     array type specification
        /// </summary>
        public ISyntaxPart TypeSpecification { get; set; }

        /// <summary>
        ///     array symbol
        /// </summary>
        public Terminal Array { get; set; }

        /// <summary>
        ///     open braces
        /// </summary>
        public Terminal OpenBraces { get; set; }

        /// <summary>
        ///     close braces
        /// </summary>
        public Terminal CloseBraces { get; set; }

        /// <summary>
        ///     of symbol
        /// </summary>
        public Terminal OfSymbol { get; set; }

        /// <summary>
        ///     const symbol
        /// </summary>
        public Terminal ConstSymbol { get; set; }

        public int Length
            => Array.Length + OpenBraces.Length + ItemLength + CloseBraces.Length +
                OfSymbol.Length + ConstSymbol.Length + TypeSpecification.Length;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Array, visitor);
            AcceptPart(this, OpenBraces, visitor);
            AcceptPart(this, visitor);
            AcceptPart(this, CloseBraces, visitor);
            AcceptPart(this, OfSymbol, visitor);
            AcceptPart(this, ConstSymbol, visitor);
            AcceptPart(this, TypeSpecification, visitor);
            visitor.EndVisit(this);
        }

    }
}