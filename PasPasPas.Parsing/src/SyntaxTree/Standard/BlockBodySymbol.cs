using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     block body
    /// </summary>
    public class BlockBodySymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///    assembler block
        /// </summary>
        public SyntaxPartBase AssemblerBlock { get; set; }

        /// <summary>
        ///     block bode
        /// </summary>
        public SyntaxPartBase Body { get; set; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public int Length
            => AssemblerBlock.Length + Body.Length;


        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

    }
}