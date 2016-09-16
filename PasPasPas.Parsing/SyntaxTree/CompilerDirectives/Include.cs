namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     include directive
    /// </summary>
    public class Include : SyntaxPartBase {

        /// <summary>
        ///     include file name
        /// </summary>
        public string FileName { get; set; }
    }
}
