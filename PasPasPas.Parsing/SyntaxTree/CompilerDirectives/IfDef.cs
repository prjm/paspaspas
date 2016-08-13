namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     conditial compilation directive (ifdef)
    /// </summary>
    public class IfDef : SyntaxPartBase {

        /// <summary>
        ///     inverts the the check for the symbol ("ifndef")
        /// </summary>
        public bool Negate { get; set; }

        /// <summary>
        ///     symbol to check
        /// </summary>
        public string SymbolName { get; set; }
    }
}
