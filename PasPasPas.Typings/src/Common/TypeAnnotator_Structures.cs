using System;
using System.Collections.Generic;
using System.Linq;
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Typings.Common {

    public partial class TypeAnnotator {

        /// <summary>
        ///     registered routines
        /// </summary>
        public List<(IRoutine, BlockOfStatements)> Routines
            => routines;

        /// <summary>
        ///     begin visit a unit
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(CompilationUnit element) {
            var unitType = TypeRegistry.CreateUnitType(element.SymbolName);
            CurrentUnit = element;
            CurrentUnit.TypeInfo = unitType.Reference;
            TypeCreator = TypeRegistry.CreateTypeFactory(unitType);
            resolver.OpenScope();
            resolver.AddToScope(KnownNames.System, TypeRegistry.SystemUnit.Reference);
        }

        /// <summary>
        ///     end visiting a unit
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(CompilationUnit element) {
            resolver.CloseScope();
            TypeCreator = default;
            CurrentUnit = default;
        }

        /// <summary>
        ///     begin visit a unit
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(BlockOfStatements element) {
            resolver.OpenScope();
            if (currentMethodImplementation.Count < 1) {
                var mainRoutineGroup = TypeCreator.CreateGlobalRoutineGroup(KnownNames.MainMethod);
                var mainRoutine = TypeCreator.CreateRoutine(mainRoutineGroup, RoutineKind.Procedure);
                mainRoutine.ResultType = NoType.Reference;
                currentMethodImplementation.Push(mainRoutine);
                RegisterRoutine(mainRoutine, element);
            }
        }

        private void RegisterRoutine(IRoutine routine, BlockOfStatements block) {
            var entry = (routine, block);
            routines.Add(entry);
        }

        /// <summary>
        ///     end visiting a unit
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(BlockOfStatements element) {
            resolver.CloseScope();

            var cmi = currentMethodImplementation.Count > 0 ? currentMethodImplementation.Peek() : default;

            if (cmi != default && string.Equals(cmi.RoutineGroup.Name, KnownNames.MainMethod, System.StringComparison.OrdinalIgnoreCase)) {
                var mi = currentMethodImplementation.Pop();
            }
        }


        /// <summary>
        ///     visit a variable declaration
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(VariableDeclaration element) {
            if (element.TypeValue is ITypedSyntaxPart typeRef && typeRef.TypeInfo != null)
                element.TypeInfo = typeRef.TypeInfo;
            else
                MarkWithErrorType(element);

            var t = element.TypeInfo.TypeDefinition;
            var fromResult = true;

            foreach (var vardef in element.Names) {

                var varname = vardef.Name.CompleteName ?? string.Empty;
                var variable = new Variable();
                var unitType = CurrentUnitType;
                var cmi = currentMethodImplementation.Count < 1 ? default : currentMethodImplementation.Peek();
                variable.Name = varname;

                if (cmi == default) {
                    unitType.Register(variable);
                    variable.Kind = VariableKind.GlobalVariable;
                }
                else {
                    cmi.Symbols.Add(varname, variable);
                    variable.Kind = VariableKind.LocalVariable;
                }

                resolver.AddToScope(varname, variable);

                if (vardef is FunctionResult fn) {
                    if (fn.Method?.TypeValue?.TypeInfo != default)
                        vardef.TypeInfo = fn.Method.TypeValue.TypeInfo;
                    else
                        MarkWithErrorType(vardef);

                    variable.TypeDefinition = vardef.TypeInfo.TypeDefinition;
                    continue;
                }

                fromResult = false;
                vardef.TypeInfo = element.TypeInfo;
                variable.TypeDefinition = vardef.TypeInfo.TypeDefinition;
            }

            if (fromResult && element.Names.Count > 0 && element.Names[0].TypeInfo != default) {
                element.TypeInfo = element.Names[0].TypeInfo;
            }
        }

        /// <summary>
        ///     create a record constant
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(RecordConstant element) {
            var typeReference = PopTypeFromStack<IStructuredType>();

            if (typeReference.IsConstant())
                element.TypeInfo = typeReference.MakeConstant();
            else
                MarkWithErrorType(element);
        }

        /// <summary>
        ///     add a field in a record constants
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(RecordConstantItem element) {
            var typeReference = PeekTypeFromStack<IStructuredType>();
            typeReference.Fields.Add(new Variable() {
                Name = element.Name.CompleteName,
                InitialValue = element.Value?.TypeInfo as IValue,
                TypeDefinition = element.Value?.TypeInfo?.TypeDefinition
            });
        }

        /// <summary>
        ///     visit a meta type reference
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(MetaType element) {
            if (element.Kind == MetaTypeKind.NamedType) {
                var name = element.AsScopedName;
                var entry = resolver.ResolveByName(default, element.AsScopedName.ToString(), 0, ResolverFlags.None);
                ITypeDefinition typeId;

                if (entry != default && entry.SymbolKind == SymbolTypeKind.TypeDefinition) {
                    element.TypeInfo = entry.TypeDefinition.Reference;
                    return;
                }

                if (entry != default) {
                    typeId = entry.TypeDefinition;
                    element.IsConstant = entry.IsConstant();
                }
                else {
                    typeId = TypeRegistry.SystemUnit.ErrorType;
                }

                element.TypeInfo = typeId.Reference;
            }

            else if (element.Kind == MetaTypeKind.StringType) {
                element.TypeInfo = TypeRegistry.SystemUnit.StringType.Reference;
            }

            else if (element.Kind == MetaTypeKind.AnsiString) {
                element.TypeInfo = TypeRegistry.SystemUnit.AnsiStringType.Reference;
            }

            else if (element.Kind == MetaTypeKind.ShortString) {
                ITypeSymbol length = TypeRegistry.Runtime.Integers.Invalid;

                if (element.Value != default && element.Value.TypeInfo != default && element.Value.TypeInfo.IsConstant())
                    length = element.Value.TypeInfo;

                if (!(length is IIntegerValue intValue)) {
                    MarkWithErrorType(element);
                    return;
                }

                if (intValue.IsNegative) {
                    MarkWithErrorType(element);
                    return;
                }

                if (intValue.UnsignedValue > byte.MaxValue) {
                    MarkWithErrorType(element);
                    return;
                }

                var stringLength = (byte)intValue.UnsignedValue;
                var userType = TypeCreator.CreateShortStringType(stringLength);
                element.TypeInfo = userType.Reference;
            }

            else if (element.Kind == MetaTypeKind.ShortStringDefault) {
                element.TypeInfo = TypeRegistry.SystemUnit.ShortStringType.Reference;
            }


            else if (element.Kind == MetaTypeKind.UnicodeString) {
                element.TypeInfo = TypeRegistry.SystemUnit.UnicodeStringType.Reference;
            }

            else if (element.Kind == MetaTypeKind.WideString) {
                element.TypeInfo = TypeRegistry.SystemUnit.WideStringType.Reference;
            }

            else if (element.Kind == MetaTypeKind.PointerType) {
                element.TypeInfo = TypeRegistry.SystemUnit.GenericPointerType.Reference;
            }

        }

        /// <summary>
        ///     visit a constant declaration
        /// </summary>
        /// <param name="element">item to visit</param>
        public void EndVisit(ConstantDeclaration element) {
            var declaredType = GetTypeOfNode(element.TypeValue);
            var inferredType = GetTypeOfNode(element.Value);

            if (inferredType.TypeDefinition is IErrorType)
                MarkWithErrorType(element);
            else if (declaredType.TypeDefinition is IErrorType)
                element.TypeInfo = inferredType;
            else
                element.TypeInfo = TypeRegistry.Runtime.Cast(TypeRegistry, inferredType as IValue, declaredType.TypeDefinition);

            resolver.AddToScope(element.SymbolName, element.TypeInfo);
        }

        /// <summary>
        ///     start vising a child of a constant expression
        /// </summary>
        /// <param name="element"></param>
        /// <param name="child"></param>
        public void StartVisitChild(ConstantDeclaration element, ISyntaxPart child) {

            if (child is IRequiresArrayExpression set) {
                var requiresArrayType = (element.TypeValue?.TypeInfo?.TypeDefinition ?? TypeRegistry.SystemUnit.ErrorType).BaseType == BaseType.Array;
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
        ///     visit a variable declaration
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(Parsing.SyntaxTree.Abstract.TypeAlias element) {

            if (element.Fragments == default || element.Fragments.Count < 1) {
                MarkWithErrorType(element);
                return;
            }

            var entry = default(ITypeSymbol);
            for (var i = 0; i < element.Fragments.Count && (i == 0 || entry != default); i++) {
                var fragment = element.Fragments[i];

                if (fragment.TypeValues.Count < 1) {
                    entry = resolver.ResolveReferenceByName(entry, fragment.Name);
                    continue;
                }

                using (var list = environment.ListPools.GetList<ITypeDefinition>()) {
                    foreach (var typeValue in fragment.TypeValues) {
                        if (typeValue.TypeInfo == default || typeValue.TypeInfo.GetBaseType() == BaseType.Error) {
                            MarkWithErrorType(element);
                            return;
                        }

                        list.Item.Add(typeValue.TypeInfo.TypeDefinition);
                    }

                    entry = resolver.ResolveReferenceByName(entry, fragment.Name, fragment.TypeValues.Count);

                    if (entry != default) {
                        var genericType = entry.TypeDefinition as IGenericType;
                        if (genericType != default)
                            entry = genericType.Bind(environment.ListPools.GetFixedArray(list), TypeCreator).Reference;
                    }
                    else
                        entry = default;
                }
            }

            var typeId = default(ITypeDefinition);

            if (entry != default && (entry.SymbolKind == SymbolTypeKind.TypeDefinition || entry.SymbolKind == SymbolTypeKind.BoundGeneric))
                typeId = entry.TypeDefinition;
            else
                typeId = TypeRegistry.SystemUnit.ErrorType;

            if (element.IsNewType) {
                typeId = TypeCreator.CreateTypeAlias(typeId, "", true);
            }

            element.TypeInfo = typeId.Reference;
        }

        /// <summary>
        ///     start visiting an enumeration type
        /// </summary>
        /// <param name="element">enumeration type definition</param>
        public void StartVisit(EnumTypeCollection element) {
            var typeDef = TypeCreator.CreateEnumType(string.Empty);
            PushTypeToStack(typeDef);
        }

        /// <summary>
        ///     create a record constant
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(RecordConstant element) {
            var typeDef = TypeCreator.CreateStructuredType(string.Empty, StructuredTypeKind.Record);
            PushTypeToStack(typeDef);
        }

        /// <summary>
        ///     end visiting an enumerated type definition
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(EnumTypeCollection element) {
            var enumType = PopTypeFromStack<IEnumeratedType>();

            var typeID = enumType.CommonTypeId;
            foreach (var enumValue in enumType.Values) {
                //Runtime.Types.MakeEnumValue(enumType, enumValue);
                //enumValue.MakeEnumValue(TypeRegistry, typeID, enumType);
            }

            element.TypeInfo = enumType.Reference;
        }

        /// <summary>
        ///     enumerated type value definition
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(EnumTypeValue element) {
            var typeDef = PeekTypeFromStack<IEnumeratedType>();
            var itemValue = element.TypeInfo as IValue;
            var enumRef = typeDef.DefineEnumValue(environment.Runtime, element.SymbolName, false, itemValue) as IEnumeratedValue;

            if (enumRef is null) {
                MarkWithErrorType(element);
                return;
            }

            element.TypeInfo = enumRef;
            resolver.AddToScope(element.SymbolName, enumRef);
        }

        /// <summary>
        ///     visit a subrange type
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(SubrangeType element) {
            var leftTypeRef = element.RangeStart?.TypeInfo;
            var rightTypeRef = element.RangeEnd?.TypeInfo;
            var left = (leftTypeRef?.TypeDefinition ?? SystemUnit.ErrorType) as IOrdinalType;
            var right = (rightTypeRef?.TypeDefinition ?? SystemUnit.ErrorType) as IOrdinalType;

            if (element.RangeStart == null && element.RangeEnd == null) {
                MarkWithErrorType(element);
                return;
            }

            var typeDef = default(ITypeSymbol);

            if (left == default)
                typeDef = ErrorReference;
            else if (element.RangeEnd == null)
                typeDef = TypeCreator.CreateSubrangeType(left, left.LowestElement, leftTypeRef as IValue).Reference;
            else
                typeDef = TypeRegistry.GetTypeForSubrangeType(TypeCreator, leftTypeRef, rightTypeRef).Reference;

            element.TypeInfo = typeDef;
        }


        /// <summary>
        ///     declare a type
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(TypeDeclaration element) {
            if (element.Generics != default && element.Generics.Count > 0) {
                var placeholder = TypeCreator.CreateGenericPlaceholder(string.Empty);
                PushTypeToStack(placeholder);
            }

            if (element.TypeValue is INamedTypeDeclaration ntd && !string.IsNullOrEmpty(element.Name.CompleteName))
                ntd.Name = element.Name.CompleteName;
        }

        /// <summary>
        ///     declare a type
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(TypeDeclaration element) {

            if (element.Generics != default && element.Generics.Count > 0) {
                var placeholder = PeekTypeFromStack<IExtensibleGenericType>();

                if (element.TypeValue is ITypedSyntaxPart type && type.TypeInfo != null) {
                    var typedef = type.TypeInfo.TypeDefinition as IExtensibleGenericType;

                    if (typedef != default)
                        foreach (var param in placeholder.GenericParameters)
                            typedef.AddGenericParameter(param);
                }
            }

            if (element.TypeValue is ITypedSyntaxPart declaredType && declaredType.TypeInfo != null) {
                element.TypeInfo = element.TypeValue.TypeInfo;
                if (element.Name.CompleteName != default)
                    resolver.AddToScope(element.Name.CompleteName, element.TypeInfo, element.Generics?.Count ?? 0);
            }
        }

        /// <summary>
        ///     declare a set type
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(SetTypeDeclaration element) {

            if (element.TypeValue is ITypedSyntaxPart declaredEnum && declaredEnum.TypeInfo != null && declaredEnum.TypeInfo.TypeDefinition is IOrdinalType ot) {
                var setType = TypeCreator.CreateSetType(ot, element.Name);
                element.TypeInfo = setType.Reference;
                return;
            }

            MarkWithErrorType(element);
        }

        /// <summary>
        ///     visit an array declaration
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ArrayTypeDeclaration element) {
            var baseTypeId = SystemUnit.ErrorType as ITypeDefinition;

            if (element.TypeValue != null && element.TypeValue.TypeInfo != null) {
                baseTypeId = element.TypeValue.TypeInfo.TypeDefinition;
            }

            using (var list = environment.ListPools.GetList<ITypeDefinition>()) {
                foreach (var indexDef in element.IndexItems) {
                    var typeInfo = indexDef.TypeInfo;

                    if (typeInfo != null) {
                        if (!(typeInfo.TypeDefinition.ResolveAlias() is IOrdinalType))
                            list.Item.Add(SystemUnit.ErrorType);
                        else
                            list.Item.Add(typeInfo.TypeDefinition);
                    }
                    else
                        list.Item.Add(SystemUnit.ErrorType);
                }

                var typeDef = default(ITypeDefinition);

                if (list.Item.Count < 1) {

                    typeDef = TypeCreator.CreateDynamicArrayType(baseTypeId, string.Empty, element.PackedType);

                }
                else {

                    for (var i = list.Item.Count - 1; i >= 0; i--) {

                        typeDef = TypeCreator.CreateStaticArrayType(baseTypeId, string.Empty, list.Item[i] as IOrdinalType, element.PackedType);
                        baseTypeId = typeDef;

                    }
                }

                element.TypeInfo = typeDef.Reference;
            }
        }

        /// <summary>
        ///     start visiting a structured type
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(StructuredType element) {
            var typeDef = TypeCreator.CreateStructuredType(element.Name, element.Kind);
            typeDef.BaseClass = SystemUnit.TObjectType;
            PushTypeToStack(typeDef);
        }

        /// <summary>
        ///     end visiting a structured type
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(StructuredType element) {
            var typeDef = PopTypeFromStack<IStructuredType>();

            foreach (var baseType in element.BaseTypes) {
                typeDef.BaseClass = baseType.TypeInfo.TypeDefinition;
            }

            element.TypeInfo = typeDef.Reference;
        }

        /// <summary>
        ///     visit a structure field definition
        /// </summary>
        /// <param name="element">field definition</param>
        public void EndVisit(StructureFields element) {
            var typeInfo = default(ITypeSymbol);

            if (element.TypeValue != null && element.TypeValue.TypeInfo != null)
                typeInfo = element.TypeValue.TypeInfo;
            else {
                typeInfo = ErrorReference;
            }

            var type = PeekTypeFromStack<IStructuredType>();

            foreach (var field in element.Fields) {
                var fieldDef = new Variable() {
                    Name = field.Name.CompleteName,

                    TypeDefinition = typeInfo.TypeDefinition,
                    Visibility = element.Visibility,
                    //ClassItem = element.ClassItem
                };

                if (element.ClassItem) {
                    type.AddField(fieldDef);
                }
                else
                    type.AddField(fieldDef);
            }
        }


        /// <summary>
        ///     visit an class of declaration
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ClassOfTypeDeclaration element) {
            if (element.TypeValue?.TypeInfo == default) {
                element.TypeInfo = ErrorReference;
                return;
            }

            var baseType = element.TypeValue.TypeInfo.TypeDefinition;

            if (!(baseType is IStructuredType)) {
                element.TypeInfo = ErrorReference;
                return;
            }

            var alias = TypeCreator.CreateMetaClassType(string.Empty, baseType);
            element.TypeInfo = alias.Reference;
        }

        /// <summary>
        ///     start visiting a method implementation
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(MethodImplementation element) {
            var isClassMethod = element.Declaration?.DefiningType != default;
            var definingType = isClassMethod ? element.Declaration.DefiningType.TypeInfo.TypeDefinition : SystemUnit.ErrorType;
            var isForward = element.Flags.HasFlag(MethodImplementationFlags.ForwardDeclaration);
            var routineGroup = default(IRoutineGroup);
            var routine = default(IRoutine);

            if (!isClassMethod) {
                var unitType = CurrentUnitType;
                routineGroup = unitType.Symbols.Where(t => t is IRoutineGroup rg && string.Equals(rg.Name, element.SymbolName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault() as IRoutineGroup;

                if (routineGroup == default) {
                    routineGroup = new RoutineGroup(CurrentUnitType, element.SymbolName);
                    unitType.Register(routineGroup);
                    resolver.AddToScope(element.SymbolName, routineGroup);
                }
                routine = new Routine(routineGroup, element.Kind);
                routineGroup.Items.Add(routine);
                routine.ResultType = NoType.Reference;
                currentMethodParameters.Push(routine);
            }
            else if (isClassMethod) {
                var baseType = definingType as IStructuredType;

                if (baseType != default)
                    routineGroup = baseType.FindMethod(element.Name.Name, element.Declaration.ClassItem);

                if (routineGroup != default && routineGroup.Items.Count > 0)
                    routine = routineGroup.Items[0];
            }

            if (routine != default) {
                resolver.OpenScope();
                currentMethodImplementation.Push(routine);
            }
        }

        /// <summary>
        ///     file type declaration
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(FileTypeDeclaration element) {

            if (element.TypeValue == default) {
                element.TypeInfo = SystemUnit.UnspecifiedFileType.Reference;
                return;
            }

            if (element.TypeValue.TypeInfo == default) {
                MarkWithErrorType(element);
                return;
            }

            var type = TypeCreator.CreateFileType(string.Empty, element.TypeValue.TypeInfo.TypeDefinition);
            element.TypeInfo = type.Reference;
        }

        /// <summary>
        ///     visit a generic constraint
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(GenericConstraint element) {
            if (element.Kind == GenericConstraintKind.Class) {
                element.TypeInfo = SystemUnit.GenericClassConstraint.Reference;
                return;
            }

            if (element.Kind == GenericConstraintKind.Record) {
                element.TypeInfo = SystemUnit.GenericRecordConstraint.Reference;
                return;
            }

            if (element.Kind == GenericConstraintKind.Constructor) {
                element.TypeInfo = SystemUnit.GenericConstructorConstraint.Reference;
                return;
            }

            if (element.Kind == GenericConstraintKind.Identifier) {
                var reference = resolver.ResolveByName(default, element.SymbolName, 0, ResolverFlags.None);

                if (reference.SymbolKind == SymbolTypeKind.TypeDefinition) {
                    element.TypeInfo = reference.TypeDefinition.Reference;
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
            var genericType = PeekTypeFromStack<IExtensibleGenericType>();

            if (genericType == default)
                return;

            using (var list = environment.ListPools.GetList<ITypeDefinition>()) {
                foreach (var constraint in element) {

                    if (constraint.TypeInfo != default)
                        list.Item.Add(constraint.TypeInfo.TypeDefinition);
                    else
                        hasError = true;
                }

                if (hasError)
                    MarkWithErrorType(element);
                else if (list.Item.Count < 1)
                    element.TypeInfo = SystemUnit.UnconstrainedGenericTypeParameter.Reference;
                else {
                    var typeDef = TypeCreator.CreateUnboundGenericTypeParameter(string.Empty, environment.ListPools.GetFixedArray(list));
                    element.TypeInfo = typeDef.Reference;
                }

                if (!hasError)
                    genericType.AddGenericParameter(element.TypeInfo.TypeDefinition);
            }
        }

        /// <summary>
        ///     visit a symbol declaration
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(DeclaredSymbolCollection element) {
            if (element == CurrentUnit?.InterfaceSymbols) {
                foreach (var symbol in CurrentUnit.InterfaceSymbols) {

                    var unitType = CurrentUnitType;
                    //var kind = ReferenceKind.Unknown;
                    /*var refSymbol = default(IRefSymbol);

                    switch (symbol) {
                        case ConstantDeclaration constDecl:
                            refSymbol = constDecl;
                            kind = ReferenceKind.RefToConstant;
                            break;
                    }

                    if (kind != ReferenceKind.Unknown)
                        unitType.RegisterSymbol(symbol.SymbolName, new Reference(kind, refSymbol), 0);
                */
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
                resolver.AddToScope(element.Name.CompleteName, unitType.Reference);
        }

    }
}
