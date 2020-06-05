#nullable disable
using System.Collections.Generic;
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Types;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     abstract type declaration
    /// </summary>
    public class TypeDeclaration : DeclaredSymbol, ISymbolWithAttributes, ITypeTarget, IDeclaredSymbolTarget, ITypedSyntaxPart {

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
        public ITypeSymbol TypeInfo { get; set; }

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
