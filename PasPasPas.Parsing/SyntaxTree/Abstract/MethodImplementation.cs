using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     method implementation
    /// </summary>
    public class MethodImplementation : DeclaredSymbol, IDeclaredSymbolTarget, IBlockTarget, IDirectiveTarget, IExpression, IParameterTarget, ITypeTarget {


        /// <summary>
        ///     calculated type value
        /// </summary>
        public ITypeDefinition TypeInfo { get; set; }

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
        ///     create a new method implementation
        /// </summary>
        public MethodImplementation() {
            Directives = new SyntaxPartCollection<MethodDirective>(this);
            Parameters = new ParameterDefinitions() { ParentItem = this };
        }

        /// <summary>
        ///     statements
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (var parameter in Parameters.Items)
                    yield return parameter;

                foreach (var directive in Directives)
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
        public ISyntaxPartList<MethodDirective> Directives { get; }

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
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

    }
}

