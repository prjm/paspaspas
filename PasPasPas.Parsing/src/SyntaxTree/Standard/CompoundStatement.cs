using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     compound statement
    /// </summary>
    public class CompoundStatement : StandardSyntaxTreeBase {

        /// <summary>
        ///     assembler block
        /// </summary>
        public SyntaxPartBase AssemblerBlock { get; set; }

        /// <summary>
        ///     statement list
        /// </summary>
        public SyntaxPartBase Statements { get; set; }

        /// <summary>
        ///     begin symbol
        /// </summary>
        public Terminal BeginSymbol { get; set; }

        /// <summary>
        ///     end symbol
        /// </summary>
        public Terminal EndSymbol { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, AssemblerBlock, visitor);
            AcceptPart(this, BeginSymbol, visitor);
            AcceptPart(this, Statements, visitor);
            AcceptPart(this, EndSymbol, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public int Length
            => AssemblerBlock.Length + BeginSymbol.Length + Statements.Length + EndSymbol.Length;
    }
}