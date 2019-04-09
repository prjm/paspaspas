using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     class of declaration
    /// </summary>
    public class ClassOfTypeDeclaration : StructuredTypeBase, ITypeTarget {

        /// <summary>
        ///     type value
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                if (TypeValue != null)
                    yield return TypeValue;
            }
        }

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
