using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     type declaration
    /// </summary>
    public class TypeDeclaration : StandardSyntaxTreeBase {

        /// <summary>
        ///     user attributes
        /// </summary>
        public SyntaxPartBase Attributes { get; set; }

        /// <summary>
        ///     hinting directives
        /// </summary>
        public ISyntaxPart Hint { get; set; }

        /// <summary>
        ///     type speicifcaiton
        /// </summary>
        public TypeSpecification TypeSpecification { get; set; }

        /// <summary>
        ///     type id
        /// </summary>
        public GenericTypeIdentifier TypeId { get; set; }


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