using System.Collections.Generic;
using System.Linq;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     method implementation
    /// </summary>
    public class MethodImplementation : DeclaredSymbol, IDeclaredSymbolTarget, IBlockTarget, IDirectiveTarget, IExpression, IParameterTarget, ITypeTarget, ILabelTarget {

        /// <summary>
        ///     new method implementation
        /// </summary>
        public MethodImplementation() {
            Parameters = new ParameterDefinitions() { Parent = this };
        }

        /// <summary>
        ///     symbols
        /// </summary>
        public DeclaredSymbols Symbols { get; }
            = new DeclaredSymbols();

        /// <summary>
        ///     method kind
        /// </summary>
        public ProcedureKind Kind { get; set; }

        /// <summary>
        ///     statements
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (ParameterTypeDefinition parameter in Parameters.Items)
                    yield return parameter;

                foreach (MethodDirective directive in Directives)
                    yield return directive;

                yield return Symbols;

                if (Block != null)
                    yield return Block;
            }
        }

        /// <summary>
        ///     statements
        /// </summary>
        public StatementBase Block { get; set; }

        /// <summary>
        ///     directives
        /// </summary>
        public IList<MethodDirective> Directives { get; }
            = new List<MethodDirective>();

        /// <summary>
        ///     hints
        /// </summary>
        public SymbolHints Hints { get; set; }

        /// <summary>
        ///     parameters
        /// </summary>
        public ParameterDefinitions Parameters { get; }

        /// <summary>
        ///     return type
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     label declarations
        /// </summary>
        public IList<SymbolName> LabelDeclarations { get; }
            = new List<SymbolName>();

        /// <summary>
        ///     labels
        /// </summary>
        public SymbolName LabelName {
            get {
                return LabelDeclarations.LastOrDefault();
            }

            set {
                LabelDeclarations.Add(value);
            }
        }
    }
}

