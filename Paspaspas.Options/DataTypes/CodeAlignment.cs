namespace PasPasPas.Options.DataTypes {

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
        OneByte,

        /// <summary>
        ///     $CODEALIGN 2
        /// </summary>
        TwoByte,


        /// <summary>
        ///     $CODEALIGN 4
        /// </summary>
        FourByte,


        /// <summary>
        ///     $CODEALIGN 8
        /// </summary>
        EightByte,


        /// <summary>
        ///     $CODEALIGN 16
        /// </summary>
        SixteenByte

    }
}
