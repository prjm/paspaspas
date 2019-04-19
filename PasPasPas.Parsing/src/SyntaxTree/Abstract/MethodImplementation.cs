using System.Collections.Generic;
using PasPasPas.Globals.Runtime;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     method implementation
    /// </summary>
    public class MethodImplementation : DeclaredSymbol, IDeclaredSymbolTarget, IBlockTarget, IDirectiveTarget, IExpression, IParameterTarget, ITypeTarget, IMethodImplementation {

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
            Directives = new SyntaxPartCollection<MethodDirective>();
            Parameters = new ParameterDefinitionCollection();
        }

        /// <summary>
        ///     statements
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (var parameter in Parameters.Items)
                    yield return parameter;

                if (TypeValue != default)
                    yield return TypeValue;

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
        ///     if <c>true</c> then no parameters were declared for this
        ///     implementation
        /// </summary>
        public bool DefaultParameters { get; set; }

        /// <summary>
        ///     previous declaration
        /// </summary>
        public StructureMethod Declaration { get; set; }

        /// <summary>
        ///     method flags
        /// </summary>
        public MethodImplementationFlags Flags { get; set; }

        /// <summary>
        ///     anchor point
        /// </summary>
        public SingleDeclaredSymbol Anchor { get; set; }

        /// <summary>
        ///     <c>true</c> if this is a forward declaration
        /// </summary>
        public bool IsForwardDeclaration
            => (Flags & MethodImplementationFlags.ForwardDeclaration) == MethodImplementationFlags.ForwardDeclaration;

        /// <summary>
        ///     returns always <c>false</c>
        /// </summary>
        public bool IsExportedMethod
            => false;

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

