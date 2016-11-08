using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     class of declaratation
    /// </summary>
    public class ClassOfTypeDeclaration : StructuredType, ITypeTarget {

        /// <summary>
        ///     type value
        /// </summary>
        public TypeSpecificationBase TypeValue { get; set; }

        /// <summary>
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts
        {
            get
            {
                if (TypeValue != null)
                    yield return TypeValue;
            }
        }

    }
}
