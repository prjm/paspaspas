namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     resource reference
    /// </summary>
    public class Resource : SyntaxPartBase {

        /// <summary>
        ///     file name
        /// </summary>
        public string Filename { get; internal set; }

        /// <summary>
        ///     resource file
        /// </summary>
        public string RcFile { get; internal set; }
    }
}
