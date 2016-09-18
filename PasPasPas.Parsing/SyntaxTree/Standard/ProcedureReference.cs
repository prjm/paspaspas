namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     procedure reference
    /// </summary>
    public class ProcedureReference : SyntaxPartBase {

        /// <summary>
        ///     procedure type
        /// </summary>
        public ProcedureTypeDefinition ProcedureType { get; set; }

    }
}