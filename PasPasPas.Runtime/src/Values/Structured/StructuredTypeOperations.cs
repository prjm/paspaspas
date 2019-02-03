using System.Collections.Immutable;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Runtime.Values.Structured {

    /// <summary>
    ///     structured type operations
    /// </summary>
    public class StructuredTypeOperations : IStructuredTypeOperations {

        /// <summary>
        ///     create new structured type operations
        /// </summary>
        /// <param name="listPools"></param>
        /// <param name="booleans"></param>
        public StructuredTypeOperations(IListPools listPools, IBooleanOperations booleans) {
            ListPools = listPools;
            Booleans = booleans;
        }

        /// <summary>
        ///     shared list pools
        /// </summary>
        public IListPools ListPools { get; }

        /// <summary>
        ///     invalid set
        /// </summary>
        public ITypeReference InvalidSet { get; }
            = new SpecialValue(SpecialConstantKind.InvalidSet);

        /// <summary>
        ///     empty set
        /// </summary>
        public ITypeReference EmptySet { get; }
            = new SetValue(KnownTypeIds.GenericPointer, ImmutableArray<ITypeReference>.Empty);

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
        public IArrayValue CreateArrayValue(int registeredType, int baseTypeId, ImmutableArray<ITypeReference> values)
            => new ArrayValue(registeredType, baseTypeId, values);

        /// <summary>
        ///     create a new record value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public ITypeReference CreateRecordValue(int typeId, ImmutableArray<ITypeReference> values)
            => new RecordValue(typeId, values);

        /// <summary>
        ///     create a new set value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public ITypeReference CreateSetValue(int typeId, ImmutableArray<ITypeReference> values)
            => new SetValue(typeId, values);


        /// <summary>
        ///     compare for equality
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public ITypeReference Equal(ITypeReference left, ITypeReference right) {
            if (left is SetValue leftSet && right is SetValue rightSet)
                return Booleans.ToBoolean(SetValue.Equal(leftSet, rightSet), KnownTypeIds.BooleanType);
            else
                return InvalidSet;
        }

        /// <summary>
        ///     unsupported operation
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public ITypeReference GreaterThen(ITypeReference left, ITypeReference right)
            => InvalidSet;

        /// <summary>
        ///     check if a set is a superset of another set
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public ITypeReference GreaterThenEqual(ITypeReference left, ITypeReference right) {
            if (left is SetValue leftSet && right is SetValue rightSet)
                return Booleans.ToBoolean(SetValue.IsSuperset(leftSet, rightSet), KnownTypeIds.BooleanType);
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
        public ITypeReference InSet(ITypeRegistry typeRegistry, ITypeReference left, ITypeReference right) {
            if (!(right is SetValue rightSet))
                return InvalidSet;

            if (!(typeRegistry.ResolveAlias(right.TypeId) is ISetType setType))
                return InvalidSet;

            if (!left.IsOrdinalValue(out var ordinalValue))
                return InvalidSet;

            var castValue = typeRegistry.Runtime.Cast(typeRegistry, ordinalValue, setType.BaseTypeId);

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
        public ITypeReference LessThen(ITypeReference left, ITypeReference right)
            => InvalidSet;

        /// <summary>
        ///     check if one set is a subset of another
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public ITypeReference LessThenOrEqual(ITypeReference left, ITypeReference right) {
            if (left is SetValue leftSet && right is SetValue rightSet) {
                return Booleans.ToBoolean(SetValue.IsSubset(leftSet, rightSet), KnownTypeIds.BooleanType);
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
        public ITypeReference NotEquals(ITypeReference left, ITypeReference right) {
            if (left is SetValue leftSet && right is SetValue rightSet)
                return Booleans.ToBoolean(!SetValue.Equal(leftSet, rightSet), KnownTypeIds.BooleanType);
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
        public ITypeReference SetDifference(ITypeRegistry types, ITypeReference left, ITypeReference right) {
            if (!(left is SetValue leftSet))
                return types.Runtime.Types.MakeErrorTypeReference();

            if (!(right is SetValue rightSet))
                return types.Runtime.Types.MakeErrorTypeReference();

            if (leftSet.IsEmpty || rightSet.IsEmpty)
                return left;

            if (!(types.ResolveAlias(left.TypeId) is ISetType leftType))
                return types.Runtime.Types.MakeErrorTypeReference();

            if (!(types.ResolveAlias(right.TypeId) is ISetType rightType))
                return types.Runtime.Types.MakeErrorTypeReference();

            using (var list = ListPools.GetList<ITypeReference>()) {

                foreach (var value in leftSet.Values)
                    if (!rightSet.Values.Contains(types.Runtime.Cast(types, value, rightType.BaseTypeId)))
                        list.Add(value);

                return new SetValue(leftType.TypeId, ListPools.GetFixedArray(list));
            }
        }

        /// <summary>
        ///     compute a set intersection
        /// </summary>
        /// <param name="typeRegistry"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public ITypeReference SetIntersection(ITypeRegistry typeRegistry, ITypeReference left, ITypeReference right) {
            if (!(left is SetValue leftSet))
                return typeRegistry.Runtime.Types.MakeErrorTypeReference();

            if (!(right is SetValue rightSet))
                return typeRegistry.Runtime.Types.MakeErrorTypeReference();

            if (leftSet.IsEmpty || rightSet.IsEmpty)
                return EmptySet;

            var baseType = typeRegistry.GetMatchingSetBaseType(left, right, out var newType);

            if (baseType == KnownTypeIds.ErrorType)
                return typeRegistry.Runtime.Types.MakeErrorTypeReference();

            var typeId = default(int);
            if (newType) {
                typeId = typeRegistry.RequireUserTypeId();
                typeRegistry.RegisterType(new SetType(typeId, baseType));
            }
            else
                typeId = left.TypeId;

            using (var list = ListPools.GetList<ITypeReference>()) {

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
        /// <returns></returns>
        public ITypeReference SetUnion(ITypeRegistry types, ITypeReference left, ITypeReference right) {
            if (!(left is SetValue leftSet))
                return types.Runtime.Types.MakeErrorTypeReference();

            if (!(right is SetValue rightSet))
                return types.Runtime.Types.MakeErrorTypeReference();

            if (leftSet.IsEmpty)
                return right;

            if (rightSet.IsEmpty)
                return left;

            var baseType = types.GetMatchingSetBaseType(left, right, out var newType);

            if (baseType == KnownTypeIds.ErrorType)
                return types.Runtime.Types.MakeErrorTypeReference();

            var typeId = default(int);
            if (newType) {
                typeId = types.RequireUserTypeId();
                types.RegisterType(new SetType(typeId, baseType));
            }
            else
                typeId = left.TypeId;

            using (var list = ListPools.GetList<ITypeReference>()) {

                foreach (var value in leftSet.Values)
                    list.Add(types.Runtime.Cast(types, value, baseType));

                foreach (var value in rightSet.Values)
                    list.Add(types.Runtime.Cast(types, value, baseType));

                return new SetValue(typeId, ListPools.GetFixedArray(list));
            }
        }
    }
}
