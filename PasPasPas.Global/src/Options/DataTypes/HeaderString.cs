namespace PasPasPas.Options.DataTypes {

    /// <summary>
    /// /   simple header string to emit in hpp file
    /// </summary>
    public class HeaderString {

        /// <summary>
        ///     create a new header string
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="emitValue"></param>
        public HeaderString(HppEmitMode mode, string emitValue) {
            Mode = mode;
            Value = emitValue;
        }

        /// <summary>
        ///     emit mode
        /// </summary>
        public HppEmitMode Mode { get; }

        /// <summary>
        ///     header value
        /// </summary>
        public string Value { get; }
    }
}