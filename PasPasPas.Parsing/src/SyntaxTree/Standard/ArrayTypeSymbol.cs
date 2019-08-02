using System.Collections.Immutable;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     array type definition
    /// </summary>
    public class ArrayTypeSymbol : VariableLengthSyntaxTreeBase<ArrayIndexSymbol> {

        /// <summary>
        ///     create a new array type symbol
        /// </summary>
        /// <param name="array"></param>
        /// <param name="openBraces"></param>
        /// <param name="closeBraces"></param>
        /// <param name="ofSymbol"></param>
        /// <param name="constSymbol"></param>
        /// <param name="typeSpecification"></param>
        /// <param name="items"></param>
        public ArrayTypeSymbol(Terminal array, Terminal openBraces, ImmutableArray<ArrayIndexSymbol> items, Terminal closeBraces, Terminal ofSymbol, Terminal constSymbol, TypeSpecificationSymbol typeSpecification) : base(items) {
            Array = array;
            OpenBraces = openBraces;
            CloseBraces = closeBraces;
            OfSymbol = ofSymbol;
            ConstSymbol = constSymbol;
            TypeSpecification = typeSpecification;
        }

        /// <summary>
        ///     true if the array is of type <c>array of const</c>
        /// </summary>
        public bool ArrayOfConst
            => ConstSymbol != null;

        /// <summary>
        ///     array type specification
        /// </summary>
        public TypeSpecificationSymbol TypeSpecification { get; }

        /// <summary>
        ///     array symbol
        /// </summary>
        public Terminal Array { get; }

        /// <summary>
        ///     open braces
        /// </summary>
        public Terminal OpenBraces { get; }

        /// <summary>
        ///     close braces
        /// </summary>
        public Terminal CloseBraces { get; }

        /// <summary>
        ///     of symbol
        /// </summary>
        public Terminal OfSymbol { get; }

        /// <summary>
        ///     const symbol
        /// </summary>
        public Terminal ConstSymbol { get; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Array.GetSymbolLength() +
               OpenBraces.GetSymbolLength() +
               ItemLength +
               CloseBraces.GetSymbolLength() +
               OfSymbol.GetSymbolLength() +
               ConstSymbol.GetSymbolLength() +
               TypeSpecification.GetSymbolLength();

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