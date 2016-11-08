using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     declaration of a set type
    /// </summary>
    public class SetTypeDeclaration : StructuredType, ITypeTarget {

        /// <summary>
        ///     set type
        /// </summary>
        public TypeSpecificationBase TypeValue { get; set; }

        /// <summary>
        ///     constant array items
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
