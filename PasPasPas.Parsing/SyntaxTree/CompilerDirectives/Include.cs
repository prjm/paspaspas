namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     include directive
    /// </summary>
    public class Include : SyntaxPartBase {
        public string Filename { get; internal set; }
    }
}
