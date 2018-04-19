using SharpFloat.FloatingPoint;

namespace PasPasPas.Global.Runtime {

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
        ///     provides operations on characters
        /// </summary>
        ICharOperations Chars { get; }

    }
}