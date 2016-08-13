namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     compiler directive to undefine a symbol
    /// </summary>
    public class UnDefineSymbol : SyntaxPartBase {

        /// <summary>
        ///     unddefined symbol name
        /// </summary>
        public string SymbolName { get; set; }
    }
}
