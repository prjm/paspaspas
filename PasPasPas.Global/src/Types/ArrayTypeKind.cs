#nullable disable
namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     array type kind
    /// </summary>
    public enum ArrayTypeKind : byte {

        /// <summary>
        ///     undefined array type kind
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     dynamic array type
        /// </summary>
        DynamicArray = 1,

        /// <summary>
        ///     static array type
        /// </summary>
        StaticArray = 2,

        /// <summary>
        ///     generic array type
        /// </summary>
        GenericArray = 3,
    }
}