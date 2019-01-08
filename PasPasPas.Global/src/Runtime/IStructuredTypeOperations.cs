using System.Collections.Immutable;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     operations for structured types
    /// </summary>
    public interface IStructuredTypeOperations {

        /// <summary>
        ///     create an array value
        /// </summary>
        /// <param name="registeredType"></param>
        /// <param name="baseTypeId"></param>
        /// <param name="values">array values</param>
        /// <returns></returns>
        IArrayValue CreateArrayValue(int registeredType, int baseTypeId, ImmutableArray<ITypeReference> values);

        /// <summary>
        ///     create a record value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        ITypeReference CreateRecordValue(int typeId, ImmutableArray<ITypeReference> values);

    }
}