using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     compound statement
    /// </summary>
    public class CompoundStatement : StandardSyntaxTreeBase {

        /// <summary>
        ///     assembler block
        /// </summary>
        public AsmBlockSymbol AssemblerBlock { get; set; }

        /// <summary>
        ///     statement list
        /// </summary>
        public StatementList Statements { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }


    }
}