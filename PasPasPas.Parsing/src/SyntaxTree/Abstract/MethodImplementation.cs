using System.Collections.Generic;
using PasPasPas.Globals.Runtime;
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
        public ITypeReference TypeInfo { get; set; }

        /// <summary>
        ///     <c>true</c> for constant items
        /// </summary>
        public bool IsConstant { get; set; }

        /// <summary>
        ///     symbols
        /// </summary>
        public DeclaredSymbolCollection Symbols { get; }
            = new DeclaredSymbolCollection();

        /// <summary>
        ///     method kind
        /// </summary>
        public ProcedureKind Kind { get; set; }

        /// <summary>
        ///     create a new method implementation
        /// </summary>
        public MethodImplementation() {
            Directives = new SyntaxPartCollection<MethodDirective>(this);
            Parameters = new ParameterDefinitionCollection() { ParentItem = this };
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
        public ISyntaxPartCollection<MethodDirective> Directives { get; }

        /// <summary>
        ///     hints
        /// </summary>
        public SymbolHints Hints { get; set; }

        /// <summary>
        ///     parameters
        /// </summary>
        public ParameterDefinitionCollection Parameters { get; }

        /// <summary>
        ///     return type
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     literal value
        /// </summary>
        public ITypeReference LiteralValue { get; set; }

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

