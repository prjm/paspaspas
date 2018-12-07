using System.Collections.Generic;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     declared constant
    /// </summary>
    public class ConstantDeclaration : DeclaredSymbol, ITypedSyntaxNode, ISymbolWithAttributes, IExpressionTarget, ITypeTarget, IRefSymbol {

        /// <summary>
        ///     constant mode
        /// </summary>
        public DeclarationMode Mode { get; set; }

        /// <summary>
        ///     declared constant type
        /// </summary>
        public DeclaredType ConstantType { get; set; }

        /// <summary>
        ///     symbol hints
        /// </summary>
        public SymbolHints Hints { get; set; }

        /// <summary>
        ///     attributes of the constants
        /// </summary>
        public IEnumerable<SymbolAttributeItem> Attributes { get; set; }

        /// <summary>
        ///     enumerate all children
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                if (TypeValue != null)
                    yield return TypeValue;
                if (Value != null)
                    yield return Value;
            }
        }

        /// <summary>
        ///     expression value
        /// </summary>
        public IExpression Value { get; set; }

        /// <summary>
        ///     type specification value
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     type information
        /// </summary>
        public ITypeReference TypeInfo {
            get; set;
        }

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
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

    }
}
