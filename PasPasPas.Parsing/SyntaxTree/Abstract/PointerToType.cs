using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     pointer to
    /// </summary>
    public class PointerToType : TypeSpecificationBase, ITypeTarget {

        /// <summary>
        ///     pointer to
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     subparts
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
