using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     property accessor
    /// </summary>
    public class ClassPropertyReadWrite : StandardSyntaxTreeBase {

        /// <summary>
        ///     accessor kind
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     member name
        /// </summary>
        public NamespaceName Member { get; set; }

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