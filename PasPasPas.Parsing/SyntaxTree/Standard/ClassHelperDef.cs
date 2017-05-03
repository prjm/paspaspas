using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     class helper declaration
    /// </summary>
    public class ClassHelperDef : StandardSyntaxTreeBase {

        /// <summary>
        ///     items
        /// </summary>
        public ClassHelperItems HelperItems { get; set; }

        /// <summary>
        ///     class parent
        /// </summary>
        public ParentClass ClassParent { get; set; }

        /// <summary>
        ///     class helper name
        /// </summary>
        public TypeName HelperName { get; set; }


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