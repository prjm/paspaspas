using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     guid declaration
    /// </summary>
    public class InterfaceGuidSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new interface guid
        /// </summary>
        /// <param name="openBraces"></param>
        /// <param name="idIdentifier"></param>
        /// <param name="stringIdentifier"></param>
        /// <param name="closeBraces"></param>
        public InterfaceGuidSymbol(Terminal openBraces, IdentifierSymbol idIdentifier, QuotedStringSymbol stringIdentifier, Terminal closeBraces) {
            OpenBraces = openBraces;
            IdIdentifier = idIdentifier;
            Id = stringIdentifier;
            CloseBraces = closeBraces;
        }

        /// <summary>
        ///     guid for this interface
        /// </summary>
        public QuotedStringSymbol Id { get; }

        /// <summary>
        ///     named guid for this interface
        /// </summary>
        public IdentifierSymbol IdIdentifier { get; }

        /// <summary>
        ///     open braces
        /// </summary>
        public Terminal OpenBraces { get; }

        /// <summary>
        ///     close braces
        /// </summary>
        public Terminal CloseBraces { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, OpenBraces, visitor);
            AcceptPart(this, IdIdentifier, visitor);
            AcceptPart(this, Id, visitor);
            AcceptPart(this, CloseBraces, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => OpenBraces.GetSymbolLength() +
                IdIdentifier.GetSymbolLength() +
                Id.GetSymbolLength() +
                CloseBraces.GetSymbolLength();

    }
}