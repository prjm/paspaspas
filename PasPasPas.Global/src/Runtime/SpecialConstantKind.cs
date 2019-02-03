namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     kinds special constants
    /// </summary>
    public enum SpecialConstantKind {

        /// <summary>
        ///     unknown constant
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     integer overflow literal
        /// </summary>
        IntegerOverflow = 3,

        /// <summary>
        ///     invalid integer
        /// </summary>
        InvalidInteger = 4,

        /// <summary>
        ///     invalid read literal
        /// </summary>
        InvalidReal = 5,

        /// <summary>
        ///     division by zero
        /// </summary>
        DivisionByZero = 6,

        /// <summary>
        ///     invalid boolean
        /// </summary>
        InvalidBool = 7,

        /// <summary>
        ///     invalid string
        /// </summary>
        InvalidString = 8,

        /// <summary>
        ///     nil pointer
        /// </summary>
        Nil = 9,

        /// <summary>
        ///     invalid char
        /// </summary>
        InvalidChar = 10,

        /// <summary>
        ///     invalid set
        /// </summary>
        InvalidSet = 11
    }
}
