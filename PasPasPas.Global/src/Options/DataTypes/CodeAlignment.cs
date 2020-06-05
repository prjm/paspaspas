#nullable disable
namespace PasPasPas.Globals.Options.DataTypes {

    /// <summary>
    ///     Option for code alignment
    /// </summary>
    public enum CodeAlignment {

        /// <summary>
        ///     Undefined alignment
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     $CODEALIGN 1
        /// </summary>
        OneByte = 1,

        /// <summary>
        ///     $CODEALIGN 2
        /// </summary>
        TwoByte = 2,


        /// <summary>
        ///     $CODEALIGN 4
        /// </summary>
        FourByte = 3,

        /// <summary>
        ///     $CODEALIGN 8
        /// </summary>
        EightByte = 4,

        /// <summary>
        ///     $CODEALIGN 16
        /// </summary>
        SixteenByte = 5

    }
}
