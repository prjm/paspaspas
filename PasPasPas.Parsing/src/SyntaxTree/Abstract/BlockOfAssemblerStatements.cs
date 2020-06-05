#nullable disable
using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     block of assembler statements
    /// </summary>
    public class BlockOfAssemblerStatements : StatementBase {

        /// <summary>
        ///     assembler statements
        /// </summary>
        public ISyntaxPartCollection<AssemblerStatement> Statements { get; }

        /// <summary>
        ///     create a new block of assembler statements
        /// </summary>
        public BlockOfAssemblerStatements()
            => Statements = new SyntaxPartCollection<AssemblerStatement>();

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Statements, visitor);
            visitor.EndVisit(this);
        }
    }
}
