using System.Collections.Generic;
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Types;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     formal parameter definition
    /// </summary>
    public class ParameterDefinition : SymbolTableEntryBase, IRefSymbol {

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
        ///     get the type id of the parameter
        /// </summary>
        public int TypeId {
            get {
                if (ParameterType == default || ParameterType.TypeValue == default || ParameterType.TypeValue.TypeInfo == default)
                    return KnownTypeIds.ErrorType;

                return ParameterType.TypeValue.TypeInfo.TypeId;
            }
        }

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
