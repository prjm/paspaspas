using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     a block of statements
    /// </summary>
    public class BlockOfStatements : AbstractSyntaxPart, IStatementTarget {

        /// <summary>
        ///     contained statements
        /// </summary>
        public IList<AbstractSyntaxPart> Statements { get; }
            = new List<AbstractSyntaxPart>();


        /// <summary>
        ///     add a statement
        /// </summary>
        /// <param name="part">statement part to add</param>
        public void Add(AbstractSyntaxPart part) {
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
