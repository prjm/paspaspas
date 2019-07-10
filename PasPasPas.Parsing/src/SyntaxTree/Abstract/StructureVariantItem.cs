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
        public ISyntaxPartCollection<StructureVariantFields> Items { get; }

        /// <summary>
        ///     type target
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     create a new variant item of a structure
        /// </summary>
        public StructureVariantItem() =>
            Items = new SyntaxPartCollection<StructureVariantFields>();

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, TypeValue, visitor);
            foreach (var field in Items)
                AcceptPart(this, field, visitor);
            visitor.EndVisit(this);
        }

    }

}