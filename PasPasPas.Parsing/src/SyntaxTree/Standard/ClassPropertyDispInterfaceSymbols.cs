using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     property accessors for disp interfaces
    /// </summary>
    public class ClassPropertyDispInterfaceSymbols : StandardSyntaxTreeBase {

        /// <summary>
        ///     Disp id directive
        /// </summary>
        public SyntaxPartBase DispId { get; set; }

        /// <summary>
        ///     read only flag
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        ///    write only
        /// </summary>
        public bool WriteOnly { get; set; }

        /// <summary>
        ///     modifier
        /// </summary>
        public Terminal Modifier { get; set; }

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
        public int Length
            => Modifier.Length + DispId.Length;

    }
}