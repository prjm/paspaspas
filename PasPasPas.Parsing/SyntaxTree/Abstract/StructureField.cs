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
                var parent = Parent as StructureFields;
                return parent?.ClassItem ?? false;
            }
        }

        /// <summary>
        ///     attributes
        /// </summary>
        public IList<SymbolAttribute> Attributes { get; set; }

        /// <summary>
        ///     symbol name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     visibility
        /// </summary>
        public MemberVisibility Visibility
            => (Parent as StructureFields).Visibility;

        /// <summary>
        ///     symbol name
        /// </summary>
        protected override string InternalSymbolName {
            get {
                return Name?.CompleteName;
            }
        }

        /// <summary>
        ///     hints
        /// </summary>
        public SymbolHints Hints
            => (Parent as StructureFields)?.Hints;

        /// <summary>
        ///     type vlaue
        /// </summary>
        public ITypeSpecification TypeValue
            => (Parent as StructureFields)?.TypeValue;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }
    }
}
