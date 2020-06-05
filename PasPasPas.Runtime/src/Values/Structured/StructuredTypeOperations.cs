#nullable disable
using System;
using System.Collections.Immutable;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Runtime.Values.Other;
using PasPasPas.Typings.Common;

namespace PasPasPas.Runtime.Values.Structured {

    /// <summary>
    ///     structured type operations
    /// </summary>
    public class StructuredTypeOperations : IStructuredTypeOperations {

        private readonly ITypeRegistryProvider provider;

        /// <summary>
        ///     create new structured type operations
        /// </summary>
        /// <param name="listPools"></param>
        /// <param name="booleans"></param>
        /// <param name="typeProvider"></param>
        public StructuredTypeOperations(ITypeRegistryProvider typeProvider, IListPools listPools, IBooleanOperations booleans) {
            provider = typeProvider;
            ListPools = listPools;
            Booleans = booleans;
            invalidSet = new Lazy<IValue>(() => new ErrorValue(provider.GetErrorType(), SpecialConstantKind.InvalidSet));
            emptySet = new Lazy<IValue>(() => new SetValue(provider.GetGenericPointerType(), ImmutableArray<IValue>.Empty));
        }

        /// <summary>
        ///     shared list pools
        /// </summary>
        public IListPools ListPools { get; }

        /// <summary>
        ///     invalid set
        /// </summary>
        public IValue InvalidSet
            => invalidSet.Value;

        private readonly Lazy<IValue> invalidSet;

        /// <summary>
        ///     empty set
        /// </summary>
        public IValue EmptySet
            => emptySet.Value;

        private readonly Lazy<IValue> emptySet;

        /// <summary>
        ///     boolean operations
        /// </summary>
        public IBooleanOperations Booleans { get; }

        /// <summary>
        ///     create a new array value
        /// </summary>
        /// <param name="registeredType"></param>
        /// <param name="baseTypeId"></param>
        /// <param name="values">array values</param>
        /// <returns></returns>
        public IArrayValue CreateArrayValue(ITypeDefinition registeredType, ITypeDefinition baseTypeId, ImmutableArray<IValue> values)
            => new ArrayValue(registeredType, baseTypeId, values);

        /// <summary>
        ///     create a new record value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public IValue CreateRecordValue(ITypeDefinition typeId, ImmutableArray<IValue> values)
            => new RecordValue(typeId, values);

        /// <summary>
        ///     create a new set value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public IValue CreateSetValue(ITypeDefinition typeId, ImmutableArray<IValue> values)
            => new SetValue(typeId, values);


        /// <summary>
        ///     compare for equality
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue Equal(IValue left, IValue right) {
            if (left is SetValue leftSet && right is SetValue rightSet)
                return Booleans.ToBoolean(SetValue.Equal(leftSet, rightSet));
            else
                return InvalidSet;
        }

        /// <summary>
        ///     unsupported operation
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue GreaterThen(IValue left, IValue right)
            => InvalidSet;

        /// <summary>
        ///     check if a set is a superset of another set
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue GreaterThenEqual(IValue left, IValue right) {
            if (left is SetValue leftSet && right is SetValue rightSet)
                return Booleans.ToBoolean(SetValue.IsSuperset(leftSet, rightSet));
            else
                return InvalidSet;
        }

        /// <summary>
        ///     test if an item is included in a set
        /// </summary>
        /// <param name="typeRegistry"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue InSet(ITypeRegistry typeRegistry, IValue left, IValue right) {
            if (!(right is SetValue rightSet))
                return InvalidSet;

            if (!(right.TypeDefinition.ResolveAlias() is ISetType setType))
                return InvalidSet;

            if (!(left is IOrdinalValue ordinalValue))
                return InvalidSet;

            var castValue = typeRegistry.Runtime.Cast(typeRegistry, ordinalValue, setType.BaseTypeDefinition);

            if (rightSet.Values.Contains(castValue))
                return typeRegistry.Runtime.Booleans.TrueValue;

            return typeRegistry.Runtime.Booleans.FalseValue;
        }

        /// <summary>
        ///     unsupported operation
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue LessThen(IValue left, IValue right)
            => InvalidSet;

        /// <summary>
        ///     check if one set is a subset of another
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue LessThenOrEqual(IValue left, IValue right) {
            if (left is SetValue leftSet && right is SetValue rightSet) {
                return Booleans.ToBoolean(SetValue.IsSubset(leftSet, rightSet));
            }
            else
                return InvalidSet;
        }

        /// <summary>
        ///     compare for inequality
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue NotEquals(IValue left, IValue right) {
            if (left is SetValue leftSet && right is SetValue rightSet)
                return Booleans.ToBoolean(!SetValue.Equal(leftSet, rightSet));
            else
                return InvalidSet;
        }

        /// <summary>
        ///     compute a set difference
        /// </summary>
        /// <param name="types"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue SetDifference(ITypeRegistry types, IValue left, IValue right) {
            if (!(left is SetValue leftSet))
                return InvalidSet;

            if (!(right is SetValue rightSet))
                return InvalidSet;

            if (leftSet.IsEmpty || rightSet.IsEmpty)
                return left;

            if (!(left.TypeDefinition.ResolveAlias() is ISetType leftType))
                return InvalidSet;

            if (!(right.TypeDefinition.ResolveAlias() is ISetType rightType))
                return InvalidSet;

            using (var list = ListPools.GetList<IValue>()) {

                foreach (var value in leftSet.Values)
                    if (!rightSet.Values.Contains(types.Runtime.Cast(types, value, rightType.BaseTypeDefinition)))
                        list.Add(value);

                return new SetValue(leftType, ListPools.GetFixedArray(list));
            }
        }

        /// <summary>
        ///     compute a set intersection
        /// </summary>
        /// <param name="typeRegistry"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="currentUnit"></param>
        /// <returns></returns>
        public IValue SetIntersection(IUnitType currentUnit, ITypeRegistry typeRegistry, IValue left, IValue right) {
            if (!(left is SetValue leftSet))
                return InvalidSet;

            if (!(right is SetValue rightSet))
                return InvalidSet;

            if (leftSet.IsEmpty || rightSet.IsEmpty)
                return EmptySet;

            var baseType = typeRegistry.GetMatchingSetBaseType(left.TypeDefinition, right.TypeDefinition, out var newType);

            if (baseType.BaseType == BaseType.Error)
                return InvalidSet;

            var typeId = default(ITypeDefinition);
            if (newType) {
                typeId = typeRegistry.CreateTypeFactory(currentUnit).CreateSetType(baseType as IOrdinalType, string.Empty);
            }
            else
                typeId = left.TypeDefinition;

            using (var list = ListPools.GetList<IValue>()) {

                foreach (var value in leftSet.Values)
                    if (rightSet.Values.Contains(value))
                        list.Add(typeRegistry.Runtime.Cast(typeRegistry, value, baseType));

                foreach (var value in rightSet.Values)
                    if (leftSet.Values.Contains(value))
                        list.Add(typeRegistry.Runtime.Cast(typeRegistry, value, baseType));

                return new SetValue(typeId, ListPools.GetFixedArray(list));
            }

        }

        /// <summary>
        ///     compute a set union
        /// </summary>
        /// <param name="types"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="currentUnit">current unit</param>
        /// <returns></returns>
        public IValue SetUnion(IUnitType currentUnit, ITypeRegistry types, IValue left, IValue right) {
            if (!(left is SetValue leftSet))
                return InvalidSet;

            if (!(right is SetValue rightSet))
                return InvalidSet;

            if (leftSet.IsEmpty)
                return right;

            if (rightSet.IsEmpty)
                return left;

            var baseType = types.GetMatchingSetBaseType(left.TypeDefinition, right.TypeDefinition, out var newType);

            if (baseType.BaseType == BaseType.Error)
                return InvalidSet;

            var typeId = default(ITypeDefinition);
            if (newType)
                typeId = types.CreateTypeFactory(currentUnit).CreateSetType(baseType as IOrdinalType, string.Empty);
            else
                typeId = left.TypeDefinition;

            using (var list = ListPools.GetList<IValue>()) {

                foreach (var value in leftSet.Values)
                    list.Add(types.Runtime.Cast(types, value, baseType));

                foreach (var value in rightSet.Values)
                    list.Add(types.Runtime.Cast(types, value, baseType));

                return new SetValue(typeId, ListPools.GetFixedArray(list));
            }
        }
    }
}
