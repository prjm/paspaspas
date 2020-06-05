#nullable disable
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Types;
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
        public ISyntaxPartCollection<ITypeSpecification> BaseTypes { get; }

        /// <summary>
        ///     fields
        /// </summary>
        public StructureFieldDefinitionCollection Fields { get; }

        /// <summary>
        ///     methods
        /// </summary>
        public StructureMethodDefinitionCollection Methods { get; }

        /// <summary>
        ///     method resolutions
        /// </summary>
        public StructureMethodResolutionDefinition MethodResolutions { get; }

        /// <summary>
        ///     properties
        /// </summary>
        public StructurePropertyDefinitionCollection Properties { get; }

        /// <summary>
        ///     variant parts
        /// </summary>
        public StructureVariantCollection Variants { get; }

        /// <summary>
        ///     creates a new structured type
        /// </summary>
        public StructuredType() {
            BaseTypes = new SyntaxPartCollection<ITypeSpecification>();
            Variants = new StructureVariantCollection();
            Fields = new StructureFieldDefinitionCollection();
            Methods = new StructureMethodDefinitionCollection();
            MethodResolutions = new StructureMethodResolutionDefinition();
            Properties = new StructurePropertyDefinitionCollection();
        }

        /// <summary>
        ///     base type values
        /// </summary>
        public ITypeSpecification TypeValue {
            get => BaseTypes.LastOrDefault();
            set => BaseTypes.Add(value);
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
        public object GuidId { get; set; }

        /// <summary>
        ///     guid name (for interfaces)
        /// </summary>
        public SymbolName GuidName { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, BaseTypes, visitor);
            AcceptPart(this, Fields.Items, visitor);
            AcceptPart(this, Methods, visitor);
            AcceptPart(this, Properties, visitor);
            AcceptPart(this, MethodResolutions.Resolutions, visitor);
            AcceptPart(this, Symbols, visitor);
            AcceptPart(this, Variants, visitor);
            visitor.EndVisit(this);
        }

    }
}
