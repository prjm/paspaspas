using System.Collections.Generic;
using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     abstract type declaration
    /// </summary>
    public class TypeDeclaration : DeclaredSymbol, ISymbolWithAttributes, ITypeTarget, IDeclaredSymbolTarget, IRefSymbol, ITypedSyntaxNode {

        /// <summary>
        ///     attributes
        /// </summary>
        public IEnumerable<SymbolAttribute> Attributes { get; set; }

        /// <summary>
        ///     declared type
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     symbol hints
        /// </summary>
        public SymbolHints Hints { get; set; }

        /// <summary>
        ///     enumerate all children
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                if (Generics != null)
                    foreach (var generic in Generics)
                        yield return generic;

                if (TypeValue != null)
                    yield return TypeValue;
            }
        }

        /// <summary>
        ///     generic typey
        /// </summary>
        public GenericTypes Generics { get; set; }

        /// <summary>
        ///     declared symbols
        /// </summary>
        public DeclaredSymbols Symbols {
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
