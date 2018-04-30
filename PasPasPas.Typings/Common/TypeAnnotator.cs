using System.Collections.Generic;
using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using PasPasPas.Typings.Operators;
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
    public class TypeAnnotator :

        IEndVisitor<ConstantValue>,
        IEndVisitor<UnaryOperator>,
        IEndVisitor<BinaryOperator>,
        IEndVisitor<VariableDeclaration>,
        IEndVisitor<ConstantDeclaration>,
        IEndVisitor<Parsing.SyntaxTree.Abstract.TypeAlias>,
        IEndVisitor<MetaType>,
        IEndVisitor<SymbolReference>,
        IStartVisitor<CompilationUnit>,
        IEndVisitor<CompilationUnit>,
        IStartVisitor<EnumType>, IEndVisitor<EnumType>,
        IEndVisitor<EnumTypeValue>,
        IEndVisitor<Parsing.SyntaxTree.Abstract.SubrangeType>,
        IEndVisitor<TypeDeclaration>,
        IEndVisitor<SetTypeDeclaration>,
        IEndVisitor<ArrayTypeDeclaration>,
        IStartVisitor<StructuredType>,
        IEndVisitor<StructuredType>,
        IStartVisitor<MethodDeclaration>, IEndVisitor<MethodDeclaration>,
        IEndVisitor<ParameterTypeDefinition>,
        IEndVisitor<StructureFields>,
        IEndVisitor<StructuredStatement>,
        IEndVisitor<SetExpression>,
        IEndVisitor<ArrayConstant> {

        private readonly IStartEndVisitor visitor;
        private readonly ITypedEnvironment environment;
        private readonly Stack<ITypeReference> currentTypeDefintion;
        private readonly Stack<Routine> currentMethodDefinition;
        private readonly Stack<ParameterGroup> currentMethodParameters;
        private readonly Resolver resolver;

        /// <summary>
        ///     current unit
        /// </summary>
        public CompilationUnit CurrentUnit { get; private set; }

        /// <summary>
        ///     as common visitor
        /// </summary>
        /// <returns></returns>
        public IStartEndVisitor AsVisitor() =>
            visitor;

        /// <summary>
        ///     create a new type annotator
        /// </summary>
        /// <param name="env">typed environment</param>
        public TypeAnnotator(ITypedEnvironment env) {
            visitor = new Visitor(this);
            environment = env;
            resolver = new Resolver(new Scope(env.TypeRegistry));
            currentTypeDefintion = new Stack<ITypeReference>();
            currentMethodDefinition = new Stack<Routine>();
            currentMethodParameters = new Stack<ParameterGroup>();
        }

        private ITypeReference GetTypeDefinition(ITypedSyntaxNode expression) {
            if (expression != null && expression.TypeInfo != null)
                return expression.TypeInfo;

            return GetTypeByIdOrUndefinedType(KnownTypeIds.ErrorType);
        }

        [System.Obsolete]
        private CommonTypeKind GetTypeKind(ITypeReference value)
            => value == null ? CommonTypeKind.UnknownType : environment.TypeRegistry.GetTypeKind(value.TypeId);

        private ITypeDefinition GetRegisteredType(ITypedSyntaxNode syntaxNode) {
            if (syntaxNode != null && syntaxNode.TypeInfo != null)
                return environment.TypeRegistry.GetTypeByIdOrUndefinedType(syntaxNode.TypeInfo.TypeId);

            return GetErrorType(syntaxNode);
        }

        private ITypeReference GetTypeByIdOrUndefinedType(int typeId)
            => environment.ConstantValues.Types.MakeReference(typeId);

        private ITypeDefinition GetErrorType(ITypedSyntaxNode node)
            => environment.TypeRegistry.GetTypeByIdOrUndefinedType(KnownTypeIds.ErrorType);

        private ITypeReference GetErrorTypeReference(ITypedSyntaxNode node)
            => environment.ConstantValues.Types.MakeReference(KnownTypeIds.ErrorType);

        private ITypeRegistry TypeRegistry
            => environment.TypeRegistry;

        /// <summary>
        ///     determine the type of a constant value
        /// </summary>
        /// <param name="element">constant value</param>
        public void EndVisit(ConstantValue element) {

            // some constant literals have already assigned type information
            // nothing has to be done
            if (element.TypeInfo != null)
                return;

            if (element.Kind == ConstantValueKind.True) {
                element.TypeInfo = environment.ConstantValues.Booleans.TrueValue;
                return;
            }

            if (element.Kind == ConstantValueKind.False) {
                element.TypeInfo = environment.ConstantValues.Booleans.FalseValue;
                return;
            }

            if (element.Kind == ConstantValueKind.Nil) {
                element.TypeInfo = environment.ConstantValues.Types.Nil;
                return;
            }

            throw new TypeAnnotationException($"Invalid type information for constant: {element.Kind}", element);
        }

        /// <summary>
        ///     annotate types at binary operators
        /// </summary>
        /// <param name="element">operator to annotate</param>
        public void EndVisit(BinaryOperator element) {
            var left = GetTypeDefinition(element.LeftOperand);
            var right = GetTypeDefinition(element.RightOperand);

            // special case range operator: this operator is
            // part of a type definition and references types, not values
            if (element.Kind == ExpressionKind.RangeOperator) {
                DefineSubrangeType(element, left, right);
                return;
            }

            var operatorId = TypeRegistry.GetOperatorId(element.Kind, left, right);
            var binaryOperator = TypeRegistry.GetOperator(operatorId);
            if (operatorId == DefinedOperators.Undefined || binaryOperator == null) {
                element.TypeInfo = GetErrorTypeReference(element);
                return;
            }

            element.TypeInfo = binaryOperator.EvaluateOperator(new Signature(left, right));
        }

        /// <summary>
        ///     helper function: define a subrange type
        /// </summary>
        /// <param name="element"></param>
        /// <param name="leftType">lower bound of the subrange</param>
        /// <param name="rightType">higher bound of the subrange</param>
        private void DefineSubrangeType(BinaryOperator element, ITypeReference leftType, ITypeReference rightType) {
            var left = leftType.TypeKind;
            var right = rightType.TypeKind;

            if (left.IsOrdinal() && right.IsOrdinal()) {
                if (left.IsIntegral() && right.IsIntegral()) {
                    var baseType = GetTypeByIdOrUndefinedType(GetSmallestIntegralTypeOrNext(leftType.TypeId, rightType.TypeId));
                    var typeDef = RegisterUserDefinedType(new Simple.SubrangeType(RequireUserTypeId(), baseType.TypeId));
                    element.TypeInfo = GetTypeByIdOrUndefinedType(typeDef.TypeId);
                }
                else if (leftType.TypeId == rightType.TypeId) {
                    var baseType = GetTypeByIdOrUndefinedType(leftType.TypeId);
                    var typeDef = RegisterUserDefinedType(new Simple.SubrangeType(RequireUserTypeId(), baseType.TypeId));
                    element.TypeInfo = GetTypeByIdOrUndefinedType(typeDef.TypeId);
                }
                else
                    element.TypeInfo = GetErrorTypeReference(element);
            }
        }

        private int GetSmallestIntegralTypeOrNext(int leftId, int rightId)
            => environment.TypeRegistry.GetSmallestIntegralTypeOrNext(leftId, rightId);

        /// <summary>
        ///     determine the type of an unary operator
        /// </summary>
        /// <param name="element">operator to determine the type of</param>
        public void EndVisit(UnaryOperator element) {

            var operand = element.Value;

            if (operand == null || operand.TypeInfo == null) {
                element.TypeInfo = GetErrorTypeReference(element);
                return;
            }

            if (element.Kind == ExpressionKind.Not) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.NotOperator, GetTypeDefinition(operand));
            }
            else if (element.Kind == ExpressionKind.UnaryMinus) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.UnaryMinus, GetTypeDefinition(operand));
            }
            else if (element.Kind == ExpressionKind.UnaryPlus) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.UnaryPlus, GetTypeDefinition(operand));
            }
            else {
                element.TypeInfo = GetErrorTypeReference(element);
            }
        }

        /// <summary>
        ///     gets the type of a given operator
        /// </summary>
        /// <param name="operatorKind"></param>
        /// <param name="typeInfo"></param>
        /// <returns></returns>
        private ITypeReference GetTypeOfOperator(int operatorKind, ITypeReference typeInfo) {
            if (typeInfo == null)
                return null;

            var operation = environment.TypeRegistry.GetOperator(operatorKind);

            if (operation == null)
                return null;

            var signature = new Signature(typeInfo);
            return operation.EvaluateOperator(signature);
        }

        /// <summary>
        ///     visit a variable declaration
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(VariableDeclaration element) {
            if (element.TypeValue is ITypedSyntaxNode typeRef && typeRef.TypeInfo != null)
                element.TypeInfo = typeRef.TypeInfo;
            else
                element.TypeInfo = GetTypeByIdOrUndefinedType(KnownTypeIds.ErrorType);

            var t = environment.TypeRegistry.GetTypeByIdOrUndefinedType(element.TypeInfo.TypeId);

            if (t is MetaStructuredTypeDeclaration metaType)
                element.TypeInfo = GetTypeByIdOrUndefinedType(metaType.BaseType);

            foreach (var vardef in element.Names)
                resolver.AddToScope(vardef.Name.CompleteName, ReferenceKind.RefToVariable, vardef);
        }

        /// <summary>
        ///     visit a variable declaration
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(Parsing.SyntaxTree.Abstract.TypeAlias element) {
            var typeName = element.AsScopedName;

            if (typeName == default(ScopedName)) {
                element.TypeInfo = GetTypeByIdOrUndefinedType(KnownTypeIds.ErrorType);
                return;
            }

            var entry = resolver.ResolveByName(typeName);
            int typeId;

            if (entry != null && entry.Kind == ReferenceKind.RefToType)
                typeId = entry.Symbol.TypeId;
            else
                typeId = KnownTypeIds.ErrorType;

            if (element.IsNewType) {
                var newTypeId = RequireUserTypeId();
                RegisterUserDefinedType(new TypeAlias(newTypeId, typeId, true));
                typeId = newTypeId;
            }

            element.TypeInfo = GetTypeByIdOrUndefinedType(typeId);
        }

        /// <summary>
        ///     visit a meta type reference
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(MetaType element) {
            if (element.Kind == MetaTypeKind.NamedType) {
                var name = element.AsScopedName;
                var entry = resolver.ResolveByName(name);
                int typeId;

                if (entry != null) {
                    typeId = entry.Symbol.TypeId;
                    element.IsConstant = entry.Kind == ReferenceKind.RefToConstant;
                }
                else {
                    typeId = KnownTypeIds.ErrorType;
                }

                element.TypeInfo = environment.TypeRegistry.MakeReference(typeId);
            }

            else if (element.Kind == MetaTypeKind.String) {
                element.TypeInfo = GetTypeByIdOrUndefinedType(KnownTypeIds.StringType);
            }

            else if (element.Kind == MetaTypeKind.AnsiString) {
                element.TypeInfo = GetTypeByIdOrUndefinedType(KnownTypeIds.AnsiStringType);
            }

            else if (element.Kind == MetaTypeKind.ShortString) {
                var userTypeId = RequireUserTypeId();
                var userType = RegisterUserDefinedType(new ShortStringType(userTypeId));
                element.TypeInfo = GetTypeByIdOrUndefinedType(userTypeId);
            }

            else if (element.Kind == MetaTypeKind.ShortStringDefault) {
                element.TypeInfo = GetTypeByIdOrUndefinedType(KnownTypeIds.ShortStringType);
            }


            else if (element.Kind == MetaTypeKind.UnicodeString) {
                element.TypeInfo = GetTypeByIdOrUndefinedType(KnownTypeIds.UnicodeStringType);
            }

            else if (element.Kind == MetaTypeKind.WideString) {
                element.TypeInfo = GetTypeByIdOrUndefinedType(KnownTypeIds.WideStringType);
            }

            else if (element.Kind == MetaTypeKind.Pointer) {
                element.TypeInfo = GetTypeByIdOrUndefinedType(KnownTypeIds.GenericPointer);
            }

        }

        /// <summary>
        ///     begin visit a unit
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(CompilationUnit element) {
            CurrentUnit = element;
            resolver.OpenScope();
            resolver.AddToScope("System", ReferenceKind.RefToUnit, environment.TypeRegistry.SystemUnit);
        }

        /// <summary>
        ///     end visiting a unit
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(CompilationUnit element) {
            resolver.CloseScope();
            CurrentUnit = null;
        }

        /// <summary>
        ///     end visiting a symbol reference
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(SymbolReference element) {
            var baseTypeValue = GetTypeByIdOrUndefinedType(KnownTypeIds.UnspecifiedType);
            var isConstant = false;

            if (element.TypeValue is ITypedSyntaxNode typeRef)
                baseTypeValue = typeRef.TypeInfo;

            if (element.TypeValue != null && element.TypeValue.TypeInfo != null)
                isConstant = element.TypeValue.TypeInfo.IsConstant;

            foreach (var part in element.SymbolParts) {

                if (baseTypeValue.TypeId == KnownTypeIds.ErrorType)
                    break;

                if (part.Kind == SymbolReferencePartKind.SubItem) {

                    if (baseTypeValue.TypeId == KnownTypeIds.UnspecifiedType) {
                        var reference = resolver.ResolveByName(new ScopedName(part.Name.Name));

                        if (reference != null && reference.Symbol != null) {
                            baseTypeValue = GetTypeByIdOrUndefinedType(reference.Symbol.TypeId);
                            isConstant = reference.Kind == ReferenceKind.RefToConstant;
                            if (isConstant)
                                baseTypeValue = (reference.Symbol as ITypedSyntaxNode)?.TypeInfo;
                        }
                        else
                            baseTypeValue = GetErrorTypeReference(element);
                    }
                }
                else if (part.Kind == SymbolReferencePartKind.CallParameters) {
                    IList<ParameterGroup> callableRoutines = new List<ParameterGroup>();

                    var signature = new ITypeReference[part.Expressions.Count];
                    for (var i = 0; i < signature.Length; i++)
                        if (part.Expressions[i] != null && part.Expressions[i].TypeInfo != null)
                            signature[i] = part.Expressions[i].TypeInfo;
                        else
                            signature[i] = GetTypeByIdOrUndefinedType(KnownTypeIds.ErrorType);

                    if (baseTypeValue.TypeId == KnownTypeIds.UnspecifiedType) {
                        var reference = resolver.ResolveByName(new ScopedName(part.Name.CompleteName), new Signature(signature));

                        if (reference == null) {
                            baseTypeValue = GetErrorTypeReference(element);
                        }
                        else if (reference.Kind == ReferenceKind.RefToGlobalRoutine) {
                            if (reference.Symbol is IRoutine routine) {
                                isConstant = routine.IsConstant;
                                routine.ResolveCall(callableRoutines, new Signature(signature));
                            }
                        }

                    }

                    else if (GetTypeKind(baseTypeValue) == CommonTypeKind.ClassType && environment.TypeRegistry.GetTypeByIdOrUndefinedType(baseTypeValue.TypeId) is StructuredTypeDeclaration structType) {
                        isConstant = false;
                        structType.ResolveCall(part.Name.CompleteName, callableRoutines, new Signature(signature));
                    }

                    if (callableRoutines.Count == 1)
                        baseTypeValue = GetTypeByIdOrUndefinedType(callableRoutines[0].ResultType);
                }

            }

            element.TypeInfo = baseTypeValue;
        }

        /// <summary>
        ///     start visiting an enumeration type
        /// </summary>
        /// <param name="element">enumeration type definition</param>
        public void StartVisit(EnumType element) {
            var typeId = RequireUserTypeId();
            var typeDef = new EnumeratedType(typeId);
            var type = GetTypeByIdOrUndefinedType(RegisterUserDefinedType(typeDef).TypeId);
            currentTypeDefintion.Push(type);
        }

        /// <summary>
        ///     register a new type definition
        /// </summary>
        /// <param name="typeDef"></param>
        private ITypeDefinition RegisterUserDefinedType(ITypeDefinition typeDef)
            => environment.TypeRegistry.RegisterType(typeDef);

        /// <summary>
        ///     require a new type id for a user defined type
        /// </summary>
        /// <returns></returns>
        private int RequireUserTypeId()
            => environment.TypeRegistry.RequireUserTypeId();

        /// <summary>
        ///     end visiting an enumerated type definition
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(EnumType element) {
            var typeDef = currentTypeDefintion.Pop();
            element.TypeInfo = typeDef;
        }

        /// <summary>
        ///     enumerated type value definition
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(EnumTypeValue element) {
            var value = currentTypeDefintion.Peek();
            var typeDef = value != null ? environment.TypeRegistry.GetTypeByIdOrUndefinedType(value.TypeId) as EnumeratedType : null;
            if (typeDef == null)
                return;

            element.TypeInfo = GetTypeByIdOrUndefinedType(typeDef.TypeInfo.TypeId);
            typeDef.DefineEnumValue(element.SymbolName, false, -1);
            resolver.AddToScope(element.SymbolName, ReferenceKind.RefToEnumMember, element);
        }

        /// <summary>
        ///     visit a subrange type
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(Parsing.SyntaxTree.Abstract.SubrangeType element) {

            var left = GetTypeKind(element.RangeStart?.TypeInfo);
            var right = GetTypeKind(element.RangeEnd?.TypeInfo);
            var type = GetTypeByIdOrUndefinedType(KnownTypeIds.ErrorType);

            if (element.RangeEnd == null) {
                type = GetTypeByIdOrUndefinedType(RegisterUserDefinedType(new Simple.SubrangeType(RequireUserTypeId(), element.RangeStart.TypeInfo.TypeId)).TypeId);
            }
            else {

                if (left.IsIntegral() && right.IsIntegral()) {
                    var baseTypeId = environment.TypeRegistry.GetSmallestIntegralTypeOrNext(element.RangeStart.TypeInfo.TypeId, element.RangeEnd.TypeInfo.TypeId);
                    type = GetTypeByIdOrUndefinedType(RegisterUserDefinedType(new Simple.SubrangeType(RequireUserTypeId(), baseTypeId)).TypeId);
                }
                else if (//
                    CommonTypeKind.WideCharType.All(left, right) ||
                    CommonTypeKind.AnsiCharType.All(left, right) ||
                    CommonTypeKind.BooleanType.All(left, right)) {
                    var baseTypeId = element.RangeStart.TypeInfo.TypeId;
                    type = GetTypeByIdOrUndefinedType(RegisterUserDefinedType(new Simple.SubrangeType(RequireUserTypeId(), baseTypeId)).TypeId);
                }
                else if (CommonTypeKind.EnumerationType.All(left, right) &&
                    element.RangeStart.TypeInfo.TypeId == element.RangeEnd.TypeInfo.TypeId) {
                    var baseTypeId = element.RangeStart.TypeInfo.TypeId;
                    type = GetTypeByIdOrUndefinedType(RegisterUserDefinedType(new Simple.SubrangeType(RequireUserTypeId(), baseTypeId)).TypeId);
                }
                else {
                    type = GetErrorTypeReference(element);
                }
            }

            element.TypeInfo = type;
        }

        /// <summary>
        ///     declare a type
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(TypeDeclaration element) {

            if (element.TypeValue is ITypedSyntaxNode declaredType && declaredType.TypeInfo != null) {
                element.TypeInfo = element.TypeValue.TypeInfo;
                resolver.AddToScope(element.Name.CompleteName, ReferenceKind.RefToType, element);
            }
        }

        /// <summary>
        ///     declare a set type
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(SetTypeDeclaration element) {

            if (element.TypeValue is ITypedSyntaxNode declaredEnum && declaredEnum.TypeInfo != null && GetTypeKind(declaredEnum.TypeInfo).IsOrdinal()) {
                var typeId = RequireUserTypeId();
                var setType = new SetType(typeId, declaredEnum.TypeInfo.TypeId);
                RegisterUserDefinedType(setType);
                element.TypeInfo = GetTypeByIdOrUndefinedType(typeId);
                return;
            }

            element.TypeInfo = GetTypeByIdOrUndefinedType(KnownTypeIds.ErrorType);
        }

        /// <summary>
        ///     visit an array declaration
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ArrayTypeDeclaration element) {
            var typeId = RequireUserTypeId();
            var typeDef = new ArrayType(typeId);

            if (element.TypeValue != null && element.TypeValue.TypeInfo != null)
                typeDef.BaseTypeId = element.TypeValue.TypeInfo.TypeId;

            typeDef.Packed = element.PackedType;

            foreach (var indexDef in element.IndexItems) {
                if (indexDef.TypeInfo != null)
                    typeDef.IndexTypes.Add(indexDef.TypeInfo);
                else
                    typeDef.IndexTypes.Add(GetTypeByIdOrUndefinedType(KnownTypeIds.ErrorType));
            }

            RegisterUserDefinedType(typeDef);
            element.TypeInfo = GetTypeByIdOrUndefinedType(typeDef.TypeInfo.TypeId);
        }

        /// <summary>
        ///     start visiting a structured type
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(StructuredType element) {
            var typeId = RequireUserTypeId();
            var metaTypeId = RequireUserTypeId();
            var typeDef = new StructuredTypeDeclaration(typeId, element.Kind);
            var metaType = new MetaStructuredTypeDeclaration(metaTypeId, typeId);
            RegisterUserDefinedType(typeDef);
            RegisterUserDefinedType(metaType);
            typeDef.BaseClass = GetTypeByIdOrUndefinedType(KnownTypeIds.TObject);
            typeDef.MetaType = metaType;

            currentTypeDefintion.Push(GetTypeByIdOrUndefinedType(typeDef.TypeInfo.TypeId));
        }

        /// <summary>
        ///     end visiting a structured type
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(StructuredType element) {
            var value = currentTypeDefintion.Pop();
            var typeDef = value != null ? environment.TypeRegistry.GetTypeByIdOrUndefinedType(value.TypeId) as StructuredTypeDeclaration : null;
            element.TypeInfo = GetTypeByIdOrUndefinedType(typeDef.MetaType.TypeInfo.TypeId);
        }

        /// <summary>
        ///     start visiting a method declaration
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(MethodDeclaration element) {
            var v = currentTypeDefintion.Peek();
            var typeDef = v != null ? environment.TypeRegistry.GetTypeByIdOrUndefinedType(v.TypeId) as StructuredTypeDeclaration : null;
            var method = typeDef.AddOrExtendMethod(element.Name.CompleteName, element.Kind);
            currentMethodDefinition.Push(method);
            currentMethodParameters.Push(method.AddParameterGroup());
        }

        /// <summary>
        ///     end visiting a method declaration
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(MethodDeclaration element) {
            if (element.Kind == ProcedureKind.Function) {
                var v = currentTypeDefintion.Peek();
                var typeDef = v != null ? environment.TypeRegistry.GetTypeByIdOrUndefinedType(v.TypeId) as StructuredTypeDeclaration : null;
                var method = currentMethodDefinition.Pop();
                var methodParams = currentMethodParameters.Pop();

                if (element.TypeValue != null && element.TypeValue.TypeInfo != null)
                    methodParams.ResultType = element.TypeValue.TypeInfo.TypeId;
                else
                    methodParams.ResultType = KnownTypeIds.ErrorType;
            }
        }

        /// <summary>
        ///     visit a parameter type definition
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ParameterTypeDefinition element) {
            if (element.TypeValue != null && element.TypeValue.TypeInfo != null) {
                var typeDef = currentTypeDefintion.Peek() as StructuredTypeDeclaration;
                var parms = currentMethodParameters.Peek();

                foreach (var name in element.Parameters) {
                    var param = parms.AddParameter(name.Name.CompleteName);
                    param.SymbolType = element.TypeValue.TypeInfo.TypeId;
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
                typeInfo = GetTypeByIdOrUndefinedType(KnownTypeIds.ErrorType);
            }

            var v = currentTypeDefintion.Peek();
            var typeDef = v != null ? environment.TypeRegistry.GetTypeByIdOrUndefinedType(v.TypeId) as StructuredTypeDeclaration : null;

            foreach (var field in element.Fields) {
                var fieldDef = new Variable() {
                    Name = field.Name.CompleteName,
                    SymbolType = typeInfo.TypeId
                };

                if (element.ClassItem)
                    typeDef.MetaType.AddField(fieldDef);
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
            var left = element.Expressions[0]?.TypeInfo;
            var right = element.Expressions[1]?.TypeInfo;
            if (left != null && right != null) {

                environment.TypeRegistry.GetTypeByIdOrUndefinedType(left.TypeId).CanBeAssignedFrom(environment.TypeRegistry.GetTypeByIdOrUndefinedType(right.TypeId));
            }
        }

        /// <summary>
        ///     visit a constant declaration
        /// </summary>
        /// <param name="element">item to visit</param>
        public void EndVisit(ConstantDeclaration element) {
            if (element.TypeValue is ITypedSyntaxNode typeRef && typeRef.TypeInfo != null && typeRef.TypeInfo.TypeId != KnownTypeIds.ErrorType)
                element.TypeInfo = typeRef.TypeInfo;

            if (element.Value is ITypedSyntaxNode autType && autType.TypeInfo != null && autType.TypeInfo.TypeId != KnownTypeIds.ErrorType)
                element.TypeInfo = autType.TypeInfo;

            resolver.AddToScope(element.SymbolName, ReferenceKind.RefToConstant, element);
        }

        /// <summary>
        ///     visit a set expression
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(SetExpression element) {
            var typeId = RequireUserTypeId();
            var isConstant = true;
            var baseType = default(ITypeReference);

            foreach (var part in element.Expressions) {

                if (part.TypeInfo == null) {
                    baseType = GetErrorTypeReference(part);
                    break;
                }

                if (baseType == null)
                    baseType = part.TypeInfo;
                else if (GetTypeKind(baseType).IsIntegral() && GetTypeKind(part.TypeInfo).IsIntegral())
                    baseType = GetTypeByIdOrUndefinedType(GetSmallestIntegralTypeOrNext(baseType.TypeId, part.TypeInfo.TypeId));
                else if (GetTypeKind(baseType).IsOrdinal() && baseType.TypeId == part.TypeInfo.TypeId)
                    baseType = part.TypeInfo;
                else {
                    baseType = GetErrorTypeReference(part);
                    break;
                }

                isConstant = isConstant && part.TypeInfo.IsConstant;
            }

            var typdef = RegisterUserDefinedType(new SetType(typeId, baseType.TypeId));
            element.TypeInfo = GetTypeByIdOrUndefinedType(typdef.TypeId);
        }

        /// <summary>
        ///     visit an array constant
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ArrayConstant element) {
            var typeId = RequireUserTypeId();
            var isConstant = true;
            var baseType = default(ITypeReference);

            foreach (var part in element.Items) {

                if (part.TypeInfo == null) {
                    baseType = GetErrorTypeReference(part);
                    break;
                }

                if (baseType == null)
                    baseType = part.TypeInfo;
                else if (GetTypeKind(baseType).IsIntegral() && GetTypeKind(part.TypeInfo).IsIntegral())
                    baseType = GetTypeByIdOrUndefinedType(GetSmallestIntegralTypeOrNext(baseType.TypeId, part.TypeInfo.TypeId));
                else if (GetTypeKind(baseType).IsOrdinal() && baseType.TypeId == part.TypeInfo.TypeId)
                    baseType = part.TypeInfo;
                else {
                    baseType = GetErrorTypeReference(part);
                    break;
                }

                isConstant = isConstant && part.TypeInfo.IsConstant;
            }

            element.TypeInfo = GetTypeByIdOrUndefinedType(RegisterUserDefinedType(new ArrayType(typeId) { BaseTypeId = baseType.TypeId }).TypeId);
        }
    }
}
