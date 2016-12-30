using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     structure field
    /// </summary>
    public class StructureField : SymbolTableEntryBase {

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
    }
}
