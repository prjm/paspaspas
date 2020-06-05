#nullable disable
namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     real type kind
    /// </summary>
    public enum RealTypeKind : byte {

        /// <summary>
        ///     undefined kind
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     single precision
        /// </summary>
        Single = 1,

        /// <summary>
        ///     double precision
        /// </summary>
        Double = 2,

        /// <summary>
        ///     extended precision
        /// </summary>
        Extended = 3,

        /// <summary>
        ///     real48 compatibility type
        /// </summary>
        Real48 = 4,

        /// <summary>
        ///     comp data type
        /// </summary>
        Comp = 5,

        /// <summary>
        ///     currency data type
        /// </summary>
        Currency = 6

    }
}
