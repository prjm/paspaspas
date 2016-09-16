namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     stack mem size directive
    /// </summary>
    public class StackMemorySize : SyntaxPartBase {

        /// <summary>
        ///     maximum stack size
        /// </summary>
        public int? MaxStackSize { get; set; }

        /// <summary>
        ///     minimum stack size
        /// </summary>
        public int? MinStackSize { get; set; }
    }
}
