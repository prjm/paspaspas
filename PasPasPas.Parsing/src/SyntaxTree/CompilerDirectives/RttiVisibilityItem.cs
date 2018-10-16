using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     rtti visibility option
    /// </summary>
    public class RttiVisibilityItem : CompilerDirectiveBase {

        /// <summary>
        ///     create a new rtti visibility item
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="comma"></param>
        public RttiVisibilityItem(Terminal mode, Terminal comma) {
            Mode = mode;
            Comma = comma;
        }

        /// <summary>
        ///     for private items
        /// </summary>
        public bool ForPrivate
            => Mode.GetSymbolKind() == TokenKind.VcPrivate;

        /// <summary>
        ///     for protected items
        /// </summary>
        public bool ForProtected
            => Mode.GetSymbolKind() == TokenKind.VcProtected;

        /// <summary>
        ///     for public items
        /// </summary>
        public bool ForPublic
            => Mode.GetSymbolKind() == TokenKind.VcPublic;

        /// <summary>
        ///     for published items
        /// </summary>
        public bool ForPublished
            => Mode.GetSymbolKind() == TokenKind.VcPublished;

        /// <summary>
        ///     mode
        /// </summary>
        public Terminal Mode { get; }

        /// <summary>
        ///     comma
        /// </summary>
        public Terminal Comma { get; }

        /// <summary>
        ///     accept a visitor
        /// </summary>
        /// <param name="visitor"></param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Mode, visitor);
            AcceptPart(this, Comma, visitor);
            visitor.EndVisit(this);
        }
    }
}
