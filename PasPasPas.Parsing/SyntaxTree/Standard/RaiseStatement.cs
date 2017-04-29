using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     raise statemenmt
    /// </summary>
    public class RaiseStatement : StandardSyntaxTreeBase {

        /// <summary>
        ///     at part
        /// </summary>
        public Expression At { get; set; }

        /// <summary>
        ///     raise part
        /// </summary>
        public Expression Raise { get; set; }

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