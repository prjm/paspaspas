#nullable disable
using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     a block of statements
    /// </summary>
    public class BlockOfStatements : StatementBase, IStatementTarget, ILabelTarget {

        /// <summary>
        ///     contained statements
        /// </summary>
        public ISyntaxPartCollection<StatementBase> Statements { get; }

        /// <summary>
        ///     creates a new block of statements
        /// </summary>
        public BlockOfStatements()
            => Statements = new SyntaxPartCollection<StatementBase>();

        /// <summary>
        ///     add a statement
        /// </summary>
        /// <param name="part">statement part to add</param>
        public void Add(StatementBase part)
            => Statements.Add(part);


        /// <summary>
        ///     get all statements
        /// </summary>
        BlockOfStatements IStatementTarget.Statements
            => this;

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
