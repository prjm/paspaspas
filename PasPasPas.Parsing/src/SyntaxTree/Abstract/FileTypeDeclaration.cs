using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     file type declaration
    /// </summary>
    public class FileTypeDeclaration : StructuredTypeBase, ITypeTarget {

        /// <summary>
        ///     type value
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, TypeValue, visitor);
            visitor.EndVisit(this);
        }
    }
}
