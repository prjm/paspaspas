#nullable disable
namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     symbol type kind
    /// </summary>
    public enum SymbolTypeKind : byte {

        /// <summary>
        ///     undefined symbol kind
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     definition of a type
        /// </summary>
        TypeDefinition = 1,

        /// <summary>
        ///     definition of routine group
        /// </summary>
        RoutineGroup = 2,

        /// <summary>
        ///     definition constant value
        /// </summary>
        Constant = 3,

        /// <summary>
        ///     invocation result of a routine
        /// </summary>
        InvocationResult = 4,

        /// <summary>
        ///     intrinsic routine result
        /// </summary>
        IntrinsicRoutineResult = 5,

        /// <summary>
        ///     operator result
        /// </summary>
        OperatorResult = 6,

        /// <summary>
        ///     variable
        /// </summary>
        Variable = 7,

        /// <summary>
        ///     cast result
        /// </summary>
        CastResult = 8,

        /// <summary>
        ///     self pointer
        /// </summary>
        Self = 9,

        /// <summary>
        ///     self pointer for class items
        /// </summary>
        SelfClass = 10,

        /// <summary>
        ///     bound generic type
        /// </summary>
        BoundGeneric = 11,
    }
}
