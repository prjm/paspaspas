using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     method implementation
    /// </summary>
    public class MethodImplementation : DeclaredSymbol, IDeclaredSymbolTarget, IBlockTarget, IDirectiveTarget, IExpression, IParameterTarget, ITypeTarget, IMethodImplementation, ITypedSyntaxPart {

        /// <summary>
        ///     calculated type value
        /// </summary>
        public IOldTypeReference TypeInfo { get; set; }

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
        public RoutineKind Kind { get; set; }

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
        public IOldTypeReference LiteralValue { get; set; }

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
        ///     global method
        /// </summary>
        public virtual bool IsGlobalMethod
            => false;

        /// <summary>
        ///     marker
        /// </summary>
        public ProcedureHeadingMarker Marker { get; }
            = new ProcedureHeadingMarker();

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Parameters.Items, visitor);
            AcceptPart(this, TypeValue, visitor);
            AcceptPart(this, Directives, visitor);
            AcceptPart(this, Symbols, visitor);
            AcceptPart(this, Marker, visitor);
            AcceptPart(this, Block, visitor);
            visitor.EndVisit(this);
        }

    }
}

