using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     method resolution
    /// </summary>
    public class MethodResolution : StandardSyntaxTreeBase {

        /// <summary>
        ///     kind (procedure/function)
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     resolve identifier
        /// </summary>
        public Identifier ResolveIdentifier { get; set; }

        /// <summary>
        ///     identifier to be resolved
        /// </summary>
        public TypeName Name { get; set; }

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