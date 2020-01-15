using System.Collections.Immutable;
using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     operations for structured types
    /// </summary>
    public interface IStructuredTypeOperations : IRelationalOperations {

        /// <summary>
        ///     empty set
        /// </summary>
        IOldTypeReference EmptySet { get; }

        /// <summary>
        ///     create an array value
        /// </summary>
        /// <param name="registeredType"></param>
        /// <param name="baseTypeId"></param>
        /// <param name="values">array values</param>
        /// <returns></returns>
        IArrayValue CreateArrayValue(int registeredType, int baseTypeId, ImmutableArray<IOldTypeReference> values);

        /// <summary>
        ///     create a record value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        IOldTypeReference CreateRecordValue(int typeId, ImmutableArray<IOldTypeReference> values);

        /// <summary>
        ///     create a set value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        IOldTypeReference CreateSetValue(int typeId, ImmutableArray<IOldTypeReference> values);

        /// <summary>
        ///     compute a set union
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        IOldTypeReference SetUnion(ITypeRegistry types, IOldTypeReference left, IOldTypeReference right);

        /// <summary>
        ///     compute a set difference
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        IOldTypeReference SetDifference(ITypeRegistry types, IOldTypeReference left, IOldTypeReference right);

        /// <summary>
        ///     compute a set intersection
        /// </summary>
        /// <param name="typeRegistry"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IOldTypeReference SetIntersection(ITypeRegistry typeRegistry, IOldTypeReference left, IOldTypeReference right);

        /// <summary>
        ///     test if an element is contained in a set
        /// </summary>
        /// <param name="typeRegistry"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IOldTypeReference InSet(ITypeRegistry typeRegistry, IOldTypeReference left, IOldTypeReference right);
    }
}