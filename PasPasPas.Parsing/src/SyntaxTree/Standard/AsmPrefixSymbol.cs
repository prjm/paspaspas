using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     asm prefix
    /// </summary>
    public class AsmPrefixSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     lock prefix
        /// </summary>
        public Identifier LockPrefix { get; set; }

        /// <summary>
        ///     segment prefix
        /// </summary>
        public SyntaxPartBase SegmentPrefix { get; set; }

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
        public int Length
            => LockPrefix.Length + SegmentPrefix.Length;

    }
}
