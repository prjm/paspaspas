using System.Collections.Generic;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using PasPasPas.Typings.Routines;
using PasPasPas.Typings.Simple;
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
        IEndVisitor<StructuredStatement>,
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
        IEndVisitor<RequiredUnitName> {

        private readonly IStartEndVisitor visitor;
        private readonly ITypedEnvironment environment;
        private readonly Stack<ITypeReference> currentTypeDefinition;
        private readonly Stack<ParameterGroup> currentMethodParameters;
        private readonly Resolver resolver;

        /// <summary>
        ///     current unit definition
        /// </summary>
        public CompilationUnit CurrentUnit { get; set; }

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
            currentTypeDefinition = new Stack<ITypeReference>();
            currentMethodParameters = new Stack<ParameterGroup>();
        }

        private ITypeReference GetTypeRefence(ITypedSyntaxNode syntaxNode) {
            if (syntaxNode != default && syntaxNode.TypeInfo != null)
                return syntaxNode.TypeInfo;

            return GetErrorTypeReference(syntaxNode);
        }

        private ITypeDefinition GetRegisteredType(ITypedSyntaxNode syntaxNode) {
            if (syntaxNode != null && syntaxNode.TypeInfo != null)
                return environment.TypeRegistry.GetTypeByIdOrUndefinedType(syntaxNode.TypeInfo.TypeId);

            return GetErrorType(syntaxNode);
        }

        private ITypeDefinition GetTypeByIdOrUndefinedType(int typeId)
            => environment.TypeRegistry.GetTypeByIdOrUndefinedType(typeId);

        private ITypeReference GetInstanceTypeById(int typeId)
            => environment.TypeRegistry.MakeTypeInstanceReference(typeId);

        private ITypeReference GetTypeReferenceById(int typeId)
            => environment.TypeRegistry.MakeTypeReference(typeId);

        private ITypeDefinition GetErrorType(ITypedSyntaxNode node)
            => environment.TypeRegistry.GetTypeByIdOrUndefinedType(KnownTypeIds.ErrorType);

        private ITypeReference GetErrorTypeReference(ITypedSyntaxNode node)
            => environment.Runtime.Types.MakeErrorTypeReference();

        private int GetSmallestIntegralTypeOrNext(int leftId, int rightId)
            => environment.TypeRegistry.GetSmallestIntegralTypeOrNext(leftId, rightId);

        private ITypeRegistry TypeRegistry
            => environment.TypeRegistry;

        private IRuntimeValueFactory Runtime
            => environment.Runtime;

        /// <summary>
        ///     visit a variable declaration
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(VariableDeclaration element) {
            if (element.TypeValue is ITypedSyntaxNode typeRef && typeRef.TypeInfo != null)
                element.TypeInfo = typeRef.TypeInfo;
            else
                element.TypeInfo = GetErrorTypeReference(element);

            var t = environment.TypeRegistry.GetTypeByIdOrUndefinedType(element.TypeInfo.TypeId);
            var fromResult = true;

            foreach (var vardef in element.Names) {

                resolver.AddToScope(vardef.Name.CompleteName, ReferenceKind.RefToVariable, vardef);

                if (vardef is FunctionResult fn) {
                    if (fn.Method.TypeValue != default)
                        vardef.TypeInfo = GetInstanceTypeById(fn.Method.TypeValue.TypeInfo.TypeId);
                    else
                        vardef.TypeInfo = GetErrorTypeReference(element);
                    continue;
                }

                fromResult = false;
                vardef.TypeInfo = element.TypeInfo;
            }

            if (fromResult && element.Names.Count > 0 && element.Names[0].TypeInfo != default) {
                element.TypeInfo = element.Names[0].TypeInfo;
            }
        }

        /// <summary>
        ///     visit a variable declaration
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(Parsing.SyntaxTree.Abstract.TypeAlias element) {

            if (element.Fragments == default || element.Fragments.Count < 1) {
                element.TypeInfo = GetErrorTypeReference(element);
                return;
            }

            var entry = default(Reference);
            for (var i = 0; i < element.Fragments.Count && (i == 0 || entry != default); i++) {
                var fragment = element.Fragments[i];

                if (fragment.TypeValues.Count < 1) {
                    entry = resolver.ResolveReferenceByName(entry, fragment.Name);
                    continue;
                }

                using (var list = environment.ListPools.GetList<int>()) {
                    foreach (var typeValue in fragment.TypeValues) {
                        if (typeValue.TypeInfo == default || typeValue.TypeInfo.TypeId == KnownTypeIds.ErrorType) {
                            element.TypeInfo = GetErrorTypeReference(element);
                            return;
                        }

                        list.Item.Add(typeValue.TypeInfo.TypeId);
                    }

                    entry = resolver.ResolveReferenceByName(entry, fragment.Name, fragment.TypeValues.Count);

                    if (entry != default) {
                        var genericType = GetTypeByIdOrUndefinedType(entry.Symbol.TypeId) as IGenericType;
                        if (genericType != default)
                            entry = genericType.Bind(environment.ListPools.GetFixedArray(list));
                    }
                    else
                        entry = default;
                }
            }

            var typeId = default(int);

            if (entry != default && (entry.Kind == ReferenceKind.RefToType || entry.Kind == ReferenceKind.RefToBoundGeneric))
                typeId = entry.Symbol.TypeId;
            else
                typeId = KnownTypeIds.ErrorType;

            var type = GetTypeByIdOrUndefinedType(typeId);

            if (element.IsNewType) {
                typeId = TypeCreator.CreateTypeAlias(typeId, true).TypeId;
            }

            element.TypeInfo = GetInstanceTypeById(typeId);
        }

        /// <summary>
        ///     visit a meta type reference
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(MetaType element) {
            if (element.Kind == MetaTypeKind.NamedType) {
                var name = element.AsScopedName;
                var entry = resolver.ResolveByName(default, element.AsScopedName.ToString(), 0, ResolverFlags.None);
                int typeId;

                if (entry != default && entry.Kind == ReferenceKind.RefToType) {
                    typeId = entry.Symbol.TypeId;
                    element.TypeInfo = environment.TypeRegistry.MakeTypeReference(typeId);
                    return;
                }

                if (entry != default) {
                    typeId = entry.Symbol.TypeId;
                    element.IsConstant = entry.Kind == ReferenceKind.RefToConstant;
                }
                else {
                    typeId = KnownTypeIds.ErrorType;
                }

                element.TypeInfo = environment.TypeRegistry.MakeTypeInstanceReference(typeId);
            }

            else if (element.Kind == MetaTypeKind.StringType) {
                element.TypeInfo = environment.Runtime.Types.MakeTypeReference(KnownTypeIds.StringType);
            }

            else if (element.Kind == MetaTypeKind.AnsiString) {
                element.TypeInfo = environment.Runtime.Types.MakeTypeReference(KnownTypeIds.AnsiStringType);
            }

            else if (element.Kind == MetaTypeKind.ShortString) {
                var length = TypeRegistry.Runtime.Integers.Invalid;

                if (element.Value != default && element.Value.TypeInfo != default && element.Value.TypeInfo.IsConstant())
                    length = element.Value.TypeInfo;

                var userType = TypeCreator.CreateShortStringType(length);
                element.TypeInfo = environment.Runtime.Types.MakeTypeReference(userType.TypeId);
            }

            else if (element.Kind == MetaTypeKind.ShortStringDefault) {
                element.TypeInfo = environment.Runtime.Types.MakeTypeReference(KnownTypeIds.ShortStringType);
            }


            else if (element.Kind == MetaTypeKind.UnicodeString) {
                element.TypeInfo = environment.Runtime.Types.MakeTypeReference(KnownTypeIds.UnicodeStringType);
            }

            else if (element.Kind == MetaTypeKind.WideString) {
                element.TypeInfo = environment.Runtime.Types.MakeTypeReference(KnownTypeIds.WideStringType);
            }

            else if (element.Kind == MetaTypeKind.PointerType) {
                element.TypeInfo = environment.Runtime.Types.MakeTypeReference(KnownTypeIds.GenericPointer);
            }

        }

        private Signature CreateSignatureFromSymbolPart(SymbolReferencePart part) {
            using (var list = environment.ListPools.GetList<ITypeReference>()) {

                for (var i = 0; i < part.Expressions.Count; i++)
                    if (part.Expressions[i] != null && part.Expressions[i].TypeInfo != null)
                        list.Add(part.Expressions[i].TypeInfo);
                    else
                        list.Add(GetErrorTypeReference(part.Expressions[i]));


                return new Signature(GetInstanceTypeById(KnownTypeIds.UnspecifiedType), environment.ListPools.GetFixedArray(list));
            }
        }

        /// <summary>
        ///     end visiting a symbol reference
        /// </summary>
        /// <param name="element">symbol reference</param>
        public void EndVisit(SymbolReference element) {
            var baseTypeValue = GetInstanceTypeById(KnownTypeIds.UnspecifiedType);

            if (element.TypeValue is ITypedSyntaxNode typeRef)
                baseTypeValue = typeRef.TypeInfo;

            if (element.Inherited) {

                foreach (var impl in currentMethodImplementation) {
                    if (impl.DefiningType == KnownTypeIds.ErrorType)
                        continue;

                    var classMethod = impl.IsClassItem;
                    var signature = impl.CreateSignature(TypeRegistry);
                    var callableRoutines = new List<IParameterGroup>();

                    var bdef = GetTypeByIdOrUndefinedType(impl.DefiningType) as StructuredTypeDeclaration;
                    var idef = GetTypeByIdOrUndefinedType(bdef.BaseClassId) as StructuredTypeDeclaration;

                    if (idef == default)
                        continue;

                    idef.ResolveCall(impl.Name, callableRoutines, signature);

                    if (callableRoutines.Count == 1)
                        baseTypeValue = callableRoutines[0].ResultType;

                    break;
                }

            }

            foreach (var part in element.SymbolParts) {

                if (part is MetaType metaType) {

                    if (metaType.Kind == MetaTypeKind.AnsiString)
                        baseTypeValue = TypeRegistry.MakeTypeReference(KnownTypeIds.AnsiStringType);

                    if (metaType.Kind == MetaTypeKind.ShortString)
                        baseTypeValue = TypeRegistry.MakeTypeReference(KnownTypeIds.ShortStringType);

                    if (metaType.Kind == MetaTypeKind.UnicodeString)
                        baseTypeValue = TypeRegistry.MakeTypeReference(KnownTypeIds.UnicodeStringType);

                    if (metaType.Kind == MetaTypeKind.WideString)
                        baseTypeValue = TypeRegistry.MakeTypeReference(KnownTypeIds.WideStringType);

                    if (metaType.Kind == MetaTypeKind.StringType)
                        baseTypeValue = TypeRegistry.MakeTypeReference(KnownTypeIds.StringType);

                }

                else if (part is SymbolReferencePart symRef) {

                    if (baseTypeValue.TypeId == KnownTypeIds.ErrorType)
                        break;

                    if (symRef.Kind == SymbolReferencePartKind.SubItem) {
                        var flags = ResolverFlags.None;
                        var classType = TypeRegistry.GetTypeByIdOrUndefinedType(baseTypeValue.TypeId) as StructuredTypeDeclaration;
                        var self = resolver.ResolveReferenceByName(default, "Self");
                        var selfType = TypeRegistry.GetTypeByIdOrUndefinedType(self?.Symbol?.TypeId ?? KnownTypeIds.ErrorType) as StructuredTypeDeclaration;

                        if (classType != default && (selfType == default || selfType.TypeId != classType.TypeId))
                            flags |= ResolverFlags.SkipPrivate;

                        if (classType != default && (selfType == default || selfType != default && !selfType.InheritsFrom(classType.TypeId)))
                            flags |= ResolverFlags.SkipProtected;

                        if (self != default && self.Kind == ReferenceKind.RefToSelfClass)
                            flags |= ResolverFlags.RequireClassSymbols;

                        baseTypeValue = resolver.ResolveTypeByName(baseTypeValue, symRef.Name, 0, flags);
                    }
                    else if (symRef.Kind == SymbolReferencePartKind.StringType) {
                        baseTypeValue = symRef.Value.TypeInfo;
                    }
                    else if ((symRef.Kind == SymbolReferencePartKind.CallParameters || symRef.Kind == SymbolReferencePartKind.StringCast) && symRef.Name != null) {
                        var callableRoutines = new List<IParameterGroup>();
                        var signature = CreateSignatureFromSymbolPart(symRef);

                        if (baseTypeValue.TypeId == KnownTypeIds.UnspecifiedType) {
                            var reference = resolver.ResolveByName(baseTypeValue, symRef.Name, 0, ResolverFlags.None);

                            if (reference == null) {
                                baseTypeValue = GetErrorTypeReference(element);
                            }
                            else if (reference.Kind == ReferenceKind.RefToGlobalRoutine) {
                                if (reference.Symbol is IRoutine routine) {
                                    routine.ResolveCall(callableRoutines, signature);
                                }
                            }
                            else if (reference.Kind == ReferenceKind.RefToType && signature.Length == 1) {
                                if (signature[0].IsConstant()) {
                                    baseTypeValue = environment.Runtime.Cast(TypeRegistry, signature[0], ((ITypeDefinition)reference.Symbol).TypeId);
                                }
                                else {
                                    var resultType = environment.TypeRegistry.Cast(signature[0].TypeId, ((ITypeDefinition)reference.Symbol).TypeId);
                                    baseTypeValue = GetInstanceTypeById(resultType);
                                }
                            }

                        }

                        else if (baseTypeValue.TypeKind == CommonTypeKind.ClassType && environment.TypeRegistry.GetTypeByIdOrUndefinedType(baseTypeValue.TypeId) is StructuredTypeDeclaration structType) {
                            structType.ResolveCall(symRef.Name, callableRoutines, signature);
                        }

                        if (callableRoutines.Count == 1) {
                            if (callableRoutines[0].ResultType.IsConstant()) {
                                baseTypeValue = callableRoutines[0].ResultType;
                            }
                            else if (callableRoutines[0].Routine is IntrinsicRoutine _) {
                                baseTypeValue = Runtime.Types.MakeInvocationResultFromIntrinsic(callableRoutines[0].Routine, callableRoutines[0]);
                            }
                            else {
                                var targetRoutine = callableRoutines[0].Routine;
                                var routineIndex = targetRoutine.GetIndexOfParameterGroup(callableRoutines[0]);
                                baseTypeValue = Runtime.Types.MakeInvocationResult(targetRoutine, routineIndex);
                            }
                        }
                    }

                }
            }

            element.TypeInfo = baseTypeValue;
        }

        /// <summary>
        ///     start visiting an enumeration type
        /// </summary>
        /// <param name="element">enumeration type definition</param>
        public void StartVisit(EnumTypeCollection element) {
            var typeDef = TypeCreator.CreateEnumType();
            var type = GetInstanceTypeById(typeDef.TypeId);
            currentTypeDefinition.Push(type);
        }

        /// <summary>
        ///     create a record constant
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(RecordConstant element) {
            var typeDef = TypeCreator.CreateStructuredType(StructuredTypeKind.Record);
            var type = GetInstanceTypeById(typeDef.TypeId);
            currentTypeDefinition.Push(type);
        }

        /// <summary>
        ///     create a record constant
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(RecordConstant element) {
            var typeReference = currentTypeDefinition.Pop();
            var typeDef = TypeRegistry.GetTypeByIdOrUndefinedType(typeReference.TypeId);
            var type = GetTypeByIdOrUndefinedType(typeDef.TypeId) as StructuredTypeDeclaration;

            if (type.IsConstant)
                element.TypeInfo = type.MakeConstant();
            else
                element.TypeInfo = GetErrorTypeReference(element);
        }

        /// <summary>
        ///     add a field in a record constants
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(RecordConstantItem element) {
            var typeReference = currentTypeDefinition.Peek();
            var type = GetTypeByIdOrUndefinedType(typeReference.TypeId) as StructuredTypeDeclaration;
            type.Fields.Add(new Variable() { Name = element.Name.CompleteName, SymbolType = element.Value.TypeInfo });
        }

        /// <summary>
        ///     end visiting an enumerated type definition
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(EnumTypeCollection element) {
            var typeReference = currentTypeDefinition.Pop();
            var typeDef = TypeRegistry.GetTypeByIdOrUndefinedType(typeReference.TypeId);

            if (typeDef is EnumeratedType enumType) {
                var typeID = enumType.CommonTypeId;
                foreach (var enumValue in enumType.Values) {
                    enumValue.MakeEnumValue(environment.Runtime, TypeRegistry, typeID, enumType.TypeId);
                }
            }

            element.TypeInfo = typeReference;
        }

        /// <summary>
        ///     enumerated type value definition
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(EnumTypeValue element) {
            var value = currentTypeDefinition.Peek();
            var typeDef = value != null ? environment.TypeRegistry.GetTypeByIdOrUndefinedType(value.TypeId) as EnumeratedType : null;
            if (typeDef == null) {
                element.TypeInfo = GetErrorTypeReference(element);
                return;
            }

            var itemValue = default(ITypeReference);
            var enumRef = typeDef.DefineEnumValue(environment.Runtime, element.SymbolName, false, itemValue);

            if (enumRef == null) {
                element.TypeInfo = GetErrorTypeReference(element);
                return;
            }

            element.TypeInfo = TypeRegistry.Runtime.MakeEnumValue(typeDef.TypeId, enumRef.Value);
            resolver.AddToScope(element.SymbolName, ReferenceKind.RefToEnumMember, enumRef);
        }

        /// <summary>
        ///     visit a subrange type
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(Parsing.SyntaxTree.Abstract.SubrangeType element) {

            var leftTypeRef = element.RangeStart?.TypeInfo;
            var rightTypeRef = element.RangeEnd?.TypeInfo;
            var left = GetTypeByIdOrUndefinedType(leftTypeRef?.TypeId ?? KnownTypeIds.ErrorType) as IOrdinalType;
            var right = GetTypeByIdOrUndefinedType(rightTypeRef?.TypeId ?? KnownTypeIds.ErrorType) as IOrdinalType;

            if (element.RangeStart == null && element.RangeEnd == null) {
                element.TypeInfo = GetErrorTypeReference(element);
                return;
            }

            var typeDef = default(ITypeReference);

            if (left == default)
                typeDef = GetErrorTypeReference(element);
            else if (element.RangeEnd == null)
                typeDef = GetInstanceTypeById(TypeCreator.CreateSubrangeType(left.TypeId, left.LowestElement, leftTypeRef).TypeId);
            else
                typeDef = GetInstanceTypeById(TypeRegistry.GetTypeForSubrangeType(leftTypeRef, rightTypeRef));

            element.TypeInfo = typeDef;
        }

        /// <summary>
        ///     declare a type
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(TypeDeclaration element) {
            if (element.Generics != default && element.Generics.Count > 0) {
                var placeholder = TypeCreator.CreateGenericPlaceholder();
                var typeRef = GetInstanceTypeById(placeholder.TypeId);
                currentTypeDefinition.Push(typeRef);
            }
        }

        /// <summary>
        ///     declare a type
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(TypeDeclaration element) {

            if (element.Generics != default && element.Generics.Count > 0) {
                var placeholderRef = currentTypeDefinition.Pop();
                var placeholder = GetTypeByIdOrUndefinedType(placeholderRef.TypeId) as IExtensibleGenericType;

                if (element.TypeValue is ITypedSyntaxNode type && type.TypeInfo != null) {
                    var typedef = GetTypeByIdOrUndefinedType(type.TypeInfo.TypeId) as IExtensibleGenericType;

                    if (typedef != default)
                        foreach (var param in placeholder.GenericParameters)
                            typedef.AddGenericParameter(param);
                }
            }

            if (element.TypeValue is ITypedSyntaxNode declaredType && declaredType.TypeInfo != null) {
                element.TypeInfo = element.TypeValue.TypeInfo;
                if (element.Name.CompleteName != default)
                    resolver.AddToScope(element.Name.CompleteName, ReferenceKind.RefToType, TypeRegistry.GetTypeByIdOrUndefinedType(element.TypeInfo.TypeId), element.Generics?.Count ?? 0);
            }
        }

        /// <summary>
        ///     declare a set type
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(SetTypeDeclaration element) {

            if (element.TypeValue is ITypedSyntaxNode declaredEnum && declaredEnum.TypeInfo != null && declaredEnum.TypeInfo.TypeKind.IsOrdinal()) {
                var setType = TypeCreator.CreateSetType(declaredEnum.TypeInfo.TypeId);
                element.TypeInfo = GetInstanceTypeById(setType.TypeId);
                return;
            }

            element.TypeInfo = GetErrorTypeReference(element);
        }

        /// <summary>
        ///     visit an array declaration
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ArrayTypeDeclaration element) {
            var baseTypeId = KnownTypeIds.ErrorType;

            if (element.TypeValue != null && element.TypeValue.TypeInfo != null) {
                baseTypeId = element.TypeValue.TypeInfo.TypeId;
            }

            using (var list = environment.ListPools.GetList<int>()) {
                foreach (var indexDef in element.IndexItems) {
                    var typeInfo = indexDef.TypeInfo;

                    if (typeInfo != null) {
                        if (!typeInfo.IsType())
                            list.Item.Add(KnownTypeIds.ErrorType);
                        else
                            list.Item.Add(typeInfo.TypeId);
                    }
                    else
                        list.Item.Add(KnownTypeIds.ErrorType);
                }

                var typeDef = default(ITypeDefinition);

                if (list.Item.Count < 1) {

                    typeDef = TypeCreator.CreateDynamicArrayType(baseTypeId, element.PackedType);

                }
                else {

                    for (var i = list.Item.Count - 1; i >= 0; i--) {

                        typeDef = TypeCreator.CreateStaticArrayType(baseTypeId, list.Item[i], element.PackedType);
                        baseTypeId = typeDef.TypeId;

                    }
                }

                element.TypeInfo = GetInstanceTypeById(typeDef.TypeId);
            }
        }

        /// <summary>
        ///     start visiting a structured type
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(StructuredType element) {
            var typeDef = TypeCreator.CreateStructuredType(element.Kind);
            typeDef.BaseClassId = KnownTypeIds.TObject;
            currentTypeDefinition.Push(GetInstanceTypeById(typeDef.TypeId));
        }

        /// <summary>
        ///     end visiting a structured type
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(StructuredType element) {
            var value = currentTypeDefinition.Pop();
            var typeDef = value != null ? environment.TypeRegistry.GetTypeByIdOrUndefinedType(value.TypeId) as StructuredTypeDeclaration : null;

            foreach (var baseType in element.BaseTypes) {
                typeDef.BaseClassId = baseType.TypeInfo.TypeId;
            }

            element.TypeInfo = GetTypeReferenceById(typeDef.TypeId);
        }

        /// <summary>
        ///     start visiting a method declaration
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(MethodDeclaration element) {
            if (element.Name == null)
                return;

            var method = default(IRoutine);
            var f = new RoutineFlags();

            if (element is GlobalMethod gm) {
                if (CurrentUnit.InterfaceSymbols != default) {
                    var unitType = GetTypeByIdOrUndefinedType(CurrentUnit.TypeInfo.TypeId) as UnitType;
                    method = new Routine(TypeRegistry, element.SymbolName, element.Kind);
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
                method = typeDef.AddOrExtendMethod(element.Name.CompleteName, element.Kind, genericTypeId);
            }

            currentTypeDefinition.Push((Routine)method);
            var parameters = ((Routine)method).AddParameterGroup();
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

            var method = currentTypeDefinition.Pop() as Routine;

            if (element.Kind == ProcedureKind.Function) {

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
        ///     visit a parameter type definition
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ParameterTypeDefinition element) {
            if (element.TypeValue != null && element.TypeValue.TypeInfo != null) {

                if (currentMethodParameters.Count < 1)
                    return;

                var parms = currentMethodParameters.Peek();

                foreach (var name in element.Parameters) {
                    var param = parms.AddParameter(name.Name.CompleteName);
                    param.SymbolType = element.TypeValue.TypeInfo;
                }
            }
        }

        /// <summary>
        ///     visit a structure field definition
        /// </summary>
        /// <param name="element">field definition</param>
        public void EndVisit(StructureFields element) {
            var typeInfo = default(ITypeReference);

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
        ///     visit a statement
        /// </summary>
        /// <param name="element">statement to check</param>
        public void EndVisit(StructuredStatement element) {
            if (element.Kind == StructuredStatementKind.Assignment)
                CheckAssigment(element);
        }

        private void CheckAssigment(StructuredStatement element) {
            var left = element.Expressions.Count > 0 ? element.Expressions[0]?.TypeInfo : null;
            var right = element.Expressions.Count > 1 ? element.Expressions[1]?.TypeInfo : null;
            if (left != null && right != null) {

                environment.TypeRegistry.GetTypeByIdOrUndefinedType(left.TypeId).CanBeAssignedFrom(environment.TypeRegistry.GetTypeByIdOrUndefinedType(right.TypeId));
            }
        }

        /// <summary>
        ///     visit a constant declaration
        /// </summary>
        /// <param name="element">item to visit</param>
        public void EndVisit(ConstantDeclaration element) {
            var declaredType = default(ITypeReference);
            var inferredType = default(ITypeReference);

            if (element.TypeValue is ITypedSyntaxNode typeRef && typeRef.TypeInfo != null && typeRef.TypeInfo.TypeId != KnownTypeIds.ErrorType)
                declaredType = typeRef.TypeInfo;

            if (element.Value is ITypedSyntaxNode autType && autType.TypeInfo != null && autType.TypeInfo.TypeId != KnownTypeIds.ErrorType)
                inferredType = autType.TypeInfo;

            if (inferredType == default)
                element.TypeInfo = GetErrorTypeReference(element);
            else if (declaredType == default)
                element.TypeInfo = inferredType;
            else
                element.TypeInfo = TypeRegistry.Runtime.Cast(TypeRegistry, inferredType, declaredType.TypeId);

            resolver.AddToScope(element.SymbolName, ReferenceKind.RefToConstant, element);
        }

        /// <summary>
        ///     start vising a child of a constant expression
        /// </summary>
        /// <param name="element"></param>
        /// <param name="child"></param>
        public void StartVisitChild(ConstantDeclaration element, ISyntaxPart child) {

            if (child is IRequiresArrayExpression set) {
                var requiresArrayType = TypeRegistry.GetTypeByIdOrUndefinedType(element.TypeValue?.TypeInfo?.TypeId ?? KnownTypeIds.ErrorType).TypeKind.IsArray();
                set.RequiresArray = requiresArrayType;
            }
        }

        /// <summary>
        ///     end visiting a constant
        /// </summary>
        /// <param name="element"></param>
        /// <param name="child"></param>
        public void EndVisitChild(ConstantDeclaration element, ISyntaxPart child) { }

        /// <summary>
        ///     visit a set expression
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(SetExpression element) {

            if (element.Expressions.Count < 1) {
                element.TypeInfo = TypeRegistry.Runtime.Structured.EmptySet;
                return;
            }

            using (var values = environment.ListPools.GetList<ITypeReference>()) {

                var baseType = GetBaseTypeForArrayConstant(element.Expressions, out var isConstant, values.Item, element, element.RequiresArray);

                if (baseType == default) {
                    element.TypeInfo = TypeRegistry.Runtime.Structured.EmptySet;
                    return;
                }

                if (baseType.TypeId == KnownTypeIds.ErrorType) {
                    element.TypeInfo = GetErrorTypeReference(element);
                    return;
                }

                var typdef = default(ITypeDefinition);

                if (!element.RequiresArray)
                    typdef = TypeCreator.CreateSetType(baseType.TypeId);
                else if (isConstant) {
                    var indexType = TypeCreator.CreateSubrangeType(KnownTypeIds.IntegerType, TypeRegistry.Runtime.Integers.Zero, TypeRegistry.Runtime.Integers.ToScaledIntegerValue(values.Item.Count - 1));
                    typdef = TypeCreator.CreateStaticArrayType(baseType.TypeId, indexType.TypeId, false);
                }
                else
                    typdef = TypeCreator.CreateDynamicArrayType(baseType.TypeId, false);

                if (!element.RequiresArray) {
                    if (isConstant)
                        element.TypeInfo = TypeRegistry.Runtime.Structured.CreateSetValue(typdef.TypeId, environment.ListPools.GetFixedArray(values));
                    else
                        element.TypeInfo = GetInstanceTypeById(typdef.TypeId);
                }
                else {
                    if (isConstant)
                        element.TypeInfo = TypeRegistry.Runtime.Structured.CreateArrayValue(typdef.TypeId, baseType.TypeId, environment.ListPools.GetFixedArray(values));
                    else
                        element.TypeInfo = GetInstanceTypeById(typdef.TypeId);
                }
            }
        }

        private bool ExpandRangeOperator(ITypedSyntaxNode part, bool requiresArray, List<ITypeReference> values, out int baseTypeId) {
            if (!(GetTypeByIdOrUndefinedType(part.TypeInfo.TypeId) is ISubrangeType subrangeType) || requiresArray) {
                baseTypeId = KnownTypeIds.ErrorType;
                return false;
            }

            baseTypeId = subrangeType.BaseTypeId;
            var baseType = GetInstanceTypeById(baseTypeId);
            var lowerBound = subrangeType.LowestElement;
            var upperBound = subrangeType.HighestElement;

            if (!subrangeType.IsValid) {
                baseTypeId = KnownTypeIds.ErrorType;
                return false;
            }

            var cardinality = subrangeType.Cardinality;

            if (cardinality < 1)
                return true;

            if (cardinality > 255) {
                baseTypeId = KnownTypeIds.ErrorType;
                return false;
            }

            while (!lowerBound.Equals(upperBound)) {
                values.Add(lowerBound);
                lowerBound = PredOrSucc.StaticExecuteCall(TypeRegistry, lowerBound, false);
            }

            if (lowerBound.Equals(upperBound))
                values.Add(lowerBound);

            return true;
        }

        private ITypeReference GetBaseTypeForArrayConstant(IEnumerable<IExpression> items, out bool isConstant, List<ITypeReference> values, ITypedSyntaxNode element, bool requiresArray) {
            var baseType = default(ITypeReference);
            isConstant = true;

            foreach (var part in items) {

                if (part is BinaryOperator binaryOperator && binaryOperator.Kind == ExpressionKind.RangeOperator) {
                    if (requiresArray ||
                        !ExpandRangeOperator(part, requiresArray, values, out var setBaseType) ||
                        baseType != default && baseType.TypeId != setBaseType)
                        return GetErrorTypeReference(part);
                    baseType = TypeRegistry.MakeTypeInstanceReference(setBaseType);
                    continue;
                }

                if (part.TypeInfo == null || !part.TypeInfo.IsConstant()) {
                    values.Clear();
                    return GetErrorTypeReference(part);
                }

                baseType = TypeRegistry.GetBaseTypeForArrayOrSet(baseType, part.TypeInfo);

                if (baseType.TypeId == KnownTypeIds.ErrorType) {
                    break;
                }

                if (!requiresArray && !baseType.IsOrdinal()) {
                    values.Clear();
                    return GetErrorTypeReference(part);
                }

                isConstant = isConstant && part.TypeInfo.IsConstant();
                values.Add(part.TypeInfo);
            }

            if (baseType == null)
                baseType = GetErrorTypeReference(element);

            return baseType;
        }

        /// <summary>
        ///     visit an array constant
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ArrayConstant element) {
            using (var constantValues = environment.ListPools.GetList<ITypeReference>()) {

                var baseType = GetBaseTypeForArrayConstant(element.Items, out var isConstant, constantValues.Item, element, true);
                var ints = TypeRegistry.Runtime.Integers;
                var indexTypeDef = TypeCreator.CreateSubrangeType(KnownTypeIds.IntegerType, ints.Zero, ints.ToScaledIntegerValue(constantValues.Item.Count));
                var arrayType = TypeCreator.CreateStaticArrayType(baseType.TypeId, indexTypeDef.TypeId, false);

                if (isConstant) {
                    element.TypeInfo = environment.Runtime.Structured.CreateArrayValue(arrayType.TypeId, baseType.TypeId, TypeRegistry.ListPools.GetFixedArray(constantValues));
                }
                else {
                    element.TypeInfo = GetInstanceTypeById(arrayType.TypeId);
                }
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
            var routine = default(IRoutine);

            if (!isClassMethod) {
                var unitType = GetTypeByIdOrUndefinedType(CurrentUnit.TypeInfo.TypeId) as UnitType;

                if (unitType.HasGlobalRoutine(element.SymbolName)) {
                    if (!unitType.AddGlobalImplementation(element.SymbolName, out routine)) {
                        routine = new Routine(TypeRegistry, element.SymbolName, element.Kind);
                    }
                }
                else {
                    routine = new Routine(TypeRegistry, element.SymbolName, element.Kind);
                    unitType.AddGlobal(routine);
                    resolver.AddToScope(element.SymbolName, ReferenceKind.RefToGlobalRoutine, routine);
                }
                currentMethodParameters.Push((routine as Routine).AddParameterGroup());
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
                using (var list = environment.ListPools.GetList<ITypeReference>()) {
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
