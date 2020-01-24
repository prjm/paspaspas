using System.Collections.Immutable;
using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     factory to create runtime values and to perform runtime operations
    /// </summary>
    public interface IRuntimeValueFactory {

        /// <summary>
        ///     providers integer operations: arithmetics, logics, relational operators
        /// </summary>
        IIntegerOperations Integers { get; }

        /// <summary>
        ///     provides real number operations: arithmetics, logics, relation operators
        /// </summary>
        IRealNumberOperations RealNumbers { get; }

        /// <summary>
        ///     provides logic operations on booleans
        /// </summary>
        IBooleanOperations Booleans { get; }

        /// <summary>
        ///     provides operations on strings
        /// </summary>
        IStringOperations Strings { get; }

        /// <summary>
        ///     make a pointer value
        /// </summary>
        /// <param name="baseValue"></param>
        /// <returns></returns>
        IOldTypeReference MakePointerValue(IOldTypeReference baseValue);

        /// <summary>
        ///     provides operations on characters
        /// </summary>
        ICharOperations Chars { get; }

        /// <summary>
        ///     structured types
        /// </summary>
        IStructuredTypeOperations Structured { get; }

        /// <summary>
        ///     type operations
        /// </summary>
        ITypeOperations Types { get; }

        /// <summary>
        ///     cast a constant value to another type
        /// </summary>
        /// <param name="value">value to cast</param>
        /// <param name="typeId">target type id</param>
        /// <param name="types"></param>
        /// <returns></returns>
        IOldTypeReference Cast(ITypeRegistry types, IValue value, int typeId);

        /// <summary>
        ///     create a new enum value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IOldTypeReference MakeEnumValue(int typeId, IOldTypeReference value);

        /// <summary>
        ///     format a constant expression
        /// </summary>
        /// <param name="values">input data</param>
        /// <returns></returns>
        IOldTypeReference FormatExpression(ImmutableArray<IOldTypeReference> values);

    }
}