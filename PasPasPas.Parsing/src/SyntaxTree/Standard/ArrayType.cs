using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     array type definition
    /// </summary>
    public class ArrayType : StandardSyntaxTreeBase {

        /// <summary>
        ///     true if the array is of type <c>array of const</c>
        /// </summary>
        public bool ArrayOfConst { get; set; }

        /// <summary>
        ///     array type specification
        /// </summary>
        public TypeSpecification TypeSpecification { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

    }
}