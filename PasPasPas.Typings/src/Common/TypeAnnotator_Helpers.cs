#nullable disable
using System;
using System.Collections.Generic;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Typings.Routines;

namespace PasPasPas.Typings.Common {

    public partial class TypeAnnotator {

        /// <summary>
        ///     system unit
        /// </summary>
        private ISystemUnit SystemUnit
            => TypeRegistry.SystemUnit;

        private ITypeCreator TypeCreator { get; set; }

        private ITypeDefinition GetSmallestIntegralTypeOrNext(ITypeDefinition leftId, ITypeDefinition rightId)
            => environment.TypeRegistry.GetSmallestIntegralTypeOrNext(leftId, rightId);

        private void MarkWithErrorType(ITypedSyntaxPart node)
            => node.TypeInfo = ErrorReference;

        private bool AreRecordTypesCompatible(ITypeDefinition leftId, ITypeDefinition rightId)
            => environment.TypeRegistry.AreRecordTypesCompatible(leftId, rightId);

        private ITypeDefinition GetSmallestTextTypeOrNext(ITypeDefinition leftId, ITypeDefinition rightId)
            => environment.TypeRegistry.GetSmallestTextTypeOrNext(leftId, rightId);

        private ITypeSymbol GetTypeOfNode(ITypedSyntaxPart syntaxNode) {
            if (syntaxNode != default && syntaxNode.TypeInfo != default)
                return syntaxNode.TypeInfo;

            return ErrorReference;
        }

        private ISignature CreateSignatureFromSymbolPart(SymbolReferencePart part) {
            using (var list = environment.ListPools.GetList<ITypeSymbol>()) {

                for (var i = 0; i < part.Expressions.Count; i++)
                    if (part.Expressions[i] != null && part.Expressions[i].TypeInfo != null)
                        list.Add(part.Expressions[i].TypeInfo);
                    else
                        list.Add(ErrorReference);

                var unspecType = TypeRegistry.SystemUnit.UnspecifiedType.Reference;
                return TypeRegistry.Runtime.Types.MakeSignature(unspecType, environment.ListPools.GetFixedArray(list));
            }
        }

        private bool ExpandRangeOperator(ITypedSyntaxPart part, bool requiresArray, List<ITypeSymbol> values, out ITypeSymbol baseTypeId) {
            if (!(part.TypeInfo.TypeDefinition is ISubrangeType subrangeType) || requiresArray) {
                baseTypeId = ErrorReference;
                return false;
            }

            baseTypeId = subrangeType.SubrangeOfType.Reference;
            var lowerBound = subrangeType.LowestElement;
            var upperBound = subrangeType.HighestElement;

            if (!subrangeType.IsValid) {
                baseTypeId = ErrorReference;
                return false;
            }

            var cardinality = subrangeType.Cardinality;

            if (cardinality < 1)
                return true;

            if (cardinality > 255) {
                baseTypeId = ErrorReference;
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

        private ITypeSymbol GetBaseTypeForArrayConstant(IEnumerable<IExpression> items, out bool isConstant, List<ITypeSymbol> values, ITypedSyntaxPart element, bool requiresArray) {
            var baseType = default(ITypeSymbol);
            isConstant = true;

            foreach (var part in items) {

                if (part is BinaryOperator binaryOperator && binaryOperator.Kind == ExpressionKind.RangeOperator) {
                    if (requiresArray ||
                        !ExpandRangeOperator(part, requiresArray, values, out var setBaseType) ||
                        baseType != default && baseType != setBaseType) {
                        values.Clear();
                        return ErrorReference;
                    }
                    baseType = setBaseType;
                    continue;
                }

                if (part.TypeInfo == null || !part.TypeInfo.IsConstant()) {
                    values.Clear();
                    return ErrorReference;
                }

                baseType = TypeRegistry.GetBaseTypeForArrayOrSet(baseType, part.TypeInfo).Reference;

                if (baseType.TypeDefinition == TypeRegistry.SystemUnit.ErrorType) {
                    values.Clear();
                    return ErrorReference;
                }

                if (!requiresArray && !(baseType.TypeDefinition is IOrdinalType)) {
                    values.Clear();
                    return ErrorReference;
                }

                isConstant = isConstant && part.TypeInfo.IsConstant();
                values.Add(part.TypeInfo);
            }

            if (baseType == default)
                baseType = ErrorReference;

            return baseType;
        }

        private ITypeRegistry TypeRegistry
            => environment.TypeRegistry;

        private IRuntimeValueFactory Runtime
            => environment.Runtime;

        private T PopTypeFromStack<T>() where T : class, ITypeDefinition
            => (currentType.Count > 0 ? currentType.Pop() as T : default(T)) ?? throw new InvalidOperationException(typeof(T).ToString());

        private T PeekTypeFromStack<T>() where T : class, ITypeDefinition
            => (currentType.Count > 0 ? currentType.Peek() as T : default(T)) ?? throw new InvalidOperationException(typeof(T).ToString());

        private void PushTypeToStack(ITypeDefinition type)
            => currentType.Push(type);

    }
}
