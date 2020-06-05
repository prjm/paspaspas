#nullable disable
namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     boolean type kind
    /// </summary>
    public enum BooleanTypeKind : byte {

        /// <summary>
        ///     undefined type kind
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     standard boolean
        /// </summary>
        Boolean = 1,

        /// <summary>
        ///     byte boolean
        /// </summary>
        ByteBool = 2,

        /// <summary>
        ///     word boolean
        /// </summary>
        WordBool = 3,

        /// <summary>
        ///     long boolean
        /// </summary>
        LongBool = 4

    }
}