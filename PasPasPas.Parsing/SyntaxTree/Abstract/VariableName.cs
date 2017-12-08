using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     variable name
    /// </summary>
    public class VariableName : DeclaredSymbol, IRefSymbol {

        /// <summary>
        ///     get the type id for this variable
        /// </summary>
        public int TypeId {
            get {
                if (ParentItem is VariableDeclaration varDeclaration && varDeclaration.TypeInfo != null)
                    return varDeclaration.TypeInfo.TypeId;
                else
                    return Signature.ErrorType;
            }
        }


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
