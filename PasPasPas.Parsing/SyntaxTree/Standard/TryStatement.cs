using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     try statement
    /// </summary>
    public class TryStatement : StandardSyntaxTreeBase {

        /// <summary>
        ///     finally part
        /// </summary>
        public StatementList Finally { get; set; }

        /// <summary>
        ///     except handlers
        /// </summary>
        public ExceptHandlers Handlers { get; set; }

        /// <summary>
        ///     try part
        /// </summary>
        public StatementList Try { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor to use</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }


    }
}