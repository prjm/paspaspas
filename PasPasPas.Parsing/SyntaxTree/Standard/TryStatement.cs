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
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }


    }
}