using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     formal parameter definition
    /// </summary>
    public class ParameterDefinition : SymbolTableEntryBase {

        /// <summary>
        ///     attributes
        /// </summary>
        public IList<SymbolAttributeItem> Attributes { get; }
            = new List<SymbolAttributeItem>();

        /// <summary>
        ///     parameter name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     parameter kind
        /// </summary>
        public ParameterReferenceKind ParameterKind { get; set; }

        /// <summary>
        ///     parameter type
        /// </summary>
        public ParameterTypeDefinition ParameterType { get; set; }

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
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }
    }
}
