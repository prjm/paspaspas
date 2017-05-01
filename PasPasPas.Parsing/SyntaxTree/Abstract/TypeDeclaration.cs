using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     abstract type declaration
    /// </summary>
    public class TypeDeclaration : DeclaredSymbol, ISymbolWithAttributes, ITypeTarget, IDeclaredSymbolTarget {

        /// <summary>
        ///     attribues
        /// </summary>
        public IEnumerable<SymbolAttribute> Attributes { get; set; }

        /// <summary>
        ///     declare type
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
                    foreach (GenericType generic in Generics)
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
                var delegatedValue = TypeValue as IDeclaredSymbolTarget;
                if (delegatedValue != null)
                    return delegatedValue.Symbols;
                else
                    return null;
            }
        }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }
    }
}
