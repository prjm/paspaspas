using System.Collections.Immutable;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.Runtime.Values.Structured {

    /// <summary>
    ///     structured type operations
    /// </summary>
    public class StructuredTypeOperations : IStructuredTypeOperations {

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
    }
}
