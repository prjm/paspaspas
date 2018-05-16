using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     except handlers
    /// </summary>
    public class ExceptHandlers : StandardSyntaxTreeBase {

        /// <summary>
        ///     else statements
        /// </summary>
        public StatementList ElseStatements { get; set; }

        /// <summary>
        ///     generic except handler statements
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