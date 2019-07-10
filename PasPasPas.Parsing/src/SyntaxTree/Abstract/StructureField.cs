using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     structure field
    /// </summary>
    public class StructureField : SymbolTableEntryBase {

        /// <summary>
        ///     attributes
        /// </summary>
        public List<SymbolAttributeItem> Attributes { get; }
            = new List<SymbolAttributeItem>();

        /// <summary>
        ///     symbol name
        /// </summary>
        public SymbolName Name { get; set; }

        // <summary>
        //     visibility
        // </summary>
        //        public MemberVisibility Visibility
        //          => (ParentItem as StructureFields).Visibility;

        /// <summary>
        ///     symbol name
        /// </summary>
        protected override string InternalSymbolName
            => Name?.CompleteName;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            visitor.EndVisit(this);
        }
    }
}
