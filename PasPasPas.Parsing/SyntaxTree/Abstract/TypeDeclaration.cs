using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     abstract type declaration
    /// </summary>
    public class TypeDeclaration : DeclaredSymbol, ISymbolWithAttributes, ITypeTarget {

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
        public override IEnumerable<ISyntaxPart> Parts
        {
            get
            {
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
    }
}
