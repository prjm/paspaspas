namespace PasPasPas.Parsing.SyntaxTree.Standard {


    /// <summary>
    ///     abstract directive
    /// </summary>
    public class AbstractDirective : StandardSyntaxTreeBase {

        /// <summary>
        ///     final or abstract
        /// </summary>
        public int Kind { get; set; }

    }
}
