using System;
using System.Collections.Generic;
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
        private readonly Stack<ITypeDefinition> currentTypeDefintion;
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
            currentTypeDefintion = new Stack<ITypeDefinition>();
            currentMethodDefinition = new Stack<Routine>();
            currentMethodParameters = new Stack<ParameterGroup>();
        }

        private ITypeDefinition GetTypeByIdOrUndefinedType(int typeId)
            => environment.TypeRegistry.GetTypeByIdOrUndefinedType(typeId);

        /// <summary>
        ///     determine the type of a constant value
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ConstantValue element) {

            if (element.Kind == ConstantValueKind.HexNumber ||
                element.Kind == ConstantValueKind.Integer ||
                element.Kind == ConstantValueKind.QuotedString ||
                element.Kind == ConstantValueKind.RealNumber ||
                element.Kind == ConstantValueKind.True ||
                element.Kind == ConstantValueKind.False) {
                var typeId = LiteralValues.GetTypeFor(element.LiteralValue);
                element.TypeInfo = GetTypeByIdOrUndefinedType(typeId);
            }
        }

        /// <summary>
        ///     annotate binary operators
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(BinaryOperator element) {

            ITypeDefinition leftType;
            ITypeDefinition rightType;
            var operatorId = -1;

            if (element.LeftOperand != null && element.LeftOperand.TypeInfo != null)
                leftType = element.LeftOperand.TypeInfo;
            else
                leftType = GetErrorType(element.LeftOperand);

            if (element.RightOperand != null && element.RightOperand.TypeInfo != null)
                rightType = element.RightOperand.TypeInfo;
            else
                rightType = GetErrorType(element.RightOperand);

            if (element.Kind == ExpressionKind.And)
                operatorId = DefinedOperators.AndOperation;
            else if (element.Kind == ExpressionKind.Or)
                operatorId = DefinedOperators.OrOperation;
            else if (element.Kind == ExpressionKind.Xor)
                operatorId = DefinedOperators.XorOperation;
            else if (element.Kind == ExpressionKind.Minus)
                operatorId = DefinedOperators.MinusOperation;
            else if (element.Kind == ExpressionKind.Times)
                operatorId = DefinedOperators.TimesOperation;
            else if (element.Kind == ExpressionKind.Div)
                operatorId = DefinedOperators.DivOperation;
            else if (element.Kind == ExpressionKind.Mod)
                operatorId = DefinedOperators.ModOperation;
            else if (element.Kind == ExpressionKind.Slash)
                operatorId = DefinedOperators.SlashOperation;
            else if (element.Kind == ExpressionKind.Shl)
                operatorId = DefinedOperators.ShlOperation;
            else if (element.Kind == ExpressionKind.Shr)
                operatorId = DefinedOperators.ShrOperation;
            else if (element.Kind == ExpressionKind.EqualsSign)
                operatorId = DefinedOperators.EqualsOperator;
            else if (element.Kind == ExpressionKind.NotEquals)
                operatorId = DefinedOperators.NotEqualsOperator;
            else if (element.Kind == ExpressionKind.GreaterThen)
                operatorId = DefinedOperators.GreaterThen;
            else if (element.Kind == ExpressionKind.GreaterThenEquals)
                operatorId = DefinedOperators.GreaterThenEqual;
            else if (element.Kind == ExpressionKind.LessThen)
                operatorId = DefinedOperators.LessThen;
            else if (element.Kind == ExpressionKind.LessThenEquals)
                operatorId = DefinedOperators.LessThenOrEqual;

            else if (element.Kind == ExpressionKind.Plus) {

                if (leftType.TypeKind.IsTextual() && leftType.TypeKind.IsTextual())
                    operatorId = DefinedOperators.ConcatOperation;
                else if (leftType.TypeKind.IsNumerical() && leftType.TypeKind.IsNumerical())
                    operatorId = DefinedOperators.PlusOperation;
            }

            else if (element.Kind == ExpressionKind.RangeOperator) {
                var left = leftType.TypeKind;
                var right = rightType.TypeKind;
                var leftId = leftType.TypeId;
                var rightId = rightType.TypeId;

                if (left.IsOrdinal() && right.IsOrdinal()) {
                    if (left.Integral() && right.Integral()) {
                        var baseType = GetTypeByIdOrUndefinedType(GetSmallestIntegralTypeOrNext(leftId, rightId));
                        element.TypeInfo = RegisterUserDefinedType(new Simple.SubrangeType(RequireUserTypeId(), baseType.TypeId));
                    }
                    else if (leftId == rightId) {
                        var baseType = GetTypeByIdOrUndefinedType(leftId);
                        element.TypeInfo = RegisterUserDefinedType(new Simple.SubrangeType(RequireUserTypeId(), baseType.TypeId));
                    }
                    else
                        element.TypeInfo = GetErrorType(element);

                    element.IsConstant = element.LeftOperand.IsConstant && element.RightOperand.IsConstant;
                }
            }

            if (operatorId >= 0) {
                element.TypeInfo = GetTypeOfOperator(operatorId, leftType, rightType, element.LeftOperand.LiteralValue, element.RightOperand.LiteralValue);
                element.IsConstant = element.LeftOperand.IsConstant && element.RightOperand.IsConstant;
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
                element.TypeInfo = GetErrorType(element);
                return;
            }

            if (element.Kind == ExpressionKind.Not) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.NotOperation, operand.TypeInfo, operand.LiteralValue);
                element.IsConstant = operand.IsConstant;
            }
            else if (element.Kind == ExpressionKind.UnaryMinus) {
                //element.LiteralValue =
                element.IsConstant = operand.IsConstant;
                if (element.IsConstant) {
                    element.LiteralValue = environment.Runtime.Constants.Negate(operand.LiteralValue);
                    element.TypeInfo = GetTypeOfLiteral(element.LiteralValue);
                }
                else {
                    element.TypeInfo = GetTypeOfOperator(DefinedOperators.UnaryMinus, operand.TypeInfo, operand.LiteralValue);
                }
            }
            else if (element.Kind == ExpressionKind.UnaryPlus) {
                element.TypeInfo = GetTypeOfOperator(DefinedOperators.UnaryPlus, operand.TypeInfo, operand.LiteralValue);
                element.IsConstant = operand.IsConstant;
            }
        }

        private ITypeDefinition GetTypeOfLiteral(object value) {
            return GetTypeByIdOrUndefinedType(LiteralValues.GetTypeFor(value));
        }

        private ITypeDefinition GetErrorType(ITypedSyntaxNode node) {
            return GetTypeByIdOrUndefinedType(TypeIds.ErrorType);
        }

        /// <summary>
        ///     gets the type of a given operator
        /// </summary>
        /// <param name="operatorKind"></param>
        /// <param name="typeInfo"></param>
        /// <param name="currentValues">current value</param>
        /// <returns></returns>
        private ITypeDefinition GetTypeOfOperator(int operatorKind, ITypeDefinition typeInfo, object currentValues) {
            if (typeInfo == null)
                return null;

            var operation = environment.TypeRegistry.GetOperator(operatorKind);

            if (operation == null)
                return null;

            var signature = new Signature(typeInfo.TypeId);
            var typeId = operation.GetOutputTypeForOperation(signature, new object[] { currentValues });
            return GetTypeByIdOrUndefinedType(typeId);
        }

        /// <summary>
        ///     gets the type of a given binary operator
        /// </summary>
        /// <param name="operatorKind"></param>
        /// <param name="typeInfo1"></param>
        /// <param name="typeInfo2"></param>
        /// <param name="left">constant left value</param>
        /// <param name="right">constant right value</param>
        /// <returns></returns>
        private ITypeDefinition GetTypeOfOperator(int operatorKind, ITypeDefinition typeInfo1, ITypeDefinition typeInfo2, object left, object right) {
            if (typeInfo1 == null)
                return null;

            if (typeInfo2 == null)
                return null;

            var operation = environment.TypeRegistry.GetOperator(operatorKind);

            if (operation == null)
                return null;

            var signature = new Signature(typeInfo1.TypeId, typeInfo2.TypeId);
            var typeId = operation.GetOutputTypeForOperation(signature, new[] { left, right });
            return GetTypeByIdOrUndefinedType(typeId);
        }

        /// <summary>
        ///     visit a variable declaration
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(VariableDeclaration element) {
            if (element.TypeValue is ITypedSyntaxNode typeRef && typeRef.TypeInfo != null)
                element.TypeInfo = typeRef.TypeInfo;
            else
                element.TypeInfo = GetTypeByIdOrUndefinedType(TypeIds.ErrorType);

            if (element.TypeInfo is MetaStructuredTypeDeclaration metaType)
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
                element.TypeInfo = GetTypeByIdOrUndefinedType(TypeIds.ErrorType);
                return;
            }

            var entry = resolver.ResolveByName(typeName);
            int typeId;

            if (entry != null && entry.Kind == ReferenceKind.RefToType)
                typeId = entry.Symbol.TypeId;
            else
                typeId = GetErrorType(element).TypeId;

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
                    typeId = GetErrorType(element).TypeId;
                }

                element.TypeInfo = GetTypeByIdOrUndefinedType(typeId);
            }

            else if (element.Kind == MetaTypeKind.String) {
                element.TypeInfo = GetTypeByIdOrUndefinedType(TypeIds.StringType);
            }

            else if (element.Kind == MetaTypeKind.AnsiString) {
                element.TypeInfo = GetTypeByIdOrUndefinedType(TypeIds.AnsiStringType);
            }

            else if (element.Kind == MetaTypeKind.ShortString) {
                var userTypeId = RequireUserTypeId();
                var userType = RegisterUserDefinedType(new ShortStringType(userTypeId));
                element.TypeInfo = GetTypeByIdOrUndefinedType(userTypeId);
            }

            else if (element.Kind == MetaTypeKind.ShortStringDefault) {
                element.TypeInfo = GetTypeByIdOrUndefinedType(TypeIds.ShortStringType);
            }


            else if (element.Kind == MetaTypeKind.UnicodeString) {
                element.TypeInfo = GetTypeByIdOrUndefinedType(TypeIds.UnicodeStringType);
            }

            else if (element.Kind == MetaTypeKind.WideString) {
                element.TypeInfo = GetTypeByIdOrUndefinedType(TypeIds.WideStringType);
            }

            else if (element.Kind == MetaTypeKind.Pointer) {
                element.TypeInfo = GetTypeByIdOrUndefinedType(TypeIds.GenericPointer);
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
            var baseTypeValue = GetTypeByIdOrUndefinedType(TypeIds.UnspecifiedType);
            var isConstant = false;

            if (element.TypeValue is ITypedSyntaxNode typeRef)
                baseTypeValue = typeRef.TypeInfo;

            if (element.TypeValue is IConstantValueNode constNode)
                isConstant = constNode.IsConstant;

            foreach (var part in element.SymbolParts) {

                if (baseTypeValue.TypeId == TypeIds.ErrorType)
                    break;

                if (part.Kind == SymbolReferencePartKind.SubItem) {

                    if (baseTypeValue.TypeId == TypeIds.UnspecifiedType) {
                        var reference = resolver.ResolveByName(new ScopedName(part.Name.Name));

                        if (reference != null && reference.Symbol != null) {
                            baseTypeValue = GetTypeByIdOrUndefinedType(reference.Symbol.TypeId);
                            isConstant = reference.Kind == ReferenceKind.RefToConstant;
                        }
                        else
                            baseTypeValue = GetErrorType(element);
                    }
                }
                else if (part.Kind == SymbolReferencePartKind.CallParameters) {
                    IList<ParameterGroup> callableRoutines = new List<ParameterGroup>();

                    var signature = new int[part.Expressions.Count];
                    for (var i = 0; i < signature.Length; i++)
                        if (part.Expressions[i] != null && part.Expressions[i].TypeInfo != null)
                            signature[i] = part.Expressions[i].TypeInfo.TypeId;
                        else
                            signature[i] = TypeIds.ErrorType;

                    if (baseTypeValue.TypeId == TypeIds.UnspecifiedType) {
                        var reference = resolver.ResolveByName(new ScopedName(part.Name.CompleteName), new Signature(signature));

                        if (reference == null) {
                            baseTypeValue = GetErrorType(element);
                        }
                        else if (reference.Kind == ReferenceKind.RefToGlobalRoutine) {
                            if (reference.Symbol is IRoutine routine) {
                                isConstant = routine.IsConstant;
                                routine.ResolveCall(callableRoutines, new Signature(signature));
                            }
                        }

                    }
                    else if (baseTypeValue.TypeKind == CommonTypeKind.ClassType && baseTypeValue is StructuredTypeDeclaration structType) {
                        isConstant = false;
                        structType.ResolveCall(part.Name.CompleteName, callableRoutines, new Signature(signature));
                    }

                    if (callableRoutines.Count == 1)
                        baseTypeValue = GetTypeByIdOrUndefinedType(callableRoutines[0].ResultType.TypeId);
                }

            }

            element.TypeInfo = baseTypeValue;
            element.IsConstant = isConstant;
        }

        /// <summary>
        ///     start visting an enumeration type
        /// </summary>
        /// <param name="element">enumeration type definition</param>
        public void StartVisit(EnumType element) {
            var typeId = RequireUserTypeId();
            var typeDef = new EnumeratedType(typeId);
            RegisterUserDefinedType(typeDef);
            currentTypeDefintion.Push(typeDef);
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
        ///     end visiting an enum type definition
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(EnumType element) {
            var typeDef = currentTypeDefintion.Pop();
            element.TypeInfo = typeDef;
        }

        /// <summary>
        ///     enum type value definition
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(EnumTypeValue element) {
            var typeDef = currentTypeDefintion.Peek() as EnumeratedType;
            if (typeDef == null)
                return;

            element.TypeInfo = typeDef;
            typeDef.DefineEnumValue(element.SymbolName, false, -1);
            resolver.AddToScope(element.SymbolName, ReferenceKind.RefToEnumMember, element);
        }

        /// <summary>
        ///     visit a subrange type
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(Parsing.SyntaxTree.Abstract.SubrangeType element) {

            var left = element.RangeStart?.TypeInfo?.TypeKind;
            var right = element.RangeEnd?.TypeInfo?.TypeKind;
            var type = GetTypeByIdOrUndefinedType(TypeIds.ErrorType);

            if (left.HasValue && element.RangeEnd == null) {
                type = RegisterUserDefinedType(new Simple.SubrangeType(RequireUserTypeId(), element.RangeStart.TypeInfo.TypeId));
            }
            else if (left.HasValue && right.HasValue) {

                if (left.Value.Integral() && right.Value.Integral()) {
                    var baseTypeId = environment.TypeRegistry.GetSmallestIntegralTypeOrNext(element.RangeStart.TypeInfo.TypeId, element.RangeEnd.TypeInfo.TypeId);
                    type = RegisterUserDefinedType(new Simple.SubrangeType(RequireUserTypeId(), baseTypeId));
                }
                else if (//
                    CommonTypeKind.WideCharType.All(left.Value, right.Value) ||
                    CommonTypeKind.AnsiCharType.All(left.Value, right.Value) ||
                    CommonTypeKind.BooleanType.All(left.Value, right.Value)) {
                    var baseTypeId = element.RangeStart.TypeInfo.TypeId;
                    type = RegisterUserDefinedType(new Simple.SubrangeType(RequireUserTypeId(), baseTypeId));
                }
                else if (CommonTypeKind.EnumerationType.All(left.Value, right.Value) &&
                    element.RangeStart.TypeInfo.TypeId == element.RangeEnd.TypeInfo.TypeId) {
                    var baseTypeId = element.RangeStart.TypeInfo.TypeId;
                    type = RegisterUserDefinedType(new Simple.SubrangeType(RequireUserTypeId(), baseTypeId));
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

            if (element.TypeValue is ITypedSyntaxNode declaredEnum && declaredEnum.TypeInfo != null && declaredEnum.TypeInfo.TypeKind.IsOrdinal()) {
                var typeId = RequireUserTypeId();
                var setType = new SetType(typeId, declaredEnum.TypeInfo.TypeId);
                RegisterUserDefinedType(setType);
                element.TypeInfo = setType;
                return;
            }

            element.TypeInfo = GetTypeByIdOrUndefinedType(TypeIds.ErrorType);
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
                    typeDef.IndexTypes.Add(GetTypeByIdOrUndefinedType(TypeIds.ErrorType));
            }

            RegisterUserDefinedType(typeDef);
            element.TypeInfo = typeDef;
        }

        /// <summary>
        ///     start visting a structured type
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(StructuredType element) {
            var typeId = RequireUserTypeId();
            var metaTypeId = RequireUserTypeId();
            var typeDef = new StructuredTypeDeclaration(typeId, element.Kind);
            var metaType = new MetaStructuredTypeDeclaration(metaTypeId, typeId);
            RegisterUserDefinedType(typeDef);
            RegisterUserDefinedType(metaType);
            typeDef.BaseClass = GetTypeByIdOrUndefinedType(TypeIds.TObject);
            typeDef.MetaType = metaType;

            currentTypeDefintion.Push(typeDef);
        }

        /// <summary>
        ///     end visiting a structured type
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(StructuredType element) {
            var typeDef = currentTypeDefintion.Pop() as StructuredTypeDeclaration;
            element.TypeInfo = typeDef.MetaType;
        }

        /// <summary>
        ///     start visting a method declaration
        /// </summary>ele
        /// <param name="element"></param>
        public void StartVisit(MethodDeclaration element) {
            var typeDef = currentTypeDefintion.Peek() as StructuredTypeDeclaration;
            var method = typeDef.AddOrExtendMethod(element.Name.CompleteName, element.Kind);
            currentMethodDefinition.Push(method);
            currentMethodParameters.Push(method.AddParameterGroup());
        }

        /// <summary>
        ///     end visting a method declaration
        /// </summary>ele
        /// <param name="element"></param>
        public void EndVisit(MethodDeclaration element) {
            if (element.Kind == ProcedureKind.Function) {
                var typeDef = currentTypeDefintion.Peek() as StructuredTypeDeclaration;
                var method = currentMethodDefinition.Pop();
                var methodParams = currentMethodParameters.Pop();

                if (element.TypeValue != null && element.TypeValue.TypeInfo != null)
                    methodParams.ResultType = element.TypeValue.TypeInfo;
                else
                    methodParams.ResultType = GetTypeByIdOrUndefinedType(TypeIds.ErrorType);
            }
        }

        /// <summary>
        ///     visit a paramer type definition
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ParameterTypeDefinition element) {
            if (element.TypeValue != null && element.TypeValue.TypeInfo != null) {
                var typeDef = currentTypeDefintion.Peek() as StructuredTypeDeclaration;
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
            ITypeDefinition typeInfo;
            if (element.TypeValue != null && element.TypeValue.TypeInfo != null)
                typeInfo = element.TypeValue.TypeInfo;
            else {
                typeInfo = GetTypeByIdOrUndefinedType(TypeIds.ErrorType);
            }

            var typeDef = currentTypeDefintion.Peek() as StructuredTypeDeclaration;

            foreach (var field in element.Fields) {
                var fieldDef = new Variable() {
                    Name = field.Name.CompleteName,
                    SymbolType = typeInfo
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
                left.CanBeAssignedFrom(right);
            }
        }

        /// <summary>
        ///     visit a constant declaration
        /// </summary>
        /// <param name="element">item to visit</param>
        public void EndVisit(ConstantDeclaration element) {
            var typeId = TypeIds.ErrorType;
            var autoTypeId = TypeIds.ErrorType;
            var declaredTypeId = TypeIds.ErrorType;

            if (element.TypeValue is ITypedSyntaxNode typeRef && typeRef.TypeInfo != null)
                declaredTypeId = typeRef.TypeInfo.TypeId;

            if (element.Value is ITypedSyntaxNode autType && autType.TypeInfo != null)
                autoTypeId = autType.TypeInfo.TypeId;

            if (declaredTypeId != TypeIds.ErrorType)
                typeId = declaredTypeId;
            else
                typeId = autoTypeId;

            element.TypeInfo = GetTypeByIdOrUndefinedType(typeId);
            resolver.AddToScope(element.SymbolName, ReferenceKind.RefToConstant, element);
        }

        /// <summary>
        ///     visit a set expression
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(SetExpression element) {
            var typeId = RequireUserTypeId();
            var isConstant = true;
            ITypeDefinition baseType = null;

            foreach (var part in element.Expressions) {

                if (part.TypeInfo == null) {
                    baseType = GetErrorType(part);
                    break;
                }

                if (baseType == null)
                    baseType = part.TypeInfo;
                else if (baseType.TypeKind.Integral() && part.TypeInfo.TypeKind.Integral())
                    baseType = GetTypeByIdOrUndefinedType(GetSmallestIntegralTypeOrNext(baseType.TypeId, part.TypeInfo.TypeId));
                else if (baseType.TypeKind.IsOrdinal() && baseType.TypeId == part.TypeInfo.TypeId)
                    baseType = part.TypeInfo;
                else {
                    baseType = GetErrorType(part);
                    break;
                }

                isConstant = isConstant && part.IsConstant;
            }

            element.TypeInfo = RegisterUserDefinedType(new SetType(typeId, baseType.TypeId));
            element.IsConstant = isConstant;
        }

        /// <summary>
        ///     visit an array constant
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ArrayConstant element) {
            var typeId = RequireUserTypeId();
            var isConstant = true;
            ITypeDefinition baseType = null;

            foreach (var part in element.Items) {

                if (part.TypeInfo == null) {
                    baseType = GetErrorType(part);
                    break;
                }

                if (baseType == null)
                    baseType = part.TypeInfo;
                else if (baseType.TypeKind.Integral() && part.TypeInfo.TypeKind.Integral())
                    baseType = GetTypeByIdOrUndefinedType(GetSmallestIntegralTypeOrNext(baseType.TypeId, part.TypeInfo.TypeId));
                else if (baseType.TypeKind.IsOrdinal() && baseType.TypeId == part.TypeInfo.TypeId)
                    baseType = part.TypeInfo;
                else {
                    baseType = GetErrorType(part);
                    break;
                }

                isConstant = isConstant && part.IsConstant;
            }

            element.TypeInfo = RegisterUserDefinedType(new ArrayType(typeId) { BaseTypeId = baseType.TypeId });
            element.IsConstant = isConstant;
        }
    }
}
