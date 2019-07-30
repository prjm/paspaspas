namespace PasPasPas.Globals.Options.DataTypes {

    /// <summary>
    ///     native int size setting
    /// </summary>
    public enum NativeIntSize {

        /// <summary>
        ///     undefined
        /// </summary>
        Undefined,

        /// <summary>
        ///     all native types 32 bit
        /// </summary>
        All32bit,

        /// <summary>
        ///     all native types 64 bit
        /// </summary>
        All64bit,


        /// <summary>
        ///     32-bit <c>LongInt</c>, <c>LongWord</c> and 64-bit <c>NativeInt</c>, <c>NativeUint</c>
        /// </summary>
        Windows64bit

    }
}
