namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     pe version directive
    /// </summary>
    public class ParsedVersion : SyntaxPartBase {

        /// <summary>
        ///     version kind
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     major version
        /// </summary>
        public int MajorVersion { get; internal set; }

        /// <summary>
        ///     minor version
        /// </summary>
        public int MinorVersion { get; internal set; }
    }
}
