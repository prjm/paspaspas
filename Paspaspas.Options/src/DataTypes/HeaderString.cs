namespace PasPasPas.Options.DataTypes {

    /// <summary>
    /// /   simple header string to emit in hpp file
    /// </summary>
    public class HeaderString {

        /// <summary>
        ///     emit mode
        /// </summary>
        public HppEmitMode Mode { get; internal set; }

        /// <summary>
        ///     header value
        /// </summary>
        public string Value { get; internal set; }
    }
}