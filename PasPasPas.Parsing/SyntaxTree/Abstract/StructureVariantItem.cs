using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     item in a variant structure
    /// </summary>
    public class StructureVariantItem : AbstractSyntaxPartBase, ITypeTarget {

        /// <summary>
        ///     variant name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     fields
        /// </summary>
        public ISyntaxPartList<StructureVariantFields> Items { get; }

        /// <summary>
        ///     type target
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     create a new variant item of a structure
        /// </summary>
        public StructureVariantItem() =>
            Items = new SyntaxPartCollection<StructureVariantFields>(this);

        /// <summary>
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                if (TypeValue != null)
                    yield return TypeValue;
                foreach (StructureVariantFields field in Items)
                    yield return field;
            }
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