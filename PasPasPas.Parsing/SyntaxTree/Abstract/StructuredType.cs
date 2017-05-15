using System.Linq;
using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using PasPasPas.Parsing.SyntaxTree.Utils;

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
        public ISyntaxPartList<ITypeSpecification> BaseTypes { get; }

        /// <summary>
        ///     fields
        /// </summary>
        public StructureFieldDefinition Fields { get; }

        /// <summary>
        ///     methods
        /// </summary>
        public StructureMethodDefinition Methods { get; }

        /// <summary>
        ///     method resolutions
        /// </summary>
        public StructureMethodResolutionDefinition MethodResolutions { get; }

        /// <summary>
        ///     properties
        /// </summary>
        public StructurePropertyDefinition Properties { get; }

        /// <summary>
        ///     variant parts
        /// </summary>
        public StructureVariant Variants { get; }

        /// <summary>
        ///     creates a new structured type
        /// </summary>
        public StructuredType() {
            BaseTypes = new SyntaxPartCollection<ITypeSpecification>(this);
            Variants = new StructureVariant() { ParentItem = this };
            Fields = new StructureFieldDefinition() { ParentItem = this };
            Methods = new StructureMethodDefinition() { ParentItem = this };
            MethodResolutions = new StructureMethodResolutionDefinition() { ParentItem = this };
            Properties = new StructurePropertyDefinition() { ParentItem = this };
        }

        /// <summary>
        ///     base type values
        /// </summary>
        public ITypeSpecification TypeValue {
            get => BaseTypes.LastOrDefault();
            set => BaseTypes.Add(value);
        }

        /// <summary>
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (ITypeSpecification baseType in BaseTypes)
                    yield return baseType;
                foreach (StructureFields fields in Fields.Items)
                    yield return fields;
                foreach (StructureMethod method in Methods)
                    yield return method;
                foreach (StructureProperty property in Properties)
                    yield return property;
                foreach (StructureMethodResolution resolution in MethodResolutions.Resolutions)
                    yield return resolution;
                foreach (DeclaredSymbol symbol in Symbols)
                    yield return symbol;
                foreach (StructureVariantItem variant in Variants.Items)
                    yield return variant;
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
        ///     guid id (for interfaces)
        /// </summary>
        public string GuidId { get; set; }

        /// <summary>
        ///     guid name (for interfaces)
        /// </summary>
        public SymbolName GuidName { get; set; }

        /// <summary>
        ///     map visilibity
        /// </summary>
        /// <param name="visibility"></param>
        /// <param name="strict"></param>
        /// <returns></returns>
        [System.Obsolete]
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
