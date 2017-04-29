using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     pointer type specification
    /// </summary>
    public class PointerType : StandardSyntaxTreeBase {

        /// <summary>
        ///     true if a generic pointer type is found
        /// </summary>
        public bool GenericPointer { get; set; }
            = false;

        /// <summary>
        ///     type specification for non generic pointers
        /// </summary>
        public TypeSpecification TypeSpecification { get; set; }

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