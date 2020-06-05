#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     asm prefix
    /// </summary>
    public class AsmPrefixSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new asm prefix symbol
        /// </summary>
        /// <param name="lockPrefix"></param>
        /// <param name="segmentPrefix"></param>
        public AsmPrefixSymbol(IdentifierSymbol lockPrefix, IdentifierSymbol segmentPrefix) {
            LockPrefix = lockPrefix;
            SegmentPrefix = segmentPrefix;
        }

        /// <summary>
        ///     lock prefix
        /// </summary>
        public IdentifierSymbol LockPrefix { get; }

        /// <summary>
        ///     segment prefix
        /// </summary>
        public SyntaxPartBase SegmentPrefix { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, LockPrefix, visitor);
            AcceptPart(this, SegmentPrefix, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => LockPrefix.GetSymbolLength() + SegmentPrefix.GetSymbolLength();

    }
}
