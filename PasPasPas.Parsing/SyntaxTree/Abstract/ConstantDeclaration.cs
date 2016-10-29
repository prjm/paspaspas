using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     declared constant
    /// </summary>
    public class ConstantDeclaration : DeclaredSymbol, ISymbolWithAttributes {

        /// <summary>
        ///     constant mode
        /// </summary>
        public ConstMode Mode { get; set; }

        /// <summary>
        ///     declared constant type
        /// </summary>
        public DeclaredType ConstantType { get; set; }

        /// <summary>
        ///     Symbol hints
        /// </summary>
        public SymbolHints Hints { get; set; }

        /// <summary>
        ///     attributes
        /// </summary>
        public IEnumerable<SymbolAttribute> Attributes { get; set; }

        /// <summary>
        ///     enumerate all children
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts
        {
            get
            {
                foreach (var attribute in Attributes)
                    yield return attribute;
            }
        }

    }
}
