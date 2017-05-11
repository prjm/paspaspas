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
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (StatementBase part in Statements)
                    yield return part;
            }
        }


        BlockOfStatements IStatementTarget.Statements
            => this;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(this, startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }

    }
}
