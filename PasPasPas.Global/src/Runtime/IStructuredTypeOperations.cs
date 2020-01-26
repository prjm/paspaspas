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
        IValue EmptySet { get; }

        /// <summary>
        ///     create an array value
        /// </summary>
        /// <param name="registeredType"></param>
        /// <param name="baseTypeId"></param>
        /// <param name="values">array values</param>
        /// <returns></returns>
        IArrayValue CreateArrayValue(int registeredType, int baseTypeId, ImmutableArray<IValue> values);

        /// <summary>
        ///     create a record value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        IValue CreateRecordValue(int typeId, ImmutableArray<IValue> values);

        /// <summary>
        ///     create a set value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        IValue CreateSetValue(int typeId, ImmutableArray<IValue> values);

        /// <summary>
        ///     compute a set union
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        IValue SetUnion(ITypeRegistry types, IValue left, IValue right);

        /// <summary>
        ///     compute a set difference
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        IValue SetDifference(ITypeRegistry types, IValue left, IValue right);

        /// <summary>
        ///     compute a set intersection
        /// </summary>
        /// <param name="typeRegistry"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IValue SetIntersection(ITypeRegistry typeRegistry, IValue left, IValue right);

        /// <summary>
        ///     test if an element is contained in a set
        /// </summary>
        /// <param name="typeRegistry"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        IValue InSet(ITypeRegistry typeRegistry, IValue left, IValue right);
    }
}