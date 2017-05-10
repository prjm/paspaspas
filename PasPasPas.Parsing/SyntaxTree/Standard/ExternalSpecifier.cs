using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     external specifier
    /// </summary>
    public class ExternalSpecifier : StandardSyntaxTreeBase {

        /// <summary>
        ///     external expression
        /// </summary>
        public ConstantExpression Expression { get; set; }

        /// <summary>
        ///     external specifier kind
        /// </summary>
        public int Kind { get; set; }


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