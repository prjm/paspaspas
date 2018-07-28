using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     library head
    /// </summary>
    public class LibraryHeadSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     hints
        /// </summary>
        public ISyntaxPart Hints { get; set; }

        /// <summary>
        ///     library name
        /// </summary>
        public NamespaceName LibraryName { get; set; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; set; }

        /// <summary>
        ///     library
        /// </summary>
        public Terminal LibrarySymbol { get; set; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => LibrarySymbol.GetSymbolLength() +
                LibraryName.GetSymbolLength() +
                Hints.GetSymbolLength() +
                Semicolon.GetSymbolLength();

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, LibrarySymbol, visitor);
            AcceptPart(this, LibraryName, visitor);
            AcceptPart(this, Hints, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }


    }
}