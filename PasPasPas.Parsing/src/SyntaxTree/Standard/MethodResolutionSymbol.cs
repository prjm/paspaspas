using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     method resolution
    /// </summary>
    public class MethodResolutionSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new method resolution symbol
        /// </summary>
        /// <param name="kindSymbol"></param>
        /// <param name="name"></param>
        /// <param name="equalsSign"></param>
        /// <param name="resolveIdentifier"></param>
        /// <param name="semicolon"></param>
        public MethodResolutionSymbol(Terminal kindSymbol, TypeNameSymbol name, Terminal equalsSign, IdentifierSymbol resolveIdentifier, Terminal semicolon) {
            KindSymbol = kindSymbol;
            Name = name;
            EqualsSign = equalsSign;
            ResolveIdentifier = resolveIdentifier;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     kind (procedure/function)
        /// </summary>
        public int Kind
            => KindSymbol.GetSymbolKind();

        /// <summary>
        ///     resolve identifier
        /// </summary>
        public IdentifierSymbol ResolveIdentifier { get; }

        /// <summary>
        ///     identifier to be resolved
        /// </summary>
        public TypeNameSymbol Name { get; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }

        /// <summary>
        ///     equals sign
        /// </summary>
        public Terminal EqualsSign { get; }

        /// <summary>
        ///     procedure kind
        /// </summary>
        public Terminal KindSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, KindSymbol, visitor);
            AcceptPart(this, Name, visitor);
            AcceptPart(this, EqualsSign, visitor);
            AcceptPart(this, ResolveIdentifier, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => KindSymbol.GetSymbolLength() +
                Name.GetSymbolLength() +
                EqualsSign.GetSymbolLength() +
                ResolveIdentifier.GetSymbolLength() +
                Semicolon.GetSymbolLength();

    }
}