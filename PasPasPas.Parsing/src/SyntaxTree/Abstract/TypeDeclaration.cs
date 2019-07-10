using System.Collections.Generic;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     abstract type declaration
    /// </summary>
    public class TypeDeclaration : DeclaredSymbol, ISymbolWithAttributes, ITypeTarget, IDeclaredSymbolTarget, IRefSymbol, ITypedSyntaxNode {

        /// <summary>
        ///     attributes
        /// </summary>
        public List<SymbolAttributeItem> Attributes { get; }

        /// <summary>
        ///     declared type
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     symbol hints
        /// </summary>
        public SymbolHints Hints { get; set; }

        /// <summary>
        ///     generic type
        /// </summary>
        public GenericTypeCollection Generics { get; set; }

        /// <summary>
        ///     declared symbols
        /// </summary>
        public DeclaredSymbolCollection Symbols {
            get {
                if (TypeValue is IDeclaredSymbolTarget delegatedValue)
                    return delegatedValue.Symbols;
                else
                    return null;
            }
        }

        /// <summary>
        ///     type info
        /// </summary>
        public ITypeReference TypeInfo { get; set; }

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
            AcceptPart(this, Generics, visitor);
            AcceptPart(this, TypeValue, visitor);
            visitor.EndVisit(this);
        }
    }
}
