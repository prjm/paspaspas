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

        private ITypeDefinition GetSmallestIntegralTypeOrNext(ITypeDefinition leftId, ITypeDefinition rightId)
            => environment.TypeRegistry.GetSmallestIntegralTypeOrNext(leftId, rightId);

        private void MarkWithErrorType(ITypedSyntaxPart node)
            => node.TypeInfo = TypeRegistry.SystemUnit.ErrorType;

        private ITypeSymbol GetTypeOfNode(ITypedSyntaxPart syntaxNode) {
            if (syntaxNode != default && syntaxNode.TypeInfo != default)
                return syntaxNode.TypeInfo;

            return TypeRegistry.SystemUnit.ErrorType;
        }

        private ISignature CreateSignatureFromSymbolPart(SymbolReferencePart part) {
            using (var list = environment.ListPools.GetList<ITypeSymbol>()) {

                for (var i = 0; i < part.Expressions.Count; i++)
                    if (part.Expressions[i] != null && part.Expressions[i].TypeInfo != null)
                        list.Add(part.Expressions[i].TypeInfo);
                    else
                        list.Add(TypeRegistry.SystemUnit.ErrorType);


                return TypeRegistry.Runtime.Types.MakeSignature(TypeRegistry.SystemUnit.UnspecifiedType, environment.ListPools.GetFixedArray(list));
            }
        }

        private bool ExpandRangeOperator(ITypedSyntaxPart part, bool requiresArray, List<ITypeSymbol> values, out ITypeSymbol baseTypeId) {
            if (!(part.TypeInfo.TypeDefinition is ISubrangeType subrangeType) || requiresArray) {
                baseTypeId = TypeRegistry.SystemUnit.ErrorType;
                return false;
            }

            baseTypeId = subrangeType.SubrangeOfType;
            var lowerBound = subrangeType.LowestElement;
            var upperBound = subrangeType.HighestElement;

            if (!subrangeType.IsValid) {
                baseTypeId = TypeRegistry.SystemUnit.ErrorType;
                return false;
            }

            var cardinality = subrangeType.Cardinality;

            if (cardinality < 1)
                return true;

            if (cardinality > 255) {
                baseTypeId = TypeRegistry.SystemUnit.ErrorType;
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
                        return TypeRegistry.SystemUnit.ErrorType;
                    }
                    baseType = setBaseType;
                    continue;
                }

                if (part.TypeInfo == null || !part.TypeInfo.IsConstant()) {
                    values.Clear();
                    return TypeRegistry.SystemUnit.ErrorType;
                }

                baseType = TypeRegistry.GetBaseTypeForArrayOrSet(baseType, part.TypeInfo);

                if (baseType.TypeDefinition == TypeRegistry.SystemUnit.ErrorType) {
                    values.Clear();
                    break;
                }

                if (!requiresArray && !(baseType.TypeDefinition is IOrdinalType)) {
                    values.Clear();
                    return TypeRegistry.SystemUnit.ErrorType;
                }

                isConstant = isConstant && part.TypeInfo.IsConstant();
                values.Add(part.TypeInfo);
            }

            if (baseType == default)
                baseType = TypeRegistry.SystemUnit.ErrorType;

            return baseType;
        }

        private ITypeRegistry TypeRegistry
            => environment.TypeRegistry;

        private IRuntimeValueFactory Runtime
            => environment.Runtime;

    }
}
