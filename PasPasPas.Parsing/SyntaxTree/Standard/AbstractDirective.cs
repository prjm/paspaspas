namespace PasPasPas.Parsing.SyntaxTree.Standard {


    /// <summary>
    ///     abstract directive
    /// </summary>
    public class AbstractDirective : SyntaxPartBase {

        /// <summary>
        ///     final or abstract
        /// </summary>
        public int Kind { get; set; }

    }
}
