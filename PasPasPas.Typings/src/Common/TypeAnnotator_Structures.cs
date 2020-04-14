using System.Collections.Generic;
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
            var unitType = TypeCreator.CreateUnitType(element.SymbolName);
            CurrentUnit = element;
            CurrentUnit.TypeInfo = unitType;
            resolver.OpenScope();
            resolver.AddToScope(KnownNames.System, ReferenceKind.RefToUnit, environment.TypeRegistry.SystemUnit);
        }

        /// <summary>
        ///     end visiting a unit
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(CompilationUnit element) {
            resolver.CloseScope();
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
                var signature = Runtime.Types.MakeSignature(TypeRegistry.SystemUnit.NoType);
                var mainRoutine = TypeCreator.CreateRoutine(mainRoutineGroup, RoutineKind.Procedure, signature);
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

                var varname = vardef.Name.CompleteName;
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

                resolver.AddToScope(varname, ReferenceKind.RefToVariable, variable);

                if (vardef is FunctionResult fn) {
                    if (fn.Method.TypeValue != default)
                        vardef.TypeInfo = fn.Method.TypeValue.TypeInfo;
                    else
                        MarkWithErrorType(vardef);

                    variable.SymbolType = vardef.TypeInfo;
                    continue;
                }

                fromResult = false;
                vardef.TypeInfo = element.TypeInfo;
                variable.SymbolType = vardef.TypeInfo;
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
            var typeReference = currentTypeDefinition.Pop();
            var type = typeReference.TypeDefinition as IStructuredType;

            if (type.IsConstant())
                element.TypeInfo = type.MakeConstant();
            else
                MarkWithErrorType(element);
        }

        /// <summary>
        ///     add a field in a record constants
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(RecordConstantItem element) {
            var typeReference = currentTypeDefinition.Peek();
            var type = typeReference.TypeDefinition as StructuredTypeDeclaration;
            type.Fields.Add(new Variable() { Name = element.Name.CompleteName, SymbolType = element.Value.TypeInfo });
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

                if (entry != default && entry.Kind == ReferenceKind.RefToType) {
                    element.TypeInfo = entry.Symbol.TypeDefinition;
                    return;
                }

                if (entry != default) {
                    typeId = entry.Symbol.TypeDefinition;
                    element.IsConstant = entry.Kind == ReferenceKind.RefToConstant;
                }
                else {
                    typeId = TypeRegistry.SystemUnit.ErrorType;
                }

                element.TypeInfo = typeId;
            }

            else if (element.Kind == MetaTypeKind.StringType) {
                element.TypeInfo = TypeRegistry.SystemUnit.StringType;
            }

            else if (element.Kind == MetaTypeKind.AnsiString) {
                element.TypeInfo = TypeRegistry.SystemUnit.AnsiStringType;
            }

            else if (element.Kind == MetaTypeKind.ShortString) {
                ITypeSymbol length = TypeRegistry.Runtime.Integers.Invalid;

                if (element.Value != default && element.Value.TypeInfo != default && element.Value.TypeInfo.IsConstant())
                    length = element.Value.TypeInfo;

                if (!(length is IIntegerValue intValue)) {
                    MarkWithErrorType(element);
                    return;
                }

                var stringLength = (byte)intValue.UnsignedValue;
                var userType = TypeCreator.CreateShortStringType(stringLength);
                element.TypeInfo = userType;
            }

            else if (element.Kind == MetaTypeKind.ShortStringDefault) {
                element.TypeInfo = TypeRegistry.SystemUnit.ShortStringType;
            }


            else if (element.Kind == MetaTypeKind.UnicodeString) {
                element.TypeInfo = TypeRegistry.SystemUnit.UnicodeStringType;
            }

            else if (element.Kind == MetaTypeKind.WideString) {
                element.TypeInfo = TypeRegistry.SystemUnit.WideStringType;
            }

            else if (element.Kind == MetaTypeKind.PointerType) {
                element.TypeInfo = TypeRegistry.SystemUnit.GenericPointerType;
            }

        }

        /// <summary>
        ///     visit a constant declaration
        /// </summary>
        /// <param name="element">item to visit</param>
        public void EndVisit(ConstantDeclaration element) {
            var declaredType = GetTypeOfNode(element.TypeValue);
            var inferredType = GetTypeOfNode(element.TypeValue);

            if (inferredType == default)
                MarkWithErrorType(element);
            else if (declaredType == default)
                element.TypeInfo = inferredType;
            else
                element.TypeInfo = TypeRegistry.Runtime.Cast(TypeRegistry, inferredType as IValue, declaredType.TypeDefinition);

            resolver.AddToScope(element.SymbolName, ReferenceKind.RefToConstant, element.TypeInfo);
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

            var entry = default(Reference);
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
                        var genericType = entry.Symbol.TypeDefinition as IGenericType;
                        if (genericType != default)
                            entry = new Reference(ReferenceKind.RefToBoundGeneric, genericType.Bind(environment.ListPools.GetFixedArray(list)));
                    }
                    else
                        entry = default;
                }
            }

            var typeId = default(ITypeDefinition);

            if (entry != default && (entry.Kind == ReferenceKind.RefToType || entry.Kind == ReferenceKind.RefToBoundGeneric))
                typeId = entry.Symbol.TypeDefinition;
            else
                typeId = TypeRegistry.SystemUnit.ErrorType;

            if (element.IsNewType) {
                typeId = TypeCreator.CreateTypeAlias(typeId, "", true);
            }

            element.TypeInfo = typeId;
        }

        /// <summary>
        ///     start visiting an enumeration type
        /// </summary>
        /// <param name="element">enumeration type definition</param>
        public void StartVisit(EnumTypeCollection element) {
            var typeDef = TypeCreator.CreateEnumType(string.Empty);
            currentTypeDefinition.Push(typeDef);
        }

        /// <summary>
        ///     create a record constant
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(RecordConstant element) {
            var typeDef = TypeCreator.CreateStructuredType(string.Empty, StructuredTypeKind.Record);
            currentTypeDefinition.Push(typeDef);
        }

        /// <summary>
        ///     end visiting an enumerated type definition
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(EnumTypeCollection element) {
            var typeReference = currentTypeDefinition.Pop();
            var typeDef = typeReference.TypeDefinition;

            if (typeDef is IEnumeratedType enumType) {
                var typeID = enumType.CommonTypeId;
                foreach (var enumValue in enumType.Values) {
                    enumValue.MakeEnumValue(TypeRegistry, typeID, enumType);
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
            var typeDef = value != default ? value.TypeDefinition as IEnumeratedType : null;
            if (typeDef == null) {
                MarkWithErrorType(element);
                return;
            }

            var itemValue = element.TypeInfo as IValue;
            var enumRef = typeDef.DefineEnumValue(environment.Runtime, element.SymbolName, false, itemValue) as IEnumeratedValue;

            if (enumRef is null) {
                MarkWithErrorType(element);
                return;
            }

            element.TypeInfo = TypeRegistry.Runtime.MakeEnumValue(typeDef, enumRef.Value);
            resolver.AddToScope(element.SymbolName, ReferenceKind.RefToEnumMember, enumRef);
        }

        /// <summary>
        ///     visit a subrange type
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(Parsing.SyntaxTree.Abstract.SubrangeType element) {

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
                typeDef = SystemUnit.ErrorType;
            else if (element.RangeEnd == null)
                typeDef = TypeCreator.CreateSubrangeType(string.Empty, left, left.LowestElement, leftTypeRef as IValue);
            else
                typeDef = TypeRegistry.GetTypeForSubrangeType(TypeCreator, leftTypeRef, rightTypeRef);

            element.TypeInfo = typeDef;
        }


        /// <summary>
        ///     declare a type
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(TypeDeclaration element) {
            if (element.Generics != default && element.Generics.Count > 0) {
                var placeholder = TypeCreator.CreateGenericPlaceholder(string.Empty);
                currentTypeDefinition.Push(placeholder);
            }
        }

        /// <summary>
        ///     declare a type
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(TypeDeclaration element) {

            if (element.Generics != default && element.Generics.Count > 0) {
                var placeholderRef = currentTypeDefinition.Pop();
                var placeholder = placeholderRef as IExtensibleGenericType;

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
                    resolver.AddToScope(element.Name.CompleteName, ReferenceKind.RefToType, element.TypeInfo.TypeDefinition, element.Generics?.Count ?? 0);
            }
        }

        /// <summary>
        ///     declare a set type
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(SetTypeDeclaration element) {

            if (element.TypeValue is ITypedSyntaxPart declaredEnum && declaredEnum.TypeInfo != null && declaredEnum.TypeInfo.TypeDefinition is IOrdinalType ot) {
                var setType = TypeCreator.CreateSetType(ot, string.Empty);
                element.TypeInfo = setType;
                return;
            }

            MarkWithErrorType(element);
        }

        /// <summary>
        ///     visit an array declaration
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ArrayTypeDeclaration element) {
            var baseTypeId = SystemUnit.ErrorType;

            if (element.TypeValue != null && element.TypeValue.TypeInfo != null) {
                baseTypeId = element.TypeValue.TypeInfo.TypeDefinition;
            }

            using (var list = environment.ListPools.GetList<ITypeDefinition>()) {
                foreach (var indexDef in element.IndexItems) {
                    var typeInfo = indexDef.TypeInfo;

                    if (typeInfo != null) {
                        if (typeInfo.GetBaseType() != BaseType.TypeAlias)
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

                element.TypeInfo = typeDef;
            }
        }

        /// <summary>
        ///     start visiting a structured type
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(StructuredType element) {
            var typeDef = TypeCreator.CreateStructuredType(string.Empty, element.Kind);
            typeDef.BaseClass = SystemUnit.TObjectType;
            currentTypeDefinition.Push(typeDef);
        }

        /// <summary>
        ///     end visiting a structured type
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(StructuredType element) {
            var value = currentTypeDefinition.Pop();
            var typeDef = value != null ? value.TypeDefinition as IStructuredType : null;

            foreach (var baseType in element.BaseTypes) {
                typeDef.BaseClass = baseType.TypeInfo.TypeDefinition;
            }

            element.TypeInfo = typeDef;
        }

    }
}
