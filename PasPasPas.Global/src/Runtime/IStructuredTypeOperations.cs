using System.Collections.Immutable;
using PasPasPas.Globals.Types;

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

        /// <summary>
        ///     create a set value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        ITypeReference CreateSetValue(int typeId, ImmutableArray<ITypeReference> values);

        /// <summary>
        ///     compute a set union
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        ITypeReference SetUnion(ITypeRegistry types, ITypeReference left, ITypeReference right);

        /// <summary>
        ///     compute a set difference
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        ITypeReference SetDifference(ITypeRegistry types, ITypeReference left, ITypeReference right);

        /// <summary>
        ///     compute a set intersection
        /// </summary>
        /// <param name="typeRegistry"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        ITypeReference SetIntersection(ITypeRegistry typeRegistry, ITypeReference left, ITypeReference right);
    }
}