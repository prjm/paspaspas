using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     list of namespace names
    /// </summary>
    public class NamespaceNameListSymbol : VariableLengthSyntaxTreeBase<NamespaceNameSymbol> {

        /// <summary>
        ///     create a new namespace name list
        /// </summary>
        /// <param name="items"></param>
        /// <param name="semicolon"></param>
        public NamespaceNameListSymbol(ImmutableArray<NamespaceNameSymbol> items, Terminal semicolon) : base(items)
            => Semicolon = semicolon;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ItemLength + Semicolon.GetSymbolLength();

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }

    }
}