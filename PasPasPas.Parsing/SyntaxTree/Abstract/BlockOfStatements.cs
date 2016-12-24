using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     a block of statements
    /// </summary>
    public class BlockOfStatements : StatementBase, IStatementTarget {

        /// <summary>
        ///     contained statements
        /// </summary>
        public IList<StatementBase> Statements { get; }
            = new List<StatementBase>();


        /// <summary>
        ///     add a statement
        /// </summary>
        /// <param name="part">statement part to add</param>
        public void Add(StatementBase part) {
            Statements.Add(part);
        }

        /// <summary>
        ///     statement parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts
            => Statements;

        BlockOfStatements IStatementTarget.Statements
            => this;

    }
}
