namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     inlining directive
    /// </summary>
    public class InlineDirective : SyntaxPartBase {

        /// <summary>
        ///     inline or assembler
        /// </summary>
        public int Kind { get; set; }

    }
}
