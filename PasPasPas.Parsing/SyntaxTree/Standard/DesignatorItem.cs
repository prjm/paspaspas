using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     designator item
    /// </summary>
    public class DesignatorItem : StandardSyntaxTreeBase {
        public DesignatorItem(SyntaxPartBase parent) {
            Parent = parent;
            parent?.Add(this);
        }

        /// <summary>
        ///     dereference
        /// </summary>
        public bool Dereference { get; set; }

        /// <summary>
        ///     index expression
        /// </summary>
        public ExpressionList IndexExpression { get; set; }

        /// <summary>
        ///     subitem
        /// </summary>
        public Identifier Subitem { get; set; }

        /// <summary>
        ///     generic type of the subitem
        /// </summary>
        public GenericSuffix SubitemGenericType { get; set; }

        /// <summary>
        ///     <true>if a parameter list</true>
        /// </summary>
        public bool ParameterList { get; set; }

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