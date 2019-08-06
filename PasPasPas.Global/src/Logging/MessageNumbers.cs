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
        ///     error code for a open <c>ifdef</c> / <c>ifndef</c>
        /// </summary>
        public const uint PendingCondition
            = 0x0301;

        /// <summary>
        ///     pending region
        /// </summary>
        public const uint PendingRegion
            = 0x0302;

    }
}
