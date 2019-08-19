namespace PasPasPas.Globals.Log {

    /// <summary>
    ///     message numbers
    /// </summary>
    public static class MessageNumbers {

        /// <summary>
        ///     default number format
        /// </summary>
        public const string NumberFormat = "X4";


        /// <summary>
        ///     message: missing token error message id
        /// </summary>
        public const uint MissingToken
            = 0x0103;

        /// <summary>
        ///     error code for a open <c>ifdef</c> / <c>ifndef</c>
        /// </summary>
        public const uint PendingCondition
            = 0x0301;

        /// <summary>
        ///     pending region
        /// </summary>
        public const uint PendingRegion
            = 0x0302;

        /// <summary>
        ///     missing file
        /// </summary>
        public const uint MissingFile
            = 0x104;
    }
}
