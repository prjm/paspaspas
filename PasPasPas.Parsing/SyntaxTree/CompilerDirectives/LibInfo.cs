namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     libprefix, libsuffix and libversion directive
    /// </summary>
    public class LibInfo : SyntaxPartBase {

        /// <summary>
        ///     prefix
        /// </summary>
        public string LibPrefix { get; set; }

        /// <summary>
        ///     suffix
        /// </summary>
        public string LibSuffix { get; set; }

        /// <summary>
        ///     version
        /// </summary>
        public string LibVersion { get; set; }
    }
}
