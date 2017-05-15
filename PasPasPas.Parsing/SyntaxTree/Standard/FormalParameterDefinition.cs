using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     formal definition parameter
    /// </summary>
    public class FormalParameterDefinition : StandardSyntaxTreeBase {

        /// <summary>
        ///     default value
        /// </summary>
        public Expression DefaultValue { get; set; }

        /// <summary>
        ///     type declaration
        /// </summary>
        public TypeSpecification TypeDeclaration { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

    }
}
