using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     property accessors for disp interfaces
    /// </summary>
    public class ClassPropertyDispInterfaceSymbols : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new disp interface symbol
        /// </summary>
        /// <param name="modifier"></param>
        public ClassPropertyDispInterfaceSymbols(Terminal modifier)
            => Modifier = modifier;

        /// <summary>
        ///     create a new property disp interface symbol
        /// </summary>
        /// <param name="dispIdSymbol"></param>
        public ClassPropertyDispInterfaceSymbols(DispIdSymbol dispIdSymbol)
            => DispId = dispIdSymbol;

        /// <summary>
        ///     Disp id directive
        /// </summary>
        public SyntaxPartBase DispId { get; }

        /// <summary>
        ///     read only flag
        /// </summary>
        public bool ReadOnly
            => Modifier.GetSymbolKind() == TokenKind.ReadOnly;

        /// <summary>
        ///    write only
        /// </summary>
        public bool WriteOnly
            => Modifier.GetSymbolKind() == TokenKind.WriteOnly;

        /// <summary>
        ///     modifier
        /// </summary>
        public Terminal Modifier { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Modifier, visitor);
            AcceptPart(this, DispId, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Modifier.GetSymbolLength() + DispId.GetSymbolLength();

    }
}