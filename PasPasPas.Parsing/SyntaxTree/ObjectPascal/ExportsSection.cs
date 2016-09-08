namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     exports section
    /// </summary>
    public class ExportsSection : SyntaxPartBase {

        /// <summary>
        ///     export name
        /// </summary>
        public PascalIdentifier ExportName { get; set; }

    }
}
