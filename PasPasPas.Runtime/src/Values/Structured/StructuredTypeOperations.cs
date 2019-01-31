using System.Collections.Immutable;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Runtime.Values.Structured {

    /// <summary>
    ///     structured type operations
    /// </summary>
    public class StructuredTypeOperations : IStructuredTypeOperations {

        /// <summary>
        ///     create new structured type operations
        /// </summary>
        /// <param name="listPools"></param>
        public StructuredTypeOperations(IListPools listPools) => ListPools = listPools;

        /// <summary>
        ///     shared list pools
        /// </summary>
        public IListPools ListPools { get; }

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
        ///     compute a set difference
        /// </summary>
        /// <param name="types"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public ITypeReference SetDifference(ITypeRegistry types, ITypeReference left, ITypeReference right) => throw new System.NotImplementedException();

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

            var baseType = types.GetMatchingSetBaseType(left, right);

            if (baseType == KnownTypeIds.ErrorType)
                return types.Runtime.Types.MakeErrorTypeReference();

            using (var list = ListPools.GetList<ITypeReference>()) {

                foreach (var value in leftSet.Values)
                    list.Add(types.Runtime.Cast(types, value, baseType));

                foreach (var value in rightSet.Values)
                    list.Add(types.Runtime.Cast(types, value, baseType));

                return new SetValue(baseType, ListPools.GetFixedArray(list));
            }
        }
    }
}
