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
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }


    }
}