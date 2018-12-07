using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     structure field
    /// </summary>
    public class StructureField : SymbolTableEntryBase {

        /// <summary>
        ///     class item
        /// </summary>
        public bool ClassItem {
            get {
                var parent = ParentItem as StructureFields;
                return parent?.ClassItem ?? false;
            }
        }

        /// <summary>
        ///     attributes
        /// </summary>
        public IList<SymbolAttributeItem> Attributes { get; set; }

        /// <summary>
        ///     symbol name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     visibility
        /// </summary>
        public MemberVisibility Visibility
            => (ParentItem as StructureFields).Visibility;

        /// <summary>
        ///     symbol name
        /// </summary>
        protected override string InternalSymbolName
            => Name?.CompleteName;

        /// <summary>
        ///     hints
        /// </summary>
        public SymbolHints Hints
            => (ParentItem as StructureFields)?.Hints;

        /// <summary>
        ///     type vlaue
        /// </summary>
        public ITypeSpecification TypeValue
            => (ParentItem as StructureFields)?.TypeValue;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }
    }
}
