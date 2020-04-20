using System.Collections.Generic;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using PasPasPas.Typings.Structured;

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
        private readonly Stack<ITypeSymbol> currentTypeDefinition;
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
            => CurrentUnit?.TypeInfo as IUnitType;

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
            currentTypeDefinition = new Stack<ITypeSymbol>();
            currentMethodParameters = new Stack<IRoutine>();
            routines = new List<(IRoutine, BlockOfStatements)>();
        }


        /// <summary>
        ///     visit generic elements
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(GenericTypeNameCollection element) {
            var hasError = false;
            var genericTypeRef = currentTypeDefinition.Peek();
            var genericType = GetTypeByIdOrUndefinedType(genericTypeRef.TypeId) as IExtensibleGenericType;

            if (genericType == default)
                return;

            using (var list = environment.ListPools.GetList<int>()) {
                foreach (var constraint in element) {

                    if (constraint.TypeInfo != default)
                        list.Item.Add(constraint.TypeInfo.TypeId);
                    else
                        hasError = true;
                }

                if (hasError)
                    element.TypeInfo = GetErrorTypeReference(default);
                else if (list.Item.Count < 1)
                    element.TypeInfo = GetInstanceTypeById(KnownTypeIds.UnconstrainedGenericTypeParameter);
                else {
                    var typeDef = TypeRegistry.TypeCreator.CreateUnboundGenericTypeParameter(environment.ListPools.GetFixedArray(list));
                    element.TypeInfo = GetInstanceTypeById(typeDef.TypeId);
                }

                if (!hasError)
                    genericType.AddGenericParameter(element.TypeInfo.TypeId);
            }
        }

        private bool AreRecordTypesCompatible(int leftId, int rightId)
            => environment.TypeRegistry.AreRecordTypesCompatible(leftId, rightId);

        private int GetSmallestTextTypeOrNext(int leftId, int rightId)
            => environment.TypeRegistry.GetSmallestTextTypeOrNext(leftId, rightId);

        /// <summary>
        ///     visit a symbol declaration
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(DeclaredSymbolCollection element) {
            if (element == CurrentUnit?.InterfaceSymbols) {
                foreach (var symbol in CurrentUnit.InterfaceSymbols) {

                    var unitType = GetTypeByIdOrUndefinedType(CurrentUnit.TypeInfo.TypeId) as UnitType;
                    var kind = ReferenceKind.Unknown;
                    var refSymbol = default(IRefSymbol);

                    switch (symbol) {
                        case ConstantDeclaration constDecl:
                            refSymbol = constDecl;
                            kind = ReferenceKind.RefToConstant;
                            break;
                    }

                    if (kind != ReferenceKind.Unknown)
                        unitType.RegisterSymbol(symbol.SymbolName, new Reference(kind, refSymbol), 0);
                }
            }
        }

        /// <summary>
        ///     use a required unit
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(RequiredUnitName element) {
            var unitType = resolver.ResolveUnit(element.Name.CompleteName);
            if (unitType != default)
                resolver.AddToScope(element.Name.CompleteName, ReferenceKind.RefToUnit, unitType);
        }

        private ITypeCreator TypeCreator
            => TypeRegistry.TypeCreator;

    }
}
