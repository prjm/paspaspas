using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     enum type value
    /// </summary>
    public class EnumTypeValue : SymbolTableEntryBase, IExpressionTarget, IRefSymbol, ITypedSyntaxPart {

        /// <summary>
        ///     enum name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     enum expression value
        /// </summary>
        public IExpression Value { get; set; }

        /// <summary>
        ///     symbol name
        /// </summary>
        protected override string InternalSymbolName
            => Name?.CompleteName;

        /// <summary>
        ///     type information
        /// </summary>
        public IOldTypeReference TypeInfo { get; set; }

        /// <summary>
        ///     type id
        /// </summary>
        public int TypeId
            => TypeInfo != null ? TypeInfo.TypeId : KnownTypeIds.ErrorType;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Value, visitor);
            visitor.EndVisit(this);
        }

    }
}
