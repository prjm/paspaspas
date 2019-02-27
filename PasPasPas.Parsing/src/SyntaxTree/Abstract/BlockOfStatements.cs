using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

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
        ///     statement parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (var part in Statements)
                    yield return part;
            }
        }


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
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

    }
}
