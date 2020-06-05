#nullable disable
using System.Collections.Generic;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     visitor to annotate types in abstract syntax trees
    /// </summary>
    /// <remarks>
    ///     also performs constant propagation, because types
    ///     can be inferred at compile time from literal constant values
    /// </remarks>
    public partial class TypeAnnotator :

        IEndVisitor<Parsing.SyntaxTree.Abstract.ConstantValue>,
        IEndVisitor<UnaryOperator>,
        IStartVisitor<BinaryOperator>, IEndVisitor<BinaryOperator>,
        IEndVisitor<VariableDeclaration>,
        IEndVisitor<ConstantDeclaration>,
        IChildVisitor<ConstantDeclaration>,
        IEndVisitor<Parsing.SyntaxTree.Abstract.TypeAlias>,
        IEndVisitor<MetaType>,
        IEndVisitor<SymbolReference>,
        IStartVisitor<CompilationUnit>,
        IEndVisitor<CompilationUnit>,
        IStartVisitor<EnumTypeCollection>, IEndVisitor<EnumTypeCollection>,
        IEndVisitor<EnumTypeValue>,
        IEndVisitor<Parsing.SyntaxTree.Abstract.SubrangeType>,
        IStartVisitor<TypeDeclaration>, IEndVisitor<TypeDeclaration>,
        IEndVisitor<SetTypeDeclaration>,
        IEndVisitor<ArrayTypeDeclaration>,
        IStartVisitor<StructuredType>,
        IEndVisitor<StructuredType>,
        IStartVisitor<MethodDeclaration>, IEndVisitor<MethodDeclaration>,
        IEndVisitor<ParameterTypeDefinition>,
        IEndVisitor<StructureFields>,
        IEndVisitor<SetExpression>,
        IEndVisitor<ArrayConstant>,
        IStartVisitor<RecordConstant>, IEndVisitor<RecordConstant>,
        IStartVisitor<MethodImplementation>, IEndVisitor<MethodImplementation>,
        IStartVisitor<ProcedureHeadingMarker>,
        IEndVisitor<RecordConstantItem>,
        IEndVisitor<ClassOfTypeDeclaration>,
        IEndVisitor<FileTypeDeclaration>,
        IEndVisitor<FormattedExpression>,
        IEndVisitor<GenericConstraint>,
        IEndVisitor<GenericTypeNameCollection>,
        IEndVisitor<DeclaredSymbolCollection>,
        IEndVisitor<RequiredUnitName>,
        IStartVisitor<BlockOfStatements>, IEndVisitor<BlockOfStatements>,
        IEndVisitor<StructuredStatement> {

        private readonly IStartEndVisitor visitor;
        private readonly ITypedEnvironment environment;
        private readonly Stack<ITypeDefinition> currentTypeDefinition;
        private readonly Stack<IRoutine> currentMethodParameters;
        private readonly Resolver resolver;
        private readonly List<(IRoutine, BlockOfStatements)> routines;

        /// <summary>
        ///     current unit definition
        /// </summary>
        public CompilationUnit CurrentUnit { get; set; }

        /// <summary>
        ///     current unit type
        /// </summary>
        public IUnitType CurrentUnitType
            => CurrentUnit?.TypeInfo.TypeDefinition as IUnitType;

        /// <summary>
        ///     cast this visitor as common visitor
        /// </summary>
        /// <returns></returns>
        public IStartEndVisitor AsVisitor()
            => visitor;

        /// <summary>
        ///     create a new type annotator
        /// </summary>
        /// <param name="env">typed environment</param>
        public TypeAnnotator(ITypedEnvironment env) {
            visitor = new ChildVisitor(this);
            environment = env;
            resolver = new Resolver(new Scope(env.TypeRegistry));
            currentTypeDefinition = new Stack<ITypeDefinition>();
            currentMethodParameters = new Stack<IRoutine>();
            routines = new List<(IRoutine, BlockOfStatements)>();
        }

        /// <summary>
        ///     reference to the error type
        /// </summary>
        private ITypeSymbol ErrorReference
            => SystemUnit.ErrorType.Reference;

        /// <summary>
        ///     no type
        /// </summary>
        private ITypeDefinition NoType
            => SystemUnit.NoType;

    }
}
