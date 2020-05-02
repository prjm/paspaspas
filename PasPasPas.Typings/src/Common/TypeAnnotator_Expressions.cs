using System.Collections.Generic;
using System.Collections.Immutable;
using PasPasPas.Globals;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Abstract;

namespace PasPasPas.Typings.Common {

    public partial class TypeAnnotator {


        /// <summary>
        ///     determine the type of a constant value
        /// </summary>
        /// <param name="element">constant value</param>
        public void EndVisit(ConstantValue element) {

            // some constant literals have already assigned type information
            // nothing has to be done
            if (element.TypeInfo != default)
                return;

            switch (element.Kind) {
                case ConstantValueKind.True:
                    element.TypeInfo = Runtime.Booleans.TrueValue;
                    return;

                case ConstantValueKind.False:
                    element.TypeInfo = Runtime.Booleans.TrueValue;
                    return;


                case ConstantValueKind.Nil:
                    element.TypeInfo = Runtime.Types.Nil;
                    return;
            }

            MarkWithErrorType(element);
        }

        /// <summary>
        ///     annotate types for binary operators
        /// </summary>
        /// <param name="element">operator to annotate</param>
        public void StartVisit(BinaryOperator element) {
            if (element.LeftOperand is IRequiresArrayExpression e1)
                e1.RequiresArray = element.RequiresArray;

            if (element.RightOperand is IRequiresArrayExpression e2)
                e2.RequiresArray = element.RequiresArray;
        }

        /// <summary>
        ///     annotate types for binary operators
        /// </summary>
        /// <param name="element">operator to annotate</param>
        public void EndVisit(BinaryOperator element) {
            var left = GetTypeOfNode(element.LeftOperand);
            var right = GetTypeOfNode(element.RightOperand);

            // special case range operator: the range operator is
            // part of a type definition and references types, not values
            if (element.Kind == ExpressionKind.RangeOperator) {
                var rangeResult = TypeRegistry.GetTypeForSubrangeType(TypeCreator, left, right);
                element.TypeInfo = rangeResult.Reference;
                return;
            }

            var operatorId = TypeRegistry.GetOperatorId(element.Kind, left, right);
            var binaryOperator = TypeRegistry.GetOperator(operatorId);
            if (operatorId == OperatorKind.Undefined || binaryOperator == default) {
                MarkWithErrorType(element);
                return;
            }

            var resultType = TypeRegistry.SystemUnit.UnspecifiedType.Reference;
            var signature = Runtime.Types.MakeSignature(resultType, left, right);
            element.TypeInfo = binaryOperator.EvaluateOperator(signature, CurrentUnitType);
        }

        /// <summary>
        ///     determine the type of an unary operator
        /// </summary>
        /// <param name="element">operator to determine the type of</param>
        public void EndVisit(UnaryOperator element) {

            var operand = element.Value;

            if (operand == default || operand.TypeInfo == default) {
                MarkWithErrorType(element);
                return;
            }

            if (element.Kind == ExpressionKind.Not) {
                element.TypeInfo = GetTypeOfOperator(OperatorKind.NotOperator, GetTypeOfNode(operand));
                return;
            }

            if (element.Kind == ExpressionKind.UnaryMinus) {
                element.TypeInfo = GetTypeOfOperator(OperatorKind.UnaryMinus, GetTypeOfNode(operand));
                return;
            }

            if (element.Kind == ExpressionKind.UnaryPlus) {
                element.TypeInfo = GetTypeOfOperator(OperatorKind.UnaryPlus, GetTypeOfNode(operand));
                return;
            }

            if (element.Kind == ExpressionKind.AddressOf) {
                element.TypeInfo = GetTypeOfOperator(OperatorKind.AtOperator, GetTypeOfNode(operand));
                return;
            }

            MarkWithErrorType(element);
        }

        /// <summary>
        ///     gets the type of a given unary operator
        /// </summary>
        /// <param name="operatorKind"></param>
        /// <param name="operand"></param>
        /// <returns></returns>
        private ITypeSymbol GetTypeOfOperator(OperatorKind operatorKind, ITypeSymbol operand) {

            var unaryOperator = TypeRegistry.GetOperator(operatorKind);

            if (unaryOperator == default)
                return ErrorReference;

            var resultType = TypeRegistry.SystemUnit.UnspecifiedType.Reference;
            var signature = Runtime.Types.MakeSignature(resultType, operand);
            return unaryOperator.EvaluateOperator(signature, CurrentUnitType);
        }

        private ImmutableArray<IValue> ToValueArray(IPoolItem<List<ITypeSymbol>> elements) {
            using (var constants = environment.ListPools.GetList<IValue>()) {
                foreach (var value in elements.Item)
                    constants.Add(value as IValue);
                return environment.ListPools.GetFixedArray(constants);
            }
        }

        /// <summary>
        ///     visit an array constant
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ArrayConstant element) {
            using (var constantValues = environment.ListPools.GetList<ITypeSymbol>()) {

                var baseType = GetBaseTypeForArrayConstant(element.Items, out var isConstant, constantValues.Item, element, true);
                var ints = TypeRegistry.Runtime.Integers;
                var ubound = ints.ToScaledIntegerValue(constantValues.Item.Count);
                var indexTypeDef = TypeCreator.CreateSubrangeType(string.Empty, ubound.TypeDefinition as IOrdinalType, ints.Zero, ubound);
                var arrayType = TypeCreator.CreateStaticArrayType(baseType.TypeDefinition, string.Empty, indexTypeDef, false);

                if (isConstant) {
                    element.TypeInfo = environment.Runtime.Structured.CreateArrayValue(arrayType, baseType.TypeDefinition, ToValueArray(constantValues));
                }
                else {
                    element.TypeInfo = arrayType.Reference;
                }
            }
        }

        /// <summary>
        ///     visit a set expression
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(SetExpression element) {

            if (element.Expressions.Count < 1) {
                element.TypeInfo = TypeRegistry.Runtime.Structured.EmptySet;
                return;
            }

            using (var values = environment.ListPools.GetList<ITypeSymbol>()) {

                var baseType = GetBaseTypeForArrayConstant(element.Expressions, out var isConstant, values.Item, element, element.RequiresArray);

                if (baseType == default) {
                    element.TypeInfo = TypeRegistry.Runtime.Structured.EmptySet;
                    return;
                }

                if (baseType.GetBaseType() == BaseType.Error) {
                    MarkWithErrorType(element);
                    return;
                }

                var typdef = default(ITypeDefinition);

                if (!element.RequiresArray)
                    typdef = TypeCreator.CreateSetType(baseType.TypeDefinition as IOrdinalType, string.Empty);
                else if (isConstant) {
                    var indexType = TypeCreator.CreateSubrangeType(string.Empty, TypeRegistry.SystemUnit.IntegerType, TypeRegistry.Runtime.Integers.Zero, TypeRegistry.Runtime.Integers.ToScaledIntegerValue(values.Item.Count - 1));
                    typdef = TypeCreator.CreateStaticArrayType(baseType.TypeDefinition, string.Empty, indexType, false);
                }
                else
                    typdef = TypeCreator.CreateDynamicArrayType(baseType.TypeDefinition, string.Empty, false);

                if (!element.RequiresArray) {
                    if (isConstant)
                        element.TypeInfo = TypeRegistry.Runtime.Structured.CreateSetValue(typdef, ToValueArray(values));
                    else
                        element.TypeInfo = typdef.Reference;
                }
                else {
                    if (isConstant)
                        element.TypeInfo = TypeRegistry.Runtime.Structured.CreateArrayValue(typdef, baseType.TypeDefinition, ToValueArray(values));
                    else
                        element.TypeInfo = typdef.Reference;
                }
            }
        }



        /// <summary>
        ///     end visiting a symbol reference
        /// </summary>
        /// <param name="element">symbol reference</param>
        public void EndVisit(SymbolReference element) {
            ITypeSymbol baseTypeValue = SystemUnit.UnspecifiedType.Reference;

            if (element.TypeValue is ITypedSyntaxPart typeRef)
                baseTypeValue = typeRef.TypeInfo;

            if (element.Inherited) {

                foreach (var impl in currentMethodImplementation) {
                    if (impl.RoutineGroup.DefiningType.BaseType == BaseType.Error)
                        continue;

                    var classMethod = impl.IsClassItem();
                    var signature = default(ISignature); // impl.CreateSignature(TypeRegistry);
                    var callableRoutines = new List<IRoutineResult>();

                    var bdef = impl.RoutineGroup.DefiningType as IStructuredType;
                    var idef = bdef.BaseClass as IStructuredType;

                    if (idef == default)
                        continue;

                    idef.ResolveCall(impl.RoutineGroup.Name, callableRoutines, signature);

                    if (callableRoutines.Count == 1)
                        baseTypeValue = callableRoutines[0];

                    break;
                }

            }

            foreach (var part in element.SymbolParts) {

                if (part is MetaType metaType) {

                    if (metaType.Kind == MetaTypeKind.AnsiString)
                        baseTypeValue = SystemUnit.AnsiStringType.Reference;

                    if (metaType.Kind == MetaTypeKind.ShortString)
                        baseTypeValue = SystemUnit.ShortStringType.Reference;

                    if (metaType.Kind == MetaTypeKind.UnicodeString)
                        baseTypeValue = SystemUnit.UnicodeStringType.Reference;

                    if (metaType.Kind == MetaTypeKind.WideString)
                        baseTypeValue = SystemUnit.WideStringType.Reference;

                    if (metaType.Kind == MetaTypeKind.StringType)
                        baseTypeValue = SystemUnit.StringType.Reference;

                }

                else if (part is SymbolReferencePart symRef) {

                    if (baseTypeValue.GetBaseType() == BaseType.Error)
                        break;

                    if (symRef.Kind == SymbolReferencePartKind.SubItem) {
                        var flags = ResolverFlags.None;
                        var classType = baseTypeValue as IStructuredType;
                        var self = resolver.ResolveReferenceByName(default, "Self");
                        var selfType = (self?.Symbol?.TypeDefinition ?? SystemUnit.ErrorType) as IStructuredType;

                        if (classType != default && (selfType == default || selfType != classType))
                            flags |= ResolverFlags.SkipPrivate;

                        if (classType != default && (selfType == default || selfType != default && !selfType.InheritsFrom(classType)))
                            flags |= ResolverFlags.SkipProtected;

                        if (self != default && self.Kind == ReferenceKind.RefToSelfClass)
                            flags |= ResolverFlags.RequireClassSymbols;

                        baseTypeValue = resolver.ResolveTypeByName(baseTypeValue, symRef.Name, 0, flags);
                    }
                    else if (symRef.Kind == SymbolReferencePartKind.StringType) {
                        baseTypeValue = symRef.Value.TypeInfo;
                    }
                    else if ((symRef.Kind == SymbolReferencePartKind.CallParameters || symRef.Kind == SymbolReferencePartKind.StringCast) && symRef.Name != null) {
                        var callableRoutines = new List<IRoutineResult>();
                        var signature = CreateSignatureFromSymbolPart(symRef);

                        if (baseTypeValue is IUnspecifiedType) {
                            var reference = resolver.ResolveByName(baseTypeValue, symRef.Name, 0, ResolverFlags.None);

                            if (reference == null) {
                                baseTypeValue = ErrorReference;
                            }
                            else if (reference.Kind == ReferenceKind.RefToGlobalRoutine) {
                                if (reference.Symbol is IRoutineGroup routine) {
                                    routine.ResolveCall(callableRoutines, signature);
                                }
                            }
                            else if (reference.Kind == ReferenceKind.RefToType && signature.Count == 1) {
                                if (signature[0].IsConstant()) {
                                    baseTypeValue = environment.Runtime.Cast(TypeRegistry, signature[0] as IValue, ((ITypeDefinition)reference.Symbol));
                                }
                                else {
                                    baseTypeValue = TypeRegistry.Cast(signature[0], reference.Symbol);
                                }
                            }

                        }

                        else if (baseTypeValue.GetBaseType() == BaseType.Structured && baseTypeValue.TypeDefinition is IStructuredType structType) {
                            structType.ResolveCall(symRef.Name, callableRoutines, signature);
                        }

                        if (callableRoutines.Count == 1) {
                            if (callableRoutines[0] is IIntrinsicInvocationResult _) {
                                baseTypeValue = Runtime.Types.MakeInvocationResultFromIntrinsic(callableRoutines[0].Routine, signature);
                            }
                            else {
                                var targetRoutine = callableRoutines[0] as IInvocationResult;
                                baseTypeValue = Runtime.Types.MakeInvocationResult(targetRoutine.RoutineIndex);
                            }
                        }
                    }


                }

                part.TypeInfo = baseTypeValue;
            }

            element.TypeInfo = baseTypeValue;
        }

        /// <summary>
        ///     visit a formatted expression
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(FormattedExpression element) {
            if (element.IsConstant) {
                using (var list = environment.ListPools.GetList<IValue>()) {
                    for (var i = 0; i < element.Expressions.Count; i++) {
                        list.Item.Add(element.Expressions[i].TypeInfo as IValue);
                    }
                    element.TypeInfo = Runtime.FormatExpression(environment.ListPools.GetFixedArray(list));
                }
            }
            else {
                var baseTypeId = element.Expressions[0].TypeInfo?.TypeDefinition ?? SystemUnit.ErrorType;
                var type = baseTypeId;
                var baseType = type.BaseType;

                switch (baseType) {
                    case BaseType.Subrange:
                    case BaseType.Boolean:
                    case BaseType.Char:
                    case BaseType.String:
                    case BaseType.Integer:
                    case BaseType.Real:
                        ISignature s;
                        var st = SystemUnit.StringType.Reference;

                        switch (element.Expressions.Count) {
                            case 1:
                                s = Runtime.Types.MakeSignature(st, element.Expressions[0].TypeInfo);
                                break;

                            case 2:
                                s = Runtime.Types.MakeSignature(st, element.Expressions[0].TypeInfo, element.Expressions[1].TypeInfo);
                                break;

                            case 3:
                                s = Runtime.Types.MakeSignature(st, element.Expressions[0].TypeInfo, element.Expressions[1].TypeInfo, element.Expressions[2].TypeInfo);
                                break;

                            default:
                                s = default;
                                break;
                        }

                        if (!(s is null))
                            element.TypeInfo = Runtime.Types.MakeInvocationResultFromIntrinsic(SystemUnit.FormatExpression, s);
                        else
                            MarkWithErrorType(element);
                        break;

                    default:
                        MarkWithErrorType(element);
                        break;
                }
            }
        }

    }
}
