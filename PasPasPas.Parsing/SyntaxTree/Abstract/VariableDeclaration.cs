using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     variable declaration
    /// </summary>
    public class VariableDeclaration : DeclaredSymbolGroup, ITypeTarget {

        /// <summary>
        ///     declaration mode
        /// </summary>
        public DeclarationMode Mode { get; set; }

        /// <summary>
        ///     optional variable type specification
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     symbol hints
        /// </summary>
        public SymbolHints Hints { get; internal set; }

        /// <summary>
        ///     variable names
        /// </summary>
        public IList<VariableName> Names { get; }
            = new List<VariableName>();

        /// <summary>
        ///     enumerate part
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (VariableName name in Names)
                    yield return name;
            }
        }

    }
}
