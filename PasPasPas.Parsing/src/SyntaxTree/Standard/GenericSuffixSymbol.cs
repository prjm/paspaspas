using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///    generic type suffix
    /// </summary>
    public class GenericSuffixSymbol : VariableLengthSyntaxTreeBase<TypeSpecificationSymbol> {

        /// <summary>
        ///     create a new generic suffix
        /// </summary>
        /// <param name="openBracket"></param>
        /// <param name="items"></param>
        /// <param name="closeBracket"></param>
        public GenericSuffixSymbol(Terminal openBracket, ImmutableArray<TypeSpecificationSymbol> items, Terminal closeBracket) : base(items) {
            OpenBracket = openBracket;
            CloseBracket = closeBracket;
        }

        /// <summary>
        ///     open bracket
        /// </summary>
        public Terminal OpenBracket { get; }

        /// <summary>
        ///     close bracket
        /// </summary>
        public Terminal CloseBracket { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, OpenBracket, visitor);
            AcceptPart(this, visitor);
            AcceptPart(this, CloseBracket, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => OpenBracket.GetSymbolLength() +
                ItemLength +
                CloseBracket.GetSymbolLength();

    }
}