#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     declaration of a set type
    /// </summary>
    public class SetTypeDeclaration : StructuredTypeBase, ITypeTarget {

        /// <summary>
        ///     set type
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

