namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     compiler directive to define a symbol
    /// </summary>
    public class DefineSymbol : SyntaxPartBase {

        /// <summary>
        ///     defined symbol name
        /// </summary>
        public string SymbolName { get; internal set; }
    }
}
