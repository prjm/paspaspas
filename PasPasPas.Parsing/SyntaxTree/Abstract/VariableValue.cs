namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     reference a variable value
    /// </summary>
    public class VariableValue : ExpressionBase {

        /// <summary>
        ///     <c>true</c> if a pointer was used
        /// </summary>
        public bool PointerTo { get; set; }

        /// <summary>
        ///     name of the variable
        /// </summary>
        public SymbolName Name { get; set; }

    }
}
