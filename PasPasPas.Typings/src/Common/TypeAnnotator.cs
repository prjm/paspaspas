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
        private readonly Stack<Routine> currentMethodParameters;
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
            currentMethodParameters = new Stack<Routine>();
            routines = new List<(IRoutine, BlockOfStatements)>();
        }

        /// <summary>
        ///     start visiting a method declaration
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(MethodDeclaration element) {
            if (element.Name == null)
                return;

            var method = default(IRoutineGroup);
            var f = new RoutineFlags();

            if (element is GlobalMethod gm) {
                if (CurrentUnit.InterfaceSymbols != default) {
                    var unitType = GetTypeByIdOrUndefinedType(CurrentUnit.TypeInfo.TypeId) as UnitType;
                    method = new RoutineGroup(TypeRegistry, element.SymbolName);
                    unitType.AddGlobal(method);
                }
            }
            else {
                var v = currentTypeDefinition.Peek();
                var m = element as StructureMethod;
                var classMethod = m?.ClassItem ?? false;
                var genericTypeId = KnownTypeIds.ErrorType;
                var d = environment.TypeRegistry.GetTypeByIdOrUndefinedType(v.TypeId);
                /*
                if (d is RoutineType rt) {
                    d =
                }
                */

                var typeDef = v != null ? d as StructuredTypeDeclaration : null;

                if (m != default && m.Generics != default && m.Generics.Count > 0) {
                    var functionType = TypeCreator.CreateRoutineType();
                    genericTypeId = functionType.TypeId;
                }

                f.IsClassItem = classMethod;
                method = typeDef.AddOrExtendMethod(element.Name.CompleteName, genericTypeId);
            }

            currentTypeDefinition.Push((RoutineGroup)method);
            var parameters = ((RoutineGroup)method).AddParameterGroup(element.Kind, GetInstanceTypeById(KnownTypeIds.NoType));
            parameters.IsClassItem = f.IsClassItem;
            currentMethodParameters.Push(parameters);
        }

        /// <summary>
        ///     end visiting a method declaration
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(MethodDeclaration element) {
            if (element.Name == default)
                return;

            var method = currentTypeDefinition.Pop() as RoutineGroup;

            if (element.Kind == RoutineKind.Function) {

                if (currentTypeDefinition.Count < 1)
                    return;

                var v = currentTypeDefinition.Peek();
                var typeDef = v != null ? environment.TypeRegistry.GetTypeByIdOrUndefinedType(v.TypeId) as StructuredTypeDeclaration : null;
                var methodParams = currentMethodParameters.Pop();

                if (element.TypeValue != null && element.TypeValue.TypeInfo != null)
                    methodParams.ResultType = element.TypeValue.TypeInfo;
                else
                    methodParams.ResultType = TypeRegistry.MakeTypeInstanceReference(KnownTypeIds.ErrorType);
            }
        }

        /// <summary>
        ///     visit a structure field definition
        /// </summary>
        /// <param name="element">field definition</param>
        public void EndVisit(StructureFields element) {
            var typeInfo = default(IOldTypeReference);

            if (element.TypeValue != null && element.TypeValue.TypeInfo != null)
                typeInfo = element.TypeValue.TypeInfo;
            else {
                typeInfo = GetErrorTypeReference(default);
            }

            var v = currentTypeDefinition.Peek();
            var typeDef = v != null ? environment.TypeRegistry.GetTypeByIdOrUndefinedType(v.TypeId) as StructuredTypeDeclaration : null;

            foreach (var field in element.Fields) {
                var fieldDef = new Variable() {
                    Name = field.Name.CompleteName,
                    SymbolType = typeInfo,
                    Visibility = element.Visibility,
                    ClassItem = element.ClassItem
                };

                if (element.ClassItem) {
                    typeDef.AddField(fieldDef);
                }
                else
                    typeDef.AddField(fieldDef);
            }
        }


        /// <summary>
        ///     visit an class of declaration
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ClassOfTypeDeclaration element) {
            if (element.TypeValue?.TypeInfo == default) {
                element.TypeInfo = GetErrorTypeReference(element);
                return;
            }

            var baseType = TypeRegistry.GetTypeByIdOrUndefinedType(element.TypeValue.TypeInfo.TypeId);

            if (!(baseType is IStructuredType)) {
                element.TypeInfo = GetErrorTypeReference(element);
                return;
            }

            var alias = TypeCreator.CreateMetaClassType(baseType.TypeId);
            element.TypeInfo = GetInstanceTypeById(alias.TypeId);
        }

        /// <summary>
        ///     start visiting a method implementation
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(MethodImplementation element) {
            var isClassMethod = element.Declaration?.DefiningType != default;
            var definingType = isClassMethod ? element.Declaration.DefiningType.TypeId : KnownTypeIds.ErrorType;
            var isForward = element.Flags.HasFlag(MethodImplementationFlags.ForwardDeclaration);
            var routine = default(IRoutineGroup);

            if (!isClassMethod) {
                var unitType = GetTypeByIdOrUndefinedType(CurrentUnit.TypeInfo.TypeId) as UnitType;

                if (unitType.HasGlobalRoutine(element.SymbolName)) {
                    if (!unitType.AddGlobalImplementation(element.SymbolName, out routine)) {
                        routine = new RoutineGroup(TypeRegistry, element.SymbolName);
                    }
                }
                else {
                    routine = new RoutineGroup(TypeRegistry, element.SymbolName);
                    unitType.AddGlobal(routine);
                    resolver.AddToScope(element.SymbolName, ReferenceKind.RefToGlobalRoutine, routine);
                }
                currentMethodParameters.Push((routine as RoutineGroup).AddParameterGroup(element.Kind, GetInstanceTypeById(KnownTypeIds.NoType)));
            }
            else if (isClassMethod) {
                var baseType = TypeRegistry.GetTypeByIdOrUndefinedType(definingType) as IStructuredType;

                if (baseType != default)
                    routine = baseType.FindMethod(element.Name.Name, element.Declaration.ClassItem);

            }

            if (routine != default) {
                resolver.OpenScope();
                currentMethodImplementation.Push(new RoutineIndex(routine, -1));
            }
        }

        /// <summary>
        ///     start visiting the procedure heading marker
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ProcedureHeadingMarker element) {
            if (currentMethodImplementation.Count < 1)
                return;

            var routine = currentMethodImplementation.Peek();
            routine.Index = 0;

            var parameters = routine.Parameters;
            if (parameters != default && parameters.Parameters != default) {
                foreach (var parameter in parameters.Parameters) {
                    resolver.AddToScope(parameter.Name, ReferenceKind.RefToParameter, parameter);
                }
            }

            if (routine.DefiningType != KnownTypeIds.UnspecifiedType) {
                var baseTypeDef = GetTypeByIdOrUndefinedType(routine.DefiningType) as IStructuredType;

                if (baseTypeDef != default && !routine.IsClassItem)
                    resolver.AddToScope("Self", ReferenceKind.RefToSelf, baseTypeDef);

                if (baseTypeDef != default && routine.IsClassItem) {
                    resolver.AddToScope("Self", ReferenceKind.RefToSelfClass, baseTypeDef);
                }
            }
        }

        /// <summary>
        ///     end visiting a method implementation
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(MethodImplementation element) {
            var isForward = element.Flags.HasFlag(MethodImplementationFlags.ForwardDeclaration);
            var isClassMethod = element.Declaration?.DefiningType != default;

            resolver.CloseScope();

            if (!isClassMethod) {
                var parameters = currentMethodParameters.Pop();
                if (element.TypeValue != null && element.TypeValue.TypeInfo != null)
                    parameters.ResultType = element.TypeValue.TypeInfo;
                else
                    parameters.ResultType = TypeRegistry.MakeTypeInstanceReference(KnownTypeIds.ErrorType);

            }

            currentMethodImplementation.Pop();
        }

        /// <summary>
        ///     file type declaration
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(FileTypeDeclaration element) {

            if (element.TypeValue == default) {
                element.TypeInfo = GetTypeReferenceById(KnownTypeIds.UntypedFile);
                return;
            }

            if (element.TypeValue.TypeInfo == default) {
                element.TypeInfo = GetErrorTypeReference(element);
                return;
            }

            var type = TypeRegistry.TypeCreator.CreateFileType(element.TypeValue.TypeInfo.TypeId);
            element.TypeInfo = GetTypeReferenceById(type.TypeId);
        }

        /// <summary>
        ///     visit a formatted expression
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(FormattedExpression element) {
            if (element.IsConstant) {
                using (var list = environment.ListPools.GetList<IOldTypeReference>()) {
                    for (var i = 0; i < element.Expressions.Count; i++) {
                        list.Item.Add(element.Expressions[i].TypeInfo);
                    }
                    element.TypeInfo = Runtime.FormatExpression(environment.ListPools.GetFixedArray(list));
                }
            }
            else {
                var baseTypeId = element.Expressions[0].TypeInfo?.TypeId ?? KnownTypeIds.ErrorType;
                var type = GetTypeByIdOrUndefinedType(baseTypeId);

                if (type.TypeKind.IsString() || type.TypeKind.IsChar() || type.TypeKind.IsNumerical() || type.TypeKind == CommonTypeKind.BooleanType)
                    element.TypeInfo = GetInstanceTypeById(KnownTypeIds.StringType);
                else
                    element.TypeInfo = GetErrorTypeReference(element);
            }
        }

        /// <summary>
        ///     visit a generic constraint
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(GenericConstraint element) {
            if (element.Kind == GenericConstraintKind.Class) {
                element.TypeInfo = GetInstanceTypeById(KnownTypeIds.GenericClassConstraint);
                return;
            }

            if (element.Kind == GenericConstraintKind.Record) {
                element.TypeInfo = GetInstanceTypeById(KnownTypeIds.GenericRecordConstraint);
                return;
            }

            if (element.Kind == GenericConstraintKind.Constructor) {
                element.TypeInfo = GetInstanceTypeById(KnownTypeIds.GenericConstructorConstraint);
                return;
            }

            if (element.Kind == GenericConstraintKind.Identifier) {
                var reference = resolver.ResolveByName(default, element.SymbolName, 0, ResolverFlags.None);

                if (reference.Kind == ReferenceKind.RefToType) {
                    element.TypeInfo = GetTypeReferenceById(reference.Symbol.TypeId);
                    return;
                }

                return;
            }
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
