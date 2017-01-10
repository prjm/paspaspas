using System.Linq;
using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     structured type definition
    /// </summary>
    public class StructuredType : StructuredTypeBase, ITypeTarget {

        /// <summary>
        ///     type kind
        /// </summary>
        public StructuredTypeKind Kind { get; set; }
            = StructuredTypeKind.Undefined;

        /// <summary>
        ///     list of base types
        /// </summary>
        public IList<ITypeSpecification> BaseTypes { get; }
                = new List<ITypeSpecification>();

        /// <summary>
        ///     fields
        /// </summary>
        public StructureFieldDefinition Fields { get; }
            = new StructureFieldDefinition();

        /// <summary>
        ///     methods
        /// </summary>
        public StructureMethodDefinition Methods { get; }
            = new StructureMethodDefinition();

        /// <summary>
        ///     method resolutions
        /// </summary>
        public StructureMethodResolutionDefinition MethodResolutions { get; }
            = new StructureMethodResolutionDefinition();

        /// <summary>
        ///     properties
        /// </summary>
        public StructurePropertyDefinition Properties { get; }
            = new StructurePropertyDefinition();

        /// <summary>
        ///     base type values
        /// </summary>
        public ITypeSpecification TypeValue {
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
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (ITypeSpecification baseType in BaseTypes)
                    yield return baseType;
                foreach (StructureFields fields in Fields.Fields)
                    yield return fields;
                foreach (StructureMethod method in Methods)
                    yield return method;
                foreach (StructureProperty property in Properties)
                    yield return property;
                foreach (StructureMethodResolution resolution in MethodResolutions.Resolutions)
                    yield return resolution;
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

        /// <summary>
        ///     map visilibity
        /// </summary>
        /// <param name="visibility"></param>
        /// <param name="strict"></param>
        /// <returns></returns>
        public static MemberVisibility MapVisibility(int visibility, bool strict) {

            switch (visibility) {
                case TokenKind.Private:
                    return strict ? MemberVisibility.StrictPrivate : MemberVisibility.Private;

                case TokenKind.Protected:
                    return strict ? MemberVisibility.StrictProtected : MemberVisibility.Protected;

                case TokenKind.Public:
                    return MemberVisibility.Public;

                case TokenKind.Published:
                    return MemberVisibility.Published;

                case TokenKind.Automated:
                    return MemberVisibility.Automated;

            }

            return MemberVisibility.Undefined;
        }
    }
}
