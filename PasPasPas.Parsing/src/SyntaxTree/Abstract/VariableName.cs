using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     variable name
    /// </summary>
    public class VariableName : DeclaredSymbol, IRefSymbol, ITypedSyntaxPart {

        /// <summary>
        ///     get the type id for this variable
        /// </summary>
        public IOldTypeReference TypeInfo { get; set; }

        /// <summary>
        ///     type id
        /// </summary>
        public int TypeId
            => TypeInfo != null ? TypeInfo.TypeId : KnownTypeIds.ErrorType;

        /// <summary>
        ///     parent declaration
        /// </summary>
        public VariableDeclaration Declaration { get; set; }

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
