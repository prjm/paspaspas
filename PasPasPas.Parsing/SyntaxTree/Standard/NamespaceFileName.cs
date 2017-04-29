using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     namespace identifiert with file name
    /// </summary>
    public class NamespaceFileName : StandardSyntaxTreeBase {

        /// <summary>
        ///     Namespace name
        /// </summary>
        public NamespaceName NamespaceName { get; set; }

        /// <summary>
        ///     filename
        /// </summary>
        public QuotedString QuotedFileName { get; set; }

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