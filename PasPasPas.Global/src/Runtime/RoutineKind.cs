#nullable disable
namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     routine kind
    /// </summary>
    public enum RoutineKind : byte {

        /// <summary>
        ///     unknown procedure kind
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     procedure
        /// </summary>
        Procedure = 1,

        /// <summary>
        ///     function
        /// </summary>
        Function = 2,

        /// <summary>
        ///     constructor
        /// </summary>
        Constructor = 3,

        /// <summary>
        ///     destructor
        /// </summary>
        Destructor = 4,

        /// <summary>
        ///     operator
        /// </summary>
        Operator = 5,

    }
}