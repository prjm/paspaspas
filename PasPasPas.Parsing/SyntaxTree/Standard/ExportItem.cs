namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     exported item
    /// </summary>
    public class ExportItem : SyntaxPartBase {

        /// <summary>
        ///     index parameter
        /// </summary>
        public Expression IndexParameter { get; set; }

        /// <summary>
        ///     name parameter
        /// </summary>
        public Expression NameParameter { get; set; }

        /// <summary>
        ///     parameter list
        /// </summary>
        public FormalParameters Parameters { get; set; }

        /// <summary>
        ///     resident flag
        /// </summary>
        public bool Resident { get; set; }

    }
}