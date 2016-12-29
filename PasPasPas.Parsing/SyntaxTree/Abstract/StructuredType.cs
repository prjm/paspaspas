using System.Linq;
using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract
{

    /// <summary>
    ///     structured type definition
    /// </summary>
    public class StructuredType : StructuredTypeBase, ITypeTarget
    {

        /// <summary>
        ///     type kind
        /// </summary>
        public StructuredTypeKind Kind { get; set; }

        /// <summary>
        ///     list of base types
        /// </summary>
        public IList<ITypeSpecification> BaseTypes { get; }
            = new List<ITypeSpecification>();

        /// <summary>
        ///     base type values
        /// </summary>
        public ITypeSpecification TypeValue
        {
            get {
                return BaseTypes.LastOrDefault();
            }

            set {
                BaseTypes.Add(value);
            }
        }

        /// <summary>
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts
        {
            get {
                foreach (ITypeSpecification baseType in BaseTypes)
                    yield return baseType;
            }
        }

        /// <summary>
        ///     sealed class
        /// </summary>
        public bool AbstractClass { get; set; }

        /// <summary>
        ///     abstract class
        /// </summary>
        public bool SealedClass { get; set; }

        /// <summary>
        ///     forward declaration
        /// </summary>
        public bool ForwardDeclaration { get; set; }
    }
}
